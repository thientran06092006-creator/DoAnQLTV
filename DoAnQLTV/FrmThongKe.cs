using DV = System.Windows.Forms.DataVisualization.Charting;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace DoAnQLTV
{
    /// <summary>
    /// Thống kê chi tiết, gồm 3 tab:
    ///  1. Top 10 sách được mượn nhiều nhất
    ///  2. Top 10 độc giả mượn nhiều nhất
    ///  3. Tiền phạt theo tháng (đã thu / chưa thu)
    /// Lọc theo khoảng ngày (mặc định 12 tháng gần nhất). Form tạo hoàn toàn bằng code.
    /// </summary>
    public class FrmThongKe : Form
    {
        private const int SoThangToiDa = 24;

        private static readonly Color MauThu = ColorTranslator.FromHtml("#16A34A");
        private static readonly Color MauChua = ColorTranslator.FromHtml("#DC2626");

        private DateTimePicker dtpTu;
        private DateTimePicker dtpDen;
        private Button btnXem;
        private TabControl tab;

        private DV.Chart chartSach;
        private DV.Chart chartDocGia;
        private DV.Chart chartPhat;
        private DataGridView dgvSach;
        private DataGridView dgvDocGia;
        private DataGridView dgvPhat;

        public FrmThongKe()
        {
            Name = "FrmThongKe";
            Text = "Thống Kê Chi Tiết";
            AutoScaleDimensions = new SizeF(96F, 96F);
            AutoScaleMode = AutoScaleMode.Dpi;
            ClientSize = new Size(1000, 630);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            StartPosition = FormStartPosition.CenterScreen;

            AppTheme.ApplyForm(this);
            KhoiTaoGiaoDien();
            Xem();
        }

        // ================= GIAO DIỆN =================

        private Label TaoNhan(Control cha, string text, int x, int y)
        {
            Label lbl = new Label();
            lbl.Text = text;
            lbl.AutoSize = true;
            lbl.Font = AppTheme.Bold;
            lbl.ForeColor = AppTheme.Primary;
            lbl.BackColor = Color.Transparent;
            lbl.Location = new Point(x, y);
            cha.Controls.Add(lbl);
            return lbl;
        }

        private DateTimePicker TaoNgay(Control cha, int x, int y, int rong)
        {
            DateTimePicker dtp = new DateTimePicker();
            dtp.Format = DateTimePickerFormat.Custom;
            dtp.CustomFormat = "dd/MM/yyyy";
            dtp.Font = new Font("Segoe UI", 11F);
            dtp.SetBounds(x, y, rong, 27);
            cha.Controls.Add(dtp);
            return dtp;
        }

        private void KhoiTaoGiaoDien()
        {
            Label tieuDe = new Label();
            tieuDe.Text = "THỐNG KÊ CHI TIẾT";
            tieuDe.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            tieuDe.ForeColor = AppTheme.Primary;
            tieuDe.BackColor = Color.Transparent;
            tieuDe.TextAlign = ContentAlignment.MiddleLeft;
            tieuDe.SetBounds(30, 14, 600, 40);
            Controls.Add(tieuDe);

            // ----- Bộ lọc ngày -----
            Panel pnlLoc = new Panel();
            pnlLoc.SetBounds(30, 64, 940, 76);
            Controls.Add(pnlLoc);
            AppTheme.StyleCard(pnlLoc);

            TaoNhan(pnlLoc, "Từ ngày", 24, 28);
            dtpTu = TaoNgay(pnlLoc, 100, 24, 150);

            TaoNhan(pnlLoc, "Đến ngày", 270, 28);
            dtpDen = TaoNgay(pnlLoc, 350, 24, 150);

            DateTime homNay = DateTime.Today;
            dtpDen.Value = homNay;
            dtpTu.Value = new DateTime(homNay.Year, homNay.Month, 1).AddMonths(-11);

            btnXem = new Button();
            btnXem.Text = "Xem thống kê";
            btnXem.SetBounds(530, 18, 140, 40);
            btnXem.Click += btnXem_Click;
            pnlLoc.Controls.Add(btnXem);
            AppTheme.StyleSuccessButton(btnXem);

            Label ghiChu = new Label();
            ghiChu.AutoSize = true;
            ghiChu.Font = AppTheme.Base;
            ghiChu.ForeColor = AppTheme.TextMuted;
            ghiChu.BackColor = Color.Transparent;
            ghiChu.Text = "Khoảng ngày áp dụng cho cả 3 tab";
            ghiChu.Location = new Point(690, 29);
            pnlLoc.Controls.Add(ghiChu);

            // ----- 3 tab -----
            tab = new TabControl();
            tab.Font = AppTheme.Base;
            tab.SetBounds(30, 160, 940, 440);
            Controls.Add(tab);

            chartSach = TaoChartTop("Lượt mượn", ColorTranslator.FromHtml("#2563EB"));
            dgvSach = TaoLuoiTop("Mã sách", "Tên sách");
            tab.TabPages.Add(TaoTrangTop("Top sách được mượn nhiều", chartSach, dgvSach));

            chartDocGia = TaoChartTop("Lượt mượn", ColorTranslator.FromHtml("#16A34A"));
            dgvDocGia = TaoLuoiTop("Mã độc giả", "Tên độc giả");
            tab.TabPages.Add(TaoTrangTop("Top độc giả mượn nhiều", chartDocGia, dgvDocGia));

            chartPhat = TaoChartPhat();
            dgvPhat = TaoLuoiPhat();
            tab.TabPages.Add(TaoTrangPhat("Tiền phạt theo tháng", chartPhat, dgvPhat));
        }

        // ----- Biểu đồ cột ngang cho "Top" -----
        private DV.Chart TaoChartTop(string tenSeries, Color mau)
        {
            DV.Chart ch = new DV.Chart();
            ch.BackColor = Color.White;

            DV.ChartArea area = ch.ChartAreas.Add("area");
            area.BackColor = Color.White;
            area.AxisX.Interval = 1;
            area.AxisX.IsReversed = true;                 // hạng 1 nằm trên cùng
            area.AxisX.MajorGrid.Enabled = false;
            area.AxisX.MajorTickMark.Enabled = false;
            area.AxisX.LabelStyle.Font = new Font("Segoe UI", 8.5F);
            area.AxisX.LabelStyle.ForeColor = AppTheme.TextMuted;
            area.AxisY.Minimum = 0;
            area.AxisY.LabelStyle.Format = "0";
            area.AxisY.LabelStyle.Font = new Font("Segoe UI", 8.5F);
            area.AxisY.LabelStyle.ForeColor = AppTheme.TextMuted;
            area.AxisY.MajorGrid.LineColor = ColorTranslator.FromHtml("#E5EAF3");
            area.AxisY.MajorTickMark.Enabled = false;

            DV.Series s = ch.Series.Add(tenSeries);
            s.ChartArea = "area";
            s.ChartType = DV.SeriesChartType.Bar;
            s.Color = mau;
            s.IsValueShownAsLabel = true;
            s.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            s["PointWidth"] = "0.6";
            return ch;
        }

        private DataGridView TaoLuoiTop(string tieuDeMa, string tieuDeTen)
        {
            DataGridView dgv = new DataGridView();
            ThemCot(dgv, "colHang", "Hạng", 12F);
            ThemCot(dgv, "colMa", tieuDeMa, 22F);
            ThemCot(dgv, "colTen", tieuDeTen, 48F);
            ThemCot(dgv, "colLuot", "Lượt", 14F);
            AppTheme.StyleGrid(dgv);
            return dgv;
        }

        private TabPage TaoTrangTop(string tieuDe, DV.Chart chart, DataGridView dgv)
        {
            TabPage trang = new TabPage(tieuDe);
            trang.UseVisualStyleBackColor = false;
            trang.BackColor = Color.White;

            chart.SetBounds(6, 6, 560, 392);
            chart.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            dgv.SetBounds(574, 6, 352, 392);
            dgv.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;

            trang.Controls.Add(chart);
            trang.Controls.Add(dgv);
            return trang;
        }

        // ----- Biểu đồ cột chồng cho "Tiền phạt" -----
        private DV.Chart TaoChartPhat()
        {
            DV.Chart ch = new DV.Chart();
            ch.BackColor = Color.White;

            DV.ChartArea area = ch.ChartAreas.Add("area");
            area.BackColor = Color.White;
            area.AxisX.Interval = 1;
            area.AxisX.MajorGrid.Enabled = false;
            area.AxisX.MajorTickMark.Enabled = false;
            area.AxisX.LabelStyle.Font = new Font("Segoe UI", 8.5F);
            area.AxisX.LabelStyle.ForeColor = AppTheme.TextMuted;
            area.AxisY.Minimum = 0;
            area.AxisY.LabelStyle.Format = "#,##0";
            area.AxisY.LabelStyle.Font = new Font("Segoe UI", 8.5F);
            area.AxisY.LabelStyle.ForeColor = AppTheme.TextMuted;
            area.AxisY.MajorGrid.LineColor = ColorTranslator.FromHtml("#E5EAF3");
            area.AxisY.MajorTickMark.Enabled = false;

            DV.Legend legend = ch.Legends.Add("legend");
            legend.Docking = DV.Docking.Top;
            legend.BackColor = Color.White;
            legend.Font = new Font("Segoe UI", 9F);

            DV.Series thu = ch.Series.Add("Đã thu");
            DV.Series chua = ch.Series.Add("Chưa thu");
            thu.Color = MauThu;
            chua.Color = MauChua;
            foreach (DV.Series s in new DV.Series[] { thu, chua })
            {
                s.ChartArea = "area";
                s.Legend = "legend";
                s.ChartType = DV.SeriesChartType.StackedColumn;
                s["PointWidth"] = "0.6";
            }
            return ch;
        }

        private DataGridView TaoLuoiPhat()
        {
            DataGridView dgv = new DataGridView();
            ThemCot(dgv, "colThang", "Tháng", 20F);
            ThemCot(dgv, "colThu", "Đã thu", 26F);
            ThemCot(dgv, "colChua", "Chưa thu", 26F);
            ThemCot(dgv, "colTong", "Tổng phạt", 28F);
            AppTheme.StyleGrid(dgv);
            return dgv;
        }

        private TabPage TaoTrangPhat(string tieuDe, DV.Chart chart, DataGridView dgv)
        {
            TabPage trang = new TabPage(tieuDe);
            trang.UseVisualStyleBackColor = false;
            trang.BackColor = Color.White;

            chart.SetBounds(6, 6, 920, 200);
            chart.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            dgv.SetBounds(6, 212, 920, 186);
            dgv.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;

            trang.Controls.Add(chart);
            trang.Controls.Add(dgv);
            return trang;
        }

        private static void ThemCot(DataGridView dgv, string ten, string tieuDe, float trongSo)
        {
            DataGridViewTextBoxColumn col = new DataGridViewTextBoxColumn();
            col.Name = ten;
            col.HeaderText = tieuDe;
            col.FillWeight = trongSo;
            col.ReadOnly = true;
            dgv.Columns.Add(col);
        }

        // ================= NẠP DỮ LIỆU =================

        private void btnXem_Click(object sender, EventArgs e)
        {
            Xem();
        }

        private void Xem()
        {
            if (dtpTu.Value.Date > dtpDen.Value.Date)
            {
                AppTheme.Warning("Ngày bắt đầu không được sau ngày kết thúc.");
                dtpTu.Focus();
                return;
            }

            DateTime tu = dtpTu.Value.Date;
            DateTime den = dtpDen.Value.Date.AddDays(1);   // lấy hết ngày cuối

            try
            {
                NapTop(chartSach, dgvSach,
                    @"SELECT TOP (10) s.MaSach AS Ma, s.TenSach AS Ten, COUNT(*) AS SoLuot
                      FROM PhieuMuon pm
                      INNER JOIN Sach s ON pm.MaSach = s.MaSach
                      WHERE pm.NgayMuon >= @Tu AND pm.NgayMuon < @Den
                      GROUP BY s.MaSach, s.TenSach
                      ORDER BY COUNT(*) DESC, s.TenSach",
                    tu, den, "Chưa có lượt mượn nào trong khoảng ngày này");

                NapTop(chartDocGia, dgvDocGia,
                    @"SELECT TOP (10) dg.MaDocGia AS Ma, dg.TenDocGia AS Ten, COUNT(*) AS SoLuot
                      FROM PhieuMuon pm
                      INNER JOIN DocGia dg ON pm.MaDocGia = dg.MaDocGia
                      WHERE pm.NgayMuon >= @Tu AND pm.NgayMuon < @Den
                      GROUP BY dg.MaDocGia, dg.TenDocGia
                      ORDER BY COUNT(*) DESC, dg.TenDocGia",
                    tu, den, "Chưa có lượt mượn nào trong khoảng ngày này");

                NapPhat(tu, den);
            }
            catch (Exception ex)
            {
                AppTheme.Error("Không thể tải thống kê.\n\n" + ex.Message);
            }
        }

        private static string RutGon(string s, int toiDa)
        {
            s = (s ?? "").Trim();
            return s.Length <= toiDa ? s : s.Substring(0, toiDa - 1) + "…";
        }

        private void NapTop(DV.Chart chart, DataGridView dgv, string sql, DateTime tu, DateTime den, string khiRong)
        {
            DataTable dt = DbConnection.ExecuteQuery(sql,
                DbConnection.Param("@Tu", tu),
                DbConnection.Param("@Den", den));

            DV.Series s = chart.Series[0];
            s.Points.Clear();
            chart.Titles.Clear();
            dgv.Rows.Clear();

            int max = 1;
            int hang = 1;
            foreach (DataRow r in dt.Rows)
            {
                string ten = r["Ten"].ToString();
                int luot = Convert.ToInt32(r["SoLuot"]);

                int idx = s.Points.AddXY(RutGon(ten, 26), luot);
                s.Points[idx].ToolTip = ten + ": " + luot + " lượt";

                dgv.Rows.Add(hang, r["Ma"], ten, luot);
                hang++;
                if (luot > max) max = luot;
            }

            // Trục lượt luôn là số nguyên
            chart.ChartAreas[0].AxisY.Interval = Math.Max(1, Math.Ceiling(max / 5.0));

            if (dt.Rows.Count == 0)
            {
                DV.Title t = chart.Titles.Add(khiRong);
                t.Font = AppTheme.Base;
                t.ForeColor = AppTheme.TextMuted;
            }

            dgv.ClearSelection();
            dgv.CurrentCell = null;
        }

        /// <summary>
        /// Tiền phạt theo tháng trả sách. Phiếu cũ chưa có TienPhat thì tính lại từ ngày tháng.
        /// Hiển thị tối đa SoThangToiDa tháng gần nhất của khoảng đã chọn.
        /// </summary>
        private void NapPhat(DateTime tu, DateTime den)
        {
            DataTable dt = DbConnection.ExecuteQuery(@"
                SELECT YEAR(pm.NgayTra) AS Nam, MONTH(pm.NgayTra) AS Thang, pm.DaThuPhat,
                       SUM(CASE WHEN pm.NgayTra > pm.HanTra
                                THEN ISNULL(pm.TienPhat, DATEDIFF(DAY, pm.HanTra, pm.NgayTra) * @Muc)
                                ELSE 0 END) AS Tien
                FROM PhieuMuon pm
                WHERE pm.NgayTra IS NOT NULL AND pm.NgayTra >= @Tu AND pm.NgayTra < @Den
                GROUP BY YEAR(pm.NgayTra), MONTH(pm.NgayTra), pm.DaThuPhat",
                DbConnection.Param("@Tu", tu),
                DbConnection.Param("@Den", den),
                DbConnection.Param("@Muc", QuyDinh.MucPhatMoiNgay));

            // key = Nam*100 + Thang ; giá trị = [đã thu, chưa thu]
            Dictionary<int, decimal[]> theoThang = new Dictionary<int, decimal[]>();
            foreach (DataRow r in dt.Rows)
            {
                int key = Convert.ToInt32(r["Nam"]) * 100 + Convert.ToInt32(r["Thang"]);
                decimal[] v;
                if (!theoThang.TryGetValue(key, out v))
                {
                    v = new decimal[2];
                    theoThang[key] = v;
                }

                bool daThu = Convert.ToBoolean(r["DaThuPhat"]);
                v[daThu ? 0 : 1] += Convert.ToDecimal(r["Tien"]);
            }

            DateTime cuoiKhoang = den.AddDays(-1);
            DateTime thangCuoi = new DateTime(cuoiKhoang.Year, cuoiKhoang.Month, 1);
            DateTime thangDau = new DateTime(tu.Year, tu.Month, 1);
            if (thangCuoi.AddMonths(-(SoThangToiDa - 1)) > thangDau)
                thangDau = thangCuoi.AddMonths(-(SoThangToiDa - 1));

            chartPhat.Series["Đã thu"].Points.Clear();
            chartPhat.Series["Chưa thu"].Points.Clear();
            dgvPhat.Rows.Clear();

            decimal tongThu = 0, tongChua = 0;
            for (DateTime m = thangDau; m <= thangCuoi; m = m.AddMonths(1))
            {
                decimal[] v;
                if (!theoThang.TryGetValue(m.Year * 100 + m.Month, out v))
                    v = new decimal[2];

                string nhan = "T" + m.Month + "/" + m.Year;
                chartPhat.Series["Đã thu"].Points.AddXY(nhan, (double)v[0]);
                chartPhat.Series["Chưa thu"].Points.AddXY(nhan, (double)v[1]);

                dgvPhat.Rows.Add(nhan,
                    QuyDinh.DinhDangTien(v[0]),
                    QuyDinh.DinhDangTien(v[1]),
                    QuyDinh.DinhDangTien(v[0] + v[1]));

                tongThu += v[0];
                tongChua += v[1];
            }

            int dong = dgvPhat.Rows.Add("Tổng cộng",
                QuyDinh.DinhDangTien(tongThu),
                QuyDinh.DinhDangTien(tongChua),
                QuyDinh.DinhDangTien(tongThu + tongChua));
            foreach (DataGridViewCell o in dgvPhat.Rows[dong].Cells)
                o.Style.Font = AppTheme.Bold;

            dgvPhat.ClearSelection();
            dgvPhat.CurrentCell = null;
        }
    }
}
