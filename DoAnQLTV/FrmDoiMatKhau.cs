using System;
using System.Drawing;
using System.Windows.Forms;

namespace DoAnQLTV
{
    /// <summary>
    /// Hộp thoại tự đổi mật khẩu của nhân viên đang đăng nhập.
    /// Form này tạo hoàn toàn bằng code, không có file Designer.
    /// Dùng: using (var f = new FrmDoiMatKhau()) f.ShowDialog(this);
    /// </summary>
    public class FrmDoiMatKhau : Form
    {
        private TextBox txtCu;
        private TextBox txtMoi;
        private TextBox txtNhapLai;
        private Button btnLuu;
        private Button btnHuy;

        public FrmDoiMatKhau()
        {
            Name = "FrmDoiMatKhau";
            Text = "Đổi mật khẩu";
            AutoScaleDimensions = new SizeF(96F, 96F);
            AutoScaleMode = AutoScaleMode.Dpi;
            ClientSize = new Size(420, 306);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            StartPosition = FormStartPosition.CenterParent;
            MaximizeBox = false;
            MinimizeBox = false;
            ShowInTaskbar = false;

            AppTheme.ApplyForm(this);

            Label tieuDe = new Label();
            tieuDe.Text = "ĐỔI MẬT KHẨU";
            tieuDe.Font = AppTheme.Heading;
            tieuDe.ForeColor = AppTheme.Primary;
            tieuDe.AutoSize = true;
            tieuDe.Location = new Point(24, 14);
            Controls.Add(tieuDe);

            txtCu = TaoOMatKhau("Mật khẩu hiện tại", 56);
            txtMoi = TaoOMatKhau("Mật khẩu mới (ít nhất 6 ký tự)", 120);
            txtNhapLai = TaoOMatKhau("Nhập lại mật khẩu mới", 184);

            btnLuu = new Button();
            btnLuu.Text = "Lưu";
            btnLuu.SetBounds(148, 250, 120, 40);
            btnLuu.Click += btnLuu_Click;
            Controls.Add(btnLuu);
            AppTheme.StylePrimaryButton(btnLuu);

            btnHuy = new Button();
            btnHuy.Text = "Hủy";
            btnHuy.DialogResult = DialogResult.Cancel;
            btnHuy.SetBounds(276, 250, 120, 40);
            Controls.Add(btnHuy);
            AppTheme.StyleButton(btnHuy,
                ColorTranslator.FromHtml("#E2E8F0"),
                ColorTranslator.FromHtml("#CBD5E1"),
                AppTheme.TextDark);

            AcceptButton = btnLuu;
            CancelButton = btnHuy;
        }

        private TextBox TaoOMatKhau(string nhan, int y)
        {
            Label lbl = new Label();
            lbl.Text = nhan;
            lbl.Font = AppTheme.Bold;
            lbl.ForeColor = AppTheme.Primary;
            lbl.AutoSize = true;
            lbl.Location = new Point(24, y);
            Controls.Add(lbl);

            TextBox txt = new TextBox();
            txt.UseSystemPasswordChar = true;
            txt.SetBounds(24, y + 24, 372, 27);
            Controls.Add(txt);
            AppTheme.StyleTextBox(txt);
            return txt;
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            if (txtCu.Text == "")
            {
                AppTheme.Warning("Vui lòng nhập mật khẩu hiện tại.");
                txtCu.Focus();
                return;
            }

            if (txtMoi.Text.Length < 6)
            {
                AppTheme.Warning("Mật khẩu mới phải có ít nhất 6 ký tự.");
                txtMoi.Focus();
                return;
            }

            if (txtMoi.Text == txtCu.Text)
            {
                AppTheme.Warning("Mật khẩu mới phải khác mật khẩu hiện tại.");
                txtMoi.Focus();
                return;
            }

            if (txtMoi.Text != txtNhapLai.Text)
            {
                AppTheme.Warning("Hai lần nhập mật khẩu mới không khớp.");
                txtNhapLai.Clear();
                txtNhapLai.Focus();
                return;
            }

            try
            {
                if (!TaiKhoan.DoiMatKhau(NhatKy.MaNhanVien, txtCu.Text, txtMoi.Text))
                {
                    AppTheme.Warning("Mật khẩu hiện tại không đúng.");
                    txtCu.Clear();
                    txtCu.Focus();
                    return;
                }

                NhatKy.Ghi("Đổi mật khẩu", "Tự đổi mật khẩu tài khoản " + NhatKy.MaNhanVien);
                AppTheme.Info("Đổi mật khẩu thành công!");
                DialogResult = DialogResult.OK;   // đóng hộp thoại
            }
            catch (Exception ex)
            {
                AppTheme.Error("Không thể đổi mật khẩu.\n\n" + ex.Message);
            }
        }
    }
}
