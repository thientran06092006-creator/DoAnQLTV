using System.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DoAnQLTV
{
    public partial class FrmDocGia : Form
    {
        private int dongDangChon = -1;
        public FrmDocGia()
        {
            InitializeComponent();

            LoadDocGia();
        }

        private void LoadDocGia()
        {
            try
            {
                using (SqlConnection conn = DbConnection.GetConnection())
                {
                    conn.Open();

                    string sql = @"
                SELECT MaDocGia, TenDocGia, NgaySinh, DiaChi, SoDienThoai
                FROM DocGia
                ORDER BY MaDocGia";

                    using (SqlDataAdapter adapter = new SqlDataAdapter(sql, conn))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);

                        dgvDocGia.Rows.Clear();

                        foreach (DataRow row in dt.Rows)
                        {
                            dgvDocGia.Rows.Add(
                                row["MaDocGia"].ToString(),
                                row["TenDocGia"].ToString(),
                                Convert.ToDateTime(row["NgaySinh"]).ToString("dd/MM/yyyy"),
                                row["DiaChi"].ToString(),
                                row["SoDienThoai"].ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Không thể tải dữ liệu độc giả.\n\n" + ex.Message,
                    "Lỗi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }
        private void btnThem_Click(object sender, EventArgs e)
        {
            string maDocGia = txtMaDocGia.Text.Trim();
            string tenDocGia = txtTenDocGia.Text.Trim();
            DateTime ngaySinh = dtpNgaySinh.Value;
            string diaChi = txtDiaChi.Text.Trim();
            string soDienThoai = txtSoDienThoai.Text.Trim();

            if (maDocGia == "")
            {
                MessageBox.Show(
                    "Vui lòng nhập mã độc giả.",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                txtMaDocGia.Focus();
                return;
            }

            if (tenDocGia == "")
            {
                MessageBox.Show(
                    "Vui lòng nhập tên độc giả.",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                txtTenDocGia.Focus();
                return;
            }

            if (diaChi == "")
            {
                MessageBox.Show(
                    "Vui lòng nhập địa chỉ.",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                txtDiaChi.Focus();
                return;
            }

            if (soDienThoai == "")
            {
                MessageBox.Show(
                    "Vui lòng nhập số điện thoại.",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                txtSoDienThoai.Focus();
                return;
            }

            try
            {
                using (SqlConnection conn = DbConnection.GetConnection())
                {
                    conn.Open();

                    string sql = @"
                INSERT INTO DocGia
                    (MaDocGia, TenDocGia, NgaySinh, DiaChi, SoDienThoai)
                VALUES
                    (@MaDocGia, @TenDocGia, @NgaySinh, @DiaChi, @SoDienThoai)";

                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@MaDocGia", maDocGia);
                        cmd.Parameters.AddWithValue("@TenDocGia", tenDocGia);
                        cmd.Parameters.AddWithValue("@NgaySinh", ngaySinh);
                        cmd.Parameters.AddWithValue("@DiaChi", diaChi);
                        cmd.Parameters.AddWithValue("@SoDienThoai", soDienThoai);

                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show(
                    "Thêm độc giả thành công.",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                LoadDocGia();

                txtMaDocGia.Clear();
                txtTenDocGia.Clear();
                txtDiaChi.Clear();
                txtSoDienThoai.Clear();

                dtpNgaySinh.Value = DateTime.Now;

                txtMaDocGia.Focus();
            }
            catch (SqlException ex)
            {
                if (ex.Number == 2627 || ex.Number == 2601)
                {
                    MessageBox.Show(
                        "Mã độc giả đã tồn tại.",
                        "Thông báo",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                }
                else
                {
                    MessageBox.Show(
                        "Không thể thêm độc giả.\n\n" + ex.Message,
                        "Lỗi",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Đã xảy ra lỗi.\n\n" + ex.Message,
                    "Lỗi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void dgvDocGia_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            dongDangChon = e.RowIndex;

            DataGridViewRow row = dgvDocGia.Rows[e.RowIndex];

            txtMaDocGia.Text = row.Cells["colMaDocGia"].Value?.ToString();
            txtTenDocGia.Text = row.Cells["colTenDocGia"].Value?.ToString();

            DateTime ngaySinh;
            if (DateTime.TryParse(
                row.Cells["colNgaySinh"].Value?.ToString(),
                out ngaySinh))
            {
                dtpNgaySinh.Value = ngaySinh;
            }

            txtDiaChi.Text = row.Cells["colDiaChi"].Value?.ToString();
            txtSoDienThoai.Text = row.Cells["colSoDienThoai"].Value?.ToString();
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (dongDangChon == -1)
            {
                MessageBox.Show(
                    "Vui lòng chọn độc giả cần sửa.",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            string maDocGia = txtMaDocGia.Text.Trim();
            string tenDocGia = txtTenDocGia.Text.Trim();
            DateTime ngaySinh = dtpNgaySinh.Value;
            string diaChi = txtDiaChi.Text.Trim();
            string soDienThoai = txtSoDienThoai.Text.Trim();

            if (maDocGia == "")
            {
                MessageBox.Show(
                    "Vui lòng nhập mã độc giả.",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                txtMaDocGia.Focus();
                return;
            }

            if (tenDocGia == "")
            {
                MessageBox.Show(
                    "Vui lòng nhập tên độc giả.",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                txtTenDocGia.Focus();
                return;
            }

            if (diaChi == "")
            {
                MessageBox.Show(
                    "Vui lòng nhập địa chỉ.",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                txtDiaChi.Focus();
                return;
            }

            if (soDienThoai == "")
            {
                MessageBox.Show(
                    "Vui lòng nhập số điện thoại.",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                txtSoDienThoai.Focus();
                return;
            }

            try
            {
                using (SqlConnection conn = DbConnection.GetConnection())
                {
                    conn.Open();

                    string sql = @"
                UPDATE DocGia
                SET
                    TenDocGia = @TenDocGia,
                    NgaySinh = @NgaySinh,
                    DiaChi = @DiaChi,
                    SoDienThoai = @SoDienThoai
                WHERE MaDocGia = @MaDocGia";

                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@TenDocGia", tenDocGia);
                        cmd.Parameters.AddWithValue("@NgaySinh", ngaySinh);
                        cmd.Parameters.AddWithValue("@DiaChi", diaChi);
                        cmd.Parameters.AddWithValue("@SoDienThoai", soDienThoai);
                        cmd.Parameters.AddWithValue("@MaDocGia", maDocGia);

                        int soDong = cmd.ExecuteNonQuery();

                        if (soDong == 0)
                        {
                            MessageBox.Show(
                                "Không tìm thấy mã độc giả cần sửa.",
                                "Thông báo",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);

                            return;
                        }
                    }
                }

                MessageBox.Show(
                    "Sửa thông tin độc giả thành công.",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                LoadDocGia();

                txtMaDocGia.Clear();
                txtTenDocGia.Clear();
                txtDiaChi.Clear();
                txtSoDienThoai.Clear();

                dtpNgaySinh.Value = DateTime.Now;

                dongDangChon = -1;

                dgvDocGia.ClearSelection();

                txtMaDocGia.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Không thể cập nhật độc giả.\n\n" + ex.Message,
                    "Lỗi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (dongDangChon == -1)
            {
                MessageBox.Show(
                    "Vui lòng chọn độc giả cần xóa.",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            string maDocGia = txtMaDocGia.Text.Trim();

            if (maDocGia == "")
            {
                MessageBox.Show(
                    "Vui lòng chọn độc giả cần xóa.",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            DialogResult result = MessageBox.Show(
                "Bạn có chắc chắn muốn xóa độc giả này không?",
                "Xác nhận xóa",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result != DialogResult.Yes)
            {
                return;
            }

            try
            {
                using (SqlConnection conn = DbConnection.GetConnection())
                {
                    conn.Open();

                    string sql = @"
                DELETE FROM DocGia
                WHERE MaDocGia = @MaDocGia";

                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@MaDocGia", maDocGia);

                        int soDong = cmd.ExecuteNonQuery();

                        if (soDong == 0)
                        {
                            MessageBox.Show(
                                "Không tìm thấy độc giả cần xóa.",
                                "Thông báo",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);

                            return;
                        }
                    }
                }

                MessageBox.Show(
                    "Xóa độc giả thành công.",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                LoadDocGia();

                txtMaDocGia.Clear();
                txtTenDocGia.Clear();
                txtDiaChi.Clear();
                txtSoDienThoai.Clear();

                dtpNgaySinh.Value = DateTime.Now;

                dongDangChon = -1;

                dgvDocGia.ClearSelection();

                txtMaDocGia.Focus();
            }
            catch (SqlException ex)
            {
                if (ex.Number == 547)
                {
                    MessageBox.Show(
                        "Không thể xóa độc giả này vì đang được sử dụng trong phiếu mượn.",
                        "Thông báo",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                }
                else
                {
                    MessageBox.Show(
                        "Không thể xóa độc giả.\n\n" + ex.Message,
                        "Lỗi",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Đã xảy ra lỗi.\n\n" + ex.Message,
                    "Lỗi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            string tuKhoa = txtTenDocGia.Text.Trim();

            if (tuKhoa == "")
            {
                MessageBox.Show(
                    "Vui lòng nhập từ khóa cần tìm.",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                txtTenDocGia.Focus();
                return;
            }

            try
            {
                using (SqlConnection conn = DbConnection.GetConnection())
                {
                    conn.Open();

                    string sql = @"
                SELECT
                    MaDocGia,
                    TenDocGia,
                    NgaySinh,
                    DiaChi,
                    SoDienThoai
                FROM DocGia
                WHERE MaDocGia LIKE @TuKhoa
                   OR TenDocGia LIKE @TuKhoa
                   OR SoDienThoai LIKE @TuKhoa
                ORDER BY MaDocGia";

                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue(
                            "@TuKhoa",
                            "%" + tuKhoa + "%");

                        using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            adapter.Fill(dt);

                            dgvDocGia.Rows.Clear();

                            foreach (DataRow row in dt.Rows)
                            {
                                dgvDocGia.Rows.Add(
                                    row["MaDocGia"].ToString(),
                                    row["TenDocGia"].ToString(),
                                    Convert.ToDateTime(row["NgaySinh"])
                                        .ToString("dd/MM/yyyy"),
                                    row["DiaChi"].ToString(),
                                    row["SoDienThoai"].ToString());
                            }

                            if (dt.Rows.Count == 0)
                            {
                                MessageBox.Show(
                                    "Không tìm thấy độc giả phù hợp.",
                                    "Thông báo",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Information);

                                return;
                            }

                            if (dt.Rows.Count == 1)
                            {
                                txtMaDocGia.Text =
                                    dt.Rows[0]["MaDocGia"].ToString();

                                txtTenDocGia.Text =
                                    dt.Rows[0]["TenDocGia"].ToString();

                                dtpNgaySinh.Value =
                                    Convert.ToDateTime(dt.Rows[0]["NgaySinh"]);

                                txtDiaChi.Text =
                                    dt.Rows[0]["DiaChi"].ToString();

                                txtSoDienThoai.Text =
                                    dt.Rows[0]["SoDienThoai"].ToString();

                                dgvDocGia.Rows[0].Selected = true;

                                dgvDocGia.CurrentCell =
                                    dgvDocGia.Rows[0]
                                        .Cells["colMaDocGia"];

                                dongDangChon = 0;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Không thể tìm kiếm độc giả.\n\n" + ex.Message,
                    "Lỗi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            txtMaDocGia.Clear();
            txtTenDocGia.Clear();
            txtDiaChi.Clear();
            txtSoDienThoai.Clear();

            dtpNgaySinh.Value = DateTime.Now;

            dongDangChon = -1;

            LoadDocGia();

            dgvDocGia.ClearSelection();

            txtMaDocGia.Focus();
        }
    }
}