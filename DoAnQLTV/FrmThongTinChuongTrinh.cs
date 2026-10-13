using System;
using System.Drawing;
using System.Windows.Forms;

namespace DoAnQLTV
{
    public partial class FrmThongTinChuongTrinh : Form
    {
        public FrmThongTinChuongTrinh()
        {
            InitializeComponent();

            AppTheme.ApplyForm(this);
            ApDungGiaoDien();
        }

        private void ApDungGiaoDien()
        {
            AppTheme.StyleCard(pnlThongTin);

            AppTheme.StyleButton(btnDong,
                ColorTranslator.FromHtml("#E2E8F0"),
                ColorTranslator.FromHtml("#CBD5E1"),
                AppTheme.TextDark);
        }

        private void btnDong_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}