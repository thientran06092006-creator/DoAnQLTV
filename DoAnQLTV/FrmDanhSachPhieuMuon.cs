using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace DoAnQLTV
{
    public partial class FrmDanhSachPhieuMuon : Form
    {
        private const string DinhDangNgay = "dd/MM/yyyy";

        // Hai nút tạo bằng code (không sửa Designer)
        private Button btnXuatExcel;
        private Button btnInQuaHan;

        public FrmDanhSachPhieuMuon()
        {
            InitializeComponent();

            AppTheme.ApplyForm(this);
            ThemNutBaoCao();
            ApDungGiaoDien();
            ThemCotTrangThai();

            LoadPhieuMuon(null);
        }

        // ================= GIAO DIỆN =================

        /// <summary>Thu ô từ khóa lại cho có chỗ, rồi thêm 2 nút báo cáo bên phải.</summary>
        private void ThemNutBaoCao()
        {
            txtTuKhoa.Width = 280;
            btnTimKiem.SetBounds(405, 18, 100, 40);
            btnLamMoi.SetBounds(513, 18, 100, 40);

            btnXuatExcel = new Button();
            btnXuatExcel.Name = "btnXuatExcel";
            btnXuatExcel.Text = "Xuất Excel quá hạn";
            btnXuatExcel.SetBounds(621, 18, 170, 40);
            btnXuatExcel.Click += btnXuatExcel_Click;

            btnInQuaHan = new Button();
            btnInQuaHan.Name = "btnInQuaHan";
            btnInQuaHan.Text = "In quá hạn";
            btnInQuaHan.SetBounds(799, 18, 110, 40);
            btnInQuaHan.Click += btnInQuaHan_Click;

            pnlThongTin.Controls.Add(btnXuatExcel);
            pnlThongTin.Controls.Add(btnInQuaHan);
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

            AppTheme.StylePrimaryButton(btnXuatExcel);
            AppTheme.StyleOutlineButton(btnInQuaHan);
        }

        /// <summary>Thêm cột "Trạng thái" và "Tiền phạt" bằng code, không cần sửa Designer.</summary>
        private void ThemCotTrangThai()
        {
            if (!dgvPhieuMuon.Columns.Contains("colTrangThai"))
            {
                DataGridViewTextBoxColumn col = new DataGridViewTextBoxColumn();
                col.Name = "colTrangThai";
                col.HeaderText = "Trạng thái";
                col.FillWeight = 80F;
                col.ReadOnly = true;
                dgvPhieuMuon.Columns.Add(col);
            }

            if (!dgvPhieuMuon.Columns.Contains("colTienPhat"))
            {
                DataGridViewTextBoxColumn col = new DataGridViewTextBoxColumn();
                col.Name = "colTienPhat";
                col.HeaderText = "Tiền phạt";
                col.FillWeight = 90F;
                col.ReadOnly = true;
                dgvPhieuMuon.Columns.Add(col);
            }
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
                       pm.TienPhat
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

                string trangThai;
                string tienPhat = "";
                Color mau;

                if (r["NgayTra"] != DBNull.Value)
                {
                    DateTime ngayTra = Convert.ToDateTime(r["NgayTra"]).Date;
                    trangThai = "Đã trả";
                    mau = AppTheme.Success;

                    // Phiếu trả trước khi có tính năng phạt thì TienPhat = NULL -> tính lại từ ngày tháng
                    decimal phat = r["TienPhat"] == DBNull.Value
                        ? QuyDinh.TinhTienPhat(hanTra, ngayTra)
                        : Convert.ToDecimal(r["TienPhat"]);
                    if (phat > 0) tienPhat = QuyDinh.DinhDangTien(phat);
                }
                else if (hanTra < DateTime.Today)
                {
                    int soNgay = (DateTime.Today - hanTra).Days;
                    trangThai = "Quá hạn " + soNgay + " ngày";
                    mau = AppTheme.Danger;
                    tienPhat = QuyDinh.DinhDangTien(soNgay * QuyDinh.MucPhatMoiNgay) + " (tạm tính)";
                }
                else
                {
                    trangThai = "Đang mượn";
                    mau = AppTheme.Accent;
                }

                int idx = dgvPhieuMuon.Rows.Add(
                    r["MaPhieu"],
                    r["DocGia"],
                    r["NhanVien"],
                    Convert.ToDateTime(r["NgayMuon"]).ToString(DinhDangNgay),
                    hanTra.ToString(DinhDangNgay),
                    r["Sach"],
                    trangThai,
                    tienPhat);

                DataGridViewCell o = dgvPhieuMuon.Rows[idx].Cells["colTrangThai"];
                o.Style.ForeColor = mau;
                o.Style.SelectionForeColor = mau;
                o.Style.Font = AppTheme.Bold;

                if (tienPhat != "")
                {
                    DataGridViewCell p = dgvPhieuMuon.Rows[idx].Cells["colTienPhat"];
                    p.Style.ForeColor = AppTheme.Danger;
                    p.Style.SelectionForeColor = AppTheme.Danger;
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
