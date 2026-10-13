using DV = System.Windows.Forms.DataVisualization.Charting;
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

            AppTheme.ApplyForm(this);

            this.StartPosition = FormStartPosition.CenterScreen;
            this.WindowState = FormWindowState.Maximized;
            this.MinimumSize = new System.Drawing.Size(1000, 650);

            // Chỉ giữ thanh menu ở phía trên.
            this.menuStrip1.Dock = DockStyle.Top;

            ApDungGiaoDien();
        }


        // ================= GIAO DIỆN =================

        private readonly Font fontNhom = new Font("Segoe UI", 9F, FontStyle.Bold);
        private readonly Font fontMoTa = new Font("Segoe UI", 8.5F);
        private readonly Color mauMucSidebar = ColorTranslator.FromHtml("#DBEAFE");
        private readonly Color mauNhomSidebar = ColorTranslator.FromHtml("#93C5FD");
        private readonly Color mauHoverSidebar = ColorTranslator.FromHtml("#2D4A9E");
        private readonly Color mauNenIcon = ColorTranslator.FromHtml("#DBEAFE");
        private readonly Color mauHoverThe = ColorTranslator.FromHtml("#F5F9FF");

        private void ApDungGiaoDien()
        {
            foreach (Control c in new Control[] { pnlContent, pnlHeader, pnlSidebar })
                AppTheme.SetDoubleBuffered(c);

            this.AutoScroll = false;

            // ----- Menu trên cùng -----
            menuStrip1.Renderer = new ToolStripProfessionalRenderer(new MenuColors());
            menuStrip1.BackColor = Color.White;
            menuStrip1.ForeColor = AppTheme.TextDark;

            // ----- Thanh bên -----
            pnlSidebar.BackColor = AppTheme.Primary;

            AppTheme.StyleButton(btnTrangChu, AppTheme.Accent, AppTheme.AccentHover, null, 0);
            btnTrangChu.Padding = new Padding(14, 0, 0, 0);

            StyleTieuDeNhom(btnQuanLy, "QUẢN LÝ");
            StyleTieuDeNhom(btnTraCuu, "TRA CỨU");
            StyleTieuDeNhom(btnMuonTra, "MƯỢN - TRẢ");
            StyleTieuDeNhom(btnTroGiup, "TRỢ GIÚP");
            StyleTieuDeNhom(btnHeThong, "HỆ THỐNG");

            foreach (Button b in new Button[]
            {
        btnQuanLySach, btnQuanLyTheLoai, btnQuanLyTacGia, btnQuanLyDocGia, btnQuanLyNhanVien,
        btnTimKiemSach, btnThongTinDocGia,
        btnLapPhieuMuon, btnTraSach, btnDanhSachPhieuMuon,
        btnThongTinChuongTrinh, btnHuongDanSuDung,
        btnDangXuat, btnThoat
            })
                StyleMucSidebar(b);

            // Chừa khoảng trống giữa các nhóm
            foreach (Panel p in new Panel[] { pnlNhomQuanLy, pnlNhomTraCuu, pnlNhomMuonTra, pnlNhomTroGiup, pnlNhomHeThong })
                p.Height += 12;

            // ----- Thanh tiêu đề -----
            pnlHeader.BackColor = Color.White;
            pnlHeader.Paint += (s, e) =>
            {
                using (var pen = new Pen(AppTheme.Border))
                    e.Graphics.DrawLine(pen, 0, pnlHeader.Height - 1, pnlHeader.Width, pnlHeader.Height - 1);
            };

            lblTieuDeTrang.ForeColor = AppTheme.Primary;
            foreach (Button b in new Button[] { btnMenu, btnThongBao })
            {
                b.FlatStyle = FlatStyle.Flat;
                b.FlatAppearance.BorderSize = 0;
                b.FlatAppearance.MouseOverBackColor = AppTheme.AccentLight;
                b.FlatAppearance.MouseDownBackColor = AppTheme.AccentLight;
                b.ForeColor = AppTheme.Primary;
                b.Cursor = Cursors.Hand;
                b.UseVisualStyleBackColor = false;
                b.BackColor = Color.White;
            }
            picDaiDien.BorderStyle = BorderStyle.None;
            lblHoTenNguoiDung.ForeColor = AppTheme.TextDark;
            lblHoTenNguoiDung.Font = AppTheme.Bold;
            lblChucVuNguoiDung.ForeColor = AppTheme.TextMuted;

            // ----- Vùng nội dung -----
            pnlContent.BackColor = AppTheme.Background;
            pnlContent.AutoScroll = true;

            lblLoiChao.ForeColor = AppTheme.Primary;
            lblMoTaTrangChu.ForeColor = AppTheme.TextMuted;
            lblCauTrichDan.ForeColor = AppTheme.TextMuted;
            lblCauTrichDan.TextAlign = ContentAlignment.MiddleRight;
            lblCauTrichDan.Width = 380;
            lblThoiGianHienTai.ForeColor = AppTheme.Accent;

            // ----- 4 thẻ thống kê -----
            StyleTheThongKe(pnlCardSach, lblTieuDeSach, lblSoSach, AppTheme.Accent);
            StyleTheThongKe(pnlCardDocGia, lblTieuDeDocGia, lblSoDocGia, AppTheme.Success);
            StyleTheThongKe(pnlCardDangMuon, lblTieuDeDangMuon, lblSoDangMuon, AppTheme.Amber);
            StyleTheThongKe(pnlCardQuaHan, lblTieuDeQuaHan, lblSoQuaHan, AppTheme.Danger);

            // ----- Biểu đồ -----
            AppTheme.StyleCard(pnlBieuDoMuonTra);
            AppTheme.StyleCard(pnlBieuDoTheLoai);
            StyleTieuDeKhoi(lblTieuDeMuonTra);
            StyleTieuDeKhoi(lblTieuDeTheLoai);

            // chartTheLoai đang nằm đè lên tiêu đề: đặt lại vị trí giống chartMuonTra
            chartTheLoai.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            chartTheLoai.Location = new Point(12, 56);
            chartTheLoai.Size = new Size(
                pnlBieuDoTheLoai.ClientSize.Width - 24,
                pnlBieuDoTheLoai.ClientSize.Height - 68);

            // ----- Truy cập nhanh -----
            AppTheme.StyleCard(pnlThaoTacNhanh);
            StyleTieuDeKhoi(lblTieuDeTruyCapNhanh);

            StyleTheTruyCap(pnlTheQuanLySach, lblTenQuanLySach, lblMoTaQuanLySach, picQuanLySach, pnlTheQuanLySach_Click);
            StyleTheTruyCap(pnlTheQuanLyDocGia, lblTenQuanLyDocGia, lblMoTaQuanLyDocGia, picQuanLyDocGia, pnlTheQuanLyDocGia_Click);
            StyleTheTruyCap(pnlTheQuanLyNhanVien, lblTenQuanLyNhanVien, lblMoTaQuanLyNhanVien, picQuanLyNhanVien, pnlTheQuanLyNhanVien_Click);
            StyleTheTruyCap(pnlTheLapPhieuMuon, lblTenLapPhieuMuon, lblMoTaLapPhieuMuon, picLapPhieuMuon, pnlTheLapPhieuMuon_Click);
            StyleTheTruyCap(pnlTheTraSach, lblTenTraSach, lblMoTaTraSach, picTraSach, pnlTheTraSach_Click);
            StyleTheTruyCap(pnlTheTimKiemSach, lblTenTimKiemSach, lblMoTaTimKiemSach, picTimKiemSach, pnlTheTimKiemSach_Click);

            // ----- Hoạt động gần đây -----
            AppTheme.StyleCard(pnlHoatDongGanDay);
            StyleTieuDeKhoi(lblTieuDeHoatDongGanDay);
            AppTheme.StyleGrid(dgvHoatDongGanDay);

            // ----- Căn lề nội dung -----
            pnlContent.SizeChanged += (s, e) => CanLeNoiDung();
            CanLeNoiDung();
        }

        private void CanLeNoiDung()
        {
            const int le = 38;
            int w = Math.Max(700, pnlContent.ClientSize.Width - 2 * le);

            foreach (Control c in new Control[] { tlpThongKe, tlpBieuDo, pnlThaoTacNhanh, pnlHoatDongGanDay })
            {
                c.Left = le;
                c.Width = w;
            }

            lblThoiGianHienTai.Left = (pnlContent.ClientSize.Width - lblThoiGianHienTai.Width) / 2;
            lblCauTrichDan.Left = pnlContent.ClientSize.Width - le - lblCauTrichDan.Width;
        }

        private void StyleTieuDeNhom(Button b, string text)
        {
            b.Text = text;
            b.FlatStyle = FlatStyle.Flat;
            b.FlatAppearance.BorderSize = 0;
            b.FlatAppearance.MouseOverBackColor = AppTheme.Primary;   // tiêu đề nhóm: không đổi màu khi rê chuột
            b.FlatAppearance.MouseDownBackColor = AppTheme.Primary;
            b.UseVisualStyleBackColor = false;
            b.BackColor = AppTheme.Primary;
            b.ForeColor = mauNhomSidebar;
            b.Font = fontNhom;
            b.Cursor = Cursors.Default;
            b.TabStop = false;
            b.Padding = new Padding(14, 0, 0, 0);
        }

        private void StyleMucSidebar(Button b)
        {
            b.FlatStyle = FlatStyle.Flat;
            b.FlatAppearance.BorderSize = 0;
            b.FlatAppearance.MouseOverBackColor = mauHoverSidebar;
            b.FlatAppearance.MouseDownBackColor = AppTheme.Accent;
            b.UseVisualStyleBackColor = false;
            b.BackColor = AppTheme.Primary;
            b.ForeColor = mauMucSidebar;
            b.Font = AppTheme.Base;
            b.Cursor = Cursors.Hand;
        }

        private void StyleTieuDeKhoi(Label lbl)
        {
            lbl.Font = AppTheme.Heading;
            lbl.ForeColor = AppTheme.Primary;
        }

        private void StyleTheThongKe(Panel card, Label tieuDe, Label so, Color mau)
        {
            AppTheme.StyleCard(card);
            tieuDe.Font = AppTheme.Base;
            tieuDe.ForeColor = AppTheme.TextMuted;
            so.Font = AppTheme.Number;
            so.ForeColor = mau;

            // Vạch màu bên trái thẻ
            card.Paint += (s, e) =>
            {
                using (var brush = new SolidBrush(mau))
                    e.Graphics.FillRectangle(brush, 0, 0, 5, card.Height);
            };
        }

        private void StyleTheTruyCap(Panel card, Label ten, Label moTa, PictureBox pic, EventHandler khiBam)
        {
            AppTheme.StyleCard(card);

            ten.Font = AppTheme.Bold;
            ten.ForeColor = AppTheme.TextDark;
            moTa.Font = fontMoTa;
            moTa.ForeColor = AppTheme.TextMuted;
            pic.BackColor = mauNenIcon;

            AppTheme.AddHover(card, Color.White, mauHoverThe);

            // Bấm vào ảnh/chữ trong thẻ cũng mở form (dùng lại đúng hàm đã có)
            foreach (Control child in card.Controls)
                child.Click += khiBam;
        }

        /// <summary>Màu cho menu trên cùng, đồng bộ với AppTheme.</summary>
        private class MenuColors : ProfessionalColorTable
        {
            public MenuColors() { UseSystemColors = false; }

            public override Color MenuItemSelected { get { return AppTheme.AccentLight; } }
            public override Color MenuItemBorder { get { return AppTheme.AccentLight; } }
            public override Color MenuItemSelectedGradientBegin { get { return AppTheme.AccentLight; } }
            public override Color MenuItemSelectedGradientEnd { get { return AppTheme.AccentLight; } }
            public override Color MenuItemPressedGradientBegin { get { return AppTheme.AccentLight; } }
            public override Color MenuItemPressedGradientEnd { get { return AppTheme.AccentLight; } }
            public override Color MenuBorder { get { return AppTheme.Border; } }
            public override Color ToolStripDropDownBackground { get { return Color.White; } }
            public override Color ImageMarginGradientBegin { get { return Color.White; } }
            public override Color ImageMarginGradientMiddle { get { return Color.White; } }
            public override Color ImageMarginGradientEnd { get { return Color.White; } }
            public override Color MenuStripGradientBegin { get { return Color.White; } }
            public override Color MenuStripGradientEnd { get { return Color.White; } }
            public override Color SeparatorDark { get { return AppTheme.Border; } }
        }

        // ================= BIỂU ĐỒ =================

        private void VeBieuDoMuonTra()
        {
            if (chartMuonTra == null) return;

            chartMuonTra.Series.Clear();
            chartMuonTra.ChartAreas.Clear();
            chartMuonTra.Legends.Clear();
            chartMuonTra.BackColor = Color.White;

            DV.ChartArea area = chartMuonTra.ChartAreas.Add("ChartArea1");
            DV.Legend legend = chartMuonTra.Legends.Add("Legend1");

            area.BackColor = Color.White;
            area.AxisX.Interval = 1;
            area.AxisX.MajorGrid.Enabled = false;
            area.AxisX.MajorTickMark.Enabled = false;
            area.AxisX.LineColor = AppTheme.Border;
            area.AxisX.LabelStyle.Font = new Font("Segoe UI", 9F);
            area.AxisX.LabelStyle.ForeColor = AppTheme.TextMuted;

            area.AxisY.Minimum = 0;
            area.AxisY.Maximum = 50;
            area.AxisY.Interval = 10;
            area.AxisY.Title = "Số lượt";
            area.AxisY.TitleFont = new Font("Segoe UI", 9F);
            area.AxisY.TitleForeColor = AppTheme.TextMuted;
            area.AxisY.MajorGrid.LineColor = AppTheme.Border;
            area.AxisY.MajorTickMark.Enabled = false;
            area.AxisY.LineColor = Color.Transparent;
            area.AxisY.LabelStyle.Font = new Font("Segoe UI", 9F);
            area.AxisY.LabelStyle.ForeColor = AppTheme.TextMuted;

            legend.Enabled = true;
            legend.Docking = DV.Docking.Bottom;
            legend.Alignment = StringAlignment.Center;
            legend.Font = new Font("Segoe UI", 9F);
            legend.ForeColor = AppTheme.TextDark;
            legend.BackColor = Color.White;

            DV.Series seriesMuon = chartMuonTra.Series.Add("SeriesMuon");
            DV.Series seriesTra = chartMuonTra.Series.Add("SeriesTra");

            foreach (DV.Series s in new DV.Series[] { seriesMuon, seriesTra })
            {
                s.ChartArea = "ChartArea1";
                s.Legend = "Legend1";
                s.ChartType = DV.SeriesChartType.Column;
                s["PointWidth"] = "0.55";
            }

            seriesMuon.LegendText = "Số lượt mượn";
            seriesMuon.Color = AppTheme.ChartPalette[0];
            seriesTra.LegendText = "Số lượt trả";
            seriesTra.Color = AppTheme.ChartPalette[1];

            // Dữ liệu minh họa trong 6 tháng.
            int[] soLuotMuon = { 24, 28, 28, 34, 37, 44 };
            int[] soLuotTra = { 20, 26, 23, 29, 34, 42 };

            for (int i = 0; i < 6; i++)
            {
                string thang = "Tháng " + (i + 1);
                seriesMuon.Points.AddXY(thang, soLuotMuon[i]);
                seriesTra.Points.AddXY(thang, soLuotTra[i]);
            }

            chartMuonTra.Visible = true;
            chartMuonTra.Invalidate();
        }

        private void VeBieuDoTheLoai()
        {
            if (chartTheLoai == null) return;

            chartTheLoai.Series.Clear();
            chartTheLoai.ChartAreas.Clear();
            chartTheLoai.Legends.Clear();
            chartTheLoai.BackColor = Color.White;

            DV.ChartArea area = chartTheLoai.ChartAreas.Add("ChartArea1");
            DV.Legend legend = chartTheLoai.Legends.Add("Legend1");

            area.BackColor = Color.White;

            legend.Enabled = true;
            legend.Docking = DV.Docking.Right;
            legend.Alignment = StringAlignment.Center;
            legend.Font = new Font("Segoe UI", 10F);
            legend.ForeColor = AppTheme.TextDark;
            legend.BackColor = Color.White;

            DV.Series s = chartTheLoai.Series.Add("TheLoai");
            s.ChartArea = "ChartArea1";
            s.Legend = "Legend1";
            s.ChartType = DV.SeriesChartType.Doughnut;
            s["DoughnutRadius"] = "55";          // lỗ giữa to hơn cho thanh thoát
            s.IsValueShownAsLabel = true;
            s.Label = "#PERCENT{P0}";
            s.LegendText = "#VALX";
            s.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            s.LabelForeColor = Color.White;
            s.BorderWidth = 2;
            s.BorderColor = Color.White;

            // Dữ liệu minh họa.
            s.Points.AddXY("Văn học", 28);
            s.Points.AddXY("Khoa học", 18);
            s.Points.AddXY("Lịch sử", 15);
            s.Points.AddXY("Kỹ thuật", 12);
            s.Points.AddXY("Thiếu nhi", 10);
            s.Points.AddXY("Khác", 17);

            for (int i = 0; i < s.Points.Count; i++)
                s.Points[i].Color = AppTheme.ChartPalette[i % AppTheme.ChartPalette.Length];

            chartTheLoai.Visible = true;
            chartTheLoai.Invalidate();
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

            VeBieuDoMuonTra();
            VeBieuDoTheLoai();
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