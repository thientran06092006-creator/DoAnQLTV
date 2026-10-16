using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace DoAnQLTV
{
    public partial class FrmThongTinDocGia : Form
    {
        private const string DinhDangNgay = "dd/MM/yyyy";
        private const string GoiY = "Chọn một độc giả trong danh sách để xem lịch sử mượn sách.";

        // Khung lịch sử mượn: tạo bằng code (không sửa Designer)
        private Panel pnlLichSu;
        private Label lblTomTat;
        private DataGridView dgvLichSu;

        public FrmThongTinDocGia()
        {
            InitializeComponent();
            AppTheme.ApplyForm(this);
            ThemKhungLichSu();
            ApDungGiaoDien();
            ThemCotHetHan();
            LoadDocGia();
            XoaLichSu();
        }

        // ================= GIAO DIỆN =================

        /// <summary>Thu danh sách độc giả lại, dành phần dưới cho lịch sử mượn.</summary>
        private void ThemKhungLichSu()
        {
            pnlDanhSach.Height = 200;   // trước đây 440

            pnlLichSu = new Panel();
            pnlLichSu.Name = "pnlLichSu";
            pnlLichSu.SetBounds(30, 372, 940, 228);

            lblTomTat = new Label();
            lblTomTat.AutoSize = false;
            lblTomTat.Font = AppTheme.Bold;
            lblTomTat.BackColor = Color.Transparent;
            lblTomTat.TextAlign = ContentAlignment.MiddleLeft;
            lblTomTat.SetBounds(16, 8, 908, 28);

            dgvLichSu = new DataGridView();
            dgvLichSu.Name = "dgvLichSu";
            dgvLichSu.SetBounds(1, 42, 938, 185);

            ThemCotLichSu("colLsMaPhieu", "Mã phiếu", 12F);
            ThemCotLichSu("colLsSach", "Sách", 26F);
            ThemCotLichSu("colLsNgayMuon", "Ngày mượn", 12F);
            ThemCotLichSu("colLsHanTra", "Hạn trả", 12F);
            ThemCotLichSu("colLsNgayTra", "Ngày trả", 12F);
            ThemCotLichSu("colLsGiaHan", "Gia hạn", 9F);
            ThemCotLichSu("colLsTrangThai", "Trạng thái", 15F);
            ThemCotLichSu("colLsTienPhat", "Tiền phạt", 22F);

            pnlLichSu.Controls.Add(lblTomTat);
            pnlLichSu.Controls.Add(dgvLichSu);
            Controls.Add(pnlLichSu);
        }

        private void ThemCotLichSu(string ten, string tieuDe, float trongSo)
        {
            DataGridViewTextBoxColumn col = new DataGridViewTextBoxColumn();
            col.Name = ten;
            col.HeaderText = tieuDe;
            col.FillWeight = trongSo;
            col.ReadOnly = true;
            dgvLichSu.Columns.Add(col);
        }

        /// <summary>Thêm cột "Hết hạn thẻ" vào danh sách độc giả (bằng code, không sửa Designer).</summary>
        private void ThemCotHetHan()
        {
            if (dgvKetQua.Columns.Contains("colHetHan")) return;

            DataGridViewTextBoxColumn col = new DataGridViewTextBoxColumn();
            col.Name = "colHetHan";
            col.HeaderText = "Hết hạn thẻ";
            col.FillWeight = 30F;
            col.ReadOnly = true;
            dgvKetQua.Columns.Add(col);
        }

        private void ApDungGiaoDien()
        {
            AppTheme.StyleCard(pnlThongTin);
            AppTheme.StyleCard(pnlDanhSach);
            AppTheme.StyleCard(pnlLichSu);

            AppTheme.StyleGrid(dgvKetQua);
            AppTheme.StyleGrid(dgvLichSu);
            AppTheme.StyleTextBox(txtTuKhoa);

            AppTheme.StyleSuccessButton(btnTimKiem);
            AppTheme.StyleButton(btnLamMoi,
                ColorTranslator.FromHtml("#E2E8F0"),
                ColorTranslator.FromHtml("#CBD5E1"),
                AppTheme.TextDark);

            // Nhấn Enter trong ô từ khóa = bấm Tìm kiếm
            AcceptButton = btnTimKiem;
        }

        // ================= NẠP DỮ LIỆU =================

        /// <summary>Lấy danh sách độc giả; tuKhoa rỗng/null = lấy tất cả.</summary>
        private DataTable TruyVanDocGia(string tuKhoa)
        {
            const string sql = @"
                SELECT MaDocGia, TenDocGia, NgaySinh, DiaChi, SoDienThoai, NgayHetHanThe
                FROM DocGia
                WHERE (@TuKhoa IS NULL
                       OR MaDocGia LIKE @TuKhoa
                       OR TenDocGia LIKE @TuKhoa
                       OR SoDienThoai LIKE @TuKhoa)
                ORDER BY MaDocGia";

            object tuKhoaLike = string.IsNullOrEmpty(tuKhoa) ? null : "%" + tuKhoa + "%";
            return DbConnection.ExecuteQuery(sql, DbConnection.Param("@TuKhoa", tuKhoaLike));
        }

        private void LoadDocGia()
        {
            try
            {
                HienThiLuoi(TruyVanDocGia(null));
            }
            catch (Exception ex)
            {
                AppTheme.Error("Không thể tải dữ liệu độc giả.\n\n" + ex.Message);
            }
        }

        private void HienThiLuoi(DataTable dt)
        {
            dgvKetQua.Rows.Clear();
            foreach (DataRow r in dt.Rows)
            {
                string ngaySinh = r["NgaySinh"] == DBNull.Value
                    ? ""
                    : Convert.ToDateTime(r["NgaySinh"]).ToString(DinhDangNgay);

                bool hetHan = false;
                string hetHanText = "";
                if (r["NgayHetHanThe"] != DBNull.Value)
                {
                    DateTime het = Convert.ToDateTime(r["NgayHetHanThe"]).Date;
                    hetHan = het < DateTime.Today;
                    hetHanText = het.ToString(DinhDangNgay) + (hetHan ? " (hết hạn)" : "");
                }

                int idx = dgvKetQua.Rows.Add(
                    r["MaDocGia"],
                    r["TenDocGia"],
                    ngaySinh,
                    r["DiaChi"],
                    r["SoDienThoai"],
                    hetHanText);

                if (hetHan)
                {
                    DataGridViewCell o = dgvKetQua.Rows[idx].Cells["colHetHan"];
                    o.Style.ForeColor = AppTheme.Danger;
                    o.Style.SelectionForeColor = AppTheme.Danger;
                    o.Style.Font = AppTheme.Bold;
                }
            }

            dgvKetQua.ClearSelection();
            dgvKetQua.CurrentCell = null;
        }

        // ================= LỊCH SỬ MƯỢN =================

        private void XoaLichSu()
        {
            dgvLichSu.Rows.Clear();
            lblTomTat.ForeColor = AppTheme.TextMuted;
            lblTomTat.Text = GoiY;
        }

        /// <summary>Hiện toàn bộ phiếu của độc giả + tóm tắt: thẻ, đang mượn, quá hạn, tiền phạt (đã thu / chưa thu).</summary>
        private void HienLichSu(string maDocGia, string tenDocGia)
        {
            try
            {
                DataTable dt = DbConnection.ExecuteQuery(@"
                    SELECT pm.MaPhieu,
                           s.MaSach + ' - ' + s.TenSach AS Sach,
                           pm.NgayMuon, pm.HanTra, pm.NgayTra, pm.TienPhat, pm.DaThuPhat, pm.SoLanGiaHan
                    FROM PhieuMuon pm
                    INNER JOIN Sach s ON pm.MaSach = s.MaSach
                    WHERE pm.MaDocGia = @MaDocGia
                    ORDER BY pm.NgayMuon DESC, pm.MaPhieu DESC",
                    DbConnection.Param("@MaDocGia", maDocGia));

                DateTime hetHanThe = DbConnection.ExecuteScalar<DateTime>(
                    "SELECT NgayHetHanThe FROM DocGia WHERE MaDocGia = @MaDocGia",
                    DbConnection.Param("@MaDocGia", maDocGia));

                dgvLichSu.Rows.Clear();

                int dangMuon = 0, quaHan = 0;
                decimal tongPhat = 0, phatChuaThu = 0, phatTamTinh = 0;

                foreach (DataRow r in dt.Rows)
                {
                    DateTime hanTra = Convert.ToDateTime(r["HanTra"]).Date;
                    int soLanGiaHan = Convert.ToInt32(r["SoLanGiaHan"]);
                    string ngayTraText = "";
                    string trangThai;
                    string tienPhatText = "";
                    Color mau;
                    Color? mauPhat = null;

                    if (r["NgayTra"] != DBNull.Value)
                    {
                        DateTime ngayTra = Convert.ToDateTime(r["NgayTra"]).Date;
                        ngayTraText = ngayTra.ToString(DinhDangNgay);
                        trangThai = "Đã trả";
                        mau = AppTheme.Success;

                        // Phiếu trả trước khi có tính năng phạt: TienPhat = NULL -> tính lại từ ngày tháng
                        decimal phat = r["TienPhat"] == DBNull.Value
                            ? QuyDinh.TinhTienPhat(hanTra, ngayTra)
                            : Convert.ToDecimal(r["TienPhat"]);

                        if (phat > 0)
                        {
                            bool daThu = Convert.ToBoolean(r["DaThuPhat"]);
                            tienPhatText = QuyDinh.DinhDangTien(phat) + (daThu ? " - Đã thu" : " - Chưa thu");
                            mauPhat = daThu ? AppTheme.Success : AppTheme.Danger;
                            tongPhat += phat;
                            if (!daThu) phatChuaThu += phat;
                        }
                    }
                    else if (hanTra < DateTime.Today)
                    {
                        int soNgay = (DateTime.Today - hanTra).Days;
                        decimal phat = soNgay * QuyDinh.MucPhatMoiNgay;
                        trangThai = "Quá hạn " + soNgay + " ngày";
                        mau = AppTheme.Danger;
                        tienPhatText = QuyDinh.DinhDangTien(phat) + " (tạm tính)";
                        mauPhat = AppTheme.Danger;
                        phatTamTinh += phat;
                        dangMuon++;
                        quaHan++;
                    }
                    else
                    {
                        trangThai = "Đang mượn";
                        mau = AppTheme.Accent;
                        dangMuon++;
                    }

                    int idx = dgvLichSu.Rows.Add(
                        r["MaPhieu"],
                        r["Sach"],
                        Convert.ToDateTime(r["NgayMuon"]).ToString(DinhDangNgay),
                        hanTra.ToString(DinhDangNgay),
                        ngayTraText,
                        soLanGiaHan > 0 ? soLanGiaHan + " lần" : "",
                        trangThai,
                        tienPhatText);

                    DataGridViewCell o = dgvLichSu.Rows[idx].Cells["colLsTrangThai"];
                    o.Style.ForeColor = mau;
                    o.Style.SelectionForeColor = mau;
                    o.Style.Font = AppTheme.Bold;

                    if (mauPhat.HasValue)
                    {
                        DataGridViewCell p = dgvLichSu.Rows[idx].Cells["colLsTienPhat"];
                        p.Style.ForeColor = mauPhat.Value;
                        p.Style.SelectionForeColor = mauPhat.Value;
                        p.Style.Font = AppTheme.Bold;
                    }
                }

                dgvLichSu.ClearSelection();
                dgvLichSu.CurrentCell = null;

                // Thẻ: NULL (default) nghĩa là không giới hạn
                bool theHetHan = hetHanThe != default(DateTime) && hetHanThe.Date < DateTime.Today;
                string chuThe = hetHanThe == default(DateTime)
                    ? "Thẻ: không giới hạn"
                    : (theHetHan
                        ? "THẺ ĐÃ HẾT HẠN (" + hetHanThe.ToString(DinhDangNgay) + ")"
                        : "Thẻ đến " + hetHanThe.ToString(DinhDangNgay));

                string tomTat = tenDocGia + "  |  " + chuThe +
                                "  |  Đang mượn: " + dangMuon + "/" + QuyDinh.SoSachToiDa +
                                "  |  Quá hạn: " + quaHan +
                                "  |  Phiếu: " + dt.Rows.Count +
                                "  |  Phạt: " + QuyDinh.DinhDangTien(tongPhat);
                if (phatChuaThu > 0)
                    tomTat += " (chưa thu " + QuyDinh.DinhDangTien(phatChuaThu) + ")";
                if (phatTamTinh > 0)
                    tomTat += " + " + QuyDinh.DinhDangTien(phatTamTinh) + " tạm tính";

                lblTomTat.Text = tomTat;
                lblTomTat.ForeColor = (theHetHan || quaHan > 0 || phatChuaThu > 0)
                    ? AppTheme.Danger
                    : AppTheme.Primary;
            }
            catch (Exception ex)
            {
                AppTheme.Error("Không thể tải lịch sử mượn.\n\n" + ex.Message);
            }
        }

        // ================= SỰ KIỆN =================

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            string tuKhoa = txtTuKhoa.Text.Trim();
            if (tuKhoa == "")
            {
                AppTheme.Warning("Vui lòng nhập từ khóa cần tìm.");
                txtTuKhoa.Focus();
                return;
            }

            try
            {
                DataTable dt = TruyVanDocGia(tuKhoa);
                HienThiLuoi(dt);

                if (dt.Rows.Count == 0)
                {
                    XoaLichSu();
                    AppTheme.Info("Không tìm thấy độc giả phù hợp.");
                    txtTuKhoa.Focus();
                }
                else if (dt.Rows.Count == 1)
                {
                    // Chỉ có 1 kết quả thì hiện luôn lịch sử mượn của người đó
                    HienLichSu(dt.Rows[0]["MaDocGia"].ToString(), dt.Rows[0]["TenDocGia"].ToString());
                }
                else
                {
                    XoaLichSu();
                }
            }
            catch (Exception ex)
            {
                AppTheme.Error("Không thể tìm kiếm độc giả.\n\n" + ex.Message);
            }
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            txtTuKhoa.Clear();
            LoadDocGia();
            XoaLichSu();
            txtTuKhoa.Focus();
        }

        private void dgvKetQua_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            DataGridViewRow row = dgvKetQua.Rows[e.RowIndex];
            string ma = row.Cells["colMaDocGia"].Value?.ToString() ?? "";
            string ten = row.Cells["colTenDocGia"].Value?.ToString() ?? "";

            txtTuKhoa.Text = ten;
            HienLichSu(ma, ten);
        }
    }
}
