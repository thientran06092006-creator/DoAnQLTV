using System;
using System.Windows.Forms;

namespace DoAnQLTV
{
    public partial class FrmLogin : Form
    {
        public FrmLogin()
        {
            InitializeComponent();

            AppTheme.ApplyForm(this);
            ApDungGiaoDien();

            // Tên đăng nhập chính là mã nhân viên trong bảng NhanVien
            lblUsername.Text = "Mã nhân viên (tên đăng nhập)";

            // Khi quay lại màn hình đăng nhập (sau khi đăng xuất): xóa mật khẩu cũ
            VisibleChanged += (s, e) =>
            {
                if (!Visible) return;

                txtPassword.Clear();
                (txtUsername.Text == "" ? txtUsername : txtPassword).Focus();
            };
        }

        private void ApDungGiaoDien()
        {
            AppTheme.StyleCard(pnlLogin);

            AppTheme.StyleTextBox(txtUsername);
            AppTheme.StyleTextBox(txtPassword);

            AppTheme.StylePrimaryButton(btnLogin);
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text;

            if (username == "")
            {
                AppTheme.Warning("Vui lòng nhập mã nhân viên.");
                txtUsername.Focus();
                return;
            }

            if (password == "")
            {
                AppTheme.Warning("Vui lòng nhập mật khẩu.");
                txtPassword.Focus();
                return;
            }

            string maNhanVien, tenNhanVien;
            bool hopLe;
            try
            {
                hopLe = TaiKhoan.DangNhap(username, password, out maNhanVien, out tenNhanVien);
            }
            catch (Exception ex)
            {
                AppTheme.Error("Không thể kiểm tra đăng nhập.\n\n" + ex.Message);
                return;
            }

            if (!hopLe)
            {
                AppTheme.Warning("Mã nhân viên hoặc mật khẩu không đúng.");
                txtPassword.Clear();
                txtPassword.Focus();
                return;
            }

            // Gán người đăng nhập TRƯỚC khi tạo FrmMain (FrmMain đọc tên này lúc Load)
            NhatKy.MaNhanVien = maNhanVien;
            NhatKy.NguoiThucHien = tenNhanVien;
            NhatKy.Ghi("Đăng nhập", "Đăng nhập hệ thống");

            FrmMain frmMain = new FrmMain();

            // Giữ FrmLogin hiện tại để dùng lại khi đăng xuất
            frmMain.Tag = this;

            // Nếu FrmMain đóng mà FrmLogin vẫn đang ẩn (tức là thoát chương trình, không phải
            // đăng xuất) thì đóng luôn FrmLogin. Không có dòng này, chương trình vẫn chạy ngầm
            // vì FrmLogin là form chính của Application.Run.
            frmMain.FormClosed += (s, args) =>
            {
                if (!this.Visible)
                    this.Close();
            };

            frmMain.Show();
            this.Hide();
        }
    }
}