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

        // Thành phần thêm bằng code (không sửa Designer)
        private Label lblSoLuong;
        private Label lblDonVi;
        private NumericUpDown nudSoLuong;

        public FrmSach()
        {
            InitializeComponent();
            AppTheme.ApplyForm(this);
            ThemOSoLuong();
            ApDungGiaoDien();
            ThemCotSoLuong();

            LoadComboBox(cboTheLoai,
                "SELECT MaTheLoai, TenTheLoai FROM TheLoai ORDER BY TenTheLoai",
                "TenTheLoai", "MaTheLoai", "thể loại");

            LoadComboBox(cboTacGia,
                "SELECT MaTacGia, TenTacGia FROM TacGia ORDER BY TenTacGia",
                "TenTacGia", "MaTacGia", "tác giả");

            LoadSach();
        }

        // ================= GIAO DIỆN =================

        /// <summary>Chừa thêm 1 hàng cho ô "Số lượng": đẩy hàng nút và danh sách xuống 48px.</summary>
        private void ThemOSoLuong()
        {
            const int doi = 48;

            pnlThongTin.Height += doi;
            foreach (Button b in new Button[] { btnThem, btnSua, btnXoa, btnTimKiem, btnLamMoi })
                b.Top += doi;
            pnlDanhSach.Top += doi;
            ClientSize = new Size(ClientSize.Width, ClientSize.Height + doi);

            lblSoLuong = new Label();
            lblSoLuong.AutoSize = true;
            lblSoLuong.Font = lblMaSach.Font;
            lblSoLuong.ForeColor = lblMaSach.ForeColor;
            lblSoLuong.Text = "Số lượng";
            lblSoLuong.Location = new Point(24, 132);
            pnlThongTin.Controls.Add(lblSoLuong);

            nudSoLuong = new NumericUpDown();
            nudSoLuong.Minimum = 1;
            nudSoLuong.Maximum = 1000;
            nudSoLuong.Value = 1;
            nudSoLuong.Font = new Font("Segoe UI", 11F);
            nudSoLuong.SetBounds(110, 128, 110, 27);
            pnlThongTin.Controls.Add(nudSoLuong);

            lblDonVi = new Label();
            lblDonVi.AutoSize = true;
            lblDonVi.Font = AppTheme.Base;
            lblDonVi.ForeColor = AppTheme.TextMuted;
            lblDonVi.Text = "bản (cuốn) của đầu sách này";
            lblDonVi.Location = new Point(232, 132);
            pnlThongTin.Controls.Add(lblDonVi);
        }

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

        private void ThemCotSoLuong()
        {
            if (!dgvSach.Columns.Contains("colSoLuong"))
            {
                DataGridViewTextBoxColumn col = new DataGridViewTextBoxColumn();
                col.Name = "colSoLuong";
                col.HeaderText = "Số lượng";
                col.FillWeight = 40F;
                col.ReadOnly = true;
                dgvSach.Columns.Add(col);
            }

            if (!dgvSach.Columns.Contains("colConLai"))
            {
                DataGridViewTextBoxColumn col = new DataGridViewTextBoxColumn();
                col.Name = "colConLai";
                col.HeaderText = "Còn lại";
                col.FillWeight = 40F;
                col.ReadOnly = true;
                dgvSach.Columns.Add(col);
            }
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

        /// <summary>Lấy danh sách sách (kèm số lượng và số bản còn lại); tuKhoa rỗng/null = lấy tất cả.</summary>
        private DataTable TruyVanSach(string tuKhoa)
        {
            const string sql = @"
                SELECT s.MaSach, s.TenSach, tl.TenTheLoai, tg.TenTacGia,
                       s.SoLuong, " + ThuVienDb.SqlConLai + @" AS ConLai
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
            {
                int conLai = Convert.ToInt32(r["ConLai"]);

                int idx = dgvSach.Rows.Add(
                    r["MaSach"], r["TenSach"], r["TenTheLoai"], r["TenTacGia"],
                    r["SoLuong"], conLai);

                // Hết sách thì tô đỏ cột "Còn lại"
                if (conLai <= 0)
                {
                    DataGridViewCell o = dgvSach.Rows[idx].Cells["colConLai"];
                    o.Style.ForeColor = AppTheme.Danger;
                    o.Style.SelectionForeColor = AppTheme.Danger;
                    o.Style.Font = AppTheme.Bold;
                }
            }

            dgvSach.ClearSelection();
            dgvSach.CurrentCell = null;
        }

        // ================= HÀM DÙNG CHUNG TRÊN FORM =================

        private void DoLenForm(string maSach, string tenSach, string tenTheLoai, string tenTacGia, int soLuong)
        {
            txtMaSach.Text = maSach;
            txtTenSach.Text = tenSach;
            cboTheLoai.SelectedIndex = cboTheLoai.FindStringExact(tenTheLoai);
            cboTacGia.SelectedIndex = cboTacGia.FindStringExact(tenTacGia);
            nudSoLuong.Value = Math.Max(nudSoLuong.Minimum, Math.Min(nudSoLuong.Maximum, soLuong));

            maSachDangChon = maSach;
            txtMaSach.ReadOnly = true;   // Không cho đổi mã khi đang sửa (mã là khóa chính)
        }

        private void XoaForm()
        {
            txtMaSach.Clear();
            txtTenSach.Clear();
            cboTheLoai.SelectedIndex = -1;
            cboTacGia.SelectedIndex = -1;
            nudSoLuong.Value = 1;

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

        private static int DocSo(object giaTri)
        {
            int n;
            return int.TryParse(giaTri == null ? "" : giaTri.ToString(), out n) ? n : 1;
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
                row.Cells["colTacGia"].Value?.ToString(),
                DocSo(row.Cells["colSoLuong"].Value));
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
                    INSERT INTO Sach (MaSach, TenSach, MaTheLoai, MaTacGia, SoLuong)
                    VALUES (@MaSach, @TenSach, @MaTheLoai, @MaTacGia, @SoLuong)",
                    DbConnection.Param("@MaSach", txtMaSach.Text.Trim()),
                    DbConnection.Param("@TenSach", txtTenSach.Text.Trim()),
                    DbConnection.Param("@MaTheLoai", cboTheLoai.SelectedValue),
                    DbConnection.Param("@MaTacGia", cboTacGia.SelectedValue),
                    DbConnection.Param("@SoLuong", (int)nudSoLuong.Value));

                NhatKy.Ghi("Thêm sách", "Thêm sách \"" + txtTenSach.Text.Trim() + "\" - Mã sách: " +
                    txtMaSach.Text.Trim() + " (" + (int)nudSoLuong.Value + " bản)");

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
                // Không cho giảm số bản xuống thấp hơn số bản đang được mượn chưa trả
                int dangMuon = DbConnection.ExecuteScalar<int>(
                    "SELECT COUNT(*) FROM PhieuMuon WHERE MaSach = @MaSach AND NgayTra IS NULL",
                    DbConnection.Param("@MaSach", maSachDangChon));

                if ((int)nudSoLuong.Value < dangMuon)
                {
                    AppTheme.Warning("Sách này đang có " + dangMuon + " bản được mượn chưa trả.\n" +
                                     "Số lượng không được nhỏ hơn " + dangMuon + ".");
                    nudSoLuong.Focus();
                    return;
                }

                int soDong = DbConnection.ExecuteNonQuery(@"
                    UPDATE Sach
                    SET TenSach   = @TenSach,
                        MaTheLoai = @MaTheLoai,
                        MaTacGia  = @MaTacGia,
                        SoLuong   = @SoLuong
                    WHERE MaSach = @MaSach",
                    DbConnection.Param("@TenSach", txtTenSach.Text.Trim()),
                    DbConnection.Param("@MaTheLoai", cboTheLoai.SelectedValue),
                    DbConnection.Param("@MaTacGia", cboTacGia.SelectedValue),
                    DbConnection.Param("@SoLuong", (int)nudSoLuong.Value),
                    DbConnection.Param("@MaSach", maSachDangChon));

                if (soDong == 0)
                {
                    AppTheme.Warning("Không tìm thấy sách cần sửa.");
                    return;
                }

                NhatKy.Ghi("Sửa sách", "Sửa sách \"" + txtTenSach.Text.Trim() + "\" - Mã sách: " +
                    maSachDangChon + " (" + (int)nudSoLuong.Value + " bản)");

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

                NhatKy.Ghi("Xóa sách", "Xóa sách \"" + txtTenSach.Text.Trim() + "\" - Mã sách: " + maSachDangChon);

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
                        r["TenTacGia"].ToString(),
                        Convert.ToInt32(r["SoLuong"]));
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
