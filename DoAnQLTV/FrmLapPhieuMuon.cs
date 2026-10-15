using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace DoAnQLTV
{
    public partial class FrmLapPhieuMuon : Form
    {
        private const string DinhDangNgay = "dd/MM/yyyy";
        private const int SoNgayMuonMacDinh = 7;

        // Mã phiếu đang được chọn để sửa/xóa. null = đang ở chế độ lập phiếu mới.
        private string maDangChon = null;

        public FrmLapPhieuMuon()
        {
            InitializeComponent();
            AppTheme.ApplyForm(this);
            ApDungGiaoDien();

            LoadSach();
            LoadDocGia();
            LoadNhanVien();
            LoadPhieuMuon();
            DatLaiGiaTriMacDinh();
        }

        // ================= GIAO DIỆN =================

        private void ApDungGiaoDien()
        {
            AppTheme.StyleCard(pnlThongTin);
            AppTheme.StyleCard(pnlDanhSach);

            AppTheme.StyleGrid(dgvPhieuMuon);

            AppTheme.StyleTextBox(txtMaPhieu);
            AppTheme.StyleComboBox(cboDocGia);
            AppTheme.StyleComboBox(cboNhanVien);
            AppTheme.StyleComboBox(cboSach);

            foreach (DateTimePicker dtp in new[] { dtpNgayMuon, dtpHanTra })
            {
                dtp.Font = new Font("Segoe UI", 11F);
                dtp.Format = DateTimePickerFormat.Custom;
                dtp.CustomFormat = DinhDangNgay;
            }

            AppTheme.StylePrimaryButton(btnThem);
            AppTheme.StyleButton(btnSua, AppTheme.Amber, ColorTranslator.FromHtml("#D97706"));
            AppTheme.StyleDangerButton(btnXoa);
            AppTheme.StyleSuccessButton(btnLapPhieu);
            AppTheme.StyleButton(btnLamMoi,
                ColorTranslator.FromHtml("#E2E8F0"),
                ColorTranslator.FromHtml("#CBD5E1"),
                AppTheme.TextDark);
        }

        // ================= NẠP DỮ LIỆU =================

        /// <summary>
        /// Nạp danh sách sách. Ô chọn hiện kèm số bản còn lại, ví dụ "Lập trình C# (còn 2/3)".
        /// Cột TenSach giữ tên gốc để dùng trong thông báo / nhật ký.
        /// </summary>
        private void LoadSach()
        {
            try
            {
                DataTable dt = DbConnection.ExecuteQuery(
                    "SELECT s.MaSach, s.TenSach, " +
                    "       s.TenSach + CASE WHEN " + ThuVienDb.SqlConLai + " > 0 " +
                    "            THEN N' (còn ' + CAST(" + ThuVienDb.SqlConLai + " AS NVARCHAR(10)) + N'/' + CAST(s.SoLuong AS NVARCHAR(10)) + N')' " +
                    "            ELSE N' (hết sách)' END AS HienThi " +
                    "FROM Sach s ORDER BY s.TenSach");

                cboSach.DisplayMember = "HienThi";
                cboSach.ValueMember = "MaSach";
                cboSach.DataSource = dt;
                cboSach.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                AppTheme.Error("Không thể tải danh sách sách.\n\n" + ex.Message);
            }
        }

        /// <summary>Tên sách gốc (không kèm "(còn x/y)") của sách đang chọn.</summary>
        private string TenSachDangChon()
        {
            DataRowView drv = cboSach.SelectedItem as DataRowView;
            return drv == null ? "" : drv["TenSach"].ToString();
        }

        private void LoadDocGia()
        {
            try
            {
                DataTable dt = DbConnection.ExecuteQuery(
                    "SELECT MaDocGia, TenDocGia FROM DocGia ORDER BY MaDocGia");

                cboDocGia.Items.Clear();
                foreach (DataRow r in dt.Rows)
                    cboDocGia.Items.Add(r["MaDocGia"] + " - " + r["TenDocGia"]);
            }
            catch (Exception ex)
            {
                AppTheme.Error("Không thể tải danh sách độc giả.\n\n" + ex.Message);
            }
        }

        private void LoadNhanVien()
        {
            try
            {
                DataTable dt = DbConnection.ExecuteQuery(
                    "SELECT MaNhanVien, TenNhanVien FROM NhanVien ORDER BY MaNhanVien");

                cboNhanVien.Items.Clear();
                foreach (DataRow r in dt.Rows)
                    cboNhanVien.Items.Add(r["MaNhanVien"] + " - " + r["TenNhanVien"]);
            }
            catch (Exception ex)
            {
                AppTheme.Error("Không thể tải danh sách nhân viên.\n\n" + ex.Message);
            }
        }

        private void LoadPhieuMuon()
        {
            try
            {
                DataTable dt = DbConnection.ExecuteQuery(@"
                    SELECT pm.MaPhieu,
                           pm.MaDocGia + ' - ' + dg.TenDocGia AS DocGia,
                           pm.MaNhanVien + ' - ' + nv.TenNhanVien AS NhanVien,
                           pm.NgayMuon,
                           pm.HanTra,
                           s.TenSach
                    FROM PhieuMuon pm
                    INNER JOIN DocGia dg ON pm.MaDocGia = dg.MaDocGia
                    INNER JOIN NhanVien nv ON pm.MaNhanVien = nv.MaNhanVien
                    INNER JOIN Sach s ON pm.MaSach = s.MaSach
                    ORDER BY pm.MaPhieu");

                dgvPhieuMuon.Rows.Clear();
                foreach (DataRow r in dt.Rows)
                {
                    dgvPhieuMuon.Rows.Add(
                        r["MaPhieu"],
                        r["DocGia"],
                        r["NhanVien"],
                        Convert.ToDateTime(r["NgayMuon"]).ToString(DinhDangNgay),
                        Convert.ToDateTime(r["HanTra"]).ToString(DinhDangNgay),
                        r["TenSach"]);
                }

                dgvPhieuMuon.ClearSelection();
                dgvPhieuMuon.CurrentCell = null;
            }
            catch (Exception ex)
            {
                AppTheme.Error("Không thể tải dữ liệu phiếu mượn.\n\n" + ex.Message);
            }
        }

        // ================= HÀM DÙNG CHUNG TRÊN FORM =================

        private void KhoaMa(bool khoa)
        {
            txtMaPhieu.ReadOnly = khoa;
            // Xám nhạt = đang khóa (không cho đổi mã vì là khóa chính)
            txtMaPhieu.BackColor = khoa ? ColorTranslator.FromHtml("#F1F5F9") : Color.White;
        }

        /// <summary>Mặc định chọn nhân viên đang đăng nhập.</summary>
        private int TimNhanVienDangNhap()
        {
            for (int i = 0; i < cboNhanVien.Items.Count; i++)
                if (string.Equals(LayMa(cboNhanVien.Items[i].ToString()), NhatKy.MaNhanVien,
                        StringComparison.OrdinalIgnoreCase))
                    return i;
            return cboNhanVien.Items.Count > 0 ? 0 : -1;
        }

        private void DatLaiGiaTriMacDinh()
        {
            txtMaPhieu.Clear();
            cboSach.SelectedIndex = -1;
            cboDocGia.SelectedIndex = cboDocGia.Items.Count > 0 ? 0 : -1;
            cboNhanVien.SelectedIndex = TimNhanVienDangNhap();
            dtpNgayMuon.Value = DateTime.Today;
            dtpHanTra.Value = DateTime.Today.AddDays(SoNgayMuonMacDinh);
        }

        private void XoaForm()
        {
            DatLaiGiaTriMacDinh();

            maDangChon = null;
            KhoaMa(false);

            dgvPhieuMuon.ClearSelection();
            dgvPhieuMuon.CurrentCell = null;
            txtMaPhieu.Focus();
        }

        /// <summary>Lấy mã từ chuỗi dạng "MA001 - Nguyễn Văn A".</summary>
        private static string LayMa(string chuoi)
        {
            int viTri = chuoi.IndexOf(" - ", StringComparison.Ordinal);
            return (viTri >= 0 ? chuoi.Substring(0, viTri) : chuoi).Trim();
        }

        private static void ChonTheoChuoi(ComboBox cbo, string chuoi)
        {
            cbo.SelectedIndex = cbo.FindStringExact(chuoi ?? "");
        }

        private bool KiemTraDuLieu()
        {
            if (txtMaPhieu.Text.Trim() == "")
            {
                AppTheme.Warning("Vui lòng nhập mã phiếu.");
                txtMaPhieu.Focus();
                return false;
            }

            if (cboDocGia.SelectedIndex == -1)
            {
                AppTheme.Warning("Vui lòng chọn độc giả.");
                cboDocGia.Focus();
                return false;
            }

            if (cboNhanVien.SelectedIndex == -1)
            {
                AppTheme.Warning("Vui lòng chọn nhân viên.");
                cboNhanVien.Focus();
                return false;
            }

            if (cboSach.SelectedIndex == -1 || cboSach.SelectedValue == null)
            {
                AppTheme.Warning("Vui lòng chọn sách.");
                cboSach.Focus();
                return false;
            }

            if (dtpHanTra.Value.Date < dtpNgayMuon.Value.Date)
            {
                AppTheme.Warning("Hạn trả không được trước ngày mượn.");
                dtpHanTra.Focus();
                return false;
            }

            return true;
        }

        /// <summary>Đưa con trỏ vào ô liên quan đến lỗi nghiệp vụ.</summary>
        private void FocusTheoLoi(LoiNghiepVu ex)
        {
            if (ex.Truong == "sach") cboSach.Focus();
            else if (ex.Truong == "docgia") cboDocGia.Focus();
        }

        // ================= SỰ KIỆN (giữ nguyên tên để Designer không bị lỗi) =================

        private void dgvPhieuMuon_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            DataGridViewRow row = dgvPhieuMuon.Rows[e.RowIndex];
            string maPhieu = row.Cells["colMaPhieu"].Value?.ToString() ?? "";

            txtMaPhieu.Text = maPhieu;
            ChonTheoChuoi(cboDocGia, row.Cells["colDocGia"].Value?.ToString());
            ChonTheoChuoi(cboNhanVien, row.Cells["colNhanVien"].Value?.ToString());

            DateTime ngay;
            if (DateTime.TryParseExact(row.Cells["colNgayMuon"].Value?.ToString(), DinhDangNgay,
                    CultureInfo.InvariantCulture, DateTimeStyles.None, out ngay))
                dtpNgayMuon.Value = ngay;

            if (DateTime.TryParseExact(row.Cells["colHanTra"].Value?.ToString(), DinhDangNgay,
                    CultureInfo.InvariantCulture, DateTimeStyles.None, out ngay))
                dtpHanTra.Value = ngay;

            try
            {
                // Lấy mã sách của phiếu (bảng chỉ hiện tên sách)
                string maSach = DbConnection.ExecuteScalar<string>(
                    "SELECT MaSach FROM PhieuMuon WHERE MaPhieu = @MaPhieu",
                    DbConnection.Param("@MaPhieu", maPhieu));

                if (maSach != null)
                    cboSach.SelectedValue = maSach;
                else
                    cboSach.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                AppTheme.Error("Không thể tải thông tin sách của phiếu.\n\n" + ex.Message);
            }

            maDangChon = maPhieu;
            KhoaMa(true);
        }

        /// <summary>Dùng chung cho nút Thêm và nút Lập phiếu (hai nút làm cùng một việc).</summary>
        private void LapPhieuMoi()
        {
            if (maDangChon != null)
            {
                AppTheme.Warning("Bạn đang chọn một phiếu có sẵn.\nBấm \"Làm mới\" để lập phiếu mới.");
                return;
            }

            if (!KiemTraDuLieu()) return;

            try
            {
                string maSach = cboSach.SelectedValue.ToString();
                string tenSach = TenSachDangChon();   // lấy trước khi nạp lại danh sách
                string maDocGia = LayMa(cboDocGia.Text);

                // Kiểm tra số bản còn lại, quá hạn, giới hạn mượn và ghi phiếu trong 1 transaction
                ThuVienDb.LapPhieu(
                    txtMaPhieu.Text.Trim(),
                    maDocGia,
                    LayMa(cboNhanVien.Text),
                    maSach,
                    dtpNgayMuon.Value.Date,
                    dtpHanTra.Value.Date);

                NhatKy.Ghi("Mượn sách", "Mượn sách \"" + tenSach + "\" - Mã DG: " + maDocGia);

                AppTheme.Info("Lập phiếu mượn thành công!");
                LoadSach();          // số bản còn lại đã thay đổi
                LoadPhieuMuon();
                XoaForm();
            }
            catch (LoiNghiepVu ex)
            {
                AppTheme.Warning(ex.Message);
                FocusTheoLoi(ex);
            }
            catch (SqlException ex) when (ex.Number == 2627 || ex.Number == 2601)
            {
                AppTheme.Warning("Mã phiếu đã tồn tại. Vui lòng nhập mã khác.");
                txtMaPhieu.Focus();
            }
            catch (SqlException ex) when (ex.Number == 547)
            {
                AppTheme.Warning("Độc giả, nhân viên hoặc sách không hợp lệ.");
            }
            catch (Exception ex)
            {
                AppTheme.Error("Không thể lập phiếu mượn.\n\n" + ex.Message);
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            LapPhieuMoi();
        }

        private void btnLapPhieu_Click(object sender, EventArgs e)
        {
            LapPhieuMoi();
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (maDangChon == null)
            {
                AppTheme.Warning("Vui lòng chọn phiếu mượn cần sửa.");
                return;
            }

            if (!KiemTraDuLieu()) return;

            try
            {
                // Phiếu chưa trả: kiểm tra số bản còn lại + giới hạn mượn (bỏ qua chính phiếu này).
                // Phiếu đã trả là lịch sử cũ, sửa không bị chặn.
                int soDong = ThuVienDb.SuaPhieu(
                    maDangChon,
                    LayMa(cboDocGia.Text),
                    LayMa(cboNhanVien.Text),
                    cboSach.SelectedValue.ToString(),
                    dtpNgayMuon.Value.Date,
                    dtpHanTra.Value.Date);

                if (soDong == 0)
                {
                    AppTheme.Warning("Không tìm thấy phiếu mượn cần sửa.");
                    return;
                }

                NhatKy.Ghi("Sửa phiếu mượn", "Sửa phiếu mượn " + maDangChon + " - Mã DG: " + LayMa(cboDocGia.Text));

                AppTheme.Info("Cập nhật phiếu mượn thành công!");
                LoadSach();
                LoadPhieuMuon();
                XoaForm();
            }
            catch (LoiNghiepVu ex)
            {
                AppTheme.Warning(ex.Message);
                FocusTheoLoi(ex);
            }
            catch (SqlException ex) when (ex.Number == 547)
            {
                AppTheme.Warning("Độc giả, nhân viên hoặc sách không hợp lệ.");
            }
            catch (Exception ex)
            {
                AppTheme.Error("Không thể cập nhật phiếu mượn.\n\n" + ex.Message);
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (maDangChon == null)
            {
                AppTheme.Warning("Vui lòng chọn phiếu mượn cần xóa.");
                return;
            }

            if (!AppTheme.Confirm("Bạn có chắc chắn muốn xóa phiếu mượn này không?"))
                return;

            try
            {
                int soDong = DbConnection.ExecuteNonQuery(
                    "DELETE FROM PhieuMuon WHERE MaPhieu = @MaPhieu",
                    DbConnection.Param("@MaPhieu", maDangChon));

                if (soDong == 0)
                {
                    AppTheme.Warning("Không tìm thấy phiếu mượn cần xóa.");
                    return;
                }

                NhatKy.Ghi("Xóa phiếu mượn", "Xóa phiếu mượn " + maDangChon + " - Mã DG: " + LayMa(cboDocGia.Text));

                AppTheme.Info("Xóa phiếu mượn thành công!");
                LoadSach();          // xóa phiếu chưa trả sẽ trả lại 1 bản
                LoadPhieuMuon();
                XoaForm();
            }
            catch (SqlException ex) when (ex.Number == 547)
            {
                AppTheme.Warning("Không thể xóa phiếu mượn vì dữ liệu đang được tham chiếu.");
            }
            catch (Exception ex)
            {
                AppTheme.Error("Không thể xóa phiếu mượn.\n\n" + ex.Message);
            }
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            LoadSach();
            LoadPhieuMuon();
            XoaForm();
        }
    }
}
