using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace DoAnQLTV
{
    public partial class FrmThongTinDocGia : Form
    {
        public FrmThongTinDocGia()
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
                SELECT
                    MaDocGia,
                    TenDocGia,
                    NgaySinh,
                    DiaChi,
                    SoDienThoai
                FROM DocGia
                ORDER BY MaDocGia";

                    using (SqlDataAdapter adapter =
                        new SqlDataAdapter(sql, conn))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);

                        dgvKetQua.Rows.Clear();

                        foreach (DataRow row in dt.Rows)
                        {
                            dgvKetQua.Rows.Add(
                                row["MaDocGia"].ToString(),
                                row["TenDocGia"].ToString(),
                                Convert.ToDateTime(
                                    row["NgaySinh"]).ToString("dd/MM/yyyy"),
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

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            string tuKhoa = txtTuKhoa.Text.Trim();

            if (tuKhoa == "")
            {
                MessageBox.Show(
                    "Vui lòng nhập từ khóa cần tìm.",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                txtTuKhoa.Focus();
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

                        using (SqlDataAdapter adapter =
                            new SqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            adapter.Fill(dt);

                            dgvKetQua.Rows.Clear();

                            foreach (DataRow row in dt.Rows)
                            {
                                dgvKetQua.Rows.Add(
                                    row["MaDocGia"].ToString(),
                                    row["TenDocGia"].ToString(),
                                    Convert.ToDateTime(
                                        row["NgaySinh"]).ToString("dd/MM/yyyy"),
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

                            dgvKetQua.ClearSelection();

                            if (dgvKetQua.Rows.Count > 0)
                            {
                                dgvKetQua.Rows[0].Selected = true;

                                dgvKetQua.CurrentCell =
                                    dgvKetQua.Rows[0].Cells["colMaDocGia"];
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
            txtTuKhoa.Clear();

            LoadDocGia();

            dgvKetQua.ClearSelection();

            txtTuKhoa.Focus();
        }

        private void dgvKetQua_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            DataGridViewRow row = dgvKetQua.Rows[e.RowIndex];

            string maDocGia =
                row.Cells["colMaDocGia"].Value?.ToString() ?? "";

            string tenDocGia =
                row.Cells["colTenDocGia"].Value?.ToString() ?? "";

            txtTuKhoa.Text = tenDocGia;
        }
    }
}
