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

            ThemCot("colLsMaPhieu", "Mã phiếu", 14F);
            ThemCot("colLsSach", "Sách", 34F);
            ThemCot("colLsNgayMuon", "Ngày mượn", 14F);
            ThemCot("colLsHanTra", "Hạn trả", 14F);
            ThemCot("colLsNgayTra", "Ngày trả", 14F);
            ThemCot("colLsTrangThai", "Trạng thái", 18F);
            ThemCot("colLsTienPhat", "Tiền phạt", 20F);

            pnlLichSu.Controls.Add(lblTomTat);
            pnlLichSu.Controls.Add(dgvLichSu);
            Controls.Add(pnlLichSu);
        }

        private void ThemCot(string ten, string tieuDe, float trongSo)
        {
            DataGridViewTextBoxColumn col = new DataGridViewTextBoxColumn();
            col.Name = ten;
            col.HeaderText = tieuDe;
            col.FillWeight = trongSo;
            col.ReadOnly = true;
            dgvLichSu.Columns.Add(col);
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
                SELECT MaDocGia, TenDocGia, NgaySinh, DiaChi, SoDienThoai
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

                dgvKetQua.Rows.Add(
                    r["MaDocGia"],
                    r["TenDocGia"],
                    ngaySinh,
                    r["DiaChi"],
                    r["SoDienThoai"]);
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

        /// <summary>Hiện toàn bộ phiếu của độc giả + tóm tắt: đang mượn, quá hạn, tiền phạt.</summary>
        private void HienLichSu(string maDocGia, string tenDocGia)
        {
            try
            {
                DataTable dt = DbConnection.ExecuteQuery(@"
                    SELECT pm.MaPhieu,
                           s.MaSach + ' - ' + s.TenSach AS Sach,
                           pm.NgayMuon, pm.HanTra, pm.NgayTra, pm.TienPhat
                    FROM PhieuMuon pm
                    INNER JOIN Sach s ON pm.MaSach = s.MaSach
                    WHERE pm.MaDocGia = @MaDocGia
                    ORDER BY pm.NgayMuon DESC, pm.MaPhieu DESC",
                    DbConnection.Param("@MaDocGia", maDocGia));

                dgvLichSu.Rows.Clear();

                int dangMuon = 0, quaHan = 0;
                decimal phatDaTinh = 0, phatTamTinh = 0;

                foreach (DataRow r in dt.Rows)
                {
                    DateTime hanTra = Convert.ToDateTime(r["HanTra"]).Date;
                    string ngayTraText = "";
                    string trangThai;
                    string tienPhatText = "";
                    Color mau;

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
                            tienPhatText = QuyDinh.DinhDangTien(phat);
                            phatDaTinh += phat;
                        }
                    }
                    else if (hanTra < DateTime.Today)
                    {
                        int soNgay = (DateTime.Today - hanTra).Days;
                        decimal phat = soNgay * QuyDinh.MucPhatMoiNgay;
                        trangThai = "Quá hạn " + soNgay + " ngày";
                        mau = AppTheme.Danger;
                        tienPhatText = QuyDinh.DinhDangTien(phat) + " (tạm tính)";
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
                        trangThai,
                        tienPhatText);

                    DataGridViewCell o = dgvLichSu.Rows[idx].Cells["colLsTrangThai"];
                    o.Style.ForeColor = mau;
                    o.Style.SelectionForeColor = mau;
                    o.Style.Font = AppTheme.Bold;
                }

                dgvLichSu.ClearSelection();
                dgvLichSu.CurrentCell = null;

                string tomTat = tenDocGia + "  |  Đang mượn: " + dangMuon + "/" + QuyDinh.SoSachToiDa +
                                " cuốn  |  Quá hạn: " + quaHan +
                                "  |  Tổng số phiếu: " + dt.Rows.Count +
                                "  |  Tiền phạt: " + QuyDinh.DinhDangTien(phatDaTinh);
                if (phatTamTinh > 0)
                    tomTat += " + " + QuyDinh.DinhDangTien(phatTamTinh) + " tạm tính";

                lblTomTat.Text = tomTat;
                lblTomTat.ForeColor = quaHan > 0 ? AppTheme.Danger : AppTheme.Primary;
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
