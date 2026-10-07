using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace DoAnQLTV
{
    public partial class FrmTimKiemSach : Form
    {
        public FrmTimKiemSach()
        {
            InitializeComponent();

            LoadSach();
        }

        // ==============================
        // TẢI DANH SÁCH SÁCH
        // ==============================
        private void LoadSach()
        {
            try
            {
                using (SqlConnection conn = DbConnection.GetConnection())
                {
                    conn.Open();

                    string sql = @"
                        SELECT
                            s.MaSach,
                            s.TenSach,
                            tl.TenTheLoai,
                            tg.TenTacGia
                        FROM Sach s
                        INNER JOIN TheLoai tl
                            ON s.MaTheLoai = tl.MaTheLoai
                        INNER JOIN TacGia tg
                            ON s.MaTacGia = tg.MaTacGia
                        ORDER BY s.MaSach";

                    using (SqlDataAdapter adapter =
                        new SqlDataAdapter(sql, conn))
                    {
                        DataTable dt = new DataTable();

                        adapter.Fill(dt);

                        dgvKetQua.Rows.Clear();

                        foreach (DataRow row in dt.Rows)
                        {
                            dgvKetQua.Rows.Add(
                                row["MaSach"].ToString(),
                                row["TenSach"].ToString(),
                                row["TenTheLoai"].ToString(),
                                row["TenTacGia"].ToString()
                            );
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Không thể tải dữ liệu sách.\n\n" + ex.Message,
                    "Lỗi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        // ==============================
        // TÌM KIẾM SÁCH
        // ==============================
        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            string tuKhoa = txtTuKhoa.Text.Trim();

            if (tuKhoa == "")
            {
                MessageBox.Show(
                    "Vui lòng nhập từ khóa cần tìm.",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );

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
                            s.MaSach,
                            s.TenSach,
                            tl.TenTheLoai,
                            tg.TenTacGia
                        FROM Sach s
                        INNER JOIN TheLoai tl
                            ON s.MaTheLoai = tl.MaTheLoai
                        INNER JOIN TacGia tg
                            ON s.MaTacGia = tg.MaTacGia
                        WHERE s.MaSach LIKE @TuKhoa
                           OR s.TenSach LIKE @TuKhoa
                           OR tl.TenTheLoai LIKE @TuKhoa
                           OR tg.TenTacGia LIKE @TuKhoa
                        ORDER BY s.MaSach";

                    using (SqlCommand cmd =
                        new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue(
                            "@TuKhoa",
                            "%" + tuKhoa + "%"
                        );

                        using (SqlDataAdapter adapter =
                            new SqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();

                            adapter.Fill(dt);

                            dgvKetQua.Rows.Clear();

                            foreach (DataRow row in dt.Rows)
                            {
                                dgvKetQua.Rows.Add(
                                    row["MaSach"].ToString(),
                                    row["TenSach"].ToString(),
                                    row["TenTheLoai"].ToString(),
                                    row["TenTacGia"].ToString()
                                );
                            }

                            // Không tìm thấy kết quả
                            if (dt.Rows.Count == 0)
                            {
                                MessageBox.Show(
                                    "Không tìm thấy sách phù hợp.",
                                    "Thông báo",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Information
                                );

                                txtTuKhoa.Focus();

                                return;
                            }

                            // Chọn dòng đầu tiên
                            dgvKetQua.ClearSelection();

                            if (dgvKetQua.Rows.Count > 0)
                            {
                                dgvKetQua.Rows[0].Selected = true;

                                dgvKetQua.CurrentCell =
                                    dgvKetQua.Rows[0]
                                    .Cells["colMaSach"];
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Không thể tìm kiếm sách.\n\n" + ex.Message,
                    "Lỗi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        // ==============================
        // LÀM MỚI
        // ==============================
        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            txtTuKhoa.Clear();

            LoadSach();

            dgvKetQua.ClearSelection();

            txtTuKhoa.Focus();
        }
    }
}