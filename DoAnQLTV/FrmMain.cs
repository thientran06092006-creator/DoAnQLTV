using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace DoAnQLTV
{
    public partial class FrmMain : Form
    {


        private FrmSach frmSach;
        private FrmTheLoai frmTheLoai;
        private FrmTacGia frmTacGia;
        private FrmDocGia frmDocGia;
        private FrmNhanVien frmNhanVien;


        private bool dangDangXuat = false;
        private bool dangXacNhanThoat = false;

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


        private void CapNhatThongKe()
        {
            try
            {
                using (SqlConnection conn = DbConnection.GetConnection())
                {
                    conn.Open();

                    string sql = @"
                SELECT
                    (SELECT COUNT(*) FROM Sach) AS TongSach,
                    (SELECT COUNT(*) FROM DocGia) AS TongDocGia,
                    (SELECT COUNT(*)
                     FROM PhieuMuon
                     WHERE NgayTra IS NULL) AS TongDangMuon,
                    (SELECT COUNT(*)
                     FROM PhieuMuon
                     WHERE NgayTra IS NULL
                       AND HanTra < CAST(GETDATE() AS DATE))
                     AS TongQuaHan";

                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            lblSoSach.Text =
                                reader["TongSach"].ToString();

                            lblSoDocGia.Text =
                                reader["TongDocGia"].ToString();

                            lblSoDangMuon.Text =
                                reader["TongDangMuon"].ToString();

                            lblSoQuaHan.Text =
                                reader["TongQuaHan"].ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Không thể tải số liệu thống kê.\n\n" + ex.Message,
                    "Lỗi thống kê",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
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
            CapNhatThongKe();
            if (System.ComponentModel.LicenseManager.UsageMode
       == System.ComponentModel.LicenseUsageMode.Designtime)
            {
                return;
            }
            // ==========================================
            // 1. DỮ LIỆU HOẠT ĐỘNG GẦN ĐÂY
            // ==========================================

            if (dgvHoatDongGanDay != null)
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
            }

            // ==========================================
            // 2. TẠO ẢNH HÌNH TRÒN
            // ==========================================

            TaoAnhHinhTron(picDaiDien);

            TaoAnhHinhTron(picQuanLySach);
            TaoAnhHinhTron(picQuanLyDocGia);
            TaoAnhHinhTron(picQuanLyNhanVien);
            TaoAnhHinhTron(picLapPhieuMuon);
            TaoAnhHinhTron(picTraSach);
            TaoAnhHinhTron(picTimKiemSach);

            // ==========================================
            // 3. ĐỒNG HỒ THỜI GIAN
            // ==========================================

            if (lblThoiGianHienTai != null)
            {
                lblThoiGianHienTai.Text =
                    DateTime.Now.ToString("HH:mm:ss");
            }

            if (timerThoiGian != null)
            {
                timerThoiGian.Start();
            }

            // ==========================================
            // 4. BIỂU ĐỒ MƯỢN - TRẢ SÁCH
            // ==========================================

            if (chartMuonTra != null)
            {
                // Xóa Series cũ để tránh lỗi không tìm thấy tên Series.
                chartMuonTra.Series.Clear();

                // Đảm bảo biểu đồ có ít nhất một ChartArea.
                if (chartMuonTra.ChartAreas.Count == 0)
                {
                    chartMuonTra.ChartAreas.Add("ChartArea1");
                }

                // Đảm bảo biểu đồ có ít nhất một Legend.
                if (chartMuonTra.Legends.Count == 0)
                {
                    chartMuonTra.Legends.Add("Legend1");
                }

                // Tạo hai nhóm dữ liệu mới.
                var seriesMuon =
                    chartMuonTra.Series.Add("SeriesMuon");

                var seriesTra =
                    chartMuonTra.Series.Add("SeriesTra");

                // Cấu hình Series mượn.
                seriesMuon.LegendText = "Số lượt mượn";
                seriesMuon.Color = Color.RoyalBlue;
                seriesMuon.ChartType =
                    System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Column;

                // Cấu hình Series trả.
                seriesTra.LegendText = "Số lượt trả";
                seriesTra.Color = Color.SeaGreen;
                seriesTra.ChartType =
                    System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Column;

                // Dữ liệu minh họa trong 6 tháng.
                int[] soLuotMuon = { 24, 28, 28, 34, 37, 44 };
                int[] soLuotTra = { 20, 26, 23, 29, 34, 42 };

                for (int i = 0; i < 6; i++)
                {
                    string thang = "Tháng " + (i + 1);

                    seriesMuon.Points.AddXY(
                        thang, soLuotMuon[i]);

                    seriesTra.Points.AddXY(
                        thang, soLuotTra[i]);
                }

                // Cấu hình trục biểu đồ.
                var areaMuonTra = chartMuonTra.ChartAreas[0];

                areaMuonTra.AxisX.Interval = 1;
                areaMuonTra.AxisX.MajorGrid.Enabled = false;
                areaMuonTra.AxisX.LabelStyle.Angle = 0;

                areaMuonTra.AxisY.Minimum = 0;
                areaMuonTra.AxisY.Maximum = 50;
                areaMuonTra.AxisY.Interval = 10;
                areaMuonTra.AxisY.Title = "Số lượt";
                areaMuonTra.AxisY.MajorGrid.Enabled = true;

                // Cấu hình chú thích.
                var legendMuonTra = chartMuonTra.Legends[0];

                legendMuonTra.Enabled = true;
                legendMuonTra.Docking =
                    System.Windows.Forms.DataVisualization.Charting.Docking.Right;

                legendMuonTra.Alignment =
                    System.Drawing.StringAlignment.Center;

                legendMuonTra.Font =
                    new Font("Segoe UI", 9F, FontStyle.Regular);

                // Tự động căn chỉnh vùng biểu đồ.
                areaMuonTra.Position.Auto = true;
                areaMuonTra.InnerPlotPosition.Auto = true;

                chartMuonTra.Visible = true;
                chartMuonTra.Invalidate();
            }

            // ==========================================
            // 5. BIỂU ĐỒ THỂ LOẠI SÁCH
            // ==========================================

            if (chartTheLoai != null)
            {
                // Xóa dữ liệu biểu đồ cũ.
                chartTheLoai.Series.Clear();

                // Đảm bảo có vùng hiển thị.
                if (chartTheLoai.ChartAreas.Count == 0)
                {
                    chartTheLoai.ChartAreas.Add("ChartArea1");
                }

                // Đảm bảo có chú thích.
                if (chartTheLoai.Legends.Count == 0)
                {
                    chartTheLoai.Legends.Add("Legend1");
                }

                // Tạo Series mới.
                var seriesTheLoai =
                    chartTheLoai.Series.Add("TheLoai");

                seriesTheLoai.ChartType =
                    System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Doughnut;

                seriesTheLoai.IsValueShownAsLabel = true;
                seriesTheLoai.Label = "#PERCENT{P0}";
                seriesTheLoai.LegendText = "#VALX";

                // Đường viền trắng giữa các phần biểu đồ.
                seriesTheLoai.BorderWidth = 1;
                seriesTheLoai.BorderColor = Color.White;

                // Dữ liệu minh họa.
                seriesTheLoai.Points.AddXY("Văn học", 28);
                seriesTheLoai.Points.AddXY("Khoa học", 18);
                seriesTheLoai.Points.AddXY("Lịch sử", 15);
                seriesTheLoai.Points.AddXY("Kỹ thuật", 12);
                seriesTheLoai.Points.AddXY("Thiếu nhi", 10);
                seriesTheLoai.Points.AddXY("Khác", 17);

                // Màu sắc từng thể loại.
                seriesTheLoai.Points[0].Color = Color.RoyalBlue;
                seriesTheLoai.Points[1].Color = Color.MediumSeaGreen;
                seriesTheLoai.Points[2].Color = Color.Orange;
                seriesTheLoai.Points[3].Color = Color.MediumPurple;
                seriesTheLoai.Points[4].Color = Color.Gold;
                seriesTheLoai.Points[5].Color = Color.LightSteelBlue;

                // Cấu hình chú thích bên phải.
                var legendTheLoai = chartTheLoai.Legends[0];

                legendTheLoai.Enabled = true;
                legendTheLoai.Docking =
                    System.Windows.Forms.DataVisualization.Charting.Docking.Right;

                legendTheLoai.Alignment =
                    System.Drawing.StringAlignment.Center;

                // Căn chỉnh vùng hiển thị.
                var areaTheLoai = chartTheLoai.ChartAreas[0];

                areaTheLoai.Position.Auto = true;
                areaTheLoai.InnerPlotPosition.Auto = true;

                chartTheLoai.Visible = true;
                chartTheLoai.Invalidate();
            }
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
            {
                if (!IsDisposed && !Disposing)
                    CapNhatThongKe();
            };

            form.Show();
        }


        private void FrmMain_Activated(object sender, EventArgs e)
        {
            CapNhatThongKe();
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
            // Nếu đang đăng xuất hoặc đã xác nhận thoát,
            // cho phép đóng mà không hỏi thêm.
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
                DongTatCaFormChucNang();
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
            MoFormChucNang(new FrmSach());
        }

        private void pnlTheQuanLyDocGia_Click(object sender, EventArgs e)
        {
            MoFormChucNang(new FrmDocGia());
        }

        private void pnlTheQuanLyNhanVien_Click(object sender, EventArgs e)
        {
            MoFormChucNang(new FrmNhanVien());
        }

        private void pnlTheLapPhieuMuon_Click(object sender, EventArgs e)
        {
            MoFormChucNang(new FrmLapPhieuMuon());
        }

        private void pnlTheTraSach_Click(object sender, EventArgs e)
        {
            MoFormChucNang(new FrmTraSach());
        }

        private void pnlTheTimKiemSach_Click(object sender, EventArgs e)
        {
            MoFormChucNang(new FrmTimKiemSach());
        }




        private void DongTatCaFormCon()
        {
            // Lấy danh sách tất cả Form chức năng đang mở.
            List<Form> danhSachForm = Application.OpenForms
                .Cast<Form>()
                .Where(f => f != this && !(f is FrmLogin))
                .ToList();

            // Đóng tất cả Form chức năng.
            foreach (Form form in danhSachForm)
            {
                if (!form.IsDisposed)
                {
                    form.Close();
                }
            }
        }

        private void btnTrangChu_Click(object sender, EventArgs e)
        {
            DongTatCaFormCon();
        }


        private void btnQuanLySach_Click(object sender, EventArgs e)
        {
            if (frmSach == null || frmSach.IsDisposed)
            {
                frmSach = new FrmSach();
                frmSach.FormClosed += (s, args) => frmSach = null;
                frmSach.Show();
            }
            else
            {
                frmSach.Activate();
            }
        }

        private void btnQuanLyTheLoai_Click(object sender, EventArgs e)
        {
            if (frmTheLoai == null || frmTheLoai.IsDisposed)
            {
                frmTheLoai = new FrmTheLoai();
                frmTheLoai.FormClosed += (s, args) => frmTheLoai = null;
                frmTheLoai.Show();
            }
            else
            {
                frmTheLoai.Activate();
            }
        }

        private void btnQuanLyTacGia_Click(object sender, EventArgs e)
        {
            if (frmTacGia == null || frmTacGia.IsDisposed)
            {
                frmTacGia = new FrmTacGia();
                frmTacGia.FormClosed += (s, args) => frmTacGia = null;
                frmTacGia.Show();
            }
            else
            {
                frmTacGia.Activate();
            }
        }

        private void btnQuanLyDocGia_Click(object sender, EventArgs e)
        {
            if (frmDocGia == null || frmDocGia.IsDisposed)
            {
                frmDocGia = new FrmDocGia();
                frmDocGia.FormClosed += (s, args) => frmDocGia = null;
                frmDocGia.Show();
            }
            else
            {
                frmDocGia.Activate();
            }
        }

        private void btnQuanLyNhanVien_Click(object sender, EventArgs e)
        {
            if (frmNhanVien == null || frmNhanVien.IsDisposed)
            {
                frmNhanVien = new FrmNhanVien();
                frmNhanVien.FormClosed += (s, args) => frmNhanVien = null;
                frmNhanVien.Show();
            }
            else
            {
                frmNhanVien.Activate();
            }
        }


        private FrmTimKiemSach frmTimKiemSach;
        private FrmThongTinDocGia frmThongTinDocGia;

        private void btnTimKiemSach_Click(object sender, EventArgs e)
        {
            if (frmTimKiemSach == null || frmTimKiemSach.IsDisposed)
            {
                frmTimKiemSach = new FrmTimKiemSach();
                frmTimKiemSach.FormClosed += (s, args) => frmTimKiemSach = null;
                frmTimKiemSach.Show();
            }
            else
            {
                frmTimKiemSach.Activate();
            }
        }

        private void btnThongTinDocGia_Click(object sender, EventArgs e)
        {
            if (frmThongTinDocGia == null || frmThongTinDocGia.IsDisposed)
            {
                frmThongTinDocGia = new FrmThongTinDocGia();
                frmThongTinDocGia.FormClosed += (s, args) => frmThongTinDocGia = null;
                frmThongTinDocGia.Show();
            }
            else
            {
                frmThongTinDocGia.Activate();
            }
        }



        private FrmLapPhieuMuon frmLapPhieuMuon;
        private FrmTraSach frmTraSach;
        private FrmDanhSachPhieuMuon frmDanhSachPhieuMuon;



        private void btnLapPhieuMuon_Click(object sender, EventArgs e)
        {
            if (frmLapPhieuMuon == null || frmLapPhieuMuon.IsDisposed)
            {
                frmLapPhieuMuon = new FrmLapPhieuMuon();
                frmLapPhieuMuon.FormClosed += (s, args) => frmLapPhieuMuon = null;
                frmLapPhieuMuon.Show();
            }
            else
            {
                frmLapPhieuMuon.Activate();
            }
        }

        private void btnTraSach_Click(object sender, EventArgs e)
        {
            if (frmTraSach == null || frmTraSach.IsDisposed)
            {
                frmTraSach = new FrmTraSach();
                frmTraSach.FormClosed += (s, args) => frmTraSach = null;
                frmTraSach.Show();
            }
            else
            {
                frmTraSach.Activate();
            }
        }

        private void btnDanhSachPhieuMuon_Click(object sender, EventArgs e)
        {
            if (frmDanhSachPhieuMuon == null || frmDanhSachPhieuMuon.IsDisposed)
            {
                frmDanhSachPhieuMuon = new FrmDanhSachPhieuMuon();
                frmDanhSachPhieuMuon.FormClosed += (s, args) => frmDanhSachPhieuMuon = null;
                frmDanhSachPhieuMuon.Show();
            }
            else
            {
                frmDanhSachPhieuMuon.Activate();
            }
        }



        private FrmThongTinChuongTrinh frmThongTinChuongTrinh;
        private FrmHuongDanSuDung frmHuongDanSuDung;



        private void btnThongTinChuongTrinh_Click(object sender, EventArgs e)
        {
            if (frmThongTinChuongTrinh == null ||
                frmThongTinChuongTrinh.IsDisposed)
            {
                frmThongTinChuongTrinh = new FrmThongTinChuongTrinh();

                frmThongTinChuongTrinh.FormClosed +=
                    (s, args) => frmThongTinChuongTrinh = null;

                frmThongTinChuongTrinh.Show();
            }
            else
            {
                frmThongTinChuongTrinh.Activate();
            }
        }

        private void btnHuongDanSuDung_Click(object sender, EventArgs e)
        {
            if (frmHuongDanSuDung == null ||
                frmHuongDanSuDung.IsDisposed)
            {
                frmHuongDanSuDung = new FrmHuongDanSuDung();

                frmHuongDanSuDung.FormClosed +=
                    (s, args) => frmHuongDanSuDung = null;

                frmHuongDanSuDung.Show();
            }
            else
            {
                frmHuongDanSuDung.Activate();
            }
        }



        private void btnDangXuat_Click(object sender, EventArgs e)
        {
            DialogResult ketQua = MessageBox.Show(
                "Bạn có chắc chắn muốn đăng xuất không?",
                "Xác nhận đăng xuất",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (ketQua != DialogResult.Yes)
                return;

            FrmLogin frmLogin = this.Tag as FrmLogin;

            if (frmLogin == null || frmLogin.IsDisposed)
            {
                MessageBox.Show(
                    "Không tìm thấy màn hình đăng nhập.",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            // Đánh dấu đang đăng xuất để FormClosing không hỏi thoát.
            dangDangXuat = true;

            // Đóng các form chức năng đang mở.
            DongTatCaFormChucNang();

            // Hiện lại màn hình đăng nhập.
            frmLogin.Show();
            frmLogin.Activate();

            // Đóng màn hình chính.
            this.Close();
        }





        private void btnThoat_Click(object sender, EventArgs e)
        {
            DialogResult ketQua = MessageBox.Show(
                "Bạn có chắc chắn muốn thoát toàn bộ chương trình không?",
                "Xác nhận thoát",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (ketQua != DialogResult.Yes)
                return;

            // Cho phép đóng FrmMain mà không hỏi xác nhận lần nữa.
            dangXacNhanThoat = true;

            // Đóng các form chức năng đang mở.
            DongTatCaFormChucNang();

            // Đóng toàn bộ ứng dụng.
            Application.Exit();
        }

    }
}