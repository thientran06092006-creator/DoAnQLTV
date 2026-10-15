using System;
using System.Data;
using System.Drawing;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace DoAnQLTV
{
    /// <summary>
    /// Đăng nhập bằng bảng NhanVien. Tên đăng nhập = MaNhanVien.
    /// Mật khẩu lưu dạng SHA-256 có "muối" riêng cho từng nhân viên (muối = mã nhân viên).
    /// Vai trò: "Admin" (quản trị viên) hoặc "NhanVien" (nhân viên thường).
    /// </summary>
    public static class TaiKhoan
    {
        public const string VaiTroAdmin = "Admin";
        public const string VaiTroNhanVien = "NhanVien";

        /// <summary>Vai trò của người đang đăng nhập (FrmLogin gán sau khi đăng nhập thành công).</summary>
        public static string VaiTroHienTai = VaiTroNhanVien;

        public static bool LaQuanTri
        {
            get { return string.Equals(VaiTroHienTai, VaiTroAdmin, StringComparison.OrdinalIgnoreCase); }
        }

        /// <summary>Tên tiếng Việt để hiển thị: Admin -> "Quản trị viên", còn lại -> "Nhân viên".</summary>
        public static string TenHienThiVaiTro(string vaiTro)
        {
            return string.Equals((vaiTro ?? "").Trim(), VaiTroAdmin, StringComparison.OrdinalIgnoreCase)
                ? "Quản trị viên"
                : "Nhân viên";
        }

        public static string BamMatKhau(string maNhanVien, string matKhau)
        {
            string muoi = (maNhanVien ?? "").Trim().ToLowerInvariant();
            using (SHA256 sha = SHA256.Create())
            {
                byte[] b = sha.ComputeHash(Encoding.UTF8.GetBytes(muoi + ":" + matKhau));
                StringBuilder sb = new StringBuilder();
                foreach (byte x in b) sb.Append(x.ToString("x2"));
                return sb.ToString();
            }
        }

        public static void DatMatKhau(string maNhanVien, string matKhauMoi)
        {
            DbConnection.ExecuteNonQuery(
                "UPDATE NhanVien SET MatKhau = @mk WHERE MaNhanVien = @ma",
                DbConnection.Param("@mk", BamMatKhau(maNhanVien, matKhauMoi)),
                DbConnection.Param("@ma", maNhanVien));
        }

        /// <summary>Mật khẩu nhập vào có đúng với mật khẩu đang lưu của nhân viên này không?</summary>
        public static bool KiemTraMatKhau(string maNhanVien, string matKhau)
        {
            string luu = DbConnection.ExecuteScalar<string>(
                "SELECT MatKhau FROM NhanVien WHERE MaNhanVien = @ma",
                DbConnection.Param("@ma", maNhanVien));

            if (string.IsNullOrWhiteSpace(luu)) return false;
            return string.Equals(luu.Trim(), BamMatKhau(maNhanVien, matKhau), StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>Tự đổi mật khẩu: phải đúng mật khẩu cũ mới đổi. Trả về false nếu mật khẩu cũ sai.</summary>
        public static bool DoiMatKhau(string maNhanVien, string matKhauCu, string matKhauMoi)
        {
            if (!KiemTraMatKhau(maNhanVien, matKhauCu)) return false;
            DatMatKhau(maNhanVien, matKhauMoi);
            return true;
        }

        /// <summary>Kiểm tra đăng nhập. Đúng thì trả về mã, tên và vai trò thật trong CSDL.</summary>
        public static bool DangNhap(string tenDangNhap, string matKhau,
            out string maNhanVien, out string tenNhanVien, out string vaiTro)
        {
            maNhanVien = null;
            tenNhanVien = null;
            vaiTro = VaiTroNhanVien;

            DataTable dt = DbConnection.ExecuteQuery(
                "SELECT MaNhanVien, TenNhanVien, MatKhau, VaiTro FROM NhanVien WHERE MaNhanVien = @ma",
                DbConnection.Param("@ma", tenDangNhap));

            if (dt.Rows.Count == 0) return false;

            DataRow r = dt.Rows[0];
            string ma = r["MaNhanVien"].ToString().Trim();
            string luu = r["MatKhau"] == DBNull.Value ? "" : r["MatKhau"].ToString().Trim();

            if (luu == "") return false;   // chưa có mật khẩu thì không cho vào
            if (!string.Equals(luu, BamMatKhau(ma, matKhau), StringComparison.OrdinalIgnoreCase))
                return false;

            maNhanVien = ma;
            tenNhanVien = r["TenNhanVien"].ToString().Trim();
            vaiTro = string.Equals((r["VaiTro"] == DBNull.Value ? "" : r["VaiTro"].ToString()).Trim(),
                        VaiTroAdmin, StringComparison.OrdinalIgnoreCase)
                ? VaiTroAdmin
                : VaiTroNhanVien;
            return true;
        }

        /// <summary>Hộp thoại nhỏ nhập mật khẩu mới (admin đặt cho người khác). Trả về null nếu bấm Hủy.</summary>
        public static string HoiMatKhauMoi(string tieuDe)
        {
            using (Form f = new Form())
            {
                f.Text = tieuDe;
                f.FormBorderStyle = FormBorderStyle.FixedDialog;
                f.StartPosition = FormStartPosition.CenterScreen;
                f.MaximizeBox = false;
                f.MinimizeBox = false;
                f.ShowInTaskbar = false;
                f.ClientSize = new Size(360, 130);

                Label lbl = new Label();
                lbl.Text = "Nhập mật khẩu mới:";
                lbl.AutoSize = true;
                lbl.Location = new Point(16, 16);

                TextBox txt = new TextBox();
                txt.UseSystemPasswordChar = true;
                txt.Location = new Point(16, 42);
                txt.Width = 328;

                Button ok = new Button();
                ok.Text = "Lưu";
                ok.DialogResult = DialogResult.OK;
                ok.SetBounds(168, 84, 84, 32);

                Button huy = new Button();
                huy.Text = "Hủy";
                huy.DialogResult = DialogResult.Cancel;
                huy.SetBounds(260, 84, 84, 32);

                f.Controls.AddRange(new Control[] { lbl, txt, ok, huy });
                f.AcceptButton = ok;
                f.CancelButton = huy;

                return f.ShowDialog() == DialogResult.OK ? txt.Text : null;
            }
        }
    }
}
