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
    public partial class FrmTacGia : Form
    {
        private int dongDangChon = -1;
        public FrmTacGia()
        {
            InitializeComponent();

            LoadTacGia();

        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            string maTacGia = txtMaTacGia.Text.Trim();
            string tenTacGia = txtTenTacGia.Text.Trim();

            if (string.IsNullOrWhiteSpace(maTacGia) ||
                string.IsNullOrWhiteSpace(tenTacGia))
            {
                MessageBox.Show(
                    "Vui lòng nhập đầy đủ mã tác giả và tên tác giả.",
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
                INSERT INTO TacGia (MaTacGia, TenTacGia)
                VALUES (@MaTacGia, @TenTacGia)";

                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@MaTacGia", maTacGia);
                        cmd.Parameters.AddWithValue("@TenTacGia", tenTacGia);

                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show(
                    "Thêm tác giả thành công!",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                LoadTacGia();

                txtMaTacGia.Clear();
                txtTenTacGia.Clear();
                txtMaTacGia.Focus();
            }
            catch (SqlException ex)
            {
                if (ex.Number == 2627 || ex.Number == 2601)
                {
                    MessageBox.Show(
                        "Mã tác giả đã tồn tại.",
                        "Thông báo",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                }
                else
                {
                    MessageBox.Show(
                        "Không thể thêm tác giả.\n\n" + ex.Message,
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

        private void dgvTacGia_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            dongDangChon = e.RowIndex;

            DataGridViewRow row = dgvTacGia.Rows[e.RowIndex];

            txtMaTacGia.Text = row.Cells["colMaTacGia"].Value?.ToString();
            txtTenTacGia.Text = row.Cells["colTenTacGia"].Value?.ToString();
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (dgvTacGia.CurrentRow == null)
            {
                MessageBox.Show(
                    "Vui lòng chọn tác giả cần sửa.",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            string maTacGia = txtMaTacGia.Text.Trim();
            string tenTacGia = txtTenTacGia.Text.Trim();

            if (string.IsNullOrWhiteSpace(maTacGia) ||
                string.IsNullOrWhiteSpace(tenTacGia))
            {
                MessageBox.Show(
                    "Vui lòng nhập đầy đủ mã tác giả và tên tác giả.",
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
                UPDATE TacGia
                SET TenTacGia = @TenTacGia
                WHERE MaTacGia = @MaTacGia";

                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@TenTacGia", tenTacGia);
                        cmd.Parameters.AddWithValue("@MaTacGia", maTacGia);

                        int soDong = cmd.ExecuteNonQuery();

                        if (soDong == 0)
                        {
                            MessageBox.Show(
                                "Không tìm thấy mã tác giả cần sửa.",
                                "Thông báo",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);

                            return;
                        }
                    }
                }

                MessageBox.Show(
                    "Cập nhật tác giả thành công!",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                LoadTacGia();

                txtMaTacGia.Clear();
                txtTenTacGia.Clear();
                txtMaTacGia.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Không thể cập nhật tác giả.\n\n" + ex.Message,
                    "Lỗi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (dgvTacGia.CurrentRow == null)
            {
                MessageBox.Show(
                    "Vui lòng chọn tác giả cần xóa.",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            string maTacGia = txtMaTacGia.Text.Trim();

            if (string.IsNullOrWhiteSpace(maTacGia))
            {
                MessageBox.Show(
                    "Vui lòng chọn tác giả cần xóa.",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            DialogResult ketQua = MessageBox.Show(
                "Bạn có chắc chắn muốn xóa tác giả này không?",
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
                DELETE FROM TacGia
                WHERE MaTacGia = @MaTacGia";

                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@MaTacGia", maTacGia);

                        int soDong = cmd.ExecuteNonQuery();

                        if (soDong == 0)
                        {
                            MessageBox.Show(
                                "Không tìm thấy tác giả cần xóa.",
                                "Thông báo",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);

                            return;
                        }
                    }
                }

                MessageBox.Show(
                    "Xóa tác giả thành công!",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                LoadTacGia();

                txtMaTacGia.Clear();
                txtTenTacGia.Clear();
                txtMaTacGia.Focus();
            }
            catch (SqlException ex)
            {
                if (ex.Number == 547)
                {
                    MessageBox.Show(
                        "Không thể xóa tác giả này vì đang được sử dụng bởi sách.",
                        "Thông báo",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                }
                else
                {
                    MessageBox.Show(
                        "Không thể xóa tác giả.\n\n" + ex.Message,
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
            string tuKhoa = txtTenTacGia.Text.Trim();

            if (tuKhoa == "")
            {
                MessageBox.Show(
                    "Vui lòng nhập từ khóa cần tìm.",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                txtTenTacGia.Focus();
                return;
            }

            try
            {
                using (SqlConnection conn = DbConnection.GetConnection())
                {
                    conn.Open();

                    string sql = @"
                SELECT MaTacGia, TenTacGia
                FROM TacGia
                WHERE MaTacGia LIKE @TuKhoa
                   OR TenTacGia LIKE @TuKhoa
                ORDER BY MaTacGia";

                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@TuKhoa", "%" + tuKhoa + "%");

                        using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            adapter.Fill(dt);

                            dgvTacGia.Rows.Clear();

                            foreach (DataRow row in dt.Rows)
                            {
                                dgvTacGia.Rows.Add(
                                    row["MaTacGia"].ToString(),
                                    row["TenTacGia"].ToString());
                            }

                            if (dt.Rows.Count == 0)
                            {
                                MessageBox.Show(
                                    "Không tìm thấy tác giả phù hợp.",
                                    "Thông báo",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Information);

                                return;
                            }

                            if (dt.Rows.Count == 1)
                            {
                                txtMaTacGia.Text =
                                    dt.Rows[0]["MaTacGia"].ToString();

                                txtTenTacGia.Text =
                                    dt.Rows[0]["TenTacGia"].ToString();

                                dgvTacGia.Rows[0].Selected = true;
                                dgvTacGia.CurrentCell =
                                    dgvTacGia.Rows[0].Cells["colMaTacGia"];
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Không thể tìm kiếm tác giả.\n\n" + ex.Message,
                    "Lỗi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            txtMaTacGia.Clear();
            txtTenTacGia.Clear();

            LoadTacGia();

            dgvTacGia.ClearSelection();

            txtMaTacGia.Focus();
        }

        private void LoadTacGia()
        {
            try
            {
                using (SqlConnection conn = DbConnection.GetConnection())
                {
                    conn.Open();

                    string sql = @"
                SELECT MaTacGia, TenTacGia
                FROM TacGia
                ORDER BY MaTacGia";

                    using (SqlDataAdapter adapter = new SqlDataAdapter(sql, conn))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);

                        dgvTacGia.Rows.Clear();

                        foreach (DataRow row in dt.Rows)
                        {
                            dgvTacGia.Rows.Add(
                                row["MaTacGia"].ToString(),
                                row["TenTacGia"].ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Không thể tải dữ liệu tác giả.\n\n" + ex.Message,
                    "Lỗi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }
    }
}
