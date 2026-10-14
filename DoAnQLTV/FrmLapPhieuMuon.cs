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

        private void LoadSach()
        {
            try
            {
                cboSach.DataSource = DbConnection.ExecuteQuery(
                    "SELECT MaSach, TenSach FROM Sach ORDER BY TenSach");
                cboSach.DisplayMember = "TenSach";
                cboSach.ValueMember = "MaSach";
                cboSach.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                AppTheme.Error("Không thể tải danh sách sách.\n\n" + ex.Message);
            }
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

        private void DatLaiGiaTriMacDinh()
        {
            txtMaPhieu.Clear();
            cboSach.SelectedIndex = -1;
            cboDocGia.SelectedIndex = cboDocGia.Items.Count > 0 ? 0 : -1;
            cboNhanVien.SelectedIndex = TimNhanVienDangNhap();
            dtpNgayMuon.Value = DateTime.Today;
            dtpHanTra.Value = DateTime.Today.AddDays(SoNgayMuonMacDinh);
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

        /// <summary>Số sách độc giả đang mượn mà chưa trả (boQuaMaPhieu: bỏ qua phiếu này khi sửa).</summary>
        private int DemSachDangMuon(string maDocGia, string boQuaMaPhieu)
        {
            return DbConnection.ExecuteScalar<int>(@"
        SELECT COUNT(*) FROM PhieuMuon
        WHERE MaDocGia = @MaDocGia AND NgayTra IS NULL
          AND (@BoQua IS NULL OR MaPhieu <> @BoQua)",
                DbConnection.Param("@MaDocGia", maDocGia),
                DbConnection.Param("@BoQua", boQuaMaPhieu));
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

        /// <summary>
        /// Sách này đang nằm trong phiếu mượn nào chưa trả? Trả về mã phiếu đó, hoặc null nếu sách đang rảnh.
        /// boQuaMaPhieu: bỏ qua phiếu này (dùng khi sửa chính phiếu đó).
        /// </summary>
        private string TimPhieuDangMuonSach(string maSach, string boQuaMaPhieu)
        {
            return DbConnection.ExecuteScalar<string>(@"
                SELECT TOP 1 MaPhieu
                FROM PhieuMuon
                WHERE MaSach = @MaSach
                  AND NgayTra IS NULL
                  AND (@BoQua IS NULL OR MaPhieu <> @BoQua)",
                DbConnection.Param("@MaSach", maSach),
                DbConnection.Param("@BoQua", boQuaMaPhieu));
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

                // Mỗi đầu sách chỉ có 1 bản: đang có người mượn chưa trả thì không cho lập phiếu mới
                int dangMuon = DemSachDangMuon(LayMa(cboDocGia.Text), null);
                if (dangMuon >= QuyDinh.SoSachToiDa)
                {
                    AppTheme.Warning("Độc giả này đang mượn " + dangMuon + " cuốn chưa trả, đã đạt giới hạn tối đa " +
                                     QuyDinh.SoSachToiDa + " cuốn.\nVui lòng trả bớt sách trước khi mượn thêm.");
                    cboDocGia.Focus();
                    return;
                }

                DbConnection.ExecuteNonQuery(@"
                    INSERT INTO PhieuMuon (MaPhieu, MaDocGia, MaNhanVien, MaSach, NgayMuon, HanTra)
                    VALUES (@MaPhieu, @MaDocGia, @MaNhanVien, @MaSach, @NgayMuon, @HanTra)",
                    DbConnection.Param("@MaPhieu", txtMaPhieu.Text.Trim()),
                    DbConnection.Param("@MaDocGia", LayMa(cboDocGia.Text)),
                    DbConnection.Param("@MaNhanVien", LayMa(cboNhanVien.Text)),
                    DbConnection.Param("@MaSach", maSach),
                    DbConnection.Param("@NgayMuon", dtpNgayMuon.Value.Date),
                    DbConnection.Param("@HanTra", dtpHanTra.Value.Date));

                NhatKy.Ghi("Mượn sách", "Mượn sách \"" + cboSach.Text + "\" - Mã DG: " + LayMa(cboDocGia.Text));

                AppTheme.Info("Lập phiếu mượn thành công!");
                LoadPhieuMuon();
                XoaForm();
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
                string maSach = cboSach.SelectedValue.ToString();

                // Chỉ kiểm tra khi phiếu này CHƯA TRẢ. Phiếu đã trả là lịch sử cũ,
                // sửa nó không được bị chặn chỉ vì sách đó đang cho người khác mượn.
                int soPhieuMo = DbConnection.ExecuteScalar<int>(
                    "SELECT COUNT(*) FROM PhieuMuon WHERE MaPhieu = @MaPhieu AND NgayTra IS NULL",
                    DbConnection.Param("@MaPhieu", maDangChon));

                if (soPhieuMo > 0)
                {
                    if (DemSachDangMuon(LayMa(cboDocGia.Text), maDangChon) >= QuyDinh.SoSachToiDa)
                    {
                        AppTheme.Warning("Độc giả được chọn đã đạt giới hạn " + QuyDinh.SoSachToiDa + " cuốn đang mượn.");
                        cboDocGia.Focus();
                        return;
                    }
                }

                int soDong = DbConnection.ExecuteNonQuery(@"
                    UPDATE PhieuMuon
                    SET MaDocGia = @MaDocGia,
                        MaNhanVien = @MaNhanVien,
                        MaSach = @MaSach,
                        NgayMuon = @NgayMuon,
                        HanTra = @HanTra
                    WHERE MaPhieu = @MaPhieu",
                    DbConnection.Param("@MaDocGia", LayMa(cboDocGia.Text)),
                    DbConnection.Param("@MaNhanVien", LayMa(cboNhanVien.Text)),
                    DbConnection.Param("@MaSach", maSach),
                    DbConnection.Param("@NgayMuon", dtpNgayMuon.Value.Date),
                    DbConnection.Param("@HanTra", dtpHanTra.Value.Date),
                    DbConnection.Param("@MaPhieu", maDangChon));

                if (soDong == 0)
                {
                    AppTheme.Warning("Không tìm thấy phiếu mượn cần sửa.");
                    return;
                }

                NhatKy.Ghi("Sửa phiếu mượn", "Sửa phiếu mượn " + maDangChon + " - Mã DG: " + LayMa(cboDocGia.Text));

                AppTheme.Info("Cập nhật phiếu mượn thành công!");
                LoadPhieuMuon();
                XoaForm();
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
            LoadPhieuMuon();
            XoaForm();
        }
    }
}
