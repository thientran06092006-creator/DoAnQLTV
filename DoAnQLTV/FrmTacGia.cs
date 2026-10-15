using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace DoAnQLTV
{
    public partial class FrmTacGia : Form
    {
        // Mã tác giả đang được chọn để sửa/xóa. null = đang ở chế độ nhập mới.
        private string maDangChon = null;

        public FrmTacGia()
        {
            InitializeComponent();
            AppTheme.ApplyForm(this);
            ApDungGiaoDien();
            LoadTacGia();
        }

        // ================= GIAO DIỆN =================

        private void ApDungGiaoDien()
        {
            AppTheme.StyleCard(pnlThongTin);
            AppTheme.StyleCard(pnlDanhSach);

            AppTheme.StyleGrid(dgvTacGia);

            AppTheme.StyleTextBox(txtMaTacGia);
            AppTheme.StyleTextBox(txtTenTacGia);

            AppTheme.StylePrimaryButton(btnThem);
            AppTheme.StyleButton(btnSua, AppTheme.Amber, ColorTranslator.FromHtml("#D97706"));
            AppTheme.StyleDangerButton(btnXoa);
            AppTheme.StyleSuccessButton(btnTimKiem);
            AppTheme.StyleButton(btnLamMoi,
                ColorTranslator.FromHtml("#E2E8F0"),
                ColorTranslator.FromHtml("#CBD5E1"),
                AppTheme.TextDark);
        }

        // ================= NẠP DỮ LIỆU =================

        /// <summary>Lấy danh sách tác giả; tuKhoa rỗng/null = lấy tất cả.</summary>
        private DataTable TruyVanTacGia(string tuKhoa)
        {
            const string sql = @"
                SELECT MaTacGia, TenTacGia
                FROM TacGia
                WHERE (@TuKhoa IS NULL OR MaTacGia LIKE @TuKhoa OR TenTacGia LIKE @TuKhoa)
                ORDER BY MaTacGia";

            object tuKhoaLike = string.IsNullOrEmpty(tuKhoa) ? null : "%" + tuKhoa + "%";
            return DbConnection.ExecuteQuery(sql, DbConnection.Param("@TuKhoa", tuKhoaLike));
        }

        private void LoadTacGia()
        {
            try
            {
                HienThiLuoi(TruyVanTacGia(null));
            }
            catch (Exception ex)
            {
                AppTheme.Error("Không thể tải dữ liệu tác giả.\n\n" + ex.Message);
            }
        }

        private void HienThiLuoi(DataTable dt)
        {
            dgvTacGia.Rows.Clear();
            foreach (DataRow r in dt.Rows)
                dgvTacGia.Rows.Add(r["MaTacGia"], r["TenTacGia"]);

            dgvTacGia.ClearSelection();
            dgvTacGia.CurrentCell = null;
        }

        // ================= HÀM DÙNG CHUNG TRÊN FORM =================

        private void KhoaMa(bool khoa)
        {
            txtMaTacGia.ReadOnly = khoa;
            // Xám nhạt = đang khóa (không cho đổi mã vì là khóa chính)
            txtMaTacGia.BackColor = khoa ? ColorTranslator.FromHtml("#F1F5F9") : Color.White;
        }

        private void DoLenForm(string maTacGia, string tenTacGia)
        {
            txtMaTacGia.Text = maTacGia;
            txtTenTacGia.Text = tenTacGia;

            maDangChon = maTacGia;
            KhoaMa(true);
        }

        private void XoaForm()
        {
            txtMaTacGia.Clear();
            txtTenTacGia.Clear();

            maDangChon = null;
            KhoaMa(false);

            dgvTacGia.ClearSelection();
            dgvTacGia.CurrentCell = null;
            txtMaTacGia.Focus();
        }

        private bool KiemTraDuLieu()
        {
            if (txtMaTacGia.Text.Trim() == "")
            {
                AppTheme.Warning("Vui lòng nhập mã tác giả.");
                txtMaTacGia.Focus();
                return false;
            }

            if (txtTenTacGia.Text.Trim() == "")
            {
                AppTheme.Warning("Vui lòng nhập tên tác giả.");
                txtTenTacGia.Focus();
                return false;
            }

            return true;
        }

        // ================= SỰ KIỆN (giữ nguyên tên để Designer không bị lỗi) =================

        private void dgvTacGia_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            DataGridViewRow row = dgvTacGia.Rows[e.RowIndex];
            DoLenForm(
                row.Cells["colMaTacGia"].Value?.ToString(),
                row.Cells["colTenTacGia"].Value?.ToString());
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            if (maDangChon != null)
            {
                AppTheme.Warning("Bạn đang chọn một tác giả có sẵn.\nBấm \"Làm mới\" để nhập tác giả mới.");
                return;
            }

            if (!KiemTraDuLieu()) return;

            try
            {
                DbConnection.ExecuteNonQuery(@"
                    INSERT INTO TacGia (MaTacGia, TenTacGia)
                    VALUES (@MaTacGia, @TenTacGia)",
                    DbConnection.Param("@MaTacGia", txtMaTacGia.Text.Trim()),
                    DbConnection.Param("@TenTacGia", txtTenTacGia.Text.Trim()));

                NhatKy.Ghi("Thêm tác giả", "Thêm tác giả \"" + txtTenTacGia.Text.Trim() + "\" - Mã TG: " + txtMaTacGia.Text.Trim());
                AppTheme.Info("Thêm tác giả thành công!");
                LoadTacGia();
                XoaForm();
            }
            catch (SqlException ex) when (ex.Number == 2627 || ex.Number == 2601)
            {
                AppTheme.Warning("Mã tác giả đã tồn tại.");
            }
            catch (Exception ex)
            {
                AppTheme.Error("Không thể thêm tác giả.\n\n" + ex.Message);
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (maDangChon == null)
            {
                AppTheme.Warning("Vui lòng chọn tác giả cần sửa.");
                return;
            }

            if (!KiemTraDuLieu()) return;

            try
            {
                int soDong = DbConnection.ExecuteNonQuery(@"
                    UPDATE TacGia
                    SET TenTacGia = @TenTacGia
                    WHERE MaTacGia = @MaTacGia",
                    DbConnection.Param("@TenTacGia", txtTenTacGia.Text.Trim()),
                    DbConnection.Param("@MaTacGia", maDangChon));

                if (soDong == 0)
                {
                    AppTheme.Warning("Không tìm thấy mã tác giả cần sửa.");
                    return;
                }

                NhatKy.Ghi("Sửa tác giả", "Sửa tác giả \"" + txtTenTacGia.Text.Trim() + "\" - Mã TG: " + maDangChon);
                AppTheme.Info("Cập nhật tác giả thành công!");
                LoadTacGia();
                XoaForm();
            }
            catch (Exception ex)
            {
                AppTheme.Error("Không thể cập nhật tác giả.\n\n" + ex.Message);
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (maDangChon == null)
            {
                AppTheme.Warning("Vui lòng chọn tác giả cần xóa.");
                return;
            }

            if (!AppTheme.Confirm("Bạn có chắc chắn muốn xóa tác giả này không?"))
                return;

            try
            {
                int soDong = DbConnection.ExecuteNonQuery(
                    "DELETE FROM TacGia WHERE MaTacGia = @MaTacGia",
                    DbConnection.Param("@MaTacGia", maDangChon));

                if (soDong == 0)
                {
                    AppTheme.Warning("Không tìm thấy tác giả cần xóa.");
                    return;
                }

                NhatKy.Ghi("Xóa tác giả", "Xóa tác giả \"" + txtTenTacGia.Text.Trim() + "\" - Mã TG: " + maDangChon);
                AppTheme.Info("Xóa tác giả thành công!");
                LoadTacGia();
                XoaForm();
            }
            catch (SqlException ex) when (ex.Number == 547)
            {
                AppTheme.Warning("Không thể xóa tác giả này vì đang được sử dụng bởi sách.");
            }
            catch (Exception ex)
            {
                AppTheme.Error("Không thể xóa tác giả.\n\n" + ex.Message);
            }
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            string tuKhoa = txtTenTacGia.Text.Trim();
            if (tuKhoa == "")
            {
                AppTheme.Warning("Vui lòng nhập từ khóa cần tìm.");
                txtTenTacGia.Focus();
                return;
            }

            try
            {
                DataTable dt = TruyVanTacGia(tuKhoa);
                HienThiLuoi(dt);

                if (dt.Rows.Count == 0)
                {
                    maDangChon = null;
                    KhoaMa(false);
                    AppTheme.Info("Không tìm thấy tác giả phù hợp.");
                }
                else if (dt.Rows.Count == 1)
                {
                    DoLenForm(
                        dt.Rows[0]["MaTacGia"].ToString(),
                        dt.Rows[0]["TenTacGia"].ToString());
                }
            }
            catch (Exception ex)
            {
                AppTheme.Error("Không thể tìm kiếm tác giả.\n\n" + ex.Message);
            }
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            LoadTacGia();
            XoaForm();
        }
    }
}