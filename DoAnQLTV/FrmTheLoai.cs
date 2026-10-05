using System.Data;
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
    public partial class FrmTheLoai : Form
    {
        private int dongDangChon = -1;
        public FrmTheLoai()
        {
            InitializeComponent();

            LoadTheLoai();
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            string maTheLoai = txtMaTheLoai.Text.Trim();
            string tenTheLoai = txtTenTheLoai.Text.Trim();

            if (string.IsNullOrWhiteSpace(maTheLoai) ||
                string.IsNullOrWhiteSpace(tenTheLoai))
            {
                MessageBox.Show(
                    "Vui lòng nhập đầy đủ mã thể loại và tên thể loại.",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            try
            {
                using (SqlConnection conn = DbConnection.GetConnection())
                {
                    conn.Open();

                    string sql = @"
                INSERT INTO TheLoai (MaTheLoai, TenTheLoai)
                VALUES (@MaTheLoai, @TenTheLoai)";

                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@MaTheLoai", maTheLoai);
                        cmd.Parameters.AddWithValue("@TenTheLoai", tenTheLoai);

                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show(
                    "Thêm thể loại thành công!",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                LoadTheLoai();

                txtMaTheLoai.Clear();
                txtTenTheLoai.Clear();
                txtMaTheLoai.Focus();
            }
            catch (SqlException ex)
            {
                if (ex.Number == 2627 || ex.Number == 2601)
                {
                    MessageBox.Show(
                        "Mã thể loại đã tồn tại.",
                        "Thông báo",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                }
                else
                {
                    MessageBox.Show(
                        "Không thể thêm thể loại.\n\n" + ex.Message,
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

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (dgvTheLoai.CurrentRow == null)
            {
                MessageBox.Show(
                    "Vui lòng chọn thể loại cần sửa.",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            string maTheLoai = txtMaTheLoai.Text.Trim();
            string tenTheLoai = txtTenTheLoai.Text.Trim();

            if (string.IsNullOrWhiteSpace(maTheLoai) ||
                string.IsNullOrWhiteSpace(tenTheLoai))
            {
                MessageBox.Show(
                    "Vui lòng nhập đầy đủ mã thể loại và tên thể loại.",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            try
            {
                using (SqlConnection conn = DbConnection.GetConnection())
                {
                    conn.Open();

                    string sql = @"
                UPDATE TheLoai
                SET TenTheLoai = @TenTheLoai
                WHERE MaTheLoai = @MaTheLoai";

                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@TenTheLoai", tenTheLoai);
                        cmd.Parameters.AddWithValue("@MaTheLoai", maTheLoai);

                        int soDong = cmd.ExecuteNonQuery();

                        if (soDong == 0)
                        {
                            MessageBox.Show(
                                "Không tìm thấy mã thể loại cần sửa.",
                                "Thông báo",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);

                            return;
                        }
                    }
                }

                MessageBox.Show(
                    "Cập nhật thể loại thành công!",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                LoadTheLoai();

                txtMaTheLoai.Clear();
                txtTenTheLoai.Clear();
                txtMaTheLoai.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Không thể cập nhật thể loại.\n\n" + ex.Message,
                    "Lỗi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void dgvTheLoai_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            dongDangChon = e.RowIndex;

            DataGridViewRow row = dgvTheLoai.Rows[e.RowIndex];

            txtMaTheLoai.Text = row.Cells["colMaTheLoai"].Value?.ToString();
            txtTenTheLoai.Text = row.Cells["colTenTheLoai"].Value?.ToString();
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (dgvTheLoai.CurrentRow == null)
            {
                MessageBox.Show(
                    "Vui lòng chọn thể loại cần xóa.",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            string maTheLoai = txtMaTheLoai.Text.Trim();

            if (string.IsNullOrWhiteSpace(maTheLoai))
            {
                MessageBox.Show(
                    "Vui lòng chọn thể loại cần xóa.",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            DialogResult ketQua = MessageBox.Show(
                "Bạn có chắc chắn muốn xóa thể loại này không?",
                "Xác nhận xóa",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (ketQua != DialogResult.Yes)
                return;

            try
            {
                using (SqlConnection conn = DbConnection.GetConnection())
                {
                    conn.Open();

                    string sql = @"
                DELETE FROM TheLoai
                WHERE MaTheLoai = @MaTheLoai";

                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@MaTheLoai", maTheLoai);

                        int soDong = cmd.ExecuteNonQuery();

                        if (soDong == 0)
                        {
                            MessageBox.Show(
                                "Không tìm thấy thể loại cần xóa.",
                                "Thông báo",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);

                            return;
                        }
                    }
                }

                MessageBox.Show(
                    "Xóa thể loại thành công!",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                LoadTheLoai();

                txtMaTheLoai.Clear();
                txtTenTheLoai.Clear();
                txtMaTheLoai.Focus();
            }
            catch (SqlException ex)
            {
                if (ex.Number == 547)
                {
                    MessageBox.Show(
                        "Không thể xóa thể loại này vì đang được sử dụng bởi sách.",
                        "Thông báo",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                }
                else
                {
                    MessageBox.Show(
                        "Không thể xóa thể loại.\n\n" + ex.Message,
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
            string tuKhoa = txtTenTheLoai.Text.Trim();

            if (tuKhoa == "")
            {
                MessageBox.Show(
                    "Vui lòng nhập từ khóa cần tìm.",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                txtTenTheLoai.Focus();
                return;
            }

            try
            {
                using (SqlConnection conn = DbConnection.GetConnection())
                {
                    conn.Open();

                    string sql = @"
                SELECT MaTheLoai, TenTheLoai
                FROM TheLoai
                WHERE MaTheLoai LIKE @TuKhoa
                   OR TenTheLoai LIKE @TuKhoa
                ORDER BY MaTheLoai";

                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@TuKhoa", "%" + tuKhoa + "%");

                        using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            adapter.Fill(dt);

                            dgvTheLoai.Rows.Clear();

                            foreach (DataRow row in dt.Rows)
                            {
                                dgvTheLoai.Rows.Add(
                                    row["MaTheLoai"].ToString(),
                                    row["TenTheLoai"].ToString());
                            }

                            if (dt.Rows.Count == 0)
                            {
                                MessageBox.Show(
                                    "Không tìm thấy thể loại phù hợp.",
                                    "Thông báo",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Information);

                                return;
                            }

                            if (dt.Rows.Count == 1)
                            {
                                txtMaTheLoai.Text =
                                    dt.Rows[0]["MaTheLoai"].ToString();

                                txtTenTheLoai.Text =
                                    dt.Rows[0]["TenTheLoai"].ToString();

                                dgvTheLoai.Rows[0].Selected = true;
                                dgvTheLoai.CurrentCell =
                                    dgvTheLoai.Rows[0].Cells["colMaTheLoai"];

                                dongDangChon = 0;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Không thể tìm kiếm thể loại.\n\n" + ex.Message,
                    "Lỗi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            txtMaTheLoai.Clear();
            txtTenTheLoai.Clear();

            dongDangChon = -1;

            LoadTheLoai();

            dgvTheLoai.ClearSelection();

            txtMaTheLoai.Focus();
        }

        private void LoadTheLoai()
        {
            try
            {
                using (SqlConnection conn = DbConnection.GetConnection())
                {
                    conn.Open();

                    string sql = @"
                SELECT MaTheLoai, TenTheLoai
                FROM TheLoai
                ORDER BY MaTheLoai";

                    using (SqlDataAdapter adapter = new SqlDataAdapter(sql, conn))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);

                        dgvTheLoai.Rows.Clear();

                        foreach (DataRow row in dt.Rows)
                        {
                            dgvTheLoai.Rows.Add(
                                row["MaTheLoai"].ToString(),
                                row["TenTheLoai"].ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Không thể tải dữ liệu thể loại.\n\n" + ex.Message,
                    "Lỗi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }
    }
}
