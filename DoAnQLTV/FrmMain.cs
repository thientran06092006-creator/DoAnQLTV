using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Data;

namespace DoAnQLTV
{
    public partial class FrmMain : Form
    {
        private bool dangDangXuat = false;
        private bool dangXacNhanThoat = false;
        private int soFormDaMo = 0;

        public FrmMain()
        {
            InitializeComponent();

            this.StartPosition = FormStartPosition.CenterScreen;
            this.WindowState = FormWindowState.Maximized;
            this.MinimumSize = new System.Drawing.Size(1000, 650);

            // Chỉ giữ thanh menu ở phía trên.
            this.menuStrip1.Dock = DockStyle.Top;

            // Màu nền vùng nội dung.
            this.BackColor = System.Drawing.Color.White;
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {
            // Để trống trong thời gian thiết kế lại giao diện.
        }

        private void MoFormChucNang(Form form)
        {
            Form formDangMo = Application.OpenForms
                .Cast<Form>()
                .FirstOrDefault(f =>
                    f.GetType() == form.GetType() &&
                    !f.IsDisposed);

            if (formDangMo != null)
            {
                form.Dispose();

                if (formDangMo.WindowState == FormWindowState.Minimized)
                    formDangMo.WindowState = FormWindowState.Normal;

                formDangMo.BringToFront();
                formDangMo.Activate();
                return;
            }

            form.StartPosition = FormStartPosition.CenterScreen;

            form.FormClosed += (s, e) =>
                soFormDaMo = Math.Max(0, soFormDaMo - 1);

            soFormDaMo++;
            form.Show();
        }

        private void quảnLýSáchToolStripMenuItem_Click(
            object sender, EventArgs e)
        {
            MoFormChucNang(new FrmSach());
        }

        private void quảnLýThểLoạiToolStripMenuItem_Click(
            object sender, EventArgs e)
        {
            MoFormChucNang(new FrmTheLoai());
        }

        private void quảnLýTácGiảToolStripMenuItem_Click(
            object sender, EventArgs e)
        {
            MoFormChucNang(new FrmTacGia());
        }

        private void quảnLýĐộcGiảToolStripMenuItem_Click(
            object sender, EventArgs e)
        {
            MoFormChucNang(new FrmDocGia());
        }

        private void quảnLýNhânViênToolStripMenuItem_Click(
            object sender, EventArgs e)
        {
            MoFormChucNang(new FrmNhanVien());
        }

        private void tìmKiếmSáchToolStripMenuItem_Click(
            object sender, EventArgs e)
        {
            MoFormChucNang(new FrmTimKiemSach());
        }

        private void thôngTinTácGiảToolStripMenuItem_Click(
            object sender, EventArgs e)
        {
            MoFormChucNang(new FrmThongTinDocGia());
        }

        private void lậpPhiếuMượnToolStripMenuItem_Click(
            object sender, EventArgs e)
        {
            MoFormChucNang(new FrmLapPhieuMuon());
        }

        private void trảSáchToolStripMenuItem_Click(
            object sender, EventArgs e)
        {
            MoFormChucNang(new FrmTraSach());
        }

        private void danhSáchPhiếuMượnToolStripMenuItem_Click(
            object sender, EventArgs e)
        {
            MoFormChucNang(new FrmDanhSachPhieuMuon());
        }

        private void thôngTinChươngTrìnhToolStripMenuItem_Click(
            object sender, EventArgs e)
        {
            MoFormChucNang(new FrmThongTinChuongTrinh());
        }

        private void hướngDẫnSửDụngToolStripMenuItem_Click(
            object sender, EventArgs e)
        {
            MoFormChucNang(new FrmHuongDanSuDung());
        }

        private void đăngXuấtToolStripMenuItem_Click(
            object sender, EventArgs e)
        {
            DialogResult ketQua = MessageBox.Show(
                "Bạn có chắc chắn muốn đăng xuất không?",
                "Xác nhận đăng xuất",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (ketQua != DialogResult.Yes)
                return;

            FrmLogin frmLogin = Tag as FrmLogin;

            if (frmLogin == null)
            {
                MessageBox.Show(
                    "Không tìm thấy màn hình đăng nhập hiện tại.",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            DongTatCaFormChucNang();

            dangDangXuat = true;
            frmLogin.Show();
            frmLogin.Activate();
            Close();
        }

        private void thoátToolStripMenuItem_Click(
            object sender, EventArgs e)
        {
            dangDangXuat = true;
            Close();
        }

        private void DongTatCaFormChucNang()
        {
            List<Form> danhSachForm = Application.OpenForms
                .Cast<Form>()
                .Where(f => f != this && !(f is FrmLogin))
                .ToList();

            foreach (Form form in danhSachForm)
            {
                if (!form.IsDisposed)
                    form.Close();
            }
        }

        private void FrmMain_FormClosing(
            object sender, FormClosingEventArgs e)
        {
            if (dangDangXuat || dangXacNhanThoat)
                return;

            DialogResult ketQua = MessageBox.Show(
                "Bạn có chắc chắn muốn thoát chương trình không?",
                "Xác nhận thoát",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (ketQua == DialogResult.Yes)
            {
                dangXacNhanThoat = true;
            }
            else
            {
                e.Cancel = true;
            }
        }
    }
}