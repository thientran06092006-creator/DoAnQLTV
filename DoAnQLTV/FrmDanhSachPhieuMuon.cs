using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace DoAnQLTV
{
    public partial class FrmDanhSachPhieuMuon : Form
    {
        private const string DinhDangNgay = "dd/MM/yyyy";

        public FrmDanhSachPhieuMuon()
        {
            InitializeComponent();

            AppTheme.ApplyForm(this);
            ApDungGiaoDien();
            ThemCotTrangThai();

            LoadPhieuMuon(null);
        }

        // ================= GIAO DIỆN =================

        private void ApDungGiaoDien()
        {
            AppTheme.StyleCard(pnlThongTin);
            AppTheme.StyleCard(pnlDanhSach);

            AppTheme.StyleGrid(dgvPhieuMuon);

            AppTheme.StyleTextBox(txtTuKhoa);

            AppTheme.StyleSuccessButton(btnTimKiem);
            AppTheme.StyleButton(btnLamMoi,
                ColorTranslator.FromHtml("#E2E8F0"),
                ColorTranslator.FromHtml("#CBD5E1"),
                AppTheme.TextDark);
        }

        /// <summary>Thêm cột "Trạng thái" bằng code, không cần sửa Designer.</summary>
        private void ThemCotTrangThai()
        {
            if (dgvPhieuMuon.Columns.Contains("colTrangThai")) return;

            DataGridViewTextBoxColumn col = new DataGridViewTextBoxColumn();
            col.Name = "colTrangThai";
            col.HeaderText = "Trạng thái";
            col.FillWeight = 80F;
            col.ReadOnly = true;
            dgvPhieuMuon.Columns.Add(col);
        }

        // ================= NẠP DỮ LIỆU =================

        /// <summary>Lấy danh sách phiếu mượn; tuKhoa rỗng/null = lấy tất cả.</summary>
        private DataTable TruyVanPhieuMuon(string tuKhoa)
        {
            const string sql = @"
                SELECT pm.MaPhieu,
                       pm.MaDocGia + ' - ' + dg.TenDocGia AS DocGia,
                       pm.MaNhanVien + ' - ' + nv.TenNhanVien AS NhanVien,
                       pm.NgayMuon,
                       pm.HanTra,
                       s.MaSach + ' - ' + s.TenSach AS Sach,
                       pm.NgayTra
                FROM PhieuMuon pm
                INNER JOIN DocGia dg ON pm.MaDocGia = dg.MaDocGia
                INNER JOIN NhanVien nv ON pm.MaNhanVien = nv.MaNhanVien
                INNER JOIN Sach s ON pm.MaSach = s.MaSach
                WHERE (@TuKhoa IS NULL
                       OR pm.MaPhieu LIKE @TuKhoa
                       OR pm.MaDocGia LIKE @TuKhoa
                       OR dg.TenDocGia LIKE @TuKhoa
                       OR nv.TenNhanVien LIKE @TuKhoa
                       OR s.MaSach LIKE @TuKhoa
                       OR s.TenSach LIKE @TuKhoa)
                ORDER BY pm.MaPhieu";

            object tuKhoaLike = string.IsNullOrEmpty(tuKhoa) ? null : "%" + tuKhoa + "%";
            return DbConnection.ExecuteQuery(sql, DbConnection.Param("@TuKhoa", tuKhoaLike));
        }

        private void LoadPhieuMuon(string tuKhoa)
        {
            try
            {
                HienThiLuoi(TruyVanPhieuMuon(tuKhoa));
            }
            catch (Exception ex)
            {
                AppTheme.Error("Không thể tải dữ liệu phiếu mượn.\n\n" + ex.Message);
            }
        }

        private void HienThiLuoi(DataTable dt)
        {
            dgvPhieuMuon.Rows.Clear();

            foreach (DataRow r in dt.Rows)
            {
                DateTime hanTra = Convert.ToDateTime(r["HanTra"]).Date;

                string trangThai;
                Color mau;
                if (r["NgayTra"] != DBNull.Value)
                {
                    trangThai = "Đã trả";
                    mau = AppTheme.Success;
                }
                else if (hanTra < DateTime.Today)
                {
                    trangThai = "Quá hạn " + (DateTime.Today - hanTra).Days + " ngày";
                    mau = AppTheme.Danger;
                }
                else
                {
                    trangThai = "Đang mượn";
                    mau = AppTheme.Accent;
                }

                int idx = dgvPhieuMuon.Rows.Add(
                    r["MaPhieu"],
                    r["DocGia"],
                    r["NhanVien"],
                    Convert.ToDateTime(r["NgayMuon"]).ToString(DinhDangNgay),
                    hanTra.ToString(DinhDangNgay),
                    r["Sach"],
                    trangThai);

                DataGridViewCell o = dgvPhieuMuon.Rows[idx].Cells["colTrangThai"];
                o.Style.ForeColor = mau;
                o.Style.SelectionForeColor = mau;
                o.Style.Font = AppTheme.Bold;
            }

            dgvPhieuMuon.ClearSelection();
            dgvPhieuMuon.CurrentCell = null;
        }

        // ================= SỰ KIỆN (giữ nguyên tên để Designer không bị lỗi) =================

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
                DataTable dt = TruyVanPhieuMuon(tuKhoa);
                HienThiLuoi(dt);

                if (dt.Rows.Count == 0)
                    AppTheme.Info("Không tìm thấy phiếu mượn phù hợp.");
            }
            catch (Exception ex)
            {
                AppTheme.Error("Không thể tìm kiếm phiếu mượn.\n\n" + ex.Message);
            }
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            txtTuKhoa.Clear();
            LoadPhieuMuon(null);
            txtTuKhoa.Focus();
        }

        private void dgvPhieuMuon_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            DataGridViewRow row = dgvPhieuMuon.Rows[e.RowIndex];
            txtTuKhoa.Text = row.Cells["colMaPhieu"].Value?.ToString() ?? "";
        }
    }
}
