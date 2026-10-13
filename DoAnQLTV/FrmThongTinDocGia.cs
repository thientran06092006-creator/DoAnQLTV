using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace DoAnQLTV
{
    public partial class FrmThongTinDocGia : Form
    {
        private const string DinhDangNgay = "dd/MM/yyyy";

        public FrmThongTinDocGia()
        {
            InitializeComponent();
            AppTheme.ApplyForm(this);
            ApDungGiaoDien();
            LoadDocGia();
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

        // ================= NẠP DỮ LIỆU =================

        /// <summary>Lấy danh sách độc giả; tuKhoa rỗng/null = lấy tất cả.</summary>
        private DataTable TruyVanDocGia(string tuKhoa)
        {
            const string sql = @"
                SELECT MaDocGia, TenDocGia, NgaySinh, DiaChi, SoDienThoai
                FROM DocGia
                WHERE (@TuKhoa IS NULL
                       OR MaDocGia LIKE @TuKhoa
                       OR TenDocGia LIKE @TuKhoa
                       OR SoDienThoai LIKE @TuKhoa)
                ORDER BY MaDocGia";

            object tuKhoaLike = string.IsNullOrEmpty(tuKhoa) ? null : "%" + tuKhoa + "%";
            return DbConnection.ExecuteQuery(sql, DbConnection.Param("@TuKhoa", tuKhoaLike));
        }

        private void LoadDocGia()
        {
            try
            {
                HienThiLuoi(TruyVanDocGia(null));
            }
            catch (Exception ex)
            {
                AppTheme.Error("Không thể tải dữ liệu độc giả.\n\n" + ex.Message);
            }
        }

        private void HienThiLuoi(DataTable dt)
        {
            dgvKetQua.Rows.Clear();
            foreach (DataRow r in dt.Rows)
            {
                string ngaySinh = r["NgaySinh"] == DBNull.Value
                    ? ""
                    : Convert.ToDateTime(r["NgaySinh"]).ToString(DinhDangNgay);

                dgvKetQua.Rows.Add(
                    r["MaDocGia"],
                    r["TenDocGia"],
                    ngaySinh,
                    r["DiaChi"],
                    r["SoDienThoai"]);
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
                DataTable dt = TruyVanDocGia(tuKhoa);
                HienThiLuoi(dt);

                if (dt.Rows.Count == 0)
                {
                    AppTheme.Info("Không tìm thấy độc giả phù hợp.");
                    txtTuKhoa.Focus();
                }
            }
            catch (Exception ex)
            {
                AppTheme.Error("Không thể tìm kiếm độc giả.\n\n" + ex.Message);
            }
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            txtTuKhoa.Clear();
            LoadDocGia();
            txtTuKhoa.Focus();
        }

        private void dgvKetQua_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            DataGridViewRow row = dgvKetQua.Rows[e.RowIndex];
            txtTuKhoa.Text = row.Cells["colTenDocGia"].Value?.ToString() ?? "";
        }
    }
}
