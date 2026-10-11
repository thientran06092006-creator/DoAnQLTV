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


        private void DongTatCaFormChucNang()
        {
            foreach (Form form in Application.OpenForms.Cast<Form>().ToList())
            {
                if (form != this && !(form is FrmLogin))
                {
                    form.Close();
                }
            }
        }



        private int soFormDaMo = 0;

        private void MoFormChucNang(Form form)
        {
            // Nếu Form cùng loại đã mở thì đưa lên trước
            foreach (Form formDangMo in Application.OpenForms)
            {
                if (formDangMo.GetType() == form.GetType())
                {
                    form.Dispose();
                    formDangMo.WindowState = FormWindowState.Normal;
                    formDangMo.BringToFront();
                    formDangMo.Activate();
                    return;
                }
            }

            form.StartPosition = FormStartPosition.Manual;

            int buoc = 35;
            int gioiHanX = Math.Max(
                0,
                Screen.PrimaryScreen.WorkingArea.Width - form.Width);

            int gioiHanY = Math.Max(
                0,
                Screen.PrimaryScreen.WorkingArea.Height - form.Height);

            form.Location = new Point(
                Math.Min(250 + (soFormDaMo * buoc) % Math.Max(1, gioiHanX), gioiHanX),
                Math.Min(50 + (soFormDaMo * buoc) % Math.Max(1, gioiHanY), gioiHanY)
            );

            soFormDaMo++;

            form.FormClosed += (s, e) =>
                soFormDaMo = Math.Max(0, soFormDaMo - 1);

            form.Show();
        }



        private void quảnLýSáchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MoFormChucNang(new FrmSach());
        }

        private void quảnLýThểLoạiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MoFormChucNang(new FrmTheLoai());
        }

        private void quảnLýTácGiảToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MoFormChucNang(new FrmTacGia());
        }

        private void quảnLýĐộcGiảToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MoFormChucNang(new FrmDocGia());
        }

        private void quảnLýNhânViênToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MoFormChucNang(new FrmNhanVien());
        }

        private void tìmKiếmSáchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MoFormChucNang(new FrmTimKiemSach());
        }

        private void thôngTinTácGiảToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MoFormChucNang(new FrmThongTinDocGia());
        }

        private void lậpPhiếuMượnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MoFormChucNang(new FrmLapPhieuMuon());
        }

        private void trảSáchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MoFormChucNang(new FrmTraSach());
        }

        private void danhSáchPhiếuMượnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MoFormChucNang(new FrmDanhSachPhieuMuon());
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
                // Lấy lại FrmLogin đã được lưu khi đăng nhập
                FrmLogin frmLogin = this.Tag as FrmLogin;

                if (frmLogin != null)
                {
                    // Đóng các Form chức năng đang mở
                    DongTatCaFormChucNang();

                    // Hiện lại màn hình đăng nhập
                    frmLogin.Show();
                    frmLogin.Activate();

                    // Đóng FrmMain hiện tại
                    this.Close();
                }
                else
                {
                    MessageBox.Show(
                        "Không tìm thấy màn hình đăng nhập hiện tại.",
                        "Thông báo",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                }
            }
        }



        private void thoátToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }



        private bool dangXacNhanThoat = false;

        private void FrmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (dangXacNhanThoat)
                return;

            DialogResult ketQua = MessageBox.Show(
                "Bạn có chắc chắn muốn thoát chương trình không?",
                "Xác nhận thoát",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (ketQua == DialogResult.No)
            {
                e.Cancel = true;
            }
            else
            {
                dangXacNhanThoat = true;
                Application.Exit();
            }
        }


        private void thôngTinChươngTrìnhToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MoFormChucNang(new FrmThongTinChuongTrinh());
        }

        private void hướngDẫnSửDụngToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MoFormChucNang(new FrmHuongDanSuDung());
        }



        private void FrmMain_Load(object sender, EventArgs e)
        {
            pnlNoiDungChinh.Left =
                (pnlTrangChu.ClientSize.Width - pnlNoiDungChinh.Width) / 2;

            pnlNhomQuanLy.Visible = false;
            pnlNhomTraCuu.Visible = false;
            pnlNhomMuonTra.Visible = false;
            pnlNhomTroGiup.Visible = false;

            SapXepMenuBenTrai();
        }


        private void btnTrangChu_Click(object sender, EventArgs e)
        {
            DongTatCaFormChucNang();

            pnlNhomQuanLy.Visible = false;
            pnlNhomTraCuu.Visible = false;
            pnlNhomMuonTra.Visible = false;
            pnlNhomTroGiup.Visible = false;

            pnlTrangChu.Visible = true;
            pnlTrangChu.BringToFront();

            SapXepMenuBenTrai();
        }


        private void btnQuanLy_Click(object sender, EventArgs e)
        {
            pnlNhomQuanLy.Visible = !pnlNhomQuanLy.Visible;
            SapXepMenuBenTrai();
        }


        private void btnQLSach_Click(object sender, EventArgs e)
        {
            MoFormChucNang(new FrmSach());
        }


        private void SapXepMenuBenTrai()
        {
            int chieuCaoNut = 45;
            int y = 0;
            int chieuRong = pnlThanhBen.ClientSize.Width;

            // 1. Nút Trang chủ
            btnTrangChu.Location = new Point(0, y);
            btnTrangChu.Size = new Size(chieuRong, chieuCaoNut);
            y += chieuCaoNut;

            // 2. Nút Quản lý
            btnQuanLy.Location = new Point(0, y);
            btnQuanLy.Size = new Size(chieuRong, chieuCaoNut);
            y += chieuCaoNut;

            // 3. Nhóm chức năng Quản lý
            if (pnlNhomQuanLy.Visible)
            {
                pnlNhomQuanLy.Location = new Point(0, y);
                pnlNhomQuanLy.Size = new Size(chieuRong, chieuCaoNut * 5);

                btnQLSach.Location = new Point(0, 0);
                btnQLSach.Size = new Size(chieuRong, chieuCaoNut);

                btnQLTheLoai.Location = new Point(0, chieuCaoNut);
                btnQLTheLoai.Size = new Size(chieuRong, chieuCaoNut);

                btnQLTacGia.Location = new Point(0, chieuCaoNut * 2);
                btnQLTacGia.Size = new Size(chieuRong, chieuCaoNut);

                btnQLDocGia.Location = new Point(0, chieuCaoNut * 3);
                btnQLDocGia.Size = new Size(chieuRong, chieuCaoNut);

                btnQLNhanVien.Location = new Point(0, chieuCaoNut * 4);
                btnQLNhanVien.Size = new Size(chieuRong, chieuCaoNut);

                y += pnlNhomQuanLy.Height;
            }

            // 4. Nút Tra cứu
            btnTraCuu.Location = new Point(0, y);
            btnTraCuu.Size = new Size(chieuRong, chieuCaoNut);
            y += chieuCaoNut;

            // 5. Nhóm chức năng Tra cứu
            if (pnlNhomTraCuu.Visible)
            {
                pnlNhomTraCuu.Location = new Point(0, y);
                pnlNhomTraCuu.Size = new Size(chieuRong, chieuCaoNut * 2);

                btnTimKiemSach.Location = new Point(0, 0);
                btnTimKiemSach.Size = new Size(chieuRong, chieuCaoNut);

                btnTraCuuDocGia.Location = new Point(0, chieuCaoNut);
                btnTraCuuDocGia.Size = new Size(chieuRong, chieuCaoNut);

                y += pnlNhomTraCuu.Height;
            }

            // 6. Nút Mượn - Trả
            btnMuonTra.Location = new Point(0, y);
            btnMuonTra.Size = new Size(chieuRong, chieuCaoNut);
            y += chieuCaoNut;

            // 7. Nhóm chức năng Mượn - Trả
            if (pnlNhomMuonTra.Visible)
            {
                pnlNhomMuonTra.Location = new Point(0, y);
                pnlNhomMuonTra.Size = new Size(chieuRong, chieuCaoNut * 3);

                btnLapPhieuMuon.Location = new Point(0, 0);
                btnLapPhieuMuon.Size = new Size(chieuRong, chieuCaoNut);

                btnTraSach.Location = new Point(0, chieuCaoNut);
                btnTraSach.Size = new Size(chieuRong, chieuCaoNut);

                btnDanhSachPhieuMuon.Location = new Point(0, chieuCaoNut * 2);
                btnDanhSachPhieuMuon.Size = new Size(chieuRong, chieuCaoNut);

                y += pnlNhomMuonTra.Height;
            }

            // 8. Nút Trợ giúp
            btnTroGiup.Location = new Point(0, y);
            btnTroGiup.Size = new Size(chieuRong, chieuCaoNut);
            btnTroGiup.Visible = true;
            btnTroGiup.BringToFront();

            y += chieuCaoNut;

            // 9. Nhóm chức năng Trợ giúp
            pnlNhomTroGiup.Size = new Size(chieuRong, chieuCaoNut * 2);

            btnThongTinChuongTrinh.Location = new Point(0, 0);
            btnThongTinChuongTrinh.Size = new Size(chieuRong, chieuCaoNut);

            btnHuongDanSuDung.Location = new Point(0, chieuCaoNut);
            btnHuongDanSuDung.Size = new Size(chieuRong, chieuCaoNut);

            if (pnlNhomTroGiup.Visible)
            {
                pnlNhomTroGiup.Location = new Point(0, y);
                pnlNhomTroGiup.Visible = true;
                pnlNhomTroGiup.BringToFront();
            }

        }


        private void btnQLTheLoai_Click(object sender, EventArgs e)
        {
            MoFormChucNang(new FrmTheLoai());
        }

        private void btnQLTacGia_Click(object sender, EventArgs e)
        {
            MoFormChucNang(new FrmTacGia());
        }

        private void btnQLDocGia_Click(object sender, EventArgs e)
        {
            MoFormChucNang(new FrmDocGia());
        }

        private void btnQLNhanVien_Click(object sender, EventArgs e)
        {
            MoFormChucNang(new FrmNhanVien());
        }

        private void btnTraCuu_Click(object sender, EventArgs e)
        {
            pnlNhomTraCuu.Visible = !pnlNhomTraCuu.Visible;
            SapXepMenuBenTrai();
        }

        private void btnTimKiemSach_Click(object sender, EventArgs e)
        {
            MoFormChucNang(new FrmTimKiemSach());
        }

        private void btnTraCuuDocGia_Click(object sender, EventArgs e)
        {
            MoFormChucNang(new FrmThongTinDocGia());
        }

        private void btnMuonTra_Click(object sender, EventArgs e)
        {
            pnlNhomMuonTra.Visible = !pnlNhomMuonTra.Visible;
            SapXepMenuBenTrai();
        }

        private void btnLapPhieuMuon_Click(object sender, EventArgs e)
        {
            MoFormChucNang(new FrmLapPhieuMuon());
        }

        private void btnTraSach_Click(object sender, EventArgs e)
        {
            MoFormChucNang(new FrmTraSach());
        }

        private void btnDanhSachPhieuMuon_Click(object sender, EventArgs e)
        {
            MoFormChucNang(new FrmDanhSachPhieuMuon());
        }

        private void btnTroGiup_Click(object sender, EventArgs e)
        {
            pnlNhomTroGiup.Visible = !pnlNhomTroGiup.Visible;
            SapXepMenuBenTrai();
        }

        private void btnHuongDanSuDung_Click(object sender, EventArgs e)
        {
            MoFormChucNang(new FrmHuongDanSuDung());
        }

        private void btnThongTinChuongTrinh_Click(object sender, EventArgs e)
        {
            MoFormChucNang(new FrmThongTinChuongTrinh());
        }

        private void btnMoQuanLySach_Click(object sender, EventArgs e)
        {
            MoFormChucNang(new FrmSach());
        }

        private void btnMoQuanLyDocGia_Click(object sender, EventArgs e)
        {
            MoFormChucNang(new FrmDocGia());
        }

        private void btnMoMuonTraSach_Click(object sender, EventArgs e)
        {
            MoFormChucNang(new FrmLapPhieuMuon());
        }


    }

}