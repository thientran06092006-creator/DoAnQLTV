using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace DoAnQLTV
{
    public partial class FrmTheLoai : Form
    {
        // Mã thể loại đang được chọn để sửa/xóa. null = đang ở chế độ nhập mới.
        private string maDangChon = null;

        public FrmTheLoai()
        {
            InitializeComponent();

            txtMaTheLoai.MaxLength = 20;
            txtTenTheLoai.MaxLength = 100;

            AppTheme.ApplyForm(this);
            ApDungGiaoDien();
            LoadTheLoai();

            txtMaTheLoai.Text = MaTuSinh.TiepTheo("TheLoai", "MaTheLoai", "TL", 4);   // <-- THÊM
        }

        // ================= GIAO DIỆN =================

        private void ApDungGiaoDien()
        {
            AppTheme.StyleCard(pnlThongTin);
            AppTheme.StyleCard(pnlDanhSach);

            AppTheme.StyleGrid(dgvTheLoai);

            AppTheme.StyleTextBox(txtMaTheLoai);
            AppTheme.StyleTextBox(txtTenTheLoai);

            AppTheme.StylePrimaryButton(btnThem);
            AppTheme.StyleButton(btnSua, AppTheme.Amber, ColorTranslator.FromHtml("#D97706"));
            AppTheme.StyleDangerButton(btnXoa);
            AppTheme.StyleSuccessButton(btnTimKiem);
            AppTheme.StyleButton(btnLamMoi,
                ColorTranslator.FromHtml("#E2E8F0"),
                ColorTranslator.FromHtml("#CBD5E1"),
                AppTheme.TextDark);
        }

        // ================= NẠP DỮ LIỆU =================

        /// <summary>Lấy danh sách thể loại; tuKhoa rỗng/null = lấy tất cả.</summary>
        private DataTable TruyVanTheLoai(string tuKhoa)
        {
            const string sql = @"
                SELECT MaTheLoai, TenTheLoai
                FROM TheLoai
                WHERE (@TuKhoa IS NULL OR MaTheLoai LIKE @TuKhoa OR TenTheLoai LIKE @TuKhoa)
                ORDER BY MaTheLoai";

            object tuKhoaLike = string.IsNullOrEmpty(tuKhoa) ? null : "%" + tuKhoa + "%";
            return DbConnection.ExecuteQuery(sql, DbConnection.Param("@TuKhoa", tuKhoaLike));
        }

        private void LoadTheLoai()
        {
            try
            {
                HienThiLuoi(TruyVanTheLoai(null));
            }
            catch (Exception ex)
            {
                AppTheme.Error("Không thể tải dữ liệu thể loại.\n\n" + ex.Message);
            }
        }

        private void HienThiLuoi(DataTable dt)
        {
            dgvTheLoai.Rows.Clear();
            foreach (DataRow r in dt.Rows)
                dgvTheLoai.Rows.Add(r["MaTheLoai"], r["TenTheLoai"]);

            dgvTheLoai.ClearSelection();
            dgvTheLoai.CurrentCell = null;
        }

        // ================= HÀM DÙNG CHUNG TRÊN FORM =================

        /// <summary>Khóa/mở ô mã và đổi màu nền theo trạng thái (xám nhạt = đang khóa).</summary>
        private void KhoaMa(bool khoa)
        {
            txtMaTheLoai.ReadOnly = khoa;
            txtMaTheLoai.BackColor = khoa ? ColorTranslator.FromHtml("#F1F5F9") : Color.White;
        }

        private void DoLenForm(string maTheLoai, string tenTheLoai)
        {
            txtMaTheLoai.Text = maTheLoai;
            txtTenTheLoai.Text = tenTheLoai;

            maDangChon = maTheLoai;
            KhoaMa(true);
        }

        private void XoaForm()
        {
            txtMaTheLoai.Text = MaTuSinh.TiepTheo("TheLoai", "MaTheLoai", "TL", 4);   // <-- THAY cho txtMaTheLoai.Clear();
            txtTenTheLoai.Clear();

            maDangChon = null;
            KhoaMa(false);

            dgvTheLoai.ClearSelection();
            dgvTheLoai.CurrentCell = null;
            txtTenTheLoai.Focus();   // <-- đổi từ txtMaTheLoai.Focus()
        }

        private bool KiemTraDuLieu()
        {
            if (txtMaTheLoai.Text.Trim() == "")
            {
                AppTheme.Warning("Vui lòng nhập mã thể loại.");
                txtMaTheLoai.Focus();
                return false;
            }

            if (txtTenTheLoai.Text.Trim() == "")
            {
                AppTheme.Warning("Vui lòng nhập tên thể loại.");
                txtTenTheLoai.Focus();
                return false;
            }

            return true;
        }

        // ================= SỰ KIỆN (giữ nguyên tên để Designer không bị lỗi) =================

        private void dgvTheLoai_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            DataGridViewRow row = dgvTheLoai.Rows[e.RowIndex];
            DoLenForm(
                row.Cells["colMaTheLoai"].Value?.ToString(),
                row.Cells["colTenTheLoai"].Value?.ToString());
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            if (maDangChon != null)
            {
                AppTheme.Warning("Bạn đang chọn một thể loại có sẵn.\nBấm \"Làm mới\" để nhập thể loại mới.");
                return;
            }

            if (!KiemTraDuLieu()) return;

            try
            {
                DbConnection.ExecuteNonQuery(@"
                    INSERT INTO TheLoai (MaTheLoai, TenTheLoai)
                    VALUES (@MaTheLoai, @TenTheLoai)",
                    DbConnection.Param("@MaTheLoai", txtMaTheLoai.Text.Trim()),
                    DbConnection.Param("@TenTheLoai", txtTenTheLoai.Text.Trim()));

                NhatKy.Ghi("Thêm thể loại", "Thêm thể loại \"" + txtTenTheLoai.Text.Trim() + "\" - Mã TL: " + txtMaTheLoai.Text.Trim());

                AppTheme.Info("Thêm thể loại thành công!");
                LoadTheLoai();
                XoaForm();
            }
            catch (SqlException ex) when (ex.Number == 2627 || ex.Number == 2601)
            {
                AppTheme.Warning("Mã thể loại đã tồn tại.");
            }
            catch (Exception ex)
            {
                AppTheme.Error("Không thể thêm thể loại.\n\n" + ex.Message);
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (maDangChon == null)
            {
                AppTheme.Warning("Vui lòng chọn thể loại cần sửa.");
                return;
            }

            if (!KiemTraDuLieu()) return;

            try
            {
                int soDong = DbConnection.ExecuteNonQuery(@"
                    UPDATE TheLoai
                    SET TenTheLoai = @TenTheLoai
                    WHERE MaTheLoai = @MaTheLoai",
                    DbConnection.Param("@TenTheLoai", txtTenTheLoai.Text.Trim()),
                    DbConnection.Param("@MaTheLoai", maDangChon));

                if (soDong == 0)
                {
                    AppTheme.Warning("Không tìm thấy mã thể loại cần sửa.");
                    return;
                }

                NhatKy.Ghi("Sửa thể loại", "Sửa thể loại \"" + txtTenTheLoai.Text.Trim() + "\" - Mã TL: " + maDangChon);
                AppTheme.Info("Cập nhật thể loại thành công!");
                LoadTheLoai();
                XoaForm();
            }
            catch (Exception ex)
            {
                AppTheme.Error("Không thể cập nhật thể loại.\n\n" + ex.Message);
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (maDangChon == null)
            {
                AppTheme.Warning("Vui lòng chọn thể loại cần xóa.");
                return;
            }

            if (!AppTheme.Confirm("Bạn có chắc chắn muốn xóa thể loại này không?"))
                return;

            try
            {
                int soDong = DbConnection.ExecuteNonQuery(
                    "DELETE FROM TheLoai WHERE MaTheLoai = @MaTheLoai",
                    DbConnection.Param("@MaTheLoai", maDangChon));

                if (soDong == 0)
                {
                    AppTheme.Warning("Không tìm thấy thể loại cần xóa.");
                    return;
                }

                NhatKy.Ghi("Xóa thể loại", "Xóa thể loại \"" + txtTenTheLoai.Text.Trim() + "\" - Mã TL: " + maDangChon);
                AppTheme.Info("Xóa thể loại thành công!");
                LoadTheLoai();
                XoaForm();
            }
            catch (SqlException ex) when (ex.Number == 547)
            {
                AppTheme.Warning("Không thể xóa thể loại này vì đang được sử dụng bởi sách.");
            }
            catch (Exception ex)
            {
                AppTheme.Error("Không thể xóa thể loại.\n\n" + ex.Message);
            }
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            string tuKhoa = txtTenTheLoai.Text.Trim();
            if (tuKhoa == "")
            {
                AppTheme.Warning("Vui lòng nhập từ khóa cần tìm.");
                txtTenTheLoai.Focus();
                return;
            }

            try
            {
                DataTable dt = TruyVanTheLoai(tuKhoa);
                HienThiLuoi(dt);

                if (dt.Rows.Count == 0)
                {
                    maDangChon = null;
                    KhoaMa(false);   // trước đây chỉ mở khóa mà quên trả màu nền về trắng
                    AppTheme.Info("Không tìm thấy thể loại phù hợp.");
                }
                else if (dt.Rows.Count == 1)
                {
                    DoLenForm(
                        dt.Rows[0]["MaTheLoai"].ToString(),
                        dt.Rows[0]["TenTheLoai"].ToString());
                }
            }
            catch (Exception ex)
            {
                AppTheme.Error("Không thể tìm kiếm thể loại.\n\n" + ex.Message);
            }
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            LoadTheLoai();
            XoaForm();
        }
    }
}
