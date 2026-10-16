using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace DoAnQLTV
{
    public partial class FrmTimKiemSach : Form
    {
        public FrmTimKiemSach()
        {
            InitializeComponent();
            AppTheme.ApplyForm(this);
            ApDungGiaoDien();
            ThemCotMoi();
            LoadSach();
        }

        // ================= GIAO DIỆN =================

        private void ApDungGiaoDien()
        {
            AppTheme.StyleCard(pnlThongTin);
            AppTheme.StyleCard(pnlDanhSach);

            AppTheme.StyleGrid(dgvKetQua);
            AppTheme.StyleTextBox(txtTuKhoa);

            AppTheme.StyleSuccessButton(btnTimKiem);
            AppTheme.StyleButton(btnLamMoi,
                ColorTranslator.FromHtml("#E2E8F0"),
                ColorTranslator.FromHtml("#CBD5E1"),
                AppTheme.TextDark);

            // Nhấn Enter trong ô từ khóa = bấm Tìm kiếm
            AcceptButton = btnTimKiem;
        }

        private void ThemCot(string ten, string tieuDe, float trongSo)
        {
            if (dgvKetQua.Columns.Contains(ten)) return;

            DataGridViewTextBoxColumn col = new DataGridViewTextBoxColumn();
            col.Name = ten;
            col.HeaderText = tieuDe;
            col.FillWeight = trongSo;
            col.ReadOnly = true;
            dgvKetQua.Columns.Add(col);
        }

        /// <summary>
        /// Thêm cột bằng code (không sửa Designer) và chỉnh lại độ rộng cho vừa 9 cột:
        /// Mã | Tên | Thể loại | Tác giả | Nhà XB | Năm XB | Vị trí kệ | Còn / Đang mượn | Tình trạng
        /// </summary>
        private void ThemCotMoi()
        {
            dgvKetQua.Columns["colMaSach"].FillWeight = 12F;
            dgvKetQua.Columns["colTenSach"].FillWeight = 30F;
            dgvKetQua.Columns["colTheLoai"].FillWeight = 16F;
            dgvKetQua.Columns["colTacGia"].FillWeight = 20F;

            ThemCot("colNXB", "Nhà XB", 20F);
            ThemCot("colNam", "Năm XB", 10F);
            ThemCot("colViTri", "Vị trí kệ", 14F);
            ThemCot("colConDangMuon", "Còn / Đang mượn", 18F);
            ThemCot("colTinhTrang", "Tình trạng", 16F);
        }

        // ================= NẠP DỮ LIỆU =================

        /// <summary>Lấy danh sách sách; tuKhoa rỗng/null = lấy tất cả.</summary>
        private DataTable TruyVanSach(string tuKhoa)
        {
            const string sql = @"
                SELECT s.MaSach, s.TenSach, tl.TenTheLoai, tg.TenTacGia,
                       s.NhaXuatBan, s.NamXuatBan, s.ViTriKe,
                       s.SoLuong, " + ThuVienDb.SqlDangMuon + @" AS DangMuon
                FROM Sach s
                INNER JOIN TheLoai tl ON s.MaTheLoai = tl.MaTheLoai
                INNER JOIN TacGia tg ON s.MaTacGia = tg.MaTacGia
                WHERE (@TuKhoa IS NULL
                       OR s.MaSach LIKE @TuKhoa
                       OR s.TenSach LIKE @TuKhoa
                       OR tl.TenTheLoai LIKE @TuKhoa
                       OR tg.TenTacGia LIKE @TuKhoa
                       OR s.NhaXuatBan LIKE @TuKhoa
                       OR s.ViTriKe LIKE @TuKhoa)
                ORDER BY s.MaSach";

            object tuKhoaLike = string.IsNullOrEmpty(tuKhoa) ? null : "%" + tuKhoa + "%";
            return DbConnection.ExecuteQuery(sql, DbConnection.Param("@TuKhoa", tuKhoaLike));
        }

        private void LoadSach()
        {
            try
            {
                HienThiLuoi(TruyVanSach(null));
            }
            catch (Exception ex)
            {
                AppTheme.Error("Không thể tải dữ liệu sách.\n\n" + ex.Message);
            }
        }

        private static string Chuoi(object giaTri)
        {
            return giaTri == null || giaTri == DBNull.Value ? "" : giaTri.ToString();
        }

        private void HienThiLuoi(DataTable dt)
        {
            dgvKetQua.Rows.Clear();
            foreach (DataRow r in dt.Rows)
            {
                int soLuong = Convert.ToInt32(r["SoLuong"]);
                int dangMuon = Convert.ToInt32(r["DangMuon"]);
                int conLai = soLuong - dangMuon;

                string tinhTrang;
                Color mau;
                if (conLai <= 0)
                {
                    tinhTrang = "Hết sách";
                    mau = AppTheme.Danger;
                }
                else
                {
                    tinhTrang = "Có thể mượn";
                    mau = AppTheme.Success;
                }

                int idx = dgvKetQua.Rows.Add(
                    r["MaSach"], r["TenSach"], r["TenTheLoai"], r["TenTacGia"],
                    Chuoi(r["NhaXuatBan"]), Chuoi(r["NamXuatBan"]), Chuoi(r["ViTriKe"]),
                    Math.Max(conLai, 0) + " / " + dangMuon,
                    tinhTrang);

                DataGridViewCell o = dgvKetQua.Rows[idx].Cells["colTinhTrang"];
                o.Style.ForeColor = mau;
                o.Style.SelectionForeColor = mau;
                o.Style.Font = AppTheme.Bold;
            }

            dgvKetQua.ClearSelection();
            dgvKetQua.CurrentCell = null;
        }

        // ================= SỰ KIỆN =================

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            string tuKhoa = txtTuKhoa.Text.Trim();
            if (tuKhoa == "")
            {
                AppTheme.Warning("Vui lòng nhập từ khóa cần tìm.");
                txtTuKhoa.Focus();
                return;
            }

            try
            {
                DataTable dt = TruyVanSach(tuKhoa);
                HienThiLuoi(dt);

                if (dt.Rows.Count == 0)
                {
                    AppTheme.Info("Không tìm thấy sách phù hợp.");
                    txtTuKhoa.Focus();
                }
            }
            catch (Exception ex)
            {
                AppTheme.Error("Không thể tìm kiếm sách.\n\n" + ex.Message);
            }
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            txtTuKhoa.Clear();
            LoadSach();
            txtTuKhoa.Focus();
        }
    }
}
