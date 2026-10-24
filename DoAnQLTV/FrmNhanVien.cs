using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace DoAnQLTV
{
    public partial class FrmNhanVien : Form
    {
        // Mã nhân viên đang được chọn để sửa/xóa. null = đang ở chế độ nhập mới.
        private string maDangChon = null;

        // Các thành phần thêm bằng code (không sửa Designer)
        private Button btnDatMatKhau;
        private Label lblVaiTro;
        private ComboBox cboVaiTro;

        private const string TenQuanTri = "Quản trị viên";
        private const string TenNhanVienThuong = "Nhân viên";

        public FrmNhanVien()
        {
            InitializeComponent();

            txtMaNhanVien.MaxLength = 20;
            txtTenNhanVien.MaxLength = 100;
            txtSoDienThoai.MaxLength = 15;
            txtDiaChi.MaxLength = 200;

            AppTheme.ApplyForm(this);
            ThemThanhPhanMoi();
            ApDungGiaoDien();
            ThemCotVaiTro();
            LoadNhanVien();

            txtMaNhanVien.Text = MaTuSinh.TiepTheo("NhanVien", "MaNhanVien", "NV", 4);   // <-- THÊM
        }

        // ================= GIAO DIỆN =================

        /// <summary>Chừa thêm 1 hàng cho "Vai trò", thêm nút "Đặt mật khẩu".</summary>
        private void ThemThanhPhanMoi()
        {
            const int doi = 48;   // đẩy hàng nút và danh sách xuống

            pnlThongTin.Height += doi;
            foreach (Button b in new Button[] { btnThem, btnSua, btnXoa, btnTimKiem, btnLamMoi })
                b.Top += doi;
            pnlDanhSach.Top += doi;
            ClientSize = new Size(ClientSize.Width, ClientSize.Height + doi);

            lblVaiTro = new Label();
            lblVaiTro.AutoSize = true;
            lblVaiTro.Font = lblMaNhanVien.Font;
            lblVaiTro.ForeColor = lblMaNhanVien.ForeColor;
            lblVaiTro.Text = "Vai trò";
            lblVaiTro.Location = new Point(24, 124);
            pnlThongTin.Controls.Add(lblVaiTro);

            cboVaiTro = new ComboBox();
            cboVaiTro.DropDownStyle = ComboBoxStyle.DropDownList;
            cboVaiTro.Items.AddRange(new object[] { TenNhanVienThuong, TenQuanTri });
            cboVaiTro.SelectedIndex = 0;
            cboVaiTro.SetBounds(130, 120, 300, 28);
            pnlThongTin.Controls.Add(cboVaiTro);

            btnDatMatKhau = new Button();
            btnDatMatKhau.Name = "btnDatMatKhau";
            btnDatMatKhau.Text = "Đặt mật khẩu";
            btnDatMatKhau.SetBounds(684, 132 + doi, 120, 40);
            btnDatMatKhau.Click += btnDatMatKhau_Click;
            pnlThongTin.Controls.Add(btnDatMatKhau);
        }

        private void ApDungGiaoDien()
        {
            AppTheme.StyleCard(pnlThongTin);
            AppTheme.StyleCard(pnlDanhSach);

            AppTheme.StyleGrid(dgvNhanVien);

            AppTheme.StyleTextBox(txtMaNhanVien);
            AppTheme.StyleTextBox(txtTenNhanVien);
            AppTheme.StyleTextBox(txtSoDienThoai);
            AppTheme.StyleTextBox(txtDiaChi);
            AppTheme.StyleComboBox(cboVaiTro);

            AppTheme.StylePrimaryButton(btnThem);
            AppTheme.StyleButton(btnSua, AppTheme.Amber, ColorTranslator.FromHtml("#D97706"));
            AppTheme.StyleDangerButton(btnXoa);
            AppTheme.StyleSuccessButton(btnTimKiem);
            AppTheme.StyleButton(btnLamMoi,
                ColorTranslator.FromHtml("#E2E8F0"),
                ColorTranslator.FromHtml("#CBD5E1"),
                AppTheme.TextDark);
            AppTheme.StyleOutlineButton(btnDatMatKhau);
        }

        private void ThemCotVaiTro()
        {
            if (dgvNhanVien.Columns.Contains("colVaiTro")) return;

            DataGridViewTextBoxColumn col = new DataGridViewTextBoxColumn();
            col.Name = "colVaiTro";
            col.HeaderText = "Vai trò";
            col.FillWeight = 25F;
            col.ReadOnly = true;
            dgvNhanVien.Columns.Add(col);
        }

        // ================= NẠP DỮ LIỆU =================

        /// <summary>Lấy danh sách nhân viên; tuKhoa rỗng/null = lấy tất cả.</summary>
        private DataTable TruyVanNhanVien(string tuKhoa)
        {
            const string sql = @"
                SELECT MaNhanVien, TenNhanVien, SoDienThoai, DiaChi, VaiTro
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
                dgvNhanVien.Rows.Add(
                    r["MaNhanVien"],
                    r["TenNhanVien"],
                    r["SoDienThoai"],
                    r["DiaChi"],
                    TaiKhoan.TenHienThiVaiTro(r["VaiTro"] as string));

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

        private void DoLenForm(string maNhanVien, string tenNhanVien, string soDienThoai,
                               string diaChi, string tenVaiTro)
        {
            txtMaNhanVien.Text = maNhanVien;
            txtTenNhanVien.Text = tenNhanVien;
            txtSoDienThoai.Text = soDienThoai;
            txtDiaChi.Text = diaChi;
            cboVaiTro.SelectedIndex = tenVaiTro == TenQuanTri ? 1 : 0;

            maDangChon = maNhanVien;
            KhoaMa(true);
        }

        private void XoaForm()
        {
            txtMaNhanVien.Text = MaTuSinh.TiepTheo("NhanVien", "MaNhanVien", "NV", 4);   // <-- THAY cho txtMaNhanVien.Clear();
            txtTenNhanVien.Clear();
            txtSoDienThoai.Clear();
            txtDiaChi.Clear();
            cboVaiTro.SelectedIndex = 0;

            maDangChon = null;
            KhoaMa(false);

            dgvNhanVien.ClearSelection();
            dgvNhanVien.CurrentCell = null;
            txtMaNhanVien.Focus();
        }

        /// <summary>Giá trị lưu trong CSDL của vai trò đang chọn trên form.</summary>
        private string VaiTroDangChon()
        {
            return cboVaiTro.SelectedIndex == 1 ? TaiKhoan.VaiTroAdmin : TaiKhoan.VaiTroNhanVien;
        }

        /// <summary>Mã này có phải là tài khoản đang đăng nhập không?</summary>
        private static bool LaChinhMinh(string maNhanVien)
        {
            return maNhanVien != null &&
                   string.Equals(maNhanVien.Trim(), (NhatKy.MaNhanVien ?? "").Trim(),
                       StringComparison.OrdinalIgnoreCase);
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

            if (txtDiaChi.Text.Trim() == "")
            {
                AppTheme.Warning("Vui lòng nhập địa chỉ.");
                txtDiaChi.Focus();
                return false;
            }

            if (cboVaiTro.SelectedIndex == -1)
            {
                AppTheme.Warning("Vui lòng chọn vai trò.");
                cboVaiTro.Focus();
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
                row.Cells["colDiaChi"].Value?.ToString(),
                row.Cells["colVaiTro"].Value?.ToString());
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
                string ma = txtMaNhanVien.Text.Trim();

                DbConnection.ExecuteNonQuery(@"
                    INSERT INTO NhanVien (MaNhanVien, TenNhanVien, SoDienThoai, DiaChi, MatKhau, VaiTro)
                    VALUES (@MaNhanVien, @TenNhanVien, @SoDienThoai, @DiaChi, @MatKhau, @VaiTro)",
                    DbConnection.Param("@MaNhanVien", ma),
                    DbConnection.Param("@TenNhanVien", txtTenNhanVien.Text.Trim()),
                    DbConnection.Param("@SoDienThoai", txtSoDienThoai.Text.Trim()),
                    DbConnection.Param("@DiaChi", txtDiaChi.Text.Trim()),
                    DbConnection.Param("@MatKhau", TaiKhoan.BamMatKhau(ma, QuyDinh.MatKhauMacDinh)),
                    DbConnection.Param("@VaiTro", VaiTroDangChon()));

                NatKyThem(ma);

                AppTheme.Info("Thêm nhân viên thành công!\nMật khẩu mặc định: " + QuyDinh.MatKhauMacDinh);
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

        private void NatKyThem(string ma)
        {
            NhatKy.Ghi("Thêm nhân viên",
                "Thêm nhân viên \"" + txtTenNhanVien.Text.Trim() + "\" - Mã NV: " + ma +
                " (" + cboVaiTro.Text + ")");
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (maDangChon == null)
            {
                AppTheme.Warning("Vui lòng chọn nhân viên cần sửa.");
                return;
            }

            if (!KiemTraDuLieu()) return;

            // Tránh tự khóa mình: không cho tự đổi vai trò của tài khoản đang đăng nhập
            if (LaChinhMinh(maDangChon) &&
                !string.Equals(VaiTroDangChon(), TaiKhoan.VaiTroHienTai, StringComparison.OrdinalIgnoreCase))
            {
                AppTheme.Warning("Bạn không thể tự đổi vai trò của tài khoản đang đăng nhập.\n" +
                                 "Hãy nhờ một quản trị viên khác thực hiện.");
                cboVaiTro.Focus();
                return;
            }

            try
            {
                int soDong = DbConnection.ExecuteNonQuery(@"
                    UPDATE NhanVien
                    SET TenNhanVien = @TenNhanVien,
                        SoDienThoai = @SoDienThoai,
                        DiaChi = @DiaChi,
                        VaiTro = @VaiTro
                    WHERE MaNhanVien = @MaNhanVien",
                    DbConnection.Param("@TenNhanVien", txtTenNhanVien.Text.Trim()),
                    DbConnection.Param("@SoDienThoai", txtSoDienThoai.Text.Trim()),
                    DbConnection.Param("@DiaChi", txtDiaChi.Text.Trim()),
                    DbConnection.Param("@VaiTro", VaiTroDangChon()),
                    DbConnection.Param("@MaNhanVien", maDangChon));

                if (soDong == 0)
                {
                    AppTheme.Warning("Không tìm thấy mã nhân viên cần sửa.");
                    return;
                }

                NhatKy.Ghi("Sửa nhân viên",
                    "Sửa nhân viên \"" + txtTenNhanVien.Text.Trim() + "\" - Mã NV: " + maDangChon +
                    " (" + cboVaiTro.Text + ")");

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

            if (LaChinhMinh(maDangChon))
            {
                AppTheme.Warning("Không thể xóa tài khoản đang đăng nhập.");
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

                NhatKy.Ghi("Xóa nhân viên",
                    "Xóa nhân viên \"" + txtTenNhanVien.Text.Trim() + "\" - Mã NV: " + maDangChon);

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
                        r["DiaChi"].ToString(),
                        TaiKhoan.TenHienThiVaiTro(r["VaiTro"] as string));
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

        private void btnDatMatKhau_Click(object sender, EventArgs e)
        {
            if (maDangChon == null)
            {
                AppTheme.Warning("Vui lòng chọn nhân viên cần đặt mật khẩu.");
                return;
            }

            string mk = TaiKhoan.HoiMatKhauMoi("Đặt mật khẩu cho nhân viên " + maDangChon);
            if (mk == null) return;

            if (mk.Length < 6)
            {
                AppTheme.Warning("Mật khẩu phải có ít nhất 6 ký tự.");
                return;
            }

            try
            {
                TaiKhoan.DatMatKhau(maDangChon, mk);
                NhatKy.Ghi("Đổi mật khẩu", "Đặt mật khẩu cho nhân viên mã " + maDangChon);
                AppTheme.Info("Đã đặt mật khẩu mới.");
            }
            catch (Exception ex)
            {
                AppTheme.Error("Không thể đặt mật khẩu.\n\n" + ex.Message);
            }
        }
    }
}
