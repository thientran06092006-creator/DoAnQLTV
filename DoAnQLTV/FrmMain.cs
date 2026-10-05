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