using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

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

        private void TaoAnhHinhTron(PictureBox pic)
        {
            if (pic.Width <= 0 || pic.Height <= 0)
                return;

            using (GraphicsPath path = new GraphicsPath())
            {
                path.AddEllipse(0, 0, pic.Width, pic.Height);
                pic.Region = new Region(path);
            }
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {
            dgvHoatDongGanDay.Rows.Clear();

            dgvHoatDongGanDay.Rows.Add(
                "08:30 12/10/2026",
                "Mượn sách",
                "Lập phiếu mượn sách",
                "Nguyễn Văn A"
            );

            dgvHoatDongGanDay.Rows.Add(
                "09:15 12/10/2026",
                "Thêm sách",
                "Thêm sách mới vào thư viện",
                "Trần Thị B"
            );

            dgvHoatDongGanDay.Rows.Add(
                "10:00 12/10/2026",
                "Trả sách",
                "Cập nhật ngày trả sách",
                "Lê Văn C"
            );

            dgvHoatDongGanDay.Rows.Add(
                "10:30 12/10/2026",
                "Cập nhật độc giả",
                "Chỉnh sửa thông tin độc giả",
                "Phạm Thị D"
            );
            dgvHoatDongGanDay.ClearSelection();

            TaoAnhHinhTron(picDaiDien);

            TaoAnhHinhTron(picQuanLySach);
            TaoAnhHinhTron(picQuanLyDocGia);
            TaoAnhHinhTron(picQuanLyNhanVien);
            TaoAnhHinhTron(picLapPhieuMuon);
            TaoAnhHinhTron(picTraSach);
            TaoAnhHinhTron(picTimKiemSach);

            lblThoiGianHienTai.Text = DateTime.Now.ToString("HH:mm:ss");
            timerThoiGian.Start();
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

        private void timerThoiGian_Tick(object sender, EventArgs e)
        {
            lblThoiGianHienTai.Text = DateTime.Now.ToString("HH:mm:ss");
        }

        
        // Tạo nhầm nhưng chưa dùng đến
        private void pnlTheQuanLyDocGia_Paint(object sender, PaintEventArgs e)
        {

        }

        private void pnlTheQuanLyNhanVien_Paint(object sender, PaintEventArgs e)
        {

        }

        private void pnlTheLapPhieuMuon_Paint(object sender, PaintEventArgs e)
        {

        }

        private void pnlTheTraSach_Paint(object sender, PaintEventArgs e)
        {

        }

        private void pnlTheTimKiemSach_Paint(object sender, PaintEventArgs e)
        {

        }

        // Truy cập nhanh
        private void pnlTheQuanLySach_Click(object sender, EventArgs e)
        {
            FrmSach frm = new FrmSach();
            frm.ShowDialog();
        }
        private void pnlTheQuanLyDocGia_Click(object sender, EventArgs e)
        {
            FrmDocGia frm = new FrmDocGia();
            frm.ShowDialog();
        }

        private void pnlTheQuanLyNhanVien_Click(object sender, EventArgs e)
        {
            FrmNhanVien frm = new FrmNhanVien();
            frm.ShowDialog();
        }

        private void pnlTheLapPhieuMuon_Click(object sender, EventArgs e)
        {
            FrmLapPhieuMuon frm = new FrmLapPhieuMuon();
            frm.ShowDialog();
        }

        private void pnlTheTraSach_Click(object sender, EventArgs e)
        {
            FrmTraSach frm = new FrmTraSach();
            frm.ShowDialog();
        }

        private void pnlTheTimKiemSach_Click(object sender, EventArgs e)
        {
            FrmTimKiemSach frm = new FrmTimKiemSach();
            frm.ShowDialog();
        }
    }
}