using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace DoAnQLTV
{
    public partial class FrmMain : Form
    {

        private bool dangDangXuat = false;
        private bool dangXacNhanThoat = false;

        public FrmMain()
        {
            InitializeComponent();
            
        }


        public void TaiThongKe()
        {
            try
            {
                using (SqlConnection conn = DbConnection.GetConnection())
                {
                    conn.Open();

                    string sql = @"
                SELECT
                    (SELECT COUNT(*) FROM Sach) AS SoSach,
                    (SELECT COUNT(*) FROM DocGia) AS SoDocGia,
                    (SELECT COUNT(*) FROM PhieuMuon) AS SoPhieuMuon,
                    (SELECT COUNT(*) FROM PhieuMuon
                     WHERE NgayTra IS NULL) AS SoPhieuChuaTra;";

                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            lblSoSach.Text = reader["SoSach"].ToString();
                            lblSoDocGia.Text = reader["SoDocGia"].ToString();
                            lblSoPhieuMuon.Text = reader["SoPhieuMuon"].ToString();
                            lblSoPhieuChuaTra.Text = reader["SoPhieuChuaTra"].ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(
                    "Không thể tải số liệu thống kê: " + ex);
            }
        }



        private void DongTatCaFormChucNang()
        {
            // Tạo danh sách riêng để tránh thay đổi tập hợp OpenForms khi đóng Form
            List<Form> danhSachForm = Application.OpenForms
                .Cast<Form>()
                .Where(f => f != this && !(f is FrmLogin))
                .ToList();

            foreach (Form form in danhSachForm)
            {
                if (!form.IsDisposed)
                {
                    form.Close();
                }
            }
        }

        private void CanGiuaNoiDung()
        {
            pnlNoiDungChinh.Left =
                Math.Max(0, (pnlTrangChu.ClientSize.Width
                - pnlNoiDungChinh.Width) / 2);

            pnlNoiDungChinh.Top =
                Math.Max(0, (pnlTrangChu.ClientSize.Height
                - pnlNoiDungChinh.Height) / 2);
        }

        private void pnlTrangChu_Resize(object sender, EventArgs e)
        {
            CanGiuaNoiDung();
        }

        private int soFormDaMo = 0;

        private void MoFormChucNang(Form form)
        {
            // Tìm Form cùng loại đang mở
            Form formDangMo = Application.OpenForms
                .Cast<Form>()
                .FirstOrDefault(f =>
                    f.GetType() == form.GetType() &&
                    !f.IsDisposed);

            // Nếu đã có Form cùng loại thì kích hoạt Form đó
            if (formDangMo != null)
            {
                form.Dispose();

                if (formDangMo.WindowState == FormWindowState.Minimized)
                {
                    formDangMo.WindowState = FormWindowState.Normal;
                }

                formDangMo.BringToFront();
                formDangMo.Activate();
                return;
            }

            // Nếu chưa mở thì hiển thị Form mới
            form.StartPosition = FormStartPosition.Manual;

            int buoc = 35;

            Rectangle vungLamViec = Screen.PrimaryScreen.WorkingArea;

            int gioiHanX = Math.Max(0, vungLamViec.Width - form.Width);
            int gioiHanY = Math.Max(0, vungLamViec.Height - form.Height);

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

                    // Đánh dấu đang đăng xuất để không hỏi thoát
                    dangDangXuat = true;

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
            // Đánh dấu đang đăng xuất để không hỏi xác nhận thoát
            dangDangXuat = true;
            this.Close();
        }



        private void FrmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Nếu đang đăng xuất thì không hỏi xác nhận thoát
            if (dangDangXuat)
                return;

            // Nếu đã xác nhận thoát rồi thì không hỏi lại
            if (dangXacNhanThoat)
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
            CanGiuaNoiDung();

            pnlNhomQuanLy.Visible = false;
            pnlNhomTraCuu.Visible = false;
            pnlNhomMuonTra.Visible = false;
            pnlNhomTroGiup.Visible = false;

            SapXepMenuBenTrai();

            pnlNoiDungChinh.Size = new Size(1190, 616);

            CanGiuaNoiDung();

            // Tải số liệu thống kê từ cơ sở dữ liệu
            TaiThongKe();
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

            // Cập nhật lại 4 ô thống kê từ cơ sở dữ liệu
            TaiThongKe();
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
                pnlNhomTroGiup.BringToFront();
            }
            else
            {
                pnlNhomTroGiup.Location = new Point(0, y);
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