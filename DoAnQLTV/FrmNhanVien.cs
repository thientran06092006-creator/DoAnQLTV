using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace DoAnQLTV
{
    public partial class FrmNhanVien : Form
    {
        // Mã nhân viên đang được chọn để sửa/xóa. null = đang ở chế độ nhập mới.
        private string maDangChon = null;

        public FrmNhanVien()
        {
            InitializeComponent();
            AppTheme.ApplyForm(this);
            ApDungGiaoDien();
            LoadNhanVien();
        }

        // ================= GIAO DIỆN =================

        private void ApDungGiaoDien()
        {
            AppTheme.StyleCard(pnlThongTin);
            AppTheme.StyleCard(pnlDanhSach);

            AppTheme.StyleGrid(dgvNhanVien);

            AppTheme.StyleTextBox(txtMaNhanVien);
            AppTheme.StyleTextBox(txtTenNhanVien);
            AppTheme.StyleTextBox(txtSoDienThoai);
            AppTheme.StyleTextBox(txtDiaChi);

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

        /// <summary>Lấy danh sách nhân viên; tuKhoa rỗng/null = lấy tất cả.</summary>
        private DataTable TruyVanNhanVien(string tuKhoa)
        {
            const string sql = @"
                SELECT MaNhanVien, TenNhanVien, SoDienThoai, DiaChi
                FROM NhanVien
                WHERE (@TuKhoa IS NULL
                       OR MaNhanVien LIKE @TuKhoa
                       OR TenNhanVien LIKE @TuKhoa
                       OR SoDienThoai LIKE @TuKhoa)
                ORDER BY MaNhanVien";

            object tuKhoaLike = string.IsNullOrEmpty(tuKhoa) ? null : "%" + tuKhoa + "%";
            return DbConnection.ExecuteQuery(sql, DbConnection.Param("@TuKhoa", tuKhoaLike));
        }

        private void LoadNhanVien()
        {
            try
            {
                HienThiLuoi(TruyVanNhanVien(null));
            }
            catch (Exception ex)
            {
                AppTheme.Error("Không thể tải dữ liệu nhân viên.\n\n" + ex.Message);
            }
        }

        private void HienThiLuoi(DataTable dt)
        {
            dgvNhanVien.Rows.Clear();
            foreach (DataRow r in dt.Rows)
                dgvNhanVien.Rows.Add(r["MaNhanVien"], r["TenNhanVien"], r["SoDienThoai"], r["DiaChi"]);

            dgvNhanVien.ClearSelection();
            dgvNhanVien.CurrentCell = null;
        }

        // ================= HÀM DÙNG CHUNG TRÊN FORM =================

        private void KhoaMa(bool khoa)
        {
            txtMaNhanVien.ReadOnly = khoa;
            // Xám nhạt = đang khóa (không cho đổi mã vì là khóa chính)
            txtMaNhanVien.BackColor = khoa ? ColorTranslator.FromHtml("#F1F5F9") : Color.White;
        }

        private void DoLenForm(string maNhanVien, string tenNhanVien, string soDienThoai, string diaChi)
        {
            txtMaNhanVien.Text = maNhanVien;
            txtTenNhanVien.Text = tenNhanVien;
            txtSoDienThoai.Text = soDienThoai;
            txtDiaChi.Text = diaChi;

            maDangChon = maNhanVien;
            KhoaMa(true);
        }

        private void XoaForm()
        {
            txtMaNhanVien.Clear();
            txtTenNhanVien.Clear();
            txtSoDienThoai.Clear();
            txtDiaChi.Clear();

            maDangChon = null;
            KhoaMa(false);

            dgvNhanVien.ClearSelection();
            dgvNhanVien.CurrentCell = null;
            txtMaNhanVien.Focus();
        }

        private bool KiemTraDuLieu()
        {
            if (txtMaNhanVien.Text.Trim() == "")
            {
                AppTheme.Warning("Vui lòng nhập mã nhân viên.");
                txtMaNhanVien.Focus();
                return false;
            }

            if (txtTenNhanVien.Text.Trim() == "")
            {
                AppTheme.Warning("Vui lòng nhập tên nhân viên.");
                txtTenNhanVien.Focus();
                return false;
            }

            if (txtSoDienThoai.Text.Trim() == "")
            {
                AppTheme.Warning("Vui lòng nhập số điện thoại.");
                txtSoDienThoai.Focus();
                return false;
            }

            if (txtDiaChi.Text.Trim() == "")
            {
                AppTheme.Warning("Vui lòng nhập địa chỉ.");
                txtDiaChi.Focus();
                return false;
            }

            return true;
        }

        // ================= SỰ KIỆN (giữ nguyên tên để Designer không bị lỗi) =================

        private void dgvNhanVien_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            DataGridViewRow row = dgvNhanVien.Rows[e.RowIndex];
            DoLenForm(
                row.Cells["colMaNhanVien"].Value?.ToString(),
                row.Cells["colTenNhanVien"].Value?.ToString(),
                row.Cells["colSoDienThoai"].Value?.ToString(),
                row.Cells["colDiaChi"].Value?.ToString());
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            if (maDangChon != null)
            {
                AppTheme.Warning("Bạn đang chọn một nhân viên có sẵn.\nBấm \"Làm mới\" để nhập nhân viên mới.");
                return;
            }

            if (!KiemTraDuLieu()) return;

            try
            {
                DbConnection.ExecuteNonQuery(@"
                    INSERT INTO NhanVien (MaNhanVien, TenNhanVien, SoDienThoai, DiaChi)
                    VALUES (@MaNhanVien, @TenNhanVien, @SoDienThoai, @DiaChi)",
                    DbConnection.Param("@MaNhanVien", txtMaNhanVien.Text.Trim()),
                    DbConnection.Param("@TenNhanVien", txtTenNhanVien.Text.Trim()),
                    DbConnection.Param("@SoDienThoai", txtSoDienThoai.Text.Trim()),
                    DbConnection.Param("@DiaChi", txtDiaChi.Text.Trim()));

                AppTheme.Info("Thêm nhân viên thành công!");
                LoadNhanVien();
                XoaForm();
            }
            catch (SqlException ex) when (ex.Number == 2627 || ex.Number == 2601)
            {
                AppTheme.Warning("Mã nhân viên đã tồn tại.");
            }
            catch (Exception ex)
            {
                AppTheme.Error("Không thể thêm nhân viên.\n\n" + ex.Message);
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (maDangChon == null)
            {
                AppTheme.Warning("Vui lòng chọn nhân viên cần sửa.");
                return;
            }

            if (!KiemTraDuLieu()) return;

            try
            {
                int soDong = DbConnection.ExecuteNonQuery(@"
                    UPDATE NhanVien
                    SET TenNhanVien = @TenNhanVien,
                        SoDienThoai = @SoDienThoai,
                        DiaChi = @DiaChi
                    WHERE MaNhanVien = @MaNhanVien",
                    DbConnection.Param("@TenNhanVien", txtTenNhanVien.Text.Trim()),
                    DbConnection.Param("@SoDienThoai", txtSoDienThoai.Text.Trim()),
                    DbConnection.Param("@DiaChi", txtDiaChi.Text.Trim()),
                    DbConnection.Param("@MaNhanVien", maDangChon));

                if (soDong == 0)
                {
                    AppTheme.Warning("Không tìm thấy mã nhân viên cần sửa.");
                    return;
                }

                AppTheme.Info("Cập nhật nhân viên thành công!");
                LoadNhanVien();
                XoaForm();
            }
            catch (Exception ex)
            {
                AppTheme.Error("Không thể cập nhật nhân viên.\n\n" + ex.Message);
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (maDangChon == null)
            {
                AppTheme.Warning("Vui lòng chọn nhân viên cần xóa.");
                return;
            }

            if (!AppTheme.Confirm("Bạn có chắc chắn muốn xóa nhân viên này không?"))
                return;

            try
            {
                int soDong = DbConnection.ExecuteNonQuery(
                    "DELETE FROM NhanVien WHERE MaNhanVien = @MaNhanVien",
                    DbConnection.Param("@MaNhanVien", maDangChon));

                if (soDong == 0)
                {
                    AppTheme.Warning("Không tìm thấy nhân viên cần xóa.");
                    return;
                }

                AppTheme.Info("Xóa nhân viên thành công!");
                LoadNhanVien();
                XoaForm();
            }
            catch (SqlException ex) when (ex.Number == 547)
            {
                AppTheme.Warning("Không thể xóa nhân viên này vì đang được sử dụng trong phiếu mượn.");
            }
            catch (Exception ex)
            {
                AppTheme.Error("Không thể xóa nhân viên.\n\n" + ex.Message);
            }
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            // Tìm theo mã, tên hoặc số điện thoại; từ khóa lấy từ ô Tên nhân viên.
            string tuKhoa = txtTenNhanVien.Text.Trim();
            if (tuKhoa == "")
            {
                AppTheme.Warning("Vui lòng nhập từ khóa cần tìm.");
                txtTenNhanVien.Focus();
                return;
            }

            try
            {
                DataTable dt = TruyVanNhanVien(tuKhoa);
                HienThiLuoi(dt);

                if (dt.Rows.Count == 0)
                {
                    maDangChon = null;
                    KhoaMa(false);
                    AppTheme.Info("Không tìm thấy nhân viên phù hợp.");
                    txtTenNhanVien.Focus();
                }
                else if (dt.Rows.Count == 1)
                {
                    DataRow r = dt.Rows[0];
                    DoLenForm(
                        r["MaNhanVien"].ToString(),
                        r["TenNhanVien"].ToString(),
                        r["SoDienThoai"].ToString(),
                        r["DiaChi"].ToString());
                }
            }
            catch (Exception ex)
            {
                AppTheme.Error("Không thể tìm kiếm nhân viên.\n\n" + ex.Message);
            }
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            LoadNhanVien();
            XoaForm();
        }
    }
}
