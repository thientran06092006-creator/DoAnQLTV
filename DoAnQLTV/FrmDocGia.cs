using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace DoAnQLTV
{
    public partial class FrmDocGia : Form
    {
        private const string DinhDangNgay = "dd/MM/yyyy";

        // Mã độc giả đang được chọn để sửa/xóa. null = đang ở chế độ nhập mới.
        private string maDangChon = null;

        // Thành phần thêm bằng code (không sửa Designer)
        private Label lblHetHan;
        private Label lblTrangThaiThe;
        private DateTimePicker dtpHetHan;
        private Button btnGiaHanThe;

        public FrmDocGia()
        {
            InitializeComponent();

            txtMaDocGia.MaxLength = 20;
            txtTenDocGia.MaxLength = 100;
            txtDiaChi.MaxLength = 200;
            txtSoDienThoai.MaxLength = 15;

            AppTheme.ApplyForm(this);
            ThemThanhPhanMoi();
            ApDungGiaoDien();
            ThemCotHetHan();
            LoadDocGia();
            DatGiaTriMoi();
        }

        // ================= GIAO DIỆN =================

        /// <summary>Chừa thêm 1 hàng cho "Hết hạn thẻ", thêm nút "Gia hạn thẻ".</summary>
        private void ThemThanhPhanMoi()
        {
            const int doi = 48;

            pnlThongTin.Height += doi;
            foreach (Button b in new Button[] { btnThem, btnSua, btnXoa, btnTimKiem, btnLamMoi })
                b.Top += doi;
            pnlDanhSach.Top += doi;
            pnlDanhSach.Height -= doi;   // thu bớt để form không cao thêm

            lblHetHan = new Label();
            lblHetHan.AutoSize = true;
            lblHetHan.Font = lblDiaChi.Font;
            lblHetHan.ForeColor = lblDiaChi.ForeColor;
            lblHetHan.Text = "Hết hạn thẻ";
            lblHetHan.Location = new Point(24, 172);
            pnlThongTin.Controls.Add(lblHetHan);

            dtpHetHan = new DateTimePicker();
            dtpHetHan.Format = DateTimePickerFormat.Custom;
            dtpHetHan.CustomFormat = DinhDangNgay;
            dtpHetHan.Font = new Font("Segoe UI", 11F);
            dtpHetHan.SetBounds(130, 168, 300, 27);
            dtpHetHan.ValueChanged += (s, e) => CapNhatTrangThaiThe();
            pnlThongTin.Controls.Add(dtpHetHan);

            lblTrangThaiThe = new Label();
            lblTrangThaiThe.AutoSize = true;
            lblTrangThaiThe.Font = AppTheme.Bold;
            lblTrangThaiThe.Location = new Point(448, 172);
            pnlThongTin.Controls.Add(lblTrangThaiThe);

            btnGiaHanThe = new Button();
            btnGiaHanThe.Name = "btnGiaHanThe";
            btnGiaHanThe.Text = "Gia hạn thẻ";
            btnGiaHanThe.SetBounds(684, 178 + doi, 120, 40);
            btnGiaHanThe.Click += btnGiaHanThe_Click;
            pnlThongTin.Controls.Add(btnGiaHanThe);
        }

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
            AppTheme.StyleOutlineButton(btnGiaHanThe);
        }

        private void ThemCotHetHan()
        {
            if (dgvDocGia.Columns.Contains("colHetHan")) return;

            DataGridViewTextBoxColumn col = new DataGridViewTextBoxColumn();
            col.Name = "colHetHan";
            col.HeaderText = "Hết hạn thẻ";
            col.FillWeight = 32F;
            col.ReadOnly = true;
            dgvDocGia.Columns.Add(col);
        }

        private void CapNhatTrangThaiThe()
        {
            DateTime het = dtpHetHan.Value.Date;
            if (het < DateTime.Today)
            {
                lblTrangThaiThe.Text = "Thẻ ĐÃ HẾT HẠN";
                lblTrangThaiThe.ForeColor = AppTheme.Danger;
            }
            else
            {
                lblTrangThaiThe.Text = "Thẻ còn hạn (" + (het - DateTime.Today).Days + " ngày)";
                lblTrangThaiThe.ForeColor = AppTheme.Success;
            }
        }

        // ================= NẠP DỮ LIỆU =================

        /// <summary>Lấy danh sách độc giả; tuKhoa rỗng/null = lấy tất cả.</summary>
        private DataTable TruyVanDocGia(string tuKhoa)
        {
            const string sql = @"
                SELECT MaDocGia, TenDocGia, NgaySinh, DiaChi, SoDienThoai, NgayHetHanThe
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

                bool hetHan = false;
                string hetHanText = "";
                if (r["NgayHetHanThe"] != DBNull.Value)
                {
                    DateTime het = Convert.ToDateTime(r["NgayHetHanThe"]).Date;
                    hetHan = het < DateTime.Today;
                    hetHanText = het.ToString(DinhDangNgay) + (hetHan ? " (hết hạn)" : "");
                }

                int idx = dgvDocGia.Rows.Add(
                    r["MaDocGia"],
                    r["TenDocGia"],
                    ngaySinh,
                    r["DiaChi"],
                    r["SoDienThoai"],
                    hetHanText);

                if (hetHan)
                {
                    DataGridViewCell o = dgvDocGia.Rows[idx].Cells["colHetHan"];
                    o.Style.ForeColor = AppTheme.Danger;
                    o.Style.SelectionForeColor = AppTheme.Danger;
                    o.Style.Font = AppTheme.Bold;
                }
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

        /// <summary>Điền sẵn mã độc giả kế tiếp (vẫn sửa được) và hạn thẻ mặc định.</summary>
        private void DatGiaTriMoi()
        {
            txtMaDocGia.Text = MaTuSinh.TiepTheo("DocGia", "MaDocGia", "DG", 4);
            dtpHetHan.Value = DateTime.Today.AddMonths(QuyDinh.SoThangHanThe);
            CapNhatTrangThaiThe();
        }

        private void DoLenForm(string maDocGia, string tenDocGia, DateTime? ngaySinh,
                               string diaChi, string soDienThoai, DateTime? hetHanThe)
        {
            txtMaDocGia.Text = maDocGia;
            txtTenDocGia.Text = tenDocGia;
            dtpNgaySinh.Value = ngaySinh ?? DateTime.Today;
            txtDiaChi.Text = diaChi;
            txtSoDienThoai.Text = soDienThoai;
            dtpHetHan.Value = hetHanThe ?? DateTime.Today.AddMonths(QuyDinh.SoThangHanThe);
            CapNhatTrangThaiThe();

            maDangChon = maDocGia;
            KhoaMa(true);
        }

        private void XoaForm()
        {
            txtTenDocGia.Clear();
            txtDiaChi.Clear();
            txtSoDienThoai.Clear();
            dtpNgaySinh.Value = DateTime.Today;

            maDangChon = null;
            KhoaMa(false);
            DatGiaTriMoi();

            dgvDocGia.ClearSelection();
            dgvDocGia.CurrentCell = null;
            txtTenDocGia.Focus();
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

            if (dtpNgaySinh.Value.Date > DateTime.Today)
            {
                AppTheme.Warning("Ngày sinh không được sau ngày hôm nay.");
                dtpNgaySinh.Focus();
                return false;
            }

            if (txtDiaChi.Text.Trim() == "")
            {
                AppTheme.Warning("Vui lòng nhập địa chỉ.");
                txtDiaChi.Focus();
                return false;
            }

            string sdt = txtSoDienThoai.Text.Trim();
            if (sdt == "")
            {
                AppTheme.Warning("Vui lòng nhập số điện thoại.");
                txtSoDienThoai.Focus();
                return false;
            }

            if (!Regex.IsMatch(sdt, @"^\d{9,11}$"))
            {
                AppTheme.Warning("Số điện thoại chỉ gồm chữ số, dài từ 9 đến 11 số.");
                txtSoDienThoai.Focus();
                return false;
            }

            return true;
        }

        /// <summary>
        /// Đọc ngày từ ô lưới. Chấp nhận "dd/MM/yyyy" hoặc "dd/MM/yyyy (hết hạn)" (lấy 10 ký tự đầu).
        /// </summary>
        private static DateTime? DocNgay(object giaTri)
        {
            if (giaTri == null || giaTri == DBNull.Value) return null;
            if (giaTri is DateTime) return (DateTime)giaTri;

            string s = giaTri.ToString().Trim();
            if (s.Length > 10) s = s.Substring(0, 10);

            DateTime kq;
            if (DateTime.TryParseExact(s, DinhDangNgay,
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
                row.Cells["colSoDienThoai"].Value?.ToString(),
                DocNgay(row.Cells["colHetHan"].Value));
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            if (maDangChon != null)
            {
                AppTheme.Warning("Bạn đang chọn một độc giả có sẵn.\nBấm \"Làm mới\" để nhập độc giả mới.");
                return;
            }

            if (!KiemTraDuLieu()) return;

            if (dtpHetHan.Value.Date < DateTime.Today)
            {
                AppTheme.Warning("Hạn thẻ của độc giả mới phải từ hôm nay trở đi.");
                dtpHetHan.Focus();
                return;
            }

            try
            {
                DbConnection.ExecuteNonQuery(@"
                    INSERT INTO DocGia (MaDocGia, TenDocGia, NgaySinh, DiaChi, SoDienThoai, NgayHetHanThe)
                    VALUES (@MaDocGia, @TenDocGia, @NgaySinh, @DiaChi, @SoDienThoai, @HetHan)",
                    DbConnection.Param("@MaDocGia", txtMaDocGia.Text.Trim()),
                    DbConnection.Param("@TenDocGia", txtTenDocGia.Text.Trim()),
                    DbConnection.Param("@NgaySinh", dtpNgaySinh.Value.Date),
                    DbConnection.Param("@DiaChi", txtDiaChi.Text.Trim()),
                    DbConnection.Param("@SoDienThoai", txtSoDienThoai.Text.Trim()),
                    DbConnection.Param("@HetHan", dtpHetHan.Value.Date));

                NhatKy.Ghi("Thêm độc giả", "Thêm độc giả \"" + txtTenDocGia.Text.Trim() + "\" - Mã DG: " + txtMaDocGia.Text.Trim());

                AppTheme.Info("Thêm độc giả thành công!\nThẻ có hiệu lực đến " + dtpHetHan.Value.ToString(DinhDangNgay) + ".");
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
                        SoDienThoai = @SoDienThoai,
                        NgayHetHanThe = @HetHan
                    WHERE MaDocGia = @MaDocGia",
                    DbConnection.Param("@TenDocGia", txtTenDocGia.Text.Trim()),
                    DbConnection.Param("@NgaySinh", dtpNgaySinh.Value.Date),
                    DbConnection.Param("@DiaChi", txtDiaChi.Text.Trim()),
                    DbConnection.Param("@SoDienThoai", txtSoDienThoai.Text.Trim()),
                    DbConnection.Param("@HetHan", dtpHetHan.Value.Date),
                    DbConnection.Param("@MaDocGia", maDangChon));

                if (soDong == 0)
                {
                    AppTheme.Warning("Không tìm thấy mã độc giả cần sửa.");
                    return;
                }

                NhatKy.Ghi("Sửa độc giả", "Sửa độc giả \"" + txtTenDocGia.Text.Trim() + "\" - Mã DG: " + maDangChon);

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

                NhatKy.Ghi("Xóa độc giả", "Xóa độc giả \"" + txtTenDocGia.Text.Trim() + "\" - Mã DG: " + maDangChon);

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
                        r["SoDienThoai"].ToString(),
                        DocNgay(r["NgayHetHanThe"]));
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

        /// <summary>Gia hạn thẻ thêm QuyDinh.SoThangHanThe tháng, tính từ hạn hiện tại (hoặc từ hôm nay nếu đã hết hạn).</summary>
        private void btnGiaHanThe_Click(object sender, EventArgs e)
        {
            if (maDangChon == null)
            {
                AppTheme.Warning("Vui lòng chọn độc giả cần gia hạn thẻ.");
                return;
            }

            try
            {
                // Đọc hạn thẻ hiện tại trong CSDL (không tin giá trị đang sửa dở trên form)
                DateTime hienTai = DbConnection.ExecuteScalar<DateTime>(
                    "SELECT NgayHetHanThe FROM DocGia WHERE MaDocGia = @Ma",
                    DbConnection.Param("@Ma", maDangChon));

                DateTime goc = hienTai.Date > DateTime.Today ? hienTai.Date : DateTime.Today;
                DateTime moi = goc.AddMonths(QuyDinh.SoThangHanThe);

                if (!AppTheme.Confirm("Gia hạn thẻ của độc giả " + maDangChon + " thêm " + QuyDinh.SoThangHanThe +
                                      " tháng,\nhạn mới: " + moi.ToString(DinhDangNgay) + "?"))
                    return;

                DbConnection.ExecuteNonQuery(
                    "UPDATE DocGia SET NgayHetHanThe = @HetHan WHERE MaDocGia = @Ma",
                    DbConnection.Param("@HetHan", moi),
                    DbConnection.Param("@Ma", maDangChon));

                NhatKy.Ghi("Gia hạn thẻ", "Gia hạn thẻ độc giả " + maDangChon + " đến " + moi.ToString(DinhDangNgay));

                AppTheme.Info("Đã gia hạn thẻ đến " + moi.ToString(DinhDangNgay) + ".");
                LoadDocGia();
                XoaForm();
            }
            catch (Exception ex)
            {
                AppTheme.Error("Không thể gia hạn thẻ.\n\n" + ex.Message);
            }
        }
    }
}
