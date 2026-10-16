using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace DoAnQLTV
{
    public partial class FrmDanhSachPhieuMuon : Form
    {
        private const string DinhDangNgay = "dd/MM/yyyy";

        // Các nút tạo bằng code (không sửa Designer)
        private Button btnGiaHan;
        private Button btnThuPhat;
        private Button btnXuatExcel;
        private Button btnInQuaHan;

        public FrmDanhSachPhieuMuon()
        {
            InitializeComponent();

            AppTheme.ApplyForm(this);
            ThemNutChucNang();
            ApDungGiaoDien();
            ThemCotTrangThai();

            LoadPhieuMuon(null);
        }

        // ================= GIAO DIỆN =================

        /// <summary>
        /// Hàng 1 giữ nguyên (ô từ khóa, Tìm kiếm, Làm mới). Thêm hàng 2 chứa các nút chức năng:
        /// Gia hạn | Thu phạt | Xuất Excel quá hạn | In quá hạn.
        /// </summary>
        private void ThemNutChucNang()
        {
            const int doi = 48;

            pnlThongTin.Height += doi;
            pnlDanhSach.Top += doi;
            ClientSize = new Size(ClientSize.Width, ClientSize.Height + doi);

            btnGiaHan = TaoNut("btnGiaHan", "Gia hạn", 24, 70, 120, btnGiaHan_Click);
            btnThuPhat = TaoNut("btnThuPhat", "Thu phạt", 152, 70, 120, btnThuPhat_Click);
            btnXuatExcel = TaoNut("btnXuatExcel", "Xuất Excel quá hạn", 280, 70, 170, btnXuatExcel_Click);
            btnInQuaHan = TaoNut("btnInQuaHan", "In quá hạn", 458, 70, 120, btnInQuaHan_Click);
        }

        private Button TaoNut(string ten, string chu, int x, int y, int rong, EventHandler khiBam)
        {
            Button b = new Button();
            b.Name = ten;
            b.Text = chu;
            b.SetBounds(x, y, rong, 40);
            b.Click += khiBam;
            pnlThongTin.Controls.Add(b);
            return b;
        }

        private void ApDungGiaoDien()
        {
            AppTheme.StyleCard(pnlThongTin);
            AppTheme.StyleCard(pnlDanhSach);

            AppTheme.StyleGrid(dgvPhieuMuon);

            AppTheme.StyleTextBox(txtTuKhoa);

            AppTheme.StyleSuccessButton(btnTimKiem);
            AppTheme.StyleButton(btnLamMoi,
                ColorTranslator.FromHtml("#E2E8F0"),
                ColorTranslator.FromHtml("#CBD5E1"),
                AppTheme.TextDark);

            AppTheme.StylePrimaryButton(btnGiaHan);
            AppTheme.StyleButton(btnThuPhat, AppTheme.Amber, ColorTranslator.FromHtml("#D97706"));
            AppTheme.StyleOutlineButton(btnXuatExcel);
            AppTheme.StyleOutlineButton(btnInQuaHan);
        }

        /// <summary>Thêm các cột "Trạng thái", "Tiền phạt", "Gia hạn" bằng code, không cần sửa Designer.</summary>
        private void ThemCotTrangThai()
        {
            ThemCot("colTrangThai", "Trạng thái", 80F);
            ThemCot("colTienPhat", "Tiền phạt", 100F);
            ThemCot("colGiaHan", "Gia hạn", 50F);
        }

        private void ThemCot(string ten, string tieuDe, float trongSo)
        {
            if (dgvPhieuMuon.Columns.Contains(ten)) return;

            DataGridViewTextBoxColumn col = new DataGridViewTextBoxColumn();
            col.Name = ten;
            col.HeaderText = tieuDe;
            col.FillWeight = trongSo;
            col.ReadOnly = true;
            dgvPhieuMuon.Columns.Add(col);
        }

        // ================= NẠP DỮ LIỆU =================

        /// <summary>Lấy danh sách phiếu mượn; tuKhoa rỗng/null = lấy tất cả.</summary>
        private DataTable TruyVanPhieuMuon(string tuKhoa)
        {
            const string sql = @"
                SELECT pm.MaPhieu,
                       pm.MaDocGia + ' - ' + dg.TenDocGia AS DocGia,
                       pm.MaNhanVien + ' - ' + nv.TenNhanVien AS NhanVien,
                       pm.NgayMuon,
                       pm.HanTra,
                       s.MaSach + ' - ' + s.TenSach AS Sach,
                       pm.NgayTra,
                       pm.TienPhat,
                       pm.DaThuPhat,
                       pm.SoLanGiaHan
                FROM PhieuMuon pm
                INNER JOIN DocGia dg ON pm.MaDocGia = dg.MaDocGia
                INNER JOIN NhanVien nv ON pm.MaNhanVien = nv.MaNhanVien
                INNER JOIN Sach s ON pm.MaSach = s.MaSach
                WHERE (@TuKhoa IS NULL
                       OR pm.MaPhieu LIKE @TuKhoa
                       OR pm.MaDocGia LIKE @TuKhoa
                       OR dg.TenDocGia LIKE @TuKhoa
                       OR nv.TenNhanVien LIKE @TuKhoa
                       OR s.MaSach LIKE @TuKhoa
                       OR s.TenSach LIKE @TuKhoa)
                ORDER BY pm.MaPhieu";

            object tuKhoaLike = string.IsNullOrEmpty(tuKhoa) ? null : "%" + tuKhoa + "%";
            return DbConnection.ExecuteQuery(sql, DbConnection.Param("@TuKhoa", tuKhoaLike));
        }

        private void LoadPhieuMuon(string tuKhoa)
        {
            try
            {
                HienThiLuoi(TruyVanPhieuMuon(tuKhoa));
            }
            catch (Exception ex)
            {
                AppTheme.Error("Không thể tải dữ liệu phiếu mượn.\n\n" + ex.Message);
            }
        }

        private void HienThiLuoi(DataTable dt)
        {
            dgvPhieuMuon.Rows.Clear();

            foreach (DataRow r in dt.Rows)
            {
                DateTime hanTra = Convert.ToDateTime(r["HanTra"]).Date;
                int soLanGiaHan = Convert.ToInt32(r["SoLanGiaHan"]);

                string trangThai;
                string tienPhat = "";
                string giaHan = "";
                Color mau;
                Color? mauPhat = null;

                if (r["NgayTra"] != DBNull.Value)
                {
                    DateTime ngayTra = Convert.ToDateTime(r["NgayTra"]).Date;
                    trangThai = "Đã trả";
                    mau = AppTheme.Success;
                    if (soLanGiaHan > 0) giaHan = soLanGiaHan + " lần";

                    // Phiếu trả trước khi có tính năng phạt thì TienPhat = NULL -> tính lại từ ngày tháng
                    decimal phat = r["TienPhat"] == DBNull.Value
                        ? QuyDinh.TinhTienPhat(hanTra, ngayTra)
                        : Convert.ToDecimal(r["TienPhat"]);

                    if (phat > 0)
                    {
                        bool daThu = Convert.ToBoolean(r["DaThuPhat"]);
                        tienPhat = QuyDinh.DinhDangTien(phat) + (daThu ? " - Đã thu" : " - Chưa thu");
                        mauPhat = daThu ? AppTheme.Success : AppTheme.Danger;
                    }
                }
                else if (hanTra < DateTime.Today)
                {
                    int soNgay = (DateTime.Today - hanTra).Days;
                    trangThai = "Quá hạn " + soNgay + " ngày";
                    mau = AppTheme.Danger;
                    tienPhat = QuyDinh.DinhDangTien(soNgay * QuyDinh.MucPhatMoiNgay) + " (tạm tính)";
                    mauPhat = AppTheme.Danger;
                    giaHan = soLanGiaHan + "/" + QuyDinh.SoLanGiaHanToiDa;
                }
                else
                {
                    trangThai = "Đang mượn";
                    mau = AppTheme.Accent;
                    giaHan = soLanGiaHan + "/" + QuyDinh.SoLanGiaHanToiDa;
                }

                int idx = dgvPhieuMuon.Rows.Add(
                    r["MaPhieu"],
                    r["DocGia"],
                    r["NhanVien"],
                    Convert.ToDateTime(r["NgayMuon"]).ToString(DinhDangNgay),
                    hanTra.ToString(DinhDangNgay),
                    r["Sach"],
                    trangThai,
                    tienPhat,
                    giaHan);

                DataGridViewCell o = dgvPhieuMuon.Rows[idx].Cells["colTrangThai"];
                o.Style.ForeColor = mau;
                o.Style.SelectionForeColor = mau;
                o.Style.Font = AppTheme.Bold;

                if (mauPhat.HasValue)
                {
                    DataGridViewCell p = dgvPhieuMuon.Rows[idx].Cells["colTienPhat"];
                    p.Style.ForeColor = mauPhat.Value;
                    p.Style.SelectionForeColor = mauPhat.Value;
                    p.Style.Font = AppTheme.Bold;
                }
            }

            dgvPhieuMuon.ClearSelection();
            dgvPhieuMuon.CurrentCell = null;
        }

        // ================= SỰ KIỆN (giữ nguyên tên để Designer không bị lỗi) =================

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
                DataTable dt = TruyVanPhieuMuon(tuKhoa);
                HienThiLuoi(dt);

                if (dt.Rows.Count == 0)
                    AppTheme.Info("Không tìm thấy phiếu mượn phù hợp.");
            }
            catch (Exception ex)
            {
                AppTheme.Error("Không thể tìm kiếm phiếu mượn.\n\n" + ex.Message);
            }
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            txtTuKhoa.Clear();
            LoadPhieuMuon(null);
            txtTuKhoa.Focus();
        }

        private void dgvPhieuMuon_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            DataGridViewRow row = dgvPhieuMuon.Rows[e.RowIndex];
            txtTuKhoa.Text = row.Cells["colMaPhieu"].Value?.ToString() ?? "";
        }

        /// <summary>Mã phiếu đang chọn trong lưới, hoặc null (kèm cảnh báo) nếu chưa chọn.</summary>
        private string LayPhieuDangChon(string viec)
        {
            if (dgvPhieuMuon.SelectedRows.Count == 0)
            {
                AppTheme.Warning("Vui lòng chọn phiếu cần " + viec + " trong danh sách.");
                return null;
            }
            return dgvPhieuMuon.SelectedRows[0].Cells["colMaPhieu"].Value?.ToString() ?? "";
        }

        // ================= GIA HẠN PHIẾU =================

        private void btnGiaHan_Click(object sender, EventArgs e)
        {
            string maPhieu = LayPhieuDangChon("gia hạn");
            if (maPhieu == null) return;

            if (!AppTheme.Confirm("Gia hạn phiếu " + maPhieu + " thêm " + QuyDinh.SoNgayMoiLanGiaHan + " ngày?\n\n" +
                                  "Mỗi phiếu gia hạn tối đa " + QuyDinh.SoLanGiaHanToiDa +
                                  " lần và chỉ gia hạn được khi chưa quá hạn."))
                return;

            try
            {
                DateTime hanMoi;
                int lanThu;
                ThuVienDb.GiaHan(maPhieu, out hanMoi, out lanThu);

                NhatKy.Ghi("Gia hạn",
                    "Gia hạn phiếu " + maPhieu + " đến " + hanMoi.ToString(DinhDangNgay) +
                    " (lần " + lanThu + "/" + QuyDinh.SoLanGiaHanToiDa + ")");

                AppTheme.Info("Đã gia hạn phiếu " + maPhieu + ".\nHạn trả mới: " + hanMoi.ToString(DinhDangNgay) +
                              " (lần " + lanThu + "/" + QuyDinh.SoLanGiaHanToiDa + ").");

                txtTuKhoa.Clear();
                LoadPhieuMuon(null);
            }
            catch (LoiNghiepVu ex)
            {
                AppTheme.Warning(ex.Message);
            }
            catch (Exception ex)
            {
                AppTheme.Error("Không thể gia hạn phiếu.\n\n" + ex.Message);
            }
        }

        // ================= THU TIỀN PHẠT =================

        private void btnThuPhat_Click(object sender, EventArgs e)
        {
            string maPhieu = LayPhieuDangChon("thu phạt");
            if (maPhieu == null) return;

            try
            {
                // Đọc trạng thái mới nhất từ CSDL (không tin dữ liệu đang hiện trên lưới)
                DataTable dt = DbConnection.ExecuteQuery(@"
                    SELECT pm.MaDocGia + ' - ' + dg.TenDocGia AS DocGia,
                           pm.HanTra, pm.NgayTra, pm.TienPhat, pm.DaThuPhat, pm.NgayThuPhat
                    FROM PhieuMuon pm
                    INNER JOIN DocGia dg ON pm.MaDocGia = dg.MaDocGia
                    WHERE pm.MaPhieu = @MaPhieu",
                    DbConnection.Param("@MaPhieu", maPhieu));

                if (dt.Rows.Count == 0)
                {
                    AppTheme.Warning("Không tìm thấy phiếu " + maPhieu + ".");
                    LoadPhieuMuon(null);
                    return;
                }

                DataRow r = dt.Rows[0];

                if (r["NgayTra"] == DBNull.Value)
                {
                    AppTheme.Warning("Phiếu " + maPhieu + " chưa trả sách.\n" +
                                     "Hãy trả sách trước (mục Trả sách), tiền phạt sẽ được tính khi trả.");
                    return;
                }

                DateTime hanTra = Convert.ToDateTime(r["HanTra"]).Date;
                DateTime ngayTra = Convert.ToDateTime(r["NgayTra"]).Date;
                decimal phat = r["TienPhat"] == DBNull.Value
                    ? QuyDinh.TinhTienPhat(hanTra, ngayTra)
                    : Convert.ToDecimal(r["TienPhat"]);

                if (phat <= 0)
                {
                    AppTheme.Info("Phiếu " + maPhieu + " không có tiền phạt.");
                    return;
                }

                if (Convert.ToBoolean(r["DaThuPhat"]))
                {
                    string ngayThu = r["NgayThuPhat"] == DBNull.Value
                        ? ""
                        : " vào " + Convert.ToDateTime(r["NgayThuPhat"]).ToString("dd/MM/yyyy HH:mm");
                    AppTheme.Info("Phiếu " + maPhieu + " đã thu phạt" + ngayThu + ".");
                    return;
                }

                string docGia = r["DocGia"].ToString();
                if (!AppTheme.Confirm("Xác nhận đã thu tiền phạt " + QuyDinh.DinhDangTien(phat) +
                                      "\ncủa độc giả " + docGia + " (phiếu " + maPhieu + ")?"))
                    return;

                // Lưu luôn số tiền (phiếu cũ chưa có TienPhat) và chỉ cập nhật nếu chưa thu
                int soDong = DbConnection.ExecuteNonQuery(@"
                    UPDATE PhieuMuon
                    SET TienPhat = @TienPhat,
                        DaThuPhat = 1,
                        NgayThuPhat = GETDATE()
                    WHERE MaPhieu = @MaPhieu
                      AND DaThuPhat = 0",
                    DbConnection.Param("@TienPhat", phat),
                    DbConnection.Param("@MaPhieu", maPhieu));

                if (soDong == 0)
                {
                    AppTheme.Warning("Phiếu này vừa được thu phạt ở nơi khác. Danh sách sẽ được làm mới.");
                }
                else
                {
                    NhatKy.Ghi("Thu phạt",
                        "Thu phạt " + QuyDinh.DinhDangTien(phat) + " - phiếu " + maPhieu + " - " + docGia);
                    AppTheme.Info("Đã ghi nhận thu phạt " + QuyDinh.DinhDangTien(phat) + ".");
                }

                txtTuKhoa.Clear();
                LoadPhieuMuon(null);
            }
            catch (Exception ex)
            {
                AppTheme.Error("Không thể thu phạt.\n\n" + ex.Message);
            }
        }

        // ================= BÁO CÁO PHIẾU QUÁ HẠN =================

        private void btnXuatExcel_Click(object sender, EventArgs e)
        {
            try
            {
                BaoCao.XuatExcelQuaHan(this);
            }
            catch (Exception ex)
            {
                AppTheme.Error("Không thể xuất báo cáo.\n\n" + ex.Message);
            }
        }

        private void btnInQuaHan_Click(object sender, EventArgs e)
        {
            try
            {
                BaoCao.InQuaHan();
            }
            catch (Exception ex)
            {
                AppTheme.Error("Không thể in báo cáo.\n\n" + ex.Message);
            }
        }
    }
}
