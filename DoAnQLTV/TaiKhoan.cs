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
    /// Mật khẩu lưu dạng SHA-256 có "muối" riêng cho từng nhân viên (muối = mã nhân viên),
    /// nên hai nhân viên cùng mật khẩu vẫn cho ra hai chuỗi băm khác nhau.
    /// </summary>
    public static class TaiKhoan
    {
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

        /// <summary>Kiểm tra đăng nhập. Đúng thì trả về mã + tên nhân viên thật trong CSDL.</summary>
        public static bool DangNhap(string tenDangNhap, string matKhau, out string maNhanVien, out string tenNhanVien)
        {
            maNhanVien = null;
            tenNhanVien = null;

            DataTable dt = DbConnection.ExecuteQuery(
                "SELECT MaNhanVien, TenNhanVien, MatKhau FROM NhanVien WHERE MaNhanVien = @ma",
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
            return true;
        }

        /// <summary>Hộp thoại nhỏ nhập mật khẩu mới. Trả về null nếu bấm Hủy.</summary>
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
