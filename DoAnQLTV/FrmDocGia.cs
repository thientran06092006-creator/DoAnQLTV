using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace DoAnQLTV
{
    public partial class FrmDocGia : Form
    {
        private const string DinhDangNgay = "dd/MM/yyyy";

        // Mã độc giả đang được chọn để sửa/xóa. null = đang ở chế độ nhập mới.
        private string maDangChon = null;

        public FrmDocGia()
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

            AppTheme.StyleGrid(dgvDocGia);

            AppTheme.StyleTextBox(txtMaDocGia);
            AppTheme.StyleTextBox(txtTenDocGia);
            AppTheme.StyleTextBox(txtDiaChi);
            AppTheme.StyleTextBox(txtSoDienThoai);

            dtpNgaySinh.Font = new Font("Segoe UI", 11F);
            dtpNgaySinh.Format = DateTimePickerFormat.Custom;
            dtpNgaySinh.CustomFormat = DinhDangNgay;

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
            dgvDocGia.Rows.Clear();
            foreach (DataRow r in dt.Rows)
            {
                string ngaySinh = r["NgaySinh"] == DBNull.Value
                    ? ""
                    : Convert.ToDateTime(r["NgaySinh"]).ToString(DinhDangNgay);

                dgvDocGia.Rows.Add(
                    r["MaDocGia"],
                    r["TenDocGia"],
                    ngaySinh,
                    r["DiaChi"],
                    r["SoDienThoai"]);
            }

            dgvDocGia.ClearSelection();
            dgvDocGia.CurrentCell = null;
        }

        // ================= HÀM DÙNG CHUNG TRÊN FORM =================

        private void KhoaMa(bool khoa)
        {
            txtMaDocGia.ReadOnly = khoa;
            // Xám nhạt = đang khóa (không cho đổi mã vì là khóa chính)
            txtMaDocGia.BackColor = khoa ? ColorTranslator.FromHtml("#F1F5F9") : Color.White;
        }

        private void DoLenForm(string maDocGia, string tenDocGia, DateTime? ngaySinh,
                               string diaChi, string soDienThoai)
        {
            txtMaDocGia.Text = maDocGia;
            txtTenDocGia.Text = tenDocGia;
            dtpNgaySinh.Value = ngaySinh ?? DateTime.Today;
            txtDiaChi.Text = diaChi;
            txtSoDienThoai.Text = soDienThoai;

            maDangChon = maDocGia;
            KhoaMa(true);
        }

        private void XoaForm()
        {
            txtMaDocGia.Clear();
            txtTenDocGia.Clear();
            txtDiaChi.Clear();
            txtSoDienThoai.Clear();
            dtpNgaySinh.Value = DateTime.Today;

            maDangChon = null;
            KhoaMa(false);

            dgvDocGia.ClearSelection();
            dgvDocGia.CurrentCell = null;
            txtMaDocGia.Focus();
        }

        private bool KiemTraDuLieu()
        {
            if (txtMaDocGia.Text.Trim() == "")
            {
                AppTheme.Warning("Vui lòng nhập mã độc giả.");
                txtMaDocGia.Focus();
                return false;
            }

            if (txtTenDocGia.Text.Trim() == "")
            {
                AppTheme.Warning("Vui lòng nhập tên độc giả.");
                txtTenDocGia.Focus();
                return false;
            }

            if (txtDiaChi.Text.Trim() == "")
            {
                AppTheme.Warning("Vui lòng nhập địa chỉ.");
                txtDiaChi.Focus();
                return false;
            }

            if (txtSoDienThoai.Text.Trim() == "")
            {
                AppTheme.Warning("Vui lòng nhập số điện thoại.");
                txtSoDienThoai.Focus();
                return false;
            }

            return true;
        }

        private static DateTime? DocNgay(object giaTri)
        {
            if (giaTri == null || giaTri == DBNull.Value) return null;

            DateTime kq;
            if (giaTri is DateTime) return (DateTime)giaTri;
            if (DateTime.TryParseExact(giaTri.ToString(), DinhDangNgay,
                    CultureInfo.InvariantCulture, DateTimeStyles.None, out kq))
                return kq;
            return null;
        }

        // ================= SỰ KIỆN (giữ nguyên tên để Designer không bị lỗi) =================

        private void dgvDocGia_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            DataGridViewRow row = dgvDocGia.Rows[e.RowIndex];
            DoLenForm(
                row.Cells["colMaDocGia"].Value?.ToString(),
                row.Cells["colTenDocGia"].Value?.ToString(),
                DocNgay(row.Cells["colNgaySinh"].Value),
                row.Cells["colDiaChi"].Value?.ToString(),
                row.Cells["colSoDienThoai"].Value?.ToString());
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            if (maDangChon != null)
            {
                AppTheme.Warning("Bạn đang chọn một độc giả có sẵn.\nBấm \"Làm mới\" để nhập độc giả mới.");
                return;
            }

            if (!KiemTraDuLieu()) return;

            try
            {
                DbConnection.ExecuteNonQuery(@"
                    INSERT INTO DocGia (MaDocGia, TenDocGia, NgaySinh, DiaChi, SoDienThoai)
                    VALUES (@MaDocGia, @TenDocGia, @NgaySinh, @DiaChi, @SoDienThoai)",
                    DbConnection.Param("@MaDocGia", txtMaDocGia.Text.Trim()),
                    DbConnection.Param("@TenDocGia", txtTenDocGia.Text.Trim()),
                    DbConnection.Param("@NgaySinh", dtpNgaySinh.Value.Date),
                    DbConnection.Param("@DiaChi", txtDiaChi.Text.Trim()),
                    DbConnection.Param("@SoDienThoai", txtSoDienThoai.Text.Trim()));

                AppTheme.Info("Thêm độc giả thành công!");
                LoadDocGia();
                XoaForm();
            }
            catch (SqlException ex) when (ex.Number == 2627 || ex.Number == 2601)
            {
                AppTheme.Warning("Mã độc giả đã tồn tại.");
            }
            catch (Exception ex)
            {
                AppTheme.Error("Không thể thêm độc giả.\n\n" + ex.Message);
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (maDangChon == null)
            {
                AppTheme.Warning("Vui lòng chọn độc giả cần sửa.");
                return;
            }

            if (!KiemTraDuLieu()) return;

            try
            {
                int soDong = DbConnection.ExecuteNonQuery(@"
                    UPDATE DocGia
                    SET TenDocGia = @TenDocGia,
                        NgaySinh = @NgaySinh,
                        DiaChi = @DiaChi,
                        SoDienThoai = @SoDienThoai
                    WHERE MaDocGia = @MaDocGia",
                    DbConnection.Param("@TenDocGia", txtTenDocGia.Text.Trim()),
                    DbConnection.Param("@NgaySinh", dtpNgaySinh.Value.Date),
                    DbConnection.Param("@DiaChi", txtDiaChi.Text.Trim()),
                    DbConnection.Param("@SoDienThoai", txtSoDienThoai.Text.Trim()),
                    DbConnection.Param("@MaDocGia", maDangChon));

                if (soDong == 0)
                {
                    AppTheme.Warning("Không tìm thấy mã độc giả cần sửa.");
                    return;
                }

                AppTheme.Info("Cập nhật độc giả thành công!");
                LoadDocGia();
                XoaForm();
            }
            catch (Exception ex)
            {
                AppTheme.Error("Không thể cập nhật độc giả.\n\n" + ex.Message);
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (maDangChon == null)
            {
                AppTheme.Warning("Vui lòng chọn độc giả cần xóa.");
                return;
            }

            if (!AppTheme.Confirm("Bạn có chắc chắn muốn xóa độc giả này không?"))
                return;

            try
            {
                int soDong = DbConnection.ExecuteNonQuery(
                    "DELETE FROM DocGia WHERE MaDocGia = @MaDocGia",
                    DbConnection.Param("@MaDocGia", maDangChon));

                if (soDong == 0)
                {
                    AppTheme.Warning("Không tìm thấy độc giả cần xóa.");
                    return;
                }

                AppTheme.Info("Xóa độc giả thành công!");
                LoadDocGia();
                XoaForm();
            }
            catch (SqlException ex) when (ex.Number == 547)
            {
                AppTheme.Warning("Không thể xóa độc giả này vì đang được sử dụng trong phiếu mượn.");
            }
            catch (Exception ex)
            {
                AppTheme.Error("Không thể xóa độc giả.\n\n" + ex.Message);
            }
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            // Tìm theo mã, tên hoặc số điện thoại; từ khóa lấy từ ô Tên độc giả.
            string tuKhoa = txtTenDocGia.Text.Trim();
            if (tuKhoa == "")
            {
                AppTheme.Warning("Vui lòng nhập từ khóa cần tìm.");
                txtTenDocGia.Focus();
                return;
            }

            try
            {
                DataTable dt = TruyVanDocGia(tuKhoa);
                HienThiLuoi(dt);

                if (dt.Rows.Count == 0)
                {
                    maDangChon = null;
                    KhoaMa(false);
                    AppTheme.Info("Không tìm thấy độc giả phù hợp.");
                    txtTenDocGia.Focus();
                }
                else if (dt.Rows.Count == 1)
                {
                    DataRow r = dt.Rows[0];
                    DoLenForm(
                        r["MaDocGia"].ToString(),
                        r["TenDocGia"].ToString(),
                        DocNgay(r["NgaySinh"]),
                        r["DiaChi"].ToString(),
                        r["SoDienThoai"].ToString());
                }
            }
            catch (Exception ex)
            {
                AppTheme.Error("Không thể tìm kiếm độc giả.\n\n" + ex.Message);
            }
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            LoadDocGia();
            XoaForm();
        }
    }
}
