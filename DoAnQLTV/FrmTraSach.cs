using System;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace DoAnQLTV
{
    public partial class FrmTraSach : Form
    {
        private const string DinhDangNgay = "dd/MM/yyyy";
        private const int SoNgayMuonMacDinh = 7;

        public FrmTraSach()
        {
            InitializeComponent();

            AppTheme.ApplyForm(this);
            ApDungGiaoDien();

            LoadDocGia();
            LoadSach();
            LoadTraSach(null);
            DatLaiGiaTriMacDinh();
        }

        // ================= GIAO DIỆN =================

        private void ApDungGiaoDien()
        {
            AppTheme.StyleCard(pnlThongTin);
            AppTheme.StyleCard(pnlDanhSach);

            AppTheme.StyleGrid(dgvTraSach);

            AppTheme.StyleTextBox(txtMaPhieu);
            AppTheme.StyleComboBox(cboDocGia);
            AppTheme.StyleComboBox(cboSach);

            AppTheme.StylePrimaryButton(btnTraSach);
            AppTheme.StyleSuccessButton(btnTimKiem);
            AppTheme.StyleButton(btnLamMoi,
                ColorTranslator.FromHtml("#E2E8F0"),
                ColorTranslator.FromHtml("#CBD5E1"),
                AppTheme.TextDark);

            // Các ô này chỉ để XEM thông tin phiếu đã chọn, đổi chúng không ảnh hưởng việc trả sách.
            // Khóa lại để người dùng không tưởng là sửa được.
            cboDocGia.Enabled = false;
            cboSach.Enabled = false;
            dtpNgayMuon.Enabled = false;
            dtpHanTra.Enabled = false;
        }

        // ================= NẠP DỮ LIỆU =================

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

        private void LoadSach()
        {
            try
            {
                DataTable dt = DbConnection.ExecuteQuery(
                    "SELECT MaSach, TenSach FROM Sach ORDER BY MaSach");

                cboSach.Items.Clear();
                foreach (DataRow r in dt.Rows)
                    cboSach.Items.Add(r["MaSach"] + " - " + r["TenSach"]);
            }
            catch (Exception ex)
            {
                AppTheme.Error("Không thể tải danh sách sách.\n\n" + ex.Message);
            }
        }

        /// <summary>Lấy các phiếu CHƯA TRẢ; tuKhoa rỗng/null = lấy tất cả.</summary>
        private DataTable TruyVanPhieuChuaTra(string tuKhoa)
        {
            const string sql = @"
                SELECT pm.MaPhieu,
                       pm.MaDocGia + ' - ' + dg.TenDocGia AS DocGia,
                       pm.MaSach + ' - ' + s.TenSach AS Sach,
                       pm.NgayMuon,
                       pm.HanTra,
                       pm.NgayTra
                FROM PhieuMuon pm
                INNER JOIN DocGia dg ON pm.MaDocGia = dg.MaDocGia
                INNER JOIN Sach s ON pm.MaSach = s.MaSach
                WHERE pm.NgayTra IS NULL
                  AND (@TuKhoa IS NULL OR pm.MaPhieu LIKE @TuKhoa)
                ORDER BY pm.MaPhieu";

            object tuKhoaLike = string.IsNullOrEmpty(tuKhoa) ? null : "%" + tuKhoa + "%";
            return DbConnection.ExecuteQuery(sql, DbConnection.Param("@TuKhoa", tuKhoaLike));
        }

        private void LoadTraSach(string tuKhoa)
        {
            try
            {
                HienThiLuoi(TruyVanPhieuChuaTra(tuKhoa));
            }
            catch (Exception ex)
            {
                AppTheme.Error("Không thể tải dữ liệu trả sách.\n\n" + ex.Message);
            }
        }

        private void HienThiLuoi(DataTable dt)
        {
            dgvTraSach.Rows.Clear();
            foreach (DataRow r in dt.Rows)
            {
                dgvTraSach.Rows.Add(
                    r["MaPhieu"],
                    r["DocGia"],
                    r["Sach"],
                    Convert.ToDateTime(r["NgayMuon"]).ToString(DinhDangNgay),
                    Convert.ToDateTime(r["HanTra"]).ToString(DinhDangNgay),
                    r["NgayTra"] == DBNull.Value ? "" : Convert.ToDateTime(r["NgayTra"]).ToString(DinhDangNgay));
            }

            dgvTraSach.ClearSelection();
            dgvTraSach.CurrentCell = null;
        }

        // ================= HÀM DÙNG CHUNG TRÊN FORM =================

        private void DatLaiGiaTriMacDinh()
        {
            txtMaPhieu.Clear();
            cboDocGia.SelectedIndex = -1;   // để trống cho đến khi chọn phiếu (tránh hiện nhầm độc giả đầu danh sách)
            cboSach.SelectedIndex = -1;
            dtpNgayMuon.Value = DateTime.Today;
            dtpHanTra.Value = DateTime.Today.AddDays(SoNgayMuonMacDinh);
            dtpNgayTra.Value = DateTime.Today;
        }

        private static bool DocNgay(object giaTri, out DateTime ngay)
        {
            return DateTime.TryParseExact(giaTri == null ? "" : giaTri.ToString(), DinhDangNgay,
                CultureInfo.InvariantCulture, DateTimeStyles.None, out ngay);
        }

        // ================= SỰ KIỆN (giữ nguyên tên để Designer không bị lỗi) =================

        private void dgvTraSach_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            DataGridViewRow row = dgvTraSach.Rows[e.RowIndex];

            txtMaPhieu.Text = row.Cells["colMaPhieu"].Value?.ToString() ?? "";
            cboDocGia.SelectedIndex = cboDocGia.FindStringExact(row.Cells["colDocGia"].Value?.ToString() ?? "");
            cboSach.SelectedIndex = cboSach.FindStringExact(row.Cells["colSach"].Value?.ToString() ?? "");

            DateTime ngay;
            if (DocNgay(row.Cells["colNgayMuon"].Value, out ngay))
                dtpNgayMuon.Value = ngay;

            if (DocNgay(row.Cells["colHanTra"].Value, out ngay))
                dtpHanTra.Value = ngay;

            dtpNgayTra.Value = DateTime.Today;
        }

        private void btnTraSach_Click(object sender, EventArgs e)
        {
            string maPhieu = txtMaPhieu.Text.Trim();

            if (maPhieu == "")
            {
                AppTheme.Warning("Vui lòng chọn phiếu cần trả sách.");
                txtMaPhieu.Focus();
                return;
            }

            DateTime ngayTra = dtpNgayTra.Value.Date;

            if (ngayTra > DateTime.Today)
            {
                AppTheme.Warning("Ngày trả không được sau ngày hôm nay.");
                dtpNgayTra.Focus();
                return;
            }

            try
            {
                DataTable dt = DbConnection.ExecuteQuery(@"
            SELECT pm.MaDocGia, s.TenSach, pm.NgayMuon, pm.HanTra
            FROM PhieuMuon pm
            INNER JOIN Sach s ON pm.MaSach = s.MaSach
            WHERE pm.MaPhieu = @MaPhieu AND pm.NgayTra IS NULL",
                    DbConnection.Param("@MaPhieu", maPhieu));

                if (dt.Rows.Count == 0)
                {
                    AppTheme.Warning("Không tìm thấy phiếu chưa trả có mã \"" + maPhieu + "\".\nPhiếu có thể không tồn tại hoặc đã được trả.");
                    txtMaPhieu.Focus();
                    return;
                }

                DataRow r = dt.Rows[0];
                string maDocGia = r["MaDocGia"].ToString();
                string tenSach = r["TenSach"].ToString();
                DateTime ngayMuon = Convert.ToDateTime(r["NgayMuon"]).Date;
                DateTime hanTra = Convert.ToDateTime(r["HanTra"]).Date;

                if (ngayTra < ngayMuon)
                {
                    AppTheme.Warning("Ngày trả không được trước ngày mượn (" + ngayMuon.ToString(DinhDangNgay) + ").");
                    dtpNgayTra.Focus();
                    return;
                }

                int soNgayTre = QuyDinh.SoNgayTre(hanTra, ngayTra);
                decimal tienPhat = QuyDinh.TinhTienPhat(hanTra, ngayTra);

                string cauHoi = "Xác nhận trả sách \"" + tenSach + "\" cho phiếu " + maPhieu + "?";
                if (soNgayTre > 0)
                    cauHoi += "\n\nTrả muộn " + soNgayTre + " ngày.\nTiền phạt: " + QuyDinh.DinhDangTien(tienPhat) +
                              " (" + QuyDinh.DinhDangTien(QuyDinh.MucPhatMoiNgay) + "/ngày).";
                if (!AppTheme.Confirm(cauHoi))
                    return;

                int soDong = DbConnection.ExecuteNonQuery(@"
            UPDATE PhieuMuon
            SET NgayTra = @NgayTra,
                TienPhat = @TienPhat
            WHERE MaPhieu = @MaPhieu
              AND NgayTra IS NULL",
                    DbConnection.Param("@NgayTra", ngayTra),
                    DbConnection.Param("@TienPhat", tienPhat),
                    DbConnection.Param("@MaPhieu", maPhieu));

                if (soDong == 0)
                {
                    AppTheme.Warning("Phiếu này vừa được trả ở nơi khác. Danh sách sẽ được làm mới.");
                    LoadTraSach(null);
                    return;
                }

                NhatKy.Ghi("Trả sách", "Trả sách \"" + tenSach + "\" - Mã DG: " + maDocGia +
                    (tienPhat > 0 ? " - Phạt: " + QuyDinh.DinhDangTien(tienPhat) : ""));

                if (soNgayTre > 0)
                {
                    bool daThu = AppTheme.Confirm("Trả sách thành công.\n\nTrả muộn " + soNgayTre + " ngày.\nTiền phạt: " +
                        QuyDinh.DinhDangTien(tienPhat) + "\n\nBạn đã thu tiền phạt này chưa?");

                    if (daThu)
                    {
                        DbConnection.ExecuteNonQuery(@"
                    UPDATE PhieuMuon
                    SET DaThuPhat = 1, NgayThuPhat = GETDATE()
                    WHERE MaPhieu = @MaPhieu",
                            DbConnection.Param("@MaPhieu", maPhieu));

                        NhatKy.Ghi("Thu phạt", "Thu phạt " + QuyDinh.DinhDangTien(tienPhat) +
                            " - phiếu " + maPhieu + " - Mã DG: " + maDocGia);
                    }
                    else
                    {
                        AppTheme.Info("Phiếu được ghi nhận là CHƯA THU phạt.\n" +
                                      "Bạn có thể thu sau ở mục Danh sách phiếu mượn (nút Thu phạt).");
                    }
                }
                else
                {
                    AppTheme.Info("Trả sách thành công.");
                }

                LoadTraSach(null);
                DatLaiGiaTriMacDinh();
                txtMaPhieu.Focus();
            }
            catch (Exception ex)
            {
                AppTheme.Error("Không thể cập nhật trạng thái trả sách.\n\n" + ex.Message);
            }
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            string tuKhoa = txtMaPhieu.Text.Trim();

            if (tuKhoa == "")
            {
                AppTheme.Warning("Vui lòng nhập mã phiếu cần tìm.");
                txtMaPhieu.Focus();
                return;
            }

            try
            {
                DataTable dt = TruyVanPhieuChuaTra(tuKhoa);
                HienThiLuoi(dt);

                if (dt.Rows.Count == 0)
                    AppTheme.Info("Không tìm thấy phiếu mượn chưa trả phù hợp.");
            }
            catch (Exception ex)
            {
                AppTheme.Error("Không thể tìm kiếm phiếu trả sách.\n\n" + ex.Message);
            }
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            DatLaiGiaTriMacDinh();
            LoadTraSach(null);
            txtMaPhieu.Focus();
        }
    }
}
