using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace DoAnQLTV
{
    public partial class FrmSach : Form
    {
        // Mã sách đang được chọn để sửa/xóa. null = đang ở chế độ nhập sách mới.
        private string maSachDangChon = null;

        public FrmSach()
        {
            InitializeComponent();
            AppTheme.ApplyForm(this);
            ApDungGiaoDien();

            LoadComboBox(cboTheLoai,
                "SELECT MaTheLoai, TenTheLoai FROM TheLoai ORDER BY TenTheLoai",
                "TenTheLoai", "MaTheLoai", "thể loại");

            LoadComboBox(cboTacGia,
                "SELECT MaTacGia, TenTacGia FROM TacGia ORDER BY TenTacGia",
                "TenTacGia", "MaTacGia", "tác giả");

            LoadSach();
        }

        // ================= GIAO DIỆN =================

        private void ApDungGiaoDien()
        {
            // Hai thẻ trắng có viền mảnh
            AppTheme.StyleCard(pnlThongTin);
            AppTheme.StyleCard(pnlDanhSach);

            AppTheme.StyleGrid(dgvSach);

            AppTheme.StyleTextBox(txtMaSach);
            AppTheme.StyleTextBox(txtTenSach);

            AppTheme.StyleComboBox(cboTheLoai);
            AppTheme.StyleComboBox(cboTacGia);

            AppTheme.StylePrimaryButton(btnThem);
            AppTheme.StyleButton(btnSua, AppTheme.Amber, ColorTranslator.FromHtml("#D97706"));
            AppTheme.StyleDangerButton(btnXoa);
            AppTheme.StyleSuccessButton(btnTimKiem);
            AppTheme.StyleButton(btnLamMoi,
                ColorTranslator.FromHtml("#E2E8F0"),
                ColorTranslator.FromHtml("#CBD5E1"),
                AppTheme.TextDark);   // nút phụ màu xám nhạt
        }

        // ================= NẠP DỮ LIỆU =================

        private void LoadComboBox(ComboBox cbo, string sql, string display, string value, string tenDanhSach)
        {
            try
            {
                cbo.DropDownStyle = ComboBoxStyle.DropDownList;
                cbo.DisplayMember = display;
                cbo.ValueMember = value;
                cbo.DataSource = DbConnection.ExecuteQuery(sql);
                cbo.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                AppTheme.Error("Không thể tải danh sách " + tenDanhSach + ".\n\n" + ex.Message);
            }
        }

        /// <summary>Lấy danh sách sách; tuKhoa rỗng/null = lấy tất cả.</summary>
        private DataTable TruyVanSach(string tuKhoa)
        {
            const string sql = @"
                SELECT s.MaSach, s.TenSach, tl.TenTheLoai, tg.TenTacGia
                FROM Sach s
                INNER JOIN TheLoai tl ON s.MaTheLoai = tl.MaTheLoai
                INNER JOIN TacGia  tg ON s.MaTacGia  = tg.MaTacGia
                WHERE (@TuKhoa IS NULL OR s.MaSach LIKE @TuKhoa OR s.TenSach LIKE @TuKhoa)
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

        private void HienThiLuoi(DataTable dt)
        {
            dgvSach.Rows.Clear();
            foreach (DataRow r in dt.Rows)
                dgvSach.Rows.Add(r["MaSach"], r["TenSach"], r["TenTheLoai"], r["TenTacGia"]);

            dgvSach.ClearSelection();
            dgvSach.CurrentCell = null;
        }

        // ================= HÀM DÙNG CHUNG TRÊN FORM =================

        private void DoLenForm(string maSach, string tenSach, string tenTheLoai, string tenTacGia)
        {
            txtMaSach.Text = maSach;
            txtTenSach.Text = tenSach;
            cboTheLoai.SelectedIndex = cboTheLoai.FindStringExact(tenTheLoai);
            cboTacGia.SelectedIndex = cboTacGia.FindStringExact(tenTacGia);

            maSachDangChon = maSach;
            txtMaSach.ReadOnly = true;   // Không cho đổi mã khi đang sửa (mã là khóa chính)
        }

        private void XoaForm()
        {
            txtMaSach.Clear();
            txtTenSach.Clear();
            cboTheLoai.SelectedIndex = -1;
            cboTacGia.SelectedIndex = -1;

            maSachDangChon = null;
            txtMaSach.ReadOnly = false;

            dgvSach.ClearSelection();
            dgvSach.CurrentCell = null;
            txtMaSach.Focus();
        }

        private bool CanhBao(string thongBao, Control control)
        {
            AppTheme.Warning(thongBao);
            control.Focus();
            return false;
        }

        private bool KiemTraDuLieu()
        {
            if (txtMaSach.Text.Trim() == "")
                return CanhBao("Vui lòng nhập mã sách.", txtMaSach);
            if (txtTenSach.Text.Trim() == "")
                return CanhBao("Vui lòng nhập tên sách.", txtTenSach);
            if (cboTheLoai.SelectedValue == null)
                return CanhBao("Vui lòng chọn thể loại.", cboTheLoai);
            if (cboTacGia.SelectedValue == null)
                return CanhBao("Vui lòng chọn tác giả.", cboTacGia);
            return true;
        }

        // ================= SỰ KIỆN (giữ nguyên tên để Designer không bị lỗi) =================

        private void dgvSach_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            DataGridViewRow row = dgvSach.Rows[e.RowIndex];
            DoLenForm(
                row.Cells["colMaSach"].Value?.ToString(),
                row.Cells["colTenSach"].Value?.ToString(),
                row.Cells["colTheLoai"].Value?.ToString(),
                row.Cells["colTacGia"].Value?.ToString());
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            if (maSachDangChon != null)
            {
                AppTheme.Warning("Bạn đang chọn một sách có sẵn.\nBấm \"Làm mới\" để nhập sách mới.");
                return;
            }

            if (!KiemTraDuLieu()) return;

            try
            {
                DbConnection.ExecuteNonQuery(@"
                    INSERT INTO Sach (MaSach, TenSach, MaTheLoai, MaTacGia)
                    VALUES (@MaSach, @TenSach, @MaTheLoai, @MaTacGia)",
                    DbConnection.Param("@MaSach", txtMaSach.Text.Trim()),
                    DbConnection.Param("@TenSach", txtTenSach.Text.Trim()),
                    DbConnection.Param("@MaTheLoai", cboTheLoai.SelectedValue),
                    DbConnection.Param("@MaTacGia", cboTacGia.SelectedValue));

                AppTheme.Info("Thêm sách thành công.");
                LoadSach();
                XoaForm();
            }
            catch (SqlException ex) when (ex.Number == 2627 || ex.Number == 2601)
            {
                AppTheme.Warning("Mã sách đã tồn tại. Vui lòng nhập mã khác.");
            }
            catch (Exception ex)
            {
                AppTheme.Error("Không thể thêm sách.\n\n" + ex.Message);
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (maSachDangChon == null)
            {
                AppTheme.Warning("Vui lòng chọn sách cần sửa.");
                return;
            }

            if (!KiemTraDuLieu()) return;

            try
            {
                int soDong = DbConnection.ExecuteNonQuery(@"
                    UPDATE Sach
                    SET TenSach   = @TenSach,
                        MaTheLoai = @MaTheLoai,
                        MaTacGia  = @MaTacGia
                    WHERE MaSach = @MaSach",
                    DbConnection.Param("@TenSach", txtTenSach.Text.Trim()),
                    DbConnection.Param("@MaTheLoai", cboTheLoai.SelectedValue),
                    DbConnection.Param("@MaTacGia", cboTacGia.SelectedValue),
                    DbConnection.Param("@MaSach", maSachDangChon));

                if (soDong == 0)
                {
                    AppTheme.Warning("Không tìm thấy sách cần sửa.");
                    return;
                }

                AppTheme.Info("Sửa thông tin sách thành công.");
                LoadSach();
                XoaForm();
            }
            catch (Exception ex)
            {
                AppTheme.Error("Không thể sửa sách.\n\n" + ex.Message);
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (maSachDangChon == null)
            {
                AppTheme.Warning("Vui lòng chọn sách cần xóa.");
                return;
            }

            if (!AppTheme.Confirm("Bạn có chắc chắn muốn xóa sách này không?"))
                return;

            try
            {
                int soDong = DbConnection.ExecuteNonQuery(
                    "DELETE FROM Sach WHERE MaSach = @MaSach",
                    DbConnection.Param("@MaSach", maSachDangChon));

                if (soDong == 0)
                {
                    AppTheme.Warning("Không tìm thấy sách cần xóa.");
                    return;
                }

                AppTheme.Info("Xóa sách thành công.");
                LoadSach();
                XoaForm();
            }
            catch (SqlException ex) when (ex.Number == 547)
            {
                AppTheme.Warning("Không thể xóa sách này vì sách đang được sử dụng trong phiếu mượn.");
            }
            catch (Exception ex)
            {
                AppTheme.Error("Không thể xóa sách.\n\n" + ex.Message);
            }
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            string tuKhoa = txtTenSach.Text.Trim();
            if (tuKhoa == "")
            {
                CanhBao("Vui lòng nhập từ khóa cần tìm.", txtTenSach);
                return;
            }

            try
            {
                DataTable dt = TruyVanSach(tuKhoa);
                HienThiLuoi(dt);

                if (dt.Rows.Count == 0)
                {
                    maSachDangChon = null;
                    txtMaSach.ReadOnly = false;
                    AppTheme.Info("Không tìm thấy sách phù hợp.");
                }
                else if (dt.Rows.Count == 1)
                {
                    DataRow r = dt.Rows[0];
                    DoLenForm(
                        r["MaSach"].ToString(),
                        r["TenSach"].ToString(),
                        r["TenTheLoai"].ToString(),
                        r["TenTacGia"].ToString());
                }
            }
            catch (Exception ex)
            {
                AppTheme.Error("Không thể tìm kiếm sách.\n\n" + ex.Message);
            }
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            LoadSach();
            XoaForm();
        }
    }
}