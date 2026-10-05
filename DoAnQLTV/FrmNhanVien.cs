using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DoAnQLTV
{
    public partial class FrmNhanVien : Form
    {
        private int dongDangChon = -1;
        public FrmNhanVien()
        {
            InitializeComponent();

            LoadNhanVien();
        }

        private void LoadNhanVien()
        {
            try
            {
                using (SqlConnection conn = DbConnection.GetConnection())
                {
                    conn.Open();

                    string sql = @"
                SELECT MaNhanVien, TenNhanVien, SoDienThoai, DiaChi
                FROM NhanVien
                ORDER BY MaNhanVien";

                    using (SqlDataAdapter adapter = new SqlDataAdapter(sql, conn))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);

                        dgvNhanVien.Rows.Clear();

                        foreach (DataRow row in dt.Rows)
                        {
                            dgvNhanVien.Rows.Add(
                                row["MaNhanVien"].ToString(),
                                row["TenNhanVien"].ToString(),
                                row["SoDienThoai"].ToString(),
                                row["DiaChi"].ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Không thể tải dữ liệu nhân viên.\n\n" + ex.Message,
                    "Lỗi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            string maNhanVien = txtMaNhanVien.Text.Trim();
            string tenNhanVien = txtTenNhanVien.Text.Trim();
            string soDienThoai = txtSoDienThoai.Text.Trim();
            string diaChi = txtDiaChi.Text.Trim();

            if (maNhanVien == "")
            {
                MessageBox.Show(
                    "Vui lòng nhập mã nhân viên.",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                txtMaNhanVien.Focus();
                return;
            }

            if (tenNhanVien == "")
            {
                MessageBox.Show(
                    "Vui lòng nhập tên nhân viên.",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                txtTenNhanVien.Focus();
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

            try
            {
                using (SqlConnection conn = DbConnection.GetConnection())
                {
                    conn.Open();

                    string sql = @"
                INSERT INTO NhanVien
                    (MaNhanVien, TenNhanVien, SoDienThoai, DiaChi)
                VALUES
                    (@MaNhanVien, @TenNhanVien, @SoDienThoai, @DiaChi)";

                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@MaNhanVien", maNhanVien);
                        cmd.Parameters.AddWithValue("@TenNhanVien", tenNhanVien);
                        cmd.Parameters.AddWithValue("@SoDienThoai", soDienThoai);
                        cmd.Parameters.AddWithValue("@DiaChi", diaChi);

                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show(
                    "Thêm nhân viên thành công.",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                LoadNhanVien();

                txtMaNhanVien.Clear();
                txtTenNhanVien.Clear();
                txtSoDienThoai.Clear();
                txtDiaChi.Clear();

                txtMaNhanVien.Focus();
            }
            catch (SqlException ex)
            {
                if (ex.Number == 2627 || ex.Number == 2601)
                {
                    MessageBox.Show(
                        "Mã nhân viên đã tồn tại.",
                        "Thông báo",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                }
                else
                {
                    MessageBox.Show(
                        "Không thể thêm nhân viên.\n\n" + ex.Message,
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

        private void dgvNhanVien_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            dongDangChon = e.RowIndex;

            DataGridViewRow row = dgvNhanVien.Rows[e.RowIndex];

            txtMaNhanVien.Text =
                row.Cells["colMaNhanVien"].Value?.ToString();

            txtTenNhanVien.Text =
                row.Cells["colTenNhanVien"].Value?.ToString();

            txtSoDienThoai.Text =
                row.Cells["colSoDienThoai"].Value?.ToString();

            txtDiaChi.Text =
                row.Cells["colDiaChi"].Value?.ToString();
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (dongDangChon == -1)
            {
                MessageBox.Show(
                    "Vui lòng chọn nhân viên cần sửa.",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            string maNhanVien = txtMaNhanVien.Text.Trim();
            string tenNhanVien = txtTenNhanVien.Text.Trim();
            string soDienThoai = txtSoDienThoai.Text.Trim();
            string diaChi = txtDiaChi.Text.Trim();

            if (maNhanVien == "")
            {
                MessageBox.Show(
                    "Vui lòng nhập mã nhân viên.",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                txtMaNhanVien.Focus();
                return;
            }

            if (tenNhanVien == "")
            {
                MessageBox.Show(
                    "Vui lòng nhập tên nhân viên.",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                txtTenNhanVien.Focus();
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

            try
            {
                using (SqlConnection conn = DbConnection.GetConnection())
                {
                    conn.Open();

                    string sql = @"
                UPDATE NhanVien
                SET
                    TenNhanVien = @TenNhanVien,
                    SoDienThoai = @SoDienThoai,
                    DiaChi = @DiaChi
                WHERE MaNhanVien = @MaNhanVien";

                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@TenNhanVien", tenNhanVien);
                        cmd.Parameters.AddWithValue("@SoDienThoai", soDienThoai);
                        cmd.Parameters.AddWithValue("@DiaChi", diaChi);
                        cmd.Parameters.AddWithValue("@MaNhanVien", maNhanVien);

                        int soDong = cmd.ExecuteNonQuery();

                        if (soDong == 0)
                        {
                            MessageBox.Show(
                                "Không tìm thấy mã nhân viên cần sửa.",
                                "Thông báo",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);

                            return;
                        }
                    }
                }

                MessageBox.Show(
                    "Sửa thông tin nhân viên thành công.",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                LoadNhanVien();

                txtMaNhanVien.Clear();
                txtTenNhanVien.Clear();
                txtSoDienThoai.Clear();
                txtDiaChi.Clear();

                dongDangChon = -1;

                dgvNhanVien.ClearSelection();

                txtMaNhanVien.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Không thể cập nhật nhân viên.\n\n" + ex.Message,
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
                    "Vui lòng chọn nhân viên cần xóa.",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            string maNhanVien = txtMaNhanVien.Text.Trim();

            if (maNhanVien == "")
            {
                MessageBox.Show(
                    "Vui lòng chọn nhân viên cần xóa.",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            DialogResult result = MessageBox.Show(
                "Bạn có chắc chắn muốn xóa nhân viên này không?",
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
                DELETE FROM NhanVien
                WHERE MaNhanVien = @MaNhanVien";

                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue(
                            "@MaNhanVien",
                            maNhanVien);

                        int soDong = cmd.ExecuteNonQuery();

                        if (soDong == 0)
                        {
                            MessageBox.Show(
                                "Không tìm thấy nhân viên cần xóa.",
                                "Thông báo",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);

                            return;
                        }
                    }
                }

                MessageBox.Show(
                    "Xóa nhân viên thành công.",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                LoadNhanVien();

                txtMaNhanVien.Clear();
                txtTenNhanVien.Clear();
                txtSoDienThoai.Clear();
                txtDiaChi.Clear();

                dongDangChon = -1;

                dgvNhanVien.ClearSelection();

                txtMaNhanVien.Focus();
            }
            catch (SqlException ex)
            {
                if (ex.Number == 547)
                {
                    MessageBox.Show(
                        "Không thể xóa nhân viên này vì đang được sử dụng trong phiếu mượn.",
                        "Thông báo",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                }
                else
                {
                    MessageBox.Show(
                        "Không thể xóa nhân viên.\n\n" + ex.Message,
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
            string tuKhoa = txtTenNhanVien.Text.Trim();

            if (tuKhoa == "")
            {
                MessageBox.Show(
                    "Vui lòng nhập từ khóa cần tìm.",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                txtTenNhanVien.Focus();
                return;
            }

            try
            {
                using (SqlConnection conn = DbConnection.GetConnection())
                {
                    conn.Open();

                    string sql = @"
                SELECT
                    MaNhanVien,
                    TenNhanVien,
                    SoDienThoai,
                    DiaChi
                FROM NhanVien
                WHERE MaNhanVien LIKE @TuKhoa
                   OR TenNhanVien LIKE @TuKhoa
                   OR SoDienThoai LIKE @TuKhoa
                ORDER BY MaNhanVien";

                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue(
                            "@TuKhoa",
                            "%" + tuKhoa + "%");

                        using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            adapter.Fill(dt);

                            dgvNhanVien.Rows.Clear();

                            foreach (DataRow row in dt.Rows)
                            {
                                dgvNhanVien.Rows.Add(
                                    row["MaNhanVien"].ToString(),
                                    row["TenNhanVien"].ToString(),
                                    row["SoDienThoai"].ToString(),
                                    row["DiaChi"].ToString());
                            }

                            if (dt.Rows.Count == 0)
                            {
                                MessageBox.Show(
                                    "Không tìm thấy nhân viên phù hợp.",
                                    "Thông báo",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Information);

                                return;
                            }

                            if (dt.Rows.Count == 1)
                            {
                                txtMaNhanVien.Text =
                                    dt.Rows[0]["MaNhanVien"].ToString();

                                txtTenNhanVien.Text =
                                    dt.Rows[0]["TenNhanVien"].ToString();

                                txtSoDienThoai.Text =
                                    dt.Rows[0]["SoDienThoai"].ToString();

                                txtDiaChi.Text =
                                    dt.Rows[0]["DiaChi"].ToString();

                                dgvNhanVien.Rows[0].Selected = true;

                                dgvNhanVien.CurrentCell =
                                    dgvNhanVien.Rows[0]
                                        .Cells["colMaNhanVien"];

                                dongDangChon = 0;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Không thể tìm kiếm nhân viên.\n\n" + ex.Message,
                    "Lỗi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            txtMaNhanVien.Clear();
            txtTenNhanVien.Clear();
            txtSoDienThoai.Clear();
            txtDiaChi.Clear();

            dongDangChon = -1;

            LoadNhanVien();

            dgvNhanVien.ClearSelection();

            txtMaNhanVien.Focus();
        }
    }
}