using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DoAnQLTV
{
    public partial class FrmMain : Form
    {

        public FrmMain()
        {
            InitializeComponent();

            this.StartPosition = FormStartPosition.CenterScreen;
            this.WindowState = FormWindowState.Maximized;

            this.Load += FrmMain_Load;
            this.pnlTrangChu.Resize += pnlTrangChu_Resize;
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {
            BoTriGiaoDienMain();
        }

        private void pnlTrangChu_Resize(object sender, EventArgs e)
        {
            BoTriGiaoDienMain();
        }

        private void BoTriGiaoDienMain()
        {
            if (pnlTrangChu == null ||
                lblTieuDeMain == null ||
                lblChaoMung == null ||
                lblIconMain == null ||
                pnlSach == null ||
                pnlDocGia == null ||
                pnlMuonTra == null ||
                lblHuongDanMain == null)
            {
                return;
            }

            int w = pnlTrangChu.ClientSize.Width;
            int h = pnlTrangChu.ClientSize.Height;

            if (w <= 0 || h <= 0)
                return;

            // Tiêu đề và lời chào
            lblTieuDeMain.SetBounds(
                20, 15, w - 40, 55);

            lblChaoMung.SetBounds(
                20, 75, w - 40, 35);

            // Biểu tượng thư viện
            lblIconMain.SetBounds(
                (w - 120) / 2, 115, 120, 85);

            // Ba thẻ chức năng
            int cardWidth = Math.Min(240, (w - 80) / 3);
            int cardHeight = 120;
            int gap = 24;
            int totalWidth = cardWidth * 3 + gap * 2;
            int startX = (w - totalWidth) / 2;

            int cardY = Math.Max(215, (h - cardHeight) / 2);

            pnlSach.SetBounds(
                startX, cardY, cardWidth, cardHeight);

            pnlDocGia.SetBounds(
                startX + cardWidth + gap,
                cardY, cardWidth, cardHeight);

            pnlMuonTra.SetBounds(
                startX + (cardWidth + gap) * 2,
                cardY, cardWidth, cardHeight);

            // Căn chỉnh nội dung bên trong các thẻ
            lblIconSach.SetBounds(
                3, 8, pnlSach.ClientSize.Width - 6, 50);

            lblSach.SetBounds(
                3, 59, pnlSach.ClientSize.Width - 6, 40);

            lblIconDocGia.SetBounds(
                3, 8, pnlDocGia.ClientSize.Width - 6, 50);

            lblDocGia.SetBounds(
                3, 59, pnlDocGia.ClientSize.Width - 6, 40);

            lblIconMuonTra.SetBounds(
                3, 8, pnlMuonTra.ClientSize.Width - 6, 50);

            lblMuonTra.SetBounds(
                3, 59, pnlMuonTra.ClientSize.Width - 6, 40);

            // Dòng hướng dẫn ở cuối vùng nội dung
            lblHuongDanMain.SetBounds(
                20, h - 48, w - 40, 30);
        }


        private void quảnLýSáchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmSach frmSach = new FrmSach();
            frmSach.ShowDialog();
        }

        private void quảnLýThểLoạiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmTheLoai frmTheLoai = new FrmTheLoai();
            frmTheLoai.ShowDialog();
        }

        private void quảnLýTácGiảToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmTacGia frmTacGia = new FrmTacGia();
            frmTacGia.ShowDialog();
        }

        private void quảnLýĐộcGiảToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmDocGia frmDocGia = new FrmDocGia();
            frmDocGia.ShowDialog();
        }

        private void quảnLýNhânViênToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmNhanVien frmNhanVien = new FrmNhanVien();
            frmNhanVien.ShowDialog();
        }

        private void tìmKiếmSáchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmTimKiemSach frmTimKiemSach = new FrmTimKiemSach();
            frmTimKiemSach.ShowDialog();
        }

        private void thôngTinTácGiảToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmThongTinDocGia frmThongTinDocGia = new FrmThongTinDocGia();
            frmThongTinDocGia.ShowDialog();
        }

        private void lậpPhiếuMượnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmLapPhieuMuon frmLapPhieuMuon = new FrmLapPhieuMuon();
            frmLapPhieuMuon.ShowDialog();
        }

        private void trảSáchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmTraSach frmTraSach = new FrmTraSach();
            frmTraSach.ShowDialog();
        }

        private void danhSáchPhiếuMượnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmDanhSachPhieuMuon frmDanhSachPhieuMuon = new FrmDanhSachPhieuMuon();
            frmDanhSachPhieuMuon.ShowDialog();
        }

        private void đăngXuấtToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult ketQua = MessageBox.Show(
                "Bạn có chắc chắn muốn đăng xuất không?",
                "Xác nhận đăng xuất",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (ketQua == DialogResult.Yes)
            {
                FrmLogin frmLogin = new FrmLogin();
                frmLogin.Show();

                this.Hide();
            }
        }

        private void thoátToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult ketQua = MessageBox.Show(
                "Bạn có chắc chắn muốn thoát chương trình không?",
                "Xác nhận thoát",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (ketQua == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void thôngTinChươngTrìnhToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmThongTinChuongTrinh frmThongTinChuongTrinh =
                new FrmThongTinChuongTrinh();

            frmThongTinChuongTrinh.ShowDialog();
        }

        private void hướngDẫnSửDụngToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmHuongDanSuDung frmHuongDanSuDung =
                new FrmHuongDanSuDung();

            frmHuongDanSuDung.ShowDialog();
        }
    }
    
}