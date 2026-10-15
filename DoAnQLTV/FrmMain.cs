using DV = System.Windows.Forms.DataVisualization.Charting;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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

        private bool daBaoLoiThongKe = false;

        public FrmMain()
        {
            InitializeComponent();

            // Toàn bộ kích thước giao diện bên dưới được tính bằng code (hàm S),
            // nên tắt tự co giãn của Designer để không bị nhân đôi tỉ lệ.
            this.AutoScaleMode = AutoScaleMode.None;

            AppTheme.ApplyForm(this);

            this.StartPosition = FormStartPosition.CenterScreen;
            this.WindowState = FormWindowState.Maximized;
            this.MinimumSize = new Size(S(1000), S(650));

            this.menuStrip1.Dock = DockStyle.Top;

            ApDungGiaoDien();
        }

        // =====================================================================
        //  GIAO DIỆN MỚI (theo mockup) - toàn bộ làm bằng code, không sửa Designer
        // =====================================================================

        /// <summary>Đổi số pixel (tính ở 96 DPI) sang pixel thật của màn hình hiện tại.</summary>
        private int S(int v)
        {
            return (int)Math.Round(v * this.DeviceDpi / 96.0);
        }

        private static Color Mau(string hex)
        {
            return ColorTranslator.FromHtml(hex);
        }

        // ----- Màu -----
        private readonly Color mauSidebar = Mau("#0F2F66");
        private readonly Color mauSidebarHover = Mau("#1B4488");
        private readonly Color mauHome = Mau("#2D6BE4");
        private readonly Color mauHomeHover = Mau("#3A78F0");
        private readonly Color mauChuNhomSB = Mau("#EAF1FF");
        private readonly Color mauChuMucSB = Mau("#C3D3F0");
        private readonly Color mauIconNhomSB = Mau("#B4C8EE");
        private readonly Color mauIconMucSB = Mau("#8FAADC");
        private readonly Color mauNavy = Mau("#1E2A5A");
        private readonly Color mauChuPhu = Mau("#64748B");
        private readonly Color mauVienThe = Mau("#E3EAF5");
        private readonly Color mauMuon = Mau("#3B82F6");
        private readonly Color mauTra = Mau("#34D399");

        // ----- Font -----
        private Font fIconNhom, fIconMuc, fIconChevron, fIconHeader, fIconThe, fIconTruyCap, fIconChan;
        private Font fChuNhom, fChuMuc, fLogo, fChan;
        private Font fTieuDeTrang, fTenNguoiDung, fChucVu, fDongHo;
        private Font fLoiChao, fMoTaBanner, fQuote;
        private Font fTieuDeKhoi, fNhanThe, fSoThe, fPhu, fTenTruyCap, fMoTaTruyCap, fSoTam, fNhanTam;

        private void TaoFont()
        {
            // Font biểu tượng: Windows 11 có Segoe Fluent Icons, Windows 10 dùng Segoe MDL2 Assets
            string tenIcon = "Segoe MDL2 Assets";
            foreach (FontFamily ff in FontFamily.Families)
            {
                if (ff.Name == "Segoe Fluent Icons") { tenIcon = ff.Name; break; }
            }

            fIconNhom = new Font(tenIcon, 12F);
            fIconMuc = new Font(tenIcon, 10.5F);
            fIconChevron = new Font(tenIcon, 8F);
            fIconHeader = new Font(tenIcon, 14F);
            fIconThe = new Font(tenIcon, 17F);
            fIconTruyCap = new Font(tenIcon, 15F);
            fIconChan = new Font(tenIcon, 15F);

            fChuNhom = new Font("Segoe UI Semibold", 10.5F);
            fChuMuc = new Font("Segoe UI", 10F);
            fLogo = new Font("Segoe UI", 10.5F, FontStyle.Bold);
            fChan = new Font("Segoe UI", 8.25F);

            fTieuDeTrang = new Font("Segoe UI Semibold", 13F);
            fTenNguoiDung = new Font("Segoe UI Semibold", 9.75F);
            fChucVu = new Font("Segoe UI", 8.5F);
            fDongHo = new Font("Segoe UI Semibold", 11F);

            fLoiChao = new Font("Segoe UI", 17F, FontStyle.Bold);
            fMoTaBanner = new Font("Segoe UI", 10.5F);
            fQuote = new Font("Segoe UI", 10.5F, FontStyle.Italic);

            fTieuDeKhoi = new Font("Segoe UI Semibold", 11F);
            fNhanThe = new Font("Segoe UI", 9.5F);
            fSoThe = new Font("Segoe UI", 20F, FontStyle.Bold);
            fPhu = new Font("Segoe UI", 8.5F);
            fTenTruyCap = new Font("Segoe UI", 10F, FontStyle.Bold);
            fMoTaTruyCap = new Font("Segoe UI", 8.5F);
            fSoTam = new Font("Segoe UI", 20F, FontStyle.Bold);
            fNhanTam = new Font("Segoe UI", 9.5F);
        }

        /// <summary>Panel bật double buffer để vẽ không nhấp nháy.</summary>
        private class DoubleBufferedPanel : Panel
        {
            public DoubleBufferedPanel()
            {
                this.DoubleBuffered = true;
                this.ResizeRedraw = true;
            }
        }

        private class MucSidebar
        {
            public string Glyph;
            public string Text;
            public int Kind; // 0 = Trang chủ, 1 = tiêu đề nhóm, 2 = mục con
        }

        private class TheTruyCap
        {
            public string Glyph;
            public Color Nen, Vien, NenHover, MauIcon;
            public bool Re;
        }

        // ----- Điều khiển tạo thêm bằng code -----
        private DoubleBufferedPanel pnlLogo, pnlFooter, pnlBanner, pnlChuThichMuonTra, pnlChuThichTheLoai;
        private ContextMenuStrip menuThongBao;

        private readonly Dictionary<Button, MucSidebar> mucSidebar = new Dictionary<Button, MucSidebar>();
        private readonly HashSet<Button> dangRe = new HashSet<Button>();
        private readonly HashSet<Button> nhomDaDong = new HashSet<Button>();
        private readonly Dictionary<Panel, int> chieuCaoNhom = new Dictionary<Panel, int>();
        private readonly Dictionary<Panel, TheTruyCap> theTruyCap = new Dictionary<Panel, TheTruyCap>();

        // ----- Dữ liệu biểu đồ thể loại (đọc từ CSDL trong VeBieuDoTheLoai) -----
        private string[] tenTheLoai = new string[0];
        private int[] soTheLoai = new int[0];
        private readonly Color[] mauTheLoai =
        {
            Mau("#3B82F6"), Mau("#34C38F"), Mau("#F59E0B"), Mau("#8B5CF6"), Mau("#FCD34D"), Mau("#94A3B8")
        };

        private bool dangCanLe = false;

        private void ApDungGiaoDien()
        {
            TaoFont();

            foreach (Control c in new Control[] { pnlContent, pnlHeader, pnlSidebar, tlpThongKe, tlpBieuDo, tlpTruyCapNhanh })
                AppTheme.SetDoubleBuffered(c);

            this.AutoScroll = false;

            // Menu chữ phía trên đã có đủ trong thanh bên, nên ẩn đi (vẫn còn trong code, không lỗi sự kiện)
            menuStrip1.Renderer = new ToolStripProfessionalRenderer(new MenuColors());
            menuStrip1.Visible = false;

            TaoSidebar();
            TaoHeader();
            TaoNoiDung();
            KhoiTaoNutHeader();

            CanLeHeader();
            CanLeNoiDung();
        }

        // =====================================================================
        //  THANH BÊN
        // =====================================================================

        private void TaoSidebar()
        {
            pnlSidebar.BackColor = mauSidebar;
            pnlSidebar.Padding = Padding.Empty;
            pnlSidebar.Width = S(264);

            DangKyMucSidebar(btnTrangChu, "\uE80F", "Trang chủ", 0);

            // Quản lý
            DangKyMucSidebar(btnQuanLySach, "\uE8F1", "Quản lý sách", 2);
            DangKyMucSidebar(btnQuanLyTheLoai, "\uE8FD", "Quản lý thể loại", 2);
            DangKyMucSidebar(btnQuanLyTacGia, "\uE70F", "Quản lý tác giả", 2);
            DangKyMucSidebar(btnQuanLyDocGia, "\uE716", "Quản lý độc giả", 2);
            DangKyMucSidebar(btnQuanLyNhanVien, "\uE77B", "Quản lý nhân viên", 2);
            CauHinhNhom(pnlNhomQuanLy, btnQuanLy, "\uE8F1", "Quản lý");

            // Tra cứu
            DangKyMucSidebar(btnTimKiemSach, "\uE721", "Tìm kiếm sách", 2);
            DangKyMucSidebar(btnThongTinDocGia, "\uE946", "Thông tin độc giả", 2);
            CauHinhNhom(pnlNhomTraCuu, btnTraCuu, "\uE721", "Tra cứu");

            // Mượn - Trả
            DangKyMucSidebar(btnLapPhieuMuon, "\uE8A5", "Lập phiếu mượn", 2);
            DangKyMucSidebar(btnTraSach, "\uE7A7", "Trả sách", 2);
            DangKyMucSidebar(btnDanhSachPhieuMuon, "\uE8FD", "Danh sách phiếu mượn", 2);
            CauHinhNhom(pnlNhomMuonTra, btnMuonTra, "\uE8A5", "Mượn - Trả");

            // Trợ giúp
            DangKyMucSidebar(btnThongTinChuongTrinh, "\uE946", "Thông tin chương trình", 2);
            DangKyMucSidebar(btnHuongDanSuDung, "\uE897", "Hướng dẫn sử dụng", 2);
            CauHinhNhom(pnlNhomTroGiup, btnTroGiup, "\uE897", "Trợ giúp");

            // Hệ thống
            DangKyMucSidebar(btnDangXuat, "\uE72B", "Đăng xuất", 2);
            DangKyMucSidebar(btnThoat, "\uE7E8", "Thoát", 2);
            CauHinhNhom(pnlNhomHeThong, btnHeThong, "\uE713", "Hệ thống");

            // Logo ở trên cùng
            pnlLogo = new DoubleBufferedPanel();
            pnlLogo.Dock = DockStyle.Top;
            pnlLogo.Height = S(84);
            pnlLogo.BackColor = mauSidebar;
            pnlLogo.Paint += VeLogo;
            pnlSidebar.Controls.Add(pnlLogo);

            // Câu chào ở dưới cùng
            pnlFooter = new DoubleBufferedPanel();
            pnlFooter.Dock = DockStyle.Bottom;
            pnlFooter.Height = S(64);
            pnlFooter.BackColor = mauSidebar;
            pnlFooter.Paint += VeChanSidebar;
            pnlSidebar.Controls.Add(pnlFooter);

            pnlSidebar.SizeChanged += (s, e) => CapNhatChanSidebar();
            CapNhatChanSidebar();
        }

        private void DangKyMucSidebar(Button b, string glyph, string text, int kind)
        {
            MucSidebar m = new MucSidebar();
            m.Glyph = glyph;
            m.Text = text;
            m.Kind = kind;
            mucSidebar[b] = m;

            b.AutoSize = false;
            b.Text = "";                       // chữ và icon được vẽ trong VeMucSidebar
            b.AccessibleName = text;
            b.Padding = Padding.Empty;
            b.FlatStyle = FlatStyle.Flat;
            b.FlatAppearance.BorderSize = 0;
            b.FlatAppearance.MouseOverBackColor = mauSidebar;
            b.FlatAppearance.MouseDownBackColor = mauSidebar;
            b.UseVisualStyleBackColor = false;
            b.BackColor = mauSidebar;
            b.TabStop = false;
            b.Cursor = Cursors.Hand;
            b.Height = (kind == 0) ? S(56) : (kind == 1 ? S(42) : S(36));
            AppTheme.SetDoubleBuffered(b);

            b.MouseEnter += (s, e) => { dangRe.Add(b); b.Invalidate(); };
            b.MouseLeave += (s, e) => { dangRe.Remove(b); b.Invalidate(); };
            b.Paint += VeMucSidebar;
        }

        private void CauHinhNhom(Panel nhom, Button header, string glyph, string text)
        {
            DangKyMucSidebar(header, glyph, text, 1);

            nhom.BackColor = mauSidebar;
            nhom.Margin = Padding.Empty;
            nhom.Padding = Padding.Empty;

            int cao = header.Height + (nhom.Controls.Count - 1) * S(36) + S(6);
            nhom.Height = cao;
            chieuCaoNhom[nhom] = cao;

            // Bấm vào tiêu đề nhóm để thu gọn / mở rộng
            header.Click += (s, e) =>
            {
                if (nhomDaDong.Contains(header))
                {
                    nhomDaDong.Remove(header);
                    nhom.Height = chieuCaoNhom[nhom];
                }
                else
                {
                    nhomDaDong.Add(header);
                    nhom.Height = header.Height + S(6);
                }
                header.Invalidate();
                CapNhatChanSidebar();
            };
        }

        private void VeMucSidebar(object sender, PaintEventArgs e)
        {
            Button b = (Button)sender;
            MucSidebar m;
            if (!mucSidebar.TryGetValue(b, out m)) return;

            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            bool re = dangRe.Contains(b);
            int ngang = (m.Kind == 0) ? S(14) : S(10);
            int doc = (m.Kind == 0) ? S(6) : S(2);
            Rectangle r = new Rectangle(ngang, doc, b.Width - 2 * ngang, b.Height - 2 * doc);

            Color nen = Color.Empty;
            bool coNen = false;
            if (m.Kind == 0) { nen = re ? mauHomeHover : mauHome; coNen = true; }
            else if (re) { nen = mauSidebarHover; coNen = true; }

            if (coNen)
            {
                using (GraphicsPath p = TaoDuongBoGoc(r, S(8)))
                using (SolidBrush br = new SolidBrush(nen))
                    g.FillPath(br, p);
            }

            int xIcon = (m.Kind == 2) ? S(34) : (m.Kind == 0 ? S(28) : S(22));
            Font fi = (m.Kind == 2) ? fIconMuc : fIconNhom;
            Color mauIcon = (m.Kind == 0) ? Color.White : (m.Kind == 1 ? mauIconNhomSB : (re ? Color.White : mauIconMucSB));
            TextRenderer.DrawText(g, m.Glyph, fi, new Rectangle(xIcon, 0, S(26), b.Height), mauIcon,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPadding);

            int xText = xIcon + S(36);
            Font ft = (m.Kind == 2) ? fChuMuc : fChuNhom;
            Color mauChu = (m.Kind == 0) ? Color.White : (m.Kind == 1 ? mauChuNhomSB : (re ? Color.White : mauChuMucSB));
            TextRenderer.DrawText(g, m.Text, ft, new Rectangle(xText, 0, Math.Max(10, b.Width - xText - S(44)), b.Height), mauChu,
                TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis | TextFormatFlags.NoPadding);

            if (m.Kind == 1)
            {
                string chev = nhomDaDong.Contains(b) ? "\uE76C" : "\uE70D";
                TextRenderer.DrawText(g, chev, fIconChevron, new Rectangle(b.Width - S(44), 0, S(24), b.Height), mauIconNhomSB,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPadding);
            }
        }

        private void VeLogo(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            VeSachMo(g, new Rectangle(S(18), S(26), S(44), S(34)), Color.FromArgb(70, 255, 255, 255), Color.White);

            TextRenderer.DrawText(g, "HỆ THỐNG QUẢN LÝ\nTHƯ VIỆN", fLogo,
                new Rectangle(S(72), 0, Math.Max(10, pnlLogo.Width - S(80)), pnlLogo.Height), Color.White,
                TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPadding);
        }

        private void VeChanSidebar(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            TextRenderer.DrawText(g, "\uE8F1", fIconChan, new Rectangle(S(16), 0, S(32), pnlFooter.Height), mauIconMucSB,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPadding);

            TextRenderer.DrawText(g, "Thư viện luôn là người bạn\nđồng hành tri thức!", fChan,
                new Rectangle(S(58), 0, Math.Max(10, pnlFooter.Width - S(66)), pnlFooter.Height), mauIconMucSB,
                TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPadding);
        }

        /// <summary>Màn hình thấp thì tự ẩn câu chào ở chân thanh bên để không đè lên mục menu.</summary>
        private void CapNhatChanSidebar()
        {
            if (pnlFooter == null || pnlLogo == null) return;

            int can = pnlLogo.Height + btnTrangChu.Height
                + pnlNhomQuanLy.Height + pnlNhomTraCuu.Height + pnlNhomMuonTra.Height
                + pnlNhomTroGiup.Height + pnlNhomHeThong.Height + pnlFooter.Height;

            pnlFooter.Visible = pnlSidebar.ClientSize.Height >= can;
        }

        // =====================================================================
        //  THANH TIÊU ĐỀ
        // =====================================================================

        private void TaoHeader()
        {
            pnlHeader.BackColor = Color.White;
            pnlHeader.Height = S(56);
            pnlHeader.Paint += (s, e) =>
            {
                using (Pen pen = new Pen(mauVienThe))
                    e.Graphics.DrawLine(pen, 0, pnlHeader.Height - 1, pnlHeader.Width, pnlHeader.Height - 1);
            };
            pnlHeader.SizeChanged += (s, e) => CanLeHeader();

            foreach (Button b in new Button[] { btnMenu, btnThongBao })
            {
                b.FlatStyle = FlatStyle.Flat;
                b.FlatAppearance.BorderSize = 0;
                b.FlatAppearance.MouseOverBackColor = Mau("#EEF3FD");
                b.FlatAppearance.MouseDownBackColor = Mau("#E1EAFB");
                b.UseVisualStyleBackColor = false;
                b.BackColor = Color.White;
                b.ForeColor = mauNavy;
                b.Font = fIconHeader;
                b.Cursor = Cursors.Hand;
                b.TabStop = false;
                b.Padding = Padding.Empty;
                b.TextAlign = ContentAlignment.MiddleCenter;
                b.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            }
            btnMenu.Text = "\uE700";       // 3 gạch
            btnThongBao.Text = "\uE7ED";   // chuông

            lblTieuDeTrang.Font = fTieuDeTrang;
            lblTieuDeTrang.ForeColor = mauNavy;
            lblTieuDeTrang.Anchor = AnchorStyles.Top | AnchorStyles.Left;

            // Đồng hồ chuyển lên thanh tiêu đề (mockup không có đồng hồ ở trang chủ)
            pnlHeader.Controls.Add(lblThoiGianHienTai);
            lblThoiGianHienTai.AutoSize = false;
            lblThoiGianHienTai.Size = new Size(S(110), S(28));
            lblThoiGianHienTai.TextAlign = ContentAlignment.MiddleRight;
            lblThoiGianHienTai.Font = fDongHo;
            lblThoiGianHienTai.ForeColor = mauChuPhu;
            lblThoiGianHienTai.Anchor = AnchorStyles.Top | AnchorStyles.Left;

            picDaiDien.BorderStyle = BorderStyle.None;
            picDaiDien.Size = new Size(S(40), S(40));
            picDaiDien.Anchor = AnchorStyles.Top | AnchorStyles.Left;

            lblHoTenNguoiDung.Font = fTenNguoiDung;
            lblHoTenNguoiDung.ForeColor = mauNavy;
            lblHoTenNguoiDung.AutoEllipsis = true;
            lblHoTenNguoiDung.Anchor = AnchorStyles.Top | AnchorStyles.Left;

            lblChucVuNguoiDung.Font = fChucVu;
            lblChucVuNguoiDung.ForeColor = mauChuPhu;
            lblChucVuNguoiDung.AutoEllipsis = true;
            lblChucVuNguoiDung.Anchor = AnchorStyles.Top | AnchorStyles.Left;
        }

        private void CanLeHeader()
        {
            int h = pnlHeader.ClientSize.Height;
            int w = pnlHeader.ClientSize.Width;
            int nut = S(40);

            btnMenu.SetBounds(S(14), (h - nut) / 2, nut, nut);
            lblTieuDeTrang.Location = new Point(btnMenu.Right + S(8), (h - lblTieuDeTrang.Height) / 2);

            int rongTen = S(170);
            int xTen = w - S(24) - rongTen;
            lblHoTenNguoiDung.SetBounds(xTen, S(8), rongTen, S(21));
            lblChucVuNguoiDung.SetBounds(xTen, S(29), rongTen, S(18));

            picDaiDien.SetBounds(xTen - S(10) - nut, (h - nut) / 2, nut, nut);
            btnThongBao.SetBounds(picDaiDien.Left - S(12) - nut, (h - nut) / 2, nut, nut);

            lblThoiGianHienTai.Location = new Point(btnThongBao.Left - S(14) - lblThoiGianHienTai.Width,
                (h - lblThoiGianHienTai.Height) / 2);
        }

        // =====================================================================
        //  VÙNG NỘI DUNG
        // =====================================================================

        private void TaoNoiDung()
        {
            pnlContent.BackColor = AppTheme.Background;
            pnlContent.AutoScroll = true;

            // ---------- Banner chào mừng ----------
            pnlBanner = new DoubleBufferedPanel();
            pnlBanner.BackColor = AppTheme.Background;
            pnlBanner.Paint += VeBanner;
            pnlBanner.SizeChanged += (s, e) => CanLeBanner();
            pnlContent.Controls.Add(pnlBanner);

            pnlBanner.Controls.Add(lblLoiChao);
            pnlBanner.Controls.Add(lblMoTaTrangChu);
            pnlBanner.Controls.Add(lblCauTrichDan);

            lblLoiChao.Font = fLoiChao;
            lblLoiChao.ForeColor = mauNavy;
            lblMoTaTrangChu.Font = fMoTaBanner;
            lblMoTaTrangChu.ForeColor = Mau("#4B5B7A");
            lblMoTaTrangChu.Text = "Chúc bạn có một ngày làm việc hiệu quả!";
            lblCauTrichDan.Font = fQuote;
            lblCauTrichDan.ForeColor = Mau("#475A7C");
            lblCauTrichDan.AutoSize = false;
            lblCauTrichDan.TextAlign = ContentAlignment.MiddleCenter;
            lblCauTrichDan.Text = "“Sách là nguồn tri thức vô tận\r\nmở ra những chân trời mới.”";

            CapNhatLoiChao();
            lblHoTenNguoiDung.TextChanged += (s, e) => CapNhatLoiChao();

            // ---------- 4 thẻ thống kê ----------
            Panel[] theTK = { pnlCardSach, pnlCardDocGia, pnlCardDangMuon, pnlCardQuaHan };
            for (int i = 0; i < theTK.Length; i++)
                theTK[i].Margin = new Padding(i == 0 ? 0 : S(7), 0, i == theTK.Length - 1 ? 0 : S(7), 0);

            lblTieuDeDangMuon.Text = "Phiếu mượn đang hiệu lực";

            StyleTheThongKe(pnlCardSach, lblTieuDeSach, lblSoSach, "\uE8F1",
                Mau("#2563EB"), Mau("#DCE9FD"), "đầu sách trong thư viện", mauChuPhu);
            StyleTheThongKe(pnlCardDocGia, lblTieuDeDocGia, lblSoDocGia, "\uE716",
                Mau("#16A34A"), Mau("#D9F3E4"), "độc giả đã đăng ký", mauChuPhu);
            StyleTheThongKe(pnlCardDangMuon, lblTieuDeDangMuon, lblSoDangMuon, "\uE8A5",
                Mau("#7C3AED"), Mau("#EBE2FD"), "phiếu chưa hoàn trả", mauChuPhu);
            StyleTheThongKe(pnlCardQuaHan, lblTieuDeQuaHan, lblSoQuaHan, "\uE7A7",
                Mau("#F59E0B"), Mau("#FEEBC8"), "cần nhắc độc giả trả sách", Mau("#DC2626"));

            // ---------- Biểu đồ ----------
            tlpBieuDo.ColumnStyles[0].SizeType = SizeType.Percent;
            tlpBieuDo.ColumnStyles[0].Width = 54F;
            tlpBieuDo.ColumnStyles[1].SizeType = SizeType.Percent;
            tlpBieuDo.ColumnStyles[1].Width = 46F;
            pnlBieuDoMuonTra.Margin = new Padding(0, 0, S(7), 0);
            pnlBieuDoTheLoai.Margin = new Padding(S(7), 0, 0, 0);

            // Biểu đồ mượn - trả
            StyleTheTrang(pnlBieuDoMuonTra);
            pnlBieuDoMuonTra.Padding = new Padding(S(12), S(50), S(12), S(12));
            chartMuonTra.Dock = DockStyle.Fill;
            lblTieuDeMuonTra.Text = "Thống kê mượn trả sách";
            StyleTieuDeKhoi(lblTieuDeMuonTra);

            pnlChuThichMuonTra = new DoubleBufferedPanel();
            pnlChuThichMuonTra.BackColor = Color.White;
            pnlChuThichMuonTra.Size = new Size(S(190), S(24));
            pnlChuThichMuonTra.Paint += VeChuThichMuonTra;
            pnlBieuDoMuonTra.Controls.Add(pnlChuThichMuonTra);
            pnlChuThichMuonTra.BringToFront();
            pnlBieuDoMuonTra.SizeChanged += (s, e) => DatChuThichMuonTra();
            DatChuThichMuonTra();

            // Biểu đồ thể loại
            StyleTheTrang(pnlBieuDoTheLoai);
            pnlBieuDoTheLoai.Padding = new Padding(S(12), S(50), S(12), S(12));
            chartTheLoai.Dock = DockStyle.Fill;
            StyleTieuDeKhoi(lblTieuDeTheLoai);

            pnlChuThichTheLoai = new DoubleBufferedPanel();
            pnlChuThichTheLoai.Dock = DockStyle.Right;
            pnlChuThichTheLoai.Width = S(250);
            pnlChuThichTheLoai.BackColor = Color.White;
            pnlChuThichTheLoai.Paint += VeChuThichTheLoai;
            pnlBieuDoTheLoai.Controls.Add(pnlChuThichTheLoai);

            chartTheLoai.PostPaint += ChartTheLoai_PostPaint;
            lblSoSach.TextChanged += (s, e) => chartTheLoai.Invalidate();

            // ---------- Truy cập nhanh ----------
            StyleTheTrang(pnlThaoTacNhanh);
            StyleTieuDeKhoi(lblTieuDeTruyCapNhanh);
            tlpTruyCapNhanh.Anchor = AnchorStyles.Top | AnchorStyles.Left;

            StyleTheTruyCap(pnlTheQuanLySach, lblTenQuanLySach, lblMoTaQuanLySach, picQuanLySach, pnlTheQuanLySach_Click,
                "\uE8F1", "#EEF4FF", "#D6E4FB", "#E2EDFF", "#2563EB", "#1D4ED8");
            StyleTheTruyCap(pnlTheQuanLyDocGia, lblTenQuanLyDocGia, lblMoTaQuanLyDocGia, picQuanLyDocGia, pnlTheQuanLyDocGia_Click,
                "\uE716", "#EAF8EF", "#CDEBD8", "#DDF3E6", "#16A34A", "#15803D");
            StyleTheTruyCap(pnlTheQuanLyNhanVien, lblTenQuanLyNhanVien, lblMoTaQuanLyNhanVien, picQuanLyNhanVien, pnlTheQuanLyNhanVien_Click,
                "\uE77B", "#F3EEFF", "#E0D4FA", "#E8DDFD", "#7C3AED", "#6D28D9");
            StyleTheTruyCap(pnlTheLapPhieuMuon, lblTenLapPhieuMuon, lblMoTaLapPhieuMuon, picLapPhieuMuon, pnlTheLapPhieuMuon_Click,
                "\uE8A5", "#FFF4E5", "#FBE0BE", "#FFEBCE", "#F59E0B", "#D97706");
            StyleTheTruyCap(pnlTheTraSach, lblTenTraSach, lblMoTaTraSach, picTraSach, pnlTheTraSach_Click,
                "\uE7A7", "#E6F8F6", "#C4EBE6", "#D5F2EE", "#14B8A6", "#0F9488");
            StyleTheTruyCap(pnlTheTimKiemSach, lblTenTimKiemSach, lblMoTaTimKiemSach, picTimKiemSach, pnlTheTimKiemSach_Click,
                "\uE721", "#FFEFF1", "#FBD3D8", "#FFE2E6", "#F43F5E", "#E11D48");

            pnlThaoTacNhanh.SizeChanged += (s, e) => CanLeTheTruyCap();

            // ---------- Hoạt động gần đây ----------
            StyleTheTrang(pnlHoatDongGanDay);
            StyleTieuDeKhoi(lblTieuDeHoatDongGanDay);
            StyleBangHoatDong();

            lnkXemNhatKy = new LinkLabel();
            lnkXemNhatKy.Text = "Xem tất cả";
            lnkXemNhatKy.AutoSize = true;
            lnkXemNhatKy.Font = fNhanThe;
            lnkXemNhatKy.BackColor = Color.Transparent;
            lnkXemNhatKy.LinkColor = Mau("#2563EB");
            lnkXemNhatKy.ActiveLinkColor = Mau("#1D4ED8");
            lnkXemNhatKy.Visible = TaiKhoan.LaQuanTri;   // chỉ quản trị viên xem toàn bộ nhật ký
            lnkXemNhatKy.LinkClicked += (s, e) => MoFormChucNang(new FrmNhatKy());
            pnlHoatDongGanDay.Controls.Add(lnkXemNhatKy);
            lnkXemNhatKy.BringToFront();

            pnlHoatDongGanDay.SizeChanged += (s, e) => CanLeHoatDong();

            // ---------- Căn lề ----------
            pnlContent.SizeChanged += (s, e) => CanLeNoiDung();
            CanLeBanner();
            CanLeTheTruyCap();
            CanLeHoatDong();
        }

        private void CapNhatLoiChao()
        {
            lblLoiChao.Text = "Xin chào, " + lblHoTenNguoiDung.Text + "!";
        }

        private void CanLeNoiDung()
        {
            if (dangCanLe || pnlBanner == null) return;
            dangCanLe = true;
            try
            {
                // Có thể xuất hiện thanh cuộn làm đổi chiều rộng, nên căn lại tối đa 2 lần
                for (int lan = 0; lan < 2; lan++)
                {
                    int rongTruoc = pnlContent.ClientSize.Width;
                    DatViTriNoiDung();
                    if (pnlContent.ClientSize.Width == rongTruoc) break;
                }
            }
            finally
            {
                dangCanLe = false;
            }
        }

        private void DatViTriNoiDung()
        {
            int le = S(20), gap = S(14), top = S(12);
            int hBanner = S(104), hThongKe = S(104), hTruyCap = S(156), hHoatDong = S(200);

            int w = Math.Max(S(640), pnlContent.ClientSize.Width - 2 * le);

            // Biểu đồ lấy phần chiều cao còn lại (tối thiểu 250)
            int conLai = pnlContent.ClientSize.Height - 2 * top - hBanner - hThongKe - hTruyCap - hHoatDong - 4 * gap;
            int hBieuDo = Math.Max(S(250), conLai);

            int oy = pnlContent.AutoScrollPosition.Y;   // âm khi đang cuộn
            int y = top;

            pnlBanner.SetBounds(le, oy + y, w, hBanner); y += hBanner + gap;
            tlpThongKe.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            tlpThongKe.SetBounds(le, oy + y, w, hThongKe); y += hThongKe + gap;
            tlpBieuDo.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            tlpBieuDo.SetBounds(le, oy + y, w, hBieuDo); y += hBieuDo + gap;
            pnlThaoTacNhanh.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            pnlThaoTacNhanh.SetBounds(le, oy + y, w, hTruyCap); y += hTruyCap + gap;
            pnlHoatDongGanDay.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            pnlHoatDongGanDay.SetBounds(le, oy + y, w, hHoatDong); y += hHoatDong + top;

            pnlContent.AutoScrollMinSize = new Size(0, y);
        }

        private void CanLeBanner()
        {
            if (pnlBanner == null) return;
            int w = pnlBanner.Width;
            int h = pnlBanner.Height;

            lblLoiChao.Location = new Point(S(150), S(20));
            lblMoTaTrangChu.Location = new Point(S(152), S(60));

            int rq = S(400);
            lblCauTrichDan.SetBounds(w - rq - S(130), (h - S(60)) / 2, rq, S(60));
            lblCauTrichDan.Visible = w >= S(900);   // màn hình hẹp thì ẩn câu trích dẫn
        }

        private void DatChuThichMuonTra()
        {
            if (pnlChuThichMuonTra == null) return;
            pnlChuThichMuonTra.Location = new Point(pnlBieuDoMuonTra.Width - pnlChuThichMuonTra.Width - S(22), S(14));
        }

        private void CanLeTheTruyCap()
        {
            tlpTruyCapNhanh.SetBounds(S(8), S(44), Math.Max(10, pnlThaoTacNhanh.Width - S(16)),
                Math.Max(10, pnlThaoTacNhanh.Height - S(44) - S(12)));
        }

        private void CanLeHoatDong()
        {
            dgvHoatDongGanDay.SetBounds(S(14), S(46), Math.Max(10, pnlHoatDongGanDay.Width - S(28)),
                Math.Max(10, pnlHoatDongGanDay.Height - S(46) - S(10)));
            if (lnkXemNhatKy != null)
                lnkXemNhatKy.Location = new Point(pnlHoatDongGanDay.Width - lnkXemNhatKy.Width - S(20), S(18));
        }

        // =====================================================================
        //  STYLE TỪNG KHỐI
        // =====================================================================

        /// <summary>Thẻ trắng bo góc, viền mảnh.</summary>
        private void StyleTheTrang(Panel card)
        {
            card.BorderStyle = BorderStyle.None;
            card.BackColor = AppTheme.Background;   // màu nền trang, để 4 góc bo nhìn tự nhiên
            AppTheme.SetDoubleBuffered(card);
            card.Paint += (s, e) => VeTheBoGoc(e.Graphics, card, Color.White, mauVienThe, S(12));
            card.SizeChanged += (s, e) => card.Invalidate();
        }

        private void StyleTieuDeKhoi(Label lbl)
        {
            lbl.Font = fTieuDeKhoi;
            lbl.ForeColor = mauNavy;
            lbl.Location = new Point(S(20), S(15));
            lbl.Anchor = AnchorStyles.Top | AnchorStyles.Left;
        }

        private void StyleTheThongKe(Panel card, Label tieuDe, Label so, string glyph,
            Color mauIcon, Color nenIcon, string phu, Color mauPhu)
        {
            StyleTheTrang(card);

            tieuDe.Font = fNhanThe;
            tieuDe.ForeColor = Mau("#475569");
            tieuDe.Location = new Point(S(88), S(18));

            so.Font = fSoThe;
            so.ForeColor = mauNavy;
            so.Location = new Point(S(86), S(34));

            Label lblPhu = new Label();
            lblPhu.AutoSize = true;
            lblPhu.BackColor = Color.Transparent;
            lblPhu.Font = fPhu;
            lblPhu.ForeColor = mauPhu;
            lblPhu.Text = phu;
            lblPhu.Location = new Point(S(88), S(76));
            card.Controls.Add(lblPhu);

            // Icon tròn màu bên trái
            card.Paint += (s, e) =>
            {
                Graphics g = e.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;
                int d = S(52);
                Rectangle rc = new Rectangle(S(20), (card.Height - d) / 2, d, d);
                using (SolidBrush br = new SolidBrush(nenIcon))
                    g.FillEllipse(br, rc);
                TextRenderer.DrawText(g, glyph, fIconThe, rc, mauIcon,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPadding);
            };
        }

        private void StyleTheTruyCap(Panel card, Label ten, Label moTa, PictureBox pic, EventHandler khiBam,
            string glyph, string nen, string vien, string hover, string mauIcon, string mauTen)
        {
            TheTruyCap t = new TheTruyCap();
            t.Glyph = glyph;
            t.Nen = Mau(nen);
            t.Vien = Mau(vien);
            t.NenHover = Mau(hover);
            t.MauIcon = Mau(mauIcon);
            theTruyCap[card] = t;

            card.BorderStyle = BorderStyle.None;
            card.BackColor = Color.White;
            card.Padding = Padding.Empty;
            card.Dock = DockStyle.Fill;
            card.Margin = new Padding(S(6), 0, S(6), 0);
            card.Cursor = Cursors.Hand;
            AppTheme.SetDoubleBuffered(card);

            pic.Visible = false;   // thay ảnh trong Resources bằng icon vẽ bằng code cho đồng bộ

            ten.Font = fTenTruyCap;
            ten.ForeColor = Mau(mauTen);
            ten.AutoSize = false;
            ten.TextAlign = ContentAlignment.MiddleCenter;
            ten.AutoEllipsis = true;
            ten.Cursor = Cursors.Hand;

            moTa.Font = fMoTaTruyCap;
            moTa.ForeColor = Mau("#6B7280");
            moTa.AutoSize = false;
            moTa.TextAlign = ContentAlignment.MiddleCenter;
            moTa.AutoEllipsis = true;
            moTa.Cursor = Cursors.Hand;

            card.Paint += (s, e) =>
            {
                Graphics g = e.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;
                VeTheBoGoc(g, card, t.Re ? t.NenHover : t.Nen, t.Re ? t.MauIcon : t.Vien, S(10));

                int d = S(44);
                Rectangle rc = new Rectangle((card.Width - d) / 2, S(10), d, d);
                using (SolidBrush br = new SolidBrush(t.MauIcon))
                    g.FillEllipse(br, rc);
                TextRenderer.DrawText(g, t.Glyph, fIconTruyCap, rc, Color.White,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPadding);
            };

            card.SizeChanged += (s, e) =>
            {
                ten.SetBounds(S(4), S(58), Math.Max(10, card.Width - S(8)), S(22));
                moTa.SetBounds(S(4), S(80), Math.Max(10, card.Width - S(8)), S(18));
                card.Invalidate();
            };
            ten.SetBounds(S(4), S(58), Math.Max(10, card.Width - S(8)), S(22));
            moTa.SetBounds(S(4), S(80), Math.Max(10, card.Width - S(8)), S(18));

            // Rê chuột: đổi màu thẻ (kể cả khi chuột nằm trên chữ)
            EventHandler vao = (s, e) => { t.Re = true; card.Invalidate(); };
            EventHandler ra = (s, e) =>
            {
                if (!card.ClientRectangle.Contains(card.PointToClient(Cursor.Position)))
                {
                    t.Re = false;
                    card.Invalidate();
                }
            };
            card.MouseEnter += vao; card.MouseLeave += ra;
            ten.MouseEnter += vao; ten.MouseLeave += ra;
            moTa.MouseEnter += vao; moTa.MouseLeave += ra;

            // Bấm vào chữ trong thẻ cũng mở form (dùng lại đúng hàm Click đã có)
            ten.Click += khiBam;
            moTa.Click += khiBam;
        }

        private void StyleBangHoatDong()
        {
            DataGridView d = dgvHoatDongGanDay;
            AppTheme.StyleGrid(d);

            d.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            d.BackgroundColor = Color.White;
            d.GridColor = Mau("#EDF1F7");
            d.RowTemplate.Height = S(28);

            d.ColumnHeadersHeight = S(30);
            d.ColumnHeadersDefaultCellStyle.BackColor = Color.White;
            d.ColumnHeadersDefaultCellStyle.ForeColor = mauNavy;
            d.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.White;
            d.ColumnHeadersDefaultCellStyle.SelectionForeColor = mauNavy;
            d.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 8.75F, FontStyle.Bold);

            d.DefaultCellStyle.Font = new Font("Segoe UI", 8.75F);
            d.DefaultCellStyle.ForeColor = Mau("#374151");
            d.DefaultCellStyle.SelectionBackColor = Mau("#F1F6FF");
            d.DefaultCellStyle.SelectionForeColor = Mau("#374151");
            d.AlternatingRowsDefaultCellStyle.BackColor = Color.White;

            colThoiGian.FillWeight = 20;
            colLoaiHoatDong.FillWeight = 16;
            colThongTinChiTiet.FillWeight = 40;
            colNguoiThucHien.FillWeight = 24;

            // Đường kẻ mảnh dưới hàng tiêu đề
            d.CellPainting += (s, e) =>
            {
                if (e.RowIndex == -1)
                {
                    e.Paint(e.ClipBounds, DataGridViewPaintParts.All & ~DataGridViewPaintParts.Border);
                    using (Pen pen = new Pen(Mau("#E3EAF5")))
                        e.Graphics.DrawLine(pen, e.CellBounds.Left, e.CellBounds.Bottom - 1, e.CellBounds.Right, e.CellBounds.Bottom - 1);
                    e.Handled = true;
                }
            };
        }

        // =====================================================================
        //  HÌNH VẼ
        // =====================================================================

        private static GraphicsPath TaoDuongBoGoc(Rectangle r, int banKinh)
        {
            int d = Math.Max(2, banKinh * 2);
            GraphicsPath p = new GraphicsPath();
            p.AddArc(r.X, r.Y, d, d, 180, 90);
            p.AddArc(r.Right - d, r.Y, d, d, 270, 90);
            p.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
            p.AddArc(r.X, r.Bottom - d, d, d, 90, 90);
            p.CloseFigure();
            return p;
        }

        private void VeTheBoGoc(Graphics g, Control c, Color nen, Color vien, int banKinh)
        {
            if (c.Width < 4 || c.Height < 4) return;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            Rectangle r = new Rectangle(0, 0, c.Width - 1, c.Height - 1);
            using (GraphicsPath p = TaoDuongBoGoc(r, banKinh))
            using (SolidBrush br = new SolidBrush(nen))
            using (Pen pen = new Pen(vien))
            {
                g.FillPath(br, p);
                g.DrawPath(pen, p);
            }
        }

        /// <summary>Quyển sách mở (dùng cho logo và trang trí banner).</summary>
        private void VeSachMo(Graphics g, Rectangle r, Color nen, Color vien)
        {
            int gx = r.X + r.Width / 2;
            Point[] trai =
            {
                new Point(r.X, r.Y + r.Height / 8),
                new Point(gx - 2, r.Y + r.Height / 4),
                new Point(gx - 2, r.Bottom),
                new Point(r.X, r.Bottom - r.Height / 8)
            };
            Point[] phai =
            {
                new Point(gx + 2, r.Y + r.Height / 4),
                new Point(r.Right, r.Y + r.Height / 8),
                new Point(r.Right, r.Bottom - r.Height / 8),
                new Point(gx + 2, r.Bottom)
            };

            using (SolidBrush br = new SolidBrush(nen))
            {
                g.FillPolygon(br, trai);
                g.FillPolygon(br, phai);
            }
            using (Pen pen = new Pen(vien, Math.Max(1, S(2))))
            {
                pen.LineJoin = LineJoin.Round;
                g.DrawPolygon(pen, trai);
                g.DrawPolygon(pen, phai);
            }
        }

        /// <summary>Chồng 4 cuốn sách màu ở góc trái banner.</summary>
        private void VeChongSach(Graphics g, int x, int day)
        {
            int[] rong = { S(92), S(80), S(70), S(58) };
            int[] cao = { S(17), S(15), S(14), S(13) };
            int[] lech = { 0, S(8), S(2), S(10) };
            string[] mau = { "#F97316", "#2563EB", "#16A34A", "#0EA5E9" };

            int y = day;
            for (int i = 0; i < mau.Length; i++)
            {
                y -= cao[i];
                Rectangle r = new Rectangle(x + lech[i], y, rong[i], cao[i]);
                using (GraphicsPath p = TaoDuongBoGoc(r, S(3)))
                using (SolidBrush br = new SolidBrush(Mau(mau[i])))
                    g.FillPath(br, p);

                // mép giấy
                Rectangle giay = new Rectangle(r.Right - S(12), r.Y + S(3), S(9), Math.Max(2, r.Height - S(6)));
                using (SolidBrush br = new SolidBrush(Color.FromArgb(235, 255, 251, 235)))
                    g.FillRectangle(br, giay);
            }
        }

        private void VeBanner(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            if (pnlBanner.Width < 20 || pnlBanner.Height < 20) return;

            Rectangle r = new Rectangle(0, 0, pnlBanner.Width - 1, pnlBanner.Height - 1);
            using (GraphicsPath p = TaoDuongBoGoc(r, S(12)))
            {
                using (LinearGradientBrush br = new LinearGradientBrush(r, Mau("#EEF5FF"), Mau("#D9E8FB"), LinearGradientMode.Horizontal))
                    g.FillPath(br, p);

                // vài hình tròn mờ trang trí (cắt theo khung bo góc)
                g.SetClip(p);
                using (SolidBrush br = new SolidBrush(Color.FromArgb(90, 255, 255, 255)))
                {
                    g.FillEllipse(br, -S(40), -S(60), S(260), S(260));
                    g.FillEllipse(br, pnlBanner.Width - S(220), -S(70), S(240), S(240));
                }
                g.ResetClip();

                using (Pen pen = new Pen(Mau("#D5E3F7")))
                    g.DrawPath(pen, p);
            }

            VeChongSach(g, S(34), pnlBanner.Height - S(18));

            VeSachMo(g, new Rectangle(pnlBanner.Width - S(112), pnlBanner.Height / 2 - S(8), S(84), S(54)),
                Color.FromArgb(150, 176, 206, 244), Color.FromArgb(210, 150, 185, 235));
        }

        private void VeChuThichMuonTra(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            string[] ten = { "Mượn sách", "Trả sách" };
            Color[] mau = { mauMuon, mauTra };
            int x = S(4);
            int h = pnlChuThichMuonTra.Height;

            for (int i = 0; i < ten.Length; i++)
            {
                using (SolidBrush br = new SolidBrush(mau[i]))
                    g.FillEllipse(br, x, (h - S(9)) / 2, S(9), S(9));
                TextRenderer.DrawText(g, ten[i], fPhu, new Rectangle(x + S(14), 0, S(80), h), Mau("#4B5563"),
                    TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPadding);
                x += S(94);
            }
        }

        private void VeChuThichTheLoai(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            int n = tenTheLoai.Length;
            int cao = S(30);
            int tong = Math.Max(1, soTheLoai.Sum());
            int y0 = (pnlChuThichTheLoai.Height - n * cao) / 2;
            int rong = pnlChuThichTheLoai.Width;

            for (int i = 0; i < n; i++)
            {
                int y = y0 + i * cao;
                using (SolidBrush br = new SolidBrush(mauTheLoai[i]))
                    g.FillEllipse(br, S(10), y + (cao - S(10)) / 2, S(10), S(10));


                TextRenderer.DrawText(g, tenTheLoai[i], fChuMuc, new Rectangle(S(30), y, Math.Max(10, rong - S(30) - S(64)), cao),
    Mau("#374151"),
    TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPadding | TextFormatFlags.EndEllipsis);

                int phanTram = (int)Math.Round(soTheLoai[i] * 100.0 / tong);
                TextRenderer.DrawText(g, phanTram + "%", fChuMuc, new Rectangle(rong - S(64), y, S(54), cao),
                    Mau("#374151"),
                    TextFormatFlags.Right | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPadding);
            }
        }

        /// <summary>Vẽ tổng số sách vào giữa lỗ của biểu đồ tròn.</summary>
        private void ChartTheLoai_PostPaint(object sender, DV.ChartPaintEventArgs e)
        {
            if (!(e.ChartElement is DV.ChartArea)) return;

            Graphics g = e.ChartGraphics.Graphics;
            float w = chartTheLoai.Width;
            float cy = chartTheLoai.Height / 2f;

            using (StringFormat sf = new StringFormat())
            using (SolidBrush brSo = new SolidBrush(mauNavy))
            using (SolidBrush brNhan = new SolidBrush(mauChuPhu))
            {
                sf.Alignment = StringAlignment.Center;
                sf.LineAlignment = StringAlignment.Center;
                g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;

                g.DrawString(lblSoSach.Text, fSoTam, brSo, new RectangleF(0, cy - S(26), w, S(36)), sf);
                g.DrawString("Tổng sách", fNhanTam, brNhan, new RectangleF(0, cy + S(8), w, S(22)), sf);
            }
        }

        /// <summary>Menu thả xuống của nút chuông, màu đồng bộ với giao diện.</summary>
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

        // =====================================================================
        //  NÚT 3 GẠCH & CHUÔNG
        // =====================================================================

        private void KhoiTaoNutHeader()
        {
            menuThongBao = new ContextMenuStrip();
            menuThongBao.Renderer = new ToolStripProfessionalRenderer(new MenuColors());
            menuThongBao.ShowImageMargin = false;
            menuThongBao.BackColor = Color.White;
            menuThongBao.Font = AppTheme.Base;

            btnMenu.Click += btnMenu_Click;
            btnThongBao.Click += btnThongBao_Click;

            // Chấm đỏ trên chuông khi có phiếu quá hạn
            btnThongBao.Paint += btnThongBao_Paint;
            lblSoQuaHan.TextChanged += (s, e) => btnThongBao.Invalidate();
        }

        // Nút 3 gạch: ẩn/hiện thanh bên
        private void btnMenu_Click(object sender, EventArgs e)
        {
            pnlSidebar.Visible = !pnlSidebar.Visible;
        }

        // Nút chuông: mở danh sách thông báo ngay dưới nút
        private void btnThongBao_Click(object sender, EventArgs e)
        {
            XayDungThongBao();
            menuThongBao.PerformLayout();

            int x = btnThongBao.Width - menuThongBao.PreferredSize.Width;
            menuThongBao.Show(btnThongBao, new Point(x, btnThongBao.Height));
        }

        private void btnThongBao_Paint(object sender, PaintEventArgs e)
        {
            if (DocSo(lblSoQuaHan) <= 0) return;

            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            using (SolidBrush brush = new SolidBrush(AppTheme.Danger))
                e.Graphics.FillEllipse(brush, btnThongBao.Width / 2 + S(4), S(7), S(9), S(9));
        }

        private void XayDungThongBao()
        {
            menuThongBao.Items.Clear();

            int quaHan = DocSo(lblSoQuaHan);
            int dangMuon = DocSo(lblSoDangMuon);

            ToolStripMenuItem tieuDe = new ToolStripMenuItem("Thông báo");
            tieuDe.Enabled = false;
            tieuDe.Font = AppTheme.Bold;
            menuThongBao.Items.Add(tieuDe);
            menuThongBao.Items.Add(new ToolStripSeparator());

            if (quaHan > 0)
                menuThongBao.Items.Add(TaoMucThongBao(
                    "Có " + quaHan + " phiếu mượn quá hạn, cần nhắc độc giả trả sách",
                    AppTheme.Danger));

            if (dangMuon > 0)
                menuThongBao.Items.Add(TaoMucThongBao(
                    "Đang có " + dangMuon + " phiếu mượn chưa hoàn tất",
                    AppTheme.TextDark));

            if (quaHan <= 0 && dangMuon <= 0)
            {
                ToolStripMenuItem rong = new ToolStripMenuItem("Không có thông báo mới");
                rong.Enabled = false;
                menuThongBao.Items.Add(rong);
            }
        }

        private ToolStripMenuItem TaoMucThongBao(string noiDung, Color mau)
        {
            ToolStripMenuItem item = new ToolStripMenuItem(noiDung);
            item.ForeColor = mau;
            item.Padding = new Padding(4, 6, 4, 6);
            // Bấm vào thông báo thì mở Danh sách phiếu mượn
            item.Click += (s, e) => btnDanhSachPhieuMuon_Click(s, e);
            return item;
        }

        private int DocSo(Label lbl)
        {
            int n;
            return int.TryParse(lbl.Text.Trim(), out n) ? n : 0;
        }

        // =====================================================================
        //  BIỂU ĐỒ
        // =====================================================================

        // Dấu vân tay dữ liệu lần vẽ trước: dữ liệu không đổi thì không vẽ lại (đỡ nháy).
        private string khoaBieuDoMuonTra = "";
        private string khoaBieuDoTheLoai = "";

        /// <summary>Vẽ lại cả 2 biểu đồ từ CSDL. Gọi từ CapNhatThongKe().</summary>
        private void CapNhatBieuDo()
        {
            try
            {
                VeBieuDoMuonTra();
                VeBieuDoTheLoai();
            }
            catch (Exception ex)
            {
                // Lỗi CSDL đã được CapNhatThongKe báo rồi, ở đây chỉ ghi log để khỏi hiện hộp thoại lặp lại
                System.Diagnostics.Debug.WriteLine("Không vẽ được biểu đồ: " + ex.Message);
            }
        }

        /// <summary>Chọn bước chia trục tung (số nguyên) sao cho có tối đa khoảng 5 vạch.</summary>
        private static int BuocChia(int max)
        {
            int[] cacBuoc = { 1, 2, 5, 10, 20, 50, 100, 200, 500, 1000 };
            foreach (int b in cacBuoc)
            {
                if (max / (double)b <= 5) return b;
            }
            return 1000;
        }

        /// <summary>
        /// Đếm số phiếu theo từng tháng, bắt đầu từ tháng "tu", đủ soThang tháng.
        /// tenCot chỉ nhận NgayMuon hoặc NgayTra (cố định trong code, không lấy từ người dùng).
        /// </summary>
        private int[] DemTheoThang(string tenCot, DateTime tu, int soThang)
        {
            DataTable dt = DbConnection.ExecuteQuery(
                "SELECT YEAR(" + tenCot + ") AS Nam, MONTH(" + tenCot + ") AS Thang, COUNT(*) AS SoLuot " +
                "FROM PhieuMuon " +
                "WHERE " + tenCot + " IS NOT NULL AND " + tenCot + " >= @Tu " +
                "GROUP BY YEAR(" + tenCot + "), MONTH(" + tenCot + ")",
                DbConnection.Param("@Tu", tu));

            int[] kq = new int[soThang];
            foreach (DataRow r in dt.Rows)
            {
                int nam = Convert.ToInt32(r["Nam"]);
                int thang = Convert.ToInt32(r["Thang"]);
                int viTri = (nam - tu.Year) * 12 + (thang - tu.Month);

                if (viTri >= 0 && viTri < soThang)
                    kq[viTri] = Convert.ToInt32(r["SoLuot"]);
            }
            return kq;
        }

        private void VeBieuDoMuonTra()
        {
            if (chartMuonTra == null) return;

            // 6 tháng gần nhất, tính cả tháng hiện tại
            const int soThang = 6;
            DateTime tu = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1).AddMonths(-(soThang - 1));

            int[] soLuotMuon = DemTheoThang("NgayMuon", tu, soThang);
            int[] soLuotTra = DemTheoThang("NgayTra", tu, soThang);

            string khoa = tu.ToString("yyyyMM") + "|" + string.Join(",", soLuotMuon) + "|" + string.Join(",", soLuotTra);
            if (khoa == khoaBieuDoMuonTra) return;   // dữ liệu không đổi
            khoaBieuDoMuonTra = khoa;

            chartMuonTra.Series.Clear();
            chartMuonTra.ChartAreas.Clear();
            chartMuonTra.Legends.Clear();      // chú thích tự vẽ ở góc phải tiêu đề
            chartMuonTra.BackColor = Color.White;

            // Trục tung tự co giãn theo số liệu, luôn là số nguyên
            int max = Math.Max(4, Math.Max(soLuotMuon.Max(), soLuotTra.Max()));
            int buoc = BuocChia(max);

            DV.ChartArea area = chartMuonTra.ChartAreas.Add("ChartArea1");
            area.BackColor = Color.White;

            area.AxisX.Interval = 1;
            area.AxisX.MajorGrid.Enabled = false;
            area.AxisX.MajorTickMark.Enabled = false;
            area.AxisX.LineColor = Mau("#CBD5E1");
            area.AxisX.LabelStyle.Font = new Font("Segoe UI", 8.5F);
            area.AxisX.LabelStyle.ForeColor = mauChuPhu;

            area.AxisY.Minimum = 0;
            area.AxisY.Maximum = Math.Ceiling(max / (double)buoc) * buoc;
            area.AxisY.Interval = buoc;
            area.AxisY.MajorGrid.LineColor = Mau("#E5EAF3");
            area.AxisY.MajorTickMark.Enabled = false;
            area.AxisY.LineColor = Color.Transparent;
            area.AxisY.LabelStyle.Font = new Font("Segoe UI", 8.5F);
            area.AxisY.LabelStyle.ForeColor = mauChuPhu;

            DV.Series seriesMuon = chartMuonTra.Series.Add("SeriesMuon");
            DV.Series seriesTra = chartMuonTra.Series.Add("SeriesTra");

            foreach (DV.Series s in new DV.Series[] { seriesMuon, seriesTra })
            {
                s.ChartArea = "ChartArea1";
                s.ChartType = DV.SeriesChartType.Column;
                s.IsVisibleInLegend = false;
                s["PointWidth"] = "0.6";
            }

            seriesMuon.Color = mauMuon;
            seriesTra.Color = mauTra;
            seriesMuon.ToolTip = "Mượn sách #VALX: #VAL lượt";
            seriesTra.ToolTip = "Trả sách #VALX: #VAL lượt";

            for (int i = 0; i < soThang; i++)
            {
                DateTime thang = tu.AddMonths(i);
                string nhan = "T" + thang.Month + "/" + thang.Year;

                seriesMuon.Points.AddXY(nhan, soLuotMuon[i]);
                seriesTra.Points.AddXY(nhan, soLuotTra[i]);
            }

            chartMuonTra.Visible = true;
            chartMuonTra.Invalidate();
        }

        private void VeBieuDoTheLoai()
        {
            if (chartTheLoai == null) return;

            // Đếm số sách theo thể loại (nhiều nhất đứng đầu)
            DataTable dt = DbConnection.ExecuteQuery(@"
                SELECT tl.TenTheLoai, COUNT(s.MaSach) AS SoSach
                FROM TheLoai tl
                INNER JOIN Sach s ON s.MaTheLoai = tl.MaTheLoai
                GROUP BY tl.MaTheLoai, tl.TenTheLoai
                ORDER BY COUNT(s.MaSach) DESC, tl.TenTheLoai");

            // Tối đa 6 phần (bằng số màu): quá 6 thể loại thì 5 thể loại đầu + "Khác"
            int soPhanToiDa = mauTheLoai.Length;
            List<string> ten = new List<string>();
            List<int> so = new List<int>();

            if (dt.Rows.Count <= soPhanToiDa)
            {
                foreach (DataRow r in dt.Rows)
                {
                    ten.Add(r["TenTheLoai"].ToString());
                    so.Add(Convert.ToInt32(r["SoSach"]));
                }
            }
            else
            {
                int khac = 0;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    int soSach = Convert.ToInt32(dt.Rows[i]["SoSach"]);
                    if (i < soPhanToiDa - 1)
                    {
                        ten.Add(dt.Rows[i]["TenTheLoai"].ToString());
                        so.Add(soSach);
                    }
                    else
                    {
                        khac += soSach;
                    }
                }
                ten.Add("Khác (" + (dt.Rows.Count - (soPhanToiDa - 1)) + " thể loại)");
                so.Add(khac);
            }

            string khoa = string.Join("|", ten) + "#" + string.Join(",", so);
            if (khoa == khoaBieuDoTheLoai) return;   // dữ liệu không đổi
            khoaBieuDoTheLoai = khoa;

            tenTheLoai = ten.ToArray();
            soTheLoai = so.ToArray();

            chartTheLoai.Series.Clear();
            chartTheLoai.ChartAreas.Clear();
            chartTheLoai.Legends.Clear();      // chú thích dạng danh sách tự vẽ bên phải
            chartTheLoai.BackColor = Color.White;

            DV.ChartArea area = chartTheLoai.ChartAreas.Add("ChartArea1");
            area.BackColor = Color.White;
            area.Position = new DV.ElementPosition(0, 0, 100, 100);
            area.InnerPlotPosition = new DV.ElementPosition(6, 6, 88, 88);

            DV.Series s = chartTheLoai.Series.Add("TheLoai");
            s.ChartArea = "ChartArea1";
            s.ChartType = DV.SeriesChartType.Doughnut;
            s.IsVisibleInLegend = false;
            s.IsValueShownAsLabel = false;
            s["DoughnutRadius"] = "62";      // lỗ giữa to để đặt số tổng
            s["PieStartAngle"] = "270";      // bắt đầu từ chính giữa phía trên
            s.BorderWidth = 3;
            s.BorderColor = Color.White;
            s["PieLabelStyle"] = "Disabled";     // tắt nhãn tên in trên vòng tròn

            if (tenTheLoai.Length == 0)
            {
                // Chưa có sách nào: vẽ vòng xám nhạt thay vì để trống
                int idx = s.Points.AddXY("Chưa có dữ liệu", 1);
                s.Points[idx].Color = Mau("#E2E8F0");
            }
            else
            {
                s.ToolTip = "#VALX: #VAL sách";
                for (int i = 0; i < tenTheLoai.Length; i++)
                {
                    int idx = s.Points.AddXY(tenTheLoai[i], soTheLoai[i]);
                    s.Points[idx].Color = mauTheLoai[i];
                }
            }

            chartTheLoai.Visible = true;
            chartTheLoai.Invalidate();
            pnlChuThichTheLoai.Invalidate();
        }

        // =====================================================================
        //  CODE CHỨC NĂNG (giữ nguyên)
        // =====================================================================

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
                // Chỉ báo 1 lần, tránh vòng lặp Activated -> lỗi -> Activated
                if (!daBaoLoiThongKe)
                {
                    daBaoLoiThongKe = true;
                    MessageBox.Show(
                        "Không thể tải số liệu thống kê.\n\n" + ex.Message,
                        "Lỗi thống kê",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }

            TaiHoatDongGanDay();
            CapNhatBieuDo();      // <-- THÊM: vẽ lại 2 biểu đồ từ dữ liệu thật
        }

        /// <summary>Nạp 4 hoạt động mới nhất từ bảng NhatKyHoatDong vào bảng ở trang chủ.</summary>
        private void TaiHoatDongGanDay()
        {
            if (dgvHoatDongGanDay == null) return;

            try
            {
                DataTable dt = NhatKy.LayMoiNhat(4);

                dgvHoatDongGanDay.Rows.Clear();
                foreach (DataRow r in dt.Rows)
                {
                    dgvHoatDongGanDay.Rows.Add(
                        Convert.ToDateTime(r["ThoiGian"]).ToString("HH:mm dd/MM/yyyy"),
                        r["LoaiHoatDong"],
                        r["ChiTiet"],
                        r["NguoiThucHien"]);
                }
                dgvHoatDongGanDay.ClearSelection();
            }
            catch (Exception ex)
            {
                // Chưa tạo bảng NhatKyHoatDong thì để trống, không báo lỗi làm phiền người dùng
                System.Diagnostics.Debug.WriteLine("Không đọc được nhật ký: " + ex.Message);
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
            // Hiện tên người đăng nhập (FrmLogin gán vào NhatKy.NguoiThucHien)
            if (!string.IsNullOrWhiteSpace(NhatKy.NguoiThucHien))
                lblHoTenNguoiDung.Text = NhatKy.NguoiThucHien;
            KhoiTaoPhanQuyen();

            CapNhatThongKe();
            if (System.ComponentModel.LicenseManager.UsageMode
       == System.ComponentModel.LicenseUsageMode.Designtime)
            {
                return;
            }
            // ==========================================
            // 1. HOẠT ĐỘNG GẦN ĐÂY (đọc từ bảng NhatKyHoatDong, nạp trong CapNhatThongKe)
            // ==========================================

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
            // 4. BIỂU ĐỒ
            // ==========================================

            CapNhatBieuDo();   // có try/catch, và bỏ qua nếu dữ liệu không đổi
        }

        private ContextMenuStrip menuNguoiDung;
        private LinkLabel lnkXemNhatKy;

        /// <summary>Phân quyền + menu người dùng. Gọi trong FrmMain_Load.</summary>
        private void KhoiTaoPhanQuyen()
        {
            lblChucVuNguoiDung.Text = TaiKhoan.LaQuanTri ? "Quản trị viên" : "Nhân viên thư viện";

            // Nhân viên thường: ẩn các mục dành riêng cho quản trị viên
            if (!TaiKhoan.LaQuanTri)
            {
                int cao = btnQuanLyNhanVien.Height;
                btnQuanLyNhanVien.Visible = false;
                pnlNhomQuanLy.Height -= cao;
                chieuCaoNhom[pnlNhomQuanLy] -= cao;
                CapNhatChanSidebar();

                AnTheNhanVien();
            }

            // Bấm vào tên / ảnh đại diện ở góc phải: menu Đổi mật khẩu, Đăng xuất
            menuNguoiDung = new ContextMenuStrip();
            menuNguoiDung.Renderer = new ToolStripProfessionalRenderer(new MenuColors());
            menuNguoiDung.ShowImageMargin = false;
            menuNguoiDung.BackColor = Color.White;
            menuNguoiDung.Font = AppTheme.Base;

            ToolStripMenuItem doiMk = new ToolStripMenuItem("Đổi mật khẩu");
            doiMk.Padding = new Padding(4, 6, 4, 6);
            doiMk.Click += (s, e) => MoHopThoaiDoiMatKhau();
            menuNguoiDung.Items.Add(doiMk);

            ToolStripMenuItem dangXuat = new ToolStripMenuItem("Đăng xuất");
            dangXuat.Padding = new Padding(4, 6, 4, 6);
            dangXuat.Click += (s, e) => btnDangXuat_Click(s, e);
            menuNguoiDung.Items.Add(dangXuat);

            EventHandler mo = (s, e) => menuNguoiDung.Show(Cursor.Position);
            foreach (Control c in new Control[] { picDaiDien, lblHoTenNguoiDung, lblChucVuNguoiDung })
            {
                c.Cursor = Cursors.Hand;
                c.Click += mo;
            }

            // Nhắc đổi mật khẩu nếu vẫn đang dùng mật khẩu mặc định
            BeginInvoke((Action)(() =>
            {
                try
                {
                    if (TaiKhoan.KiemTraMatKhau(NhatKy.MaNhanVien, QuyDinh.MatKhauMacDinh) &&
                        AppTheme.Confirm("Tài khoản của bạn đang dùng mật khẩu mặc định.\nBạn có muốn đổi mật khẩu ngay không?"))
                        MoHopThoaiDoiMatKhau();
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("Không kiểm tra được mật khẩu mặc định: " + ex.Message);
                }
            }));
        }

        private void MoHopThoaiDoiMatKhau()
        {
            using (FrmDoiMatKhau f = new FrmDoiMatKhau())
                f.ShowDialog(this);
        }

        /// <summary>Bỏ thẻ "Quản lý nhân viên" khỏi Truy cập nhanh và dồn các thẻ còn lại.</summary>
        private void AnTheNhanVien()
        {
            tlpTruyCapNhanh.Controls.Remove(pnlTheQuanLyNhanVien);
            tlpTruyCapNhanh.SetColumn(pnlTheLapPhieuMuon, 2);
            tlpTruyCapNhanh.SetColumn(pnlTheTraSach, 3);
            tlpTruyCapNhanh.SetColumn(pnlTheTimKiemSach, 4);
            tlpTruyCapNhanh.ColumnStyles.RemoveAt(tlpTruyCapNhanh.ColumnStyles.Count - 1);
            tlpTruyCapNhanh.ColumnCount = 5;
            foreach (ColumnStyle cs in tlpTruyCapNhanh.ColumnStyles)
            {
                cs.SizeType = SizeType.Percent;
                cs.Width = 20F;
            }
        }

        private void MoFormChucNang(Form form)
        {

            if ((form is FrmNhanVien || form is FrmNhatKy) && !TaiKhoan.LaQuanTri)
            {
                form.Dispose();
                AppTheme.Warning("Chức năng này chỉ dành cho quản trị viên.");
                return;
            }

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


        // Designer đang gắn các hàm Paint này vào thẻ, nên phải giữ lại (xóa sẽ lỗi build)
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


        // ===== Các nút thanh bên: dùng chung MoFormChucNang với thẻ "Truy cập nhanh" =====
        // MoFormChucNang tự kiểm tra form đã mở chưa (theo loại form) nên không bao giờ mở trùng.

        private void btnQuanLySach_Click(object sender, EventArgs e) { MoFormChucNang(new FrmSach()); }
        private void btnQuanLyTheLoai_Click(object sender, EventArgs e) { MoFormChucNang(new FrmTheLoai()); }
        private void btnQuanLyTacGia_Click(object sender, EventArgs e) { MoFormChucNang(new FrmTacGia()); }
        private void btnQuanLyDocGia_Click(object sender, EventArgs e) { MoFormChucNang(new FrmDocGia()); }
        private void btnQuanLyNhanVien_Click(object sender, EventArgs e) { MoFormChucNang(new FrmNhanVien()); }

        private void btnTimKiemSach_Click(object sender, EventArgs e) { MoFormChucNang(new FrmTimKiemSach()); }
        private void btnThongTinDocGia_Click(object sender, EventArgs e) { MoFormChucNang(new FrmThongTinDocGia()); }

        private void btnLapPhieuMuon_Click(object sender, EventArgs e) { MoFormChucNang(new FrmLapPhieuMuon()); }
        private void btnTraSach_Click(object sender, EventArgs e) { MoFormChucNang(new FrmTraSach()); }
        private void btnDanhSachPhieuMuon_Click(object sender, EventArgs e) { MoFormChucNang(new FrmDanhSachPhieuMuon()); }

        private void btnThongTinChuongTrinh_Click(object sender, EventArgs e) { MoFormChucNang(new FrmThongTinChuongTrinh()); }
        private void btnHuongDanSuDung_Click(object sender, EventArgs e) { MoFormChucNang(new FrmHuongDanSuDung()); }



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
