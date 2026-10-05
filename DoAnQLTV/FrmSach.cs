using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace DoAnQLTV
{
    public partial class FrmSach : Form
    {
        private int dongDangChon = -1;
        public FrmSach()
        {
            InitializeComponent();

            LoadTheLoai();
            LoadTacGia();
            LoadSach();
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

                        cboTheLoai.Items.Clear();

                        foreach (DataRow row in dt.Rows)
                        {
                            cboTheLoai.Items.Add(
                                row["TenTheLoai"].ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Không thể tải danh sách thể loại.\n\n" + ex.Message,
                    "Lỗi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
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

                        cboTacGia.Items.Clear();

                        foreach (DataRow row in dt.Rows)
                        {
                            cboTacGia.Items.Add(
                                row["TenTacGia"].ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Không thể tải danh sách tác giả.\n\n" + ex.Message,
                    "Lỗi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

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

                    using (SqlDataAdapter adapter = new SqlDataAdapter(sql, conn))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);

                        dgvSach.Rows.Clear();

                        foreach (DataRow row in dt.Rows)
                        {
                            dgvSach.Rows.Add(
                                row["MaSach"].ToString(),
                                row["TenSach"].ToString(),
                                row["TenTheLoai"].ToString(),
                                row["TenTacGia"].ToString());
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
                    MessageBoxIcon.Error);
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            string maSach = txtMaSach.Text.Trim();
            string tenSach = txtTenSach.Text.Trim();
            string tenTheLoai = cboTheLoai.Text.Trim();
            string tenTacGia = cboTacGia.Text.Trim();

            if (maSach == "")
            {
                MessageBox.Show(
                    "Vui lòng nhập mã sách.",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                txtMaSach.Focus();
                return;
            }

            if (tenSach == "")
            {
                MessageBox.Show(
                    "Vui lòng nhập tên sách.",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                txtTenSach.Focus();
                return;
            }

            if (tenTheLoai == "")
            {
                MessageBox.Show(
                    "Vui lòng chọn thể loại.",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                cboTheLoai.Focus();
                return;
            }

            if (tenTacGia == "")
            {
                MessageBox.Show(
                    "Vui lòng chọn tác giả.",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                cboTacGia.Focus();
                return;
            }

            try
            {
                using (SqlConnection conn = DbConnection.GetConnection())
                {
                    conn.Open();

                    string sql = @"
                INSERT INTO Sach
                    (MaSach, TenSach, MaTheLoai, MaTacGia)
                SELECT
                    @MaSach,
                    @TenSach,
                    tl.MaTheLoai,
                    tg.MaTacGia
                FROM TheLoai tl
                CROSS JOIN TacGia tg
                WHERE tl.TenTheLoai = @TenTheLoai
                  AND tg.TenTacGia = @TenTacGia";

                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@MaSach", maSach);
                        cmd.Parameters.AddWithValue("@TenSach", tenSach);
                        cmd.Parameters.AddWithValue("@TenTheLoai", tenTheLoai);
                        cmd.Parameters.AddWithValue("@TenTacGia", tenTacGia);

                        int soDong = cmd.ExecuteNonQuery();

                        if (soDong == 0)
                        {
                            MessageBox.Show(
                                "Không tìm thấy thể loại hoặc tác giả tương ứng.",
                                "Thông báo",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);

                            return;
                        }
                    }
                }

                MessageBox.Show(
                    "Thêm sách thành công.",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                LoadSach();

                txtMaSach.Clear();
                txtTenSach.Clear();
                cboTheLoai.SelectedIndex = -1;
                cboTacGia.SelectedIndex = -1;

                txtMaSach.Focus();
            }
            catch (SqlException ex)
            {
                if (ex.Number == 2627 || ex.Number == 2601)
                {
                    MessageBox.Show(
                        "Mã sách đã tồn tại. Vui lòng nhập mã khác.",
                        "Thông báo",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                }
                else
                {
                    MessageBox.Show(
                        "Không thể thêm sách.\n\n" + ex.Message,
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

        private void dgvSach_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            dongDangChon = e.RowIndex;

            DataGridViewRow row = dgvSach.Rows[e.RowIndex];

            txtMaSach.Text = row.Cells["colMaSach"].Value?.ToString();
            txtTenSach.Text = row.Cells["colTenSach"].Value?.ToString();
            cboTheLoai.Text = row.Cells["colTheLoai"].Value?.ToString();
            cboTacGia.Text = row.Cells["colTacGia"].Value?.ToString();
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (dongDangChon == -1)
            {
                MessageBox.Show(
                    "Vui lòng chọn sách cần sửa.",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            string maSach = txtMaSach.Text.Trim();
            string tenSach = txtTenSach.Text.Trim();
            string tenTheLoai = cboTheLoai.Text.Trim();
            string tenTacGia = cboTacGia.Text.Trim();

            if (maSach == "")
            {
                MessageBox.Show(
                    "Vui lòng nhập mã sách.",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                txtMaSach.Focus();
                return;
            }

            if (tenSach == "")
            {
                MessageBox.Show(
                    "Vui lòng nhập tên sách.",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                txtTenSach.Focus();
                return;
            }

            if (tenTheLoai == "")
            {
                MessageBox.Show(
                    "Vui lòng chọn thể loại.",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                cboTheLoai.Focus();
                return;
            }

            if (tenTacGia == "")
            {
                MessageBox.Show(
                    "Vui lòng chọn tác giả.",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                cboTacGia.Focus();
                return;
            }

            DataGridViewRow row = dgvSach.Rows[dongDangChon];

            string maSachCu =
                row.Cells["colMaSach"].Value?.ToString() ?? "";

            if (maSachCu == "")
            {
                MessageBox.Show(
                    "Không xác định được mã sách cần sửa.",
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
                UPDATE Sach
                SET
                    MaSach = @MaSachMoi,
                    TenSach = @TenSach,
                    MaTheLoai = (
                        SELECT MaTheLoai
                        FROM TheLoai
                        WHERE TenTheLoai = @TenTheLoai
                    ),
                    MaTacGia = (
                        SELECT MaTacGia
                        FROM TacGia
                        WHERE TenTacGia = @TenTacGia
                    )
                WHERE MaSach = @MaSachCu";

                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@MaSachMoi", maSach);
                        cmd.Parameters.AddWithValue("@TenSach", tenSach);
                        cmd.Parameters.AddWithValue("@TenTheLoai", tenTheLoai);
                        cmd.Parameters.AddWithValue("@TenTacGia", tenTacGia);
                        cmd.Parameters.AddWithValue("@MaSachCu", maSachCu);

                        int soDong = cmd.ExecuteNonQuery();

                        if (soDong == 0)
                        {
                            MessageBox.Show(
                                "Không tìm thấy sách cần sửa hoặc dữ liệu không thay đổi.",
                                "Thông báo",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);

                            return;
                        }
                    }
                }

                MessageBox.Show(
                    "Sửa thông tin sách thành công.",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                LoadSach();

                txtMaSach.Clear();
                txtTenSach.Clear();
                cboTheLoai.SelectedIndex = -1;
                cboTacGia.SelectedIndex = -1;

                dongDangChon = -1;

                dgvSach.ClearSelection();

                txtMaSach.Focus();
            }
            catch (SqlException ex)
            {
                if (ex.Number == 2627 || ex.Number == 2601)
                {
                    MessageBox.Show(
                        "Mã sách mới đã tồn tại. Vui lòng nhập mã khác.",
                        "Thông báo",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                }
                else
                {
                    MessageBox.Show(
                        "Không thể sửa sách.\n\n" + ex.Message,
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

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (dongDangChon == -1)
            {
                MessageBox.Show(
                    "Vui lòng chọn sách cần xóa.",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            DataGridViewRow row = dgvSach.Rows[dongDangChon];

            string maSach =
                row.Cells["colMaSach"].Value?.ToString() ?? "";

            if (maSach == "")
            {
                MessageBox.Show(
                    "Không xác định được mã sách cần xóa.",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            DialogResult result = MessageBox.Show(
                "Bạn có chắc chắn muốn xóa sách này không?",
                "Xác nhận xóa",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.No)
            {
                return;
            }

            try
            {
                using (SqlConnection conn = DbConnection.GetConnection())
                {
                    conn.Open();

                    string sql = @"
                DELETE FROM Sach
                WHERE MaSach = @MaSach";

                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@MaSach", maSach);

                        int soDong = cmd.ExecuteNonQuery();

                        if (soDong == 0)
                        {
                            MessageBox.Show(
                                "Không tìm thấy sách cần xóa.",
                                "Thông báo",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);

                            return;
                        }
                    }
                }

                MessageBox.Show(
                    "Xóa sách thành công.",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                LoadSach();

                txtMaSach.Clear();
                txtTenSach.Clear();
                cboTheLoai.SelectedIndex = -1;
                cboTacGia.SelectedIndex = -1;

                dongDangChon = -1;

                dgvSach.ClearSelection();

                txtMaSach.Focus();
            }
            catch (SqlException ex)
            {
                if (ex.Number == 547)
                {
                    MessageBox.Show(
                        "Không thể xóa sách này vì sách đang được sử dụng trong phiếu mượn.",
                        "Không thể xóa",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                }
                else
                {
                    MessageBox.Show(
                        "Không thể xóa sách.\n\n" + ex.Message,
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
            string tuKhoa = txtTenSach.Text.Trim();

            if (tuKhoa == "")
            {
                MessageBox.Show(
                    "Vui lòng nhập từ khóa cần tìm.",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                txtTenSach.Focus();
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
                ORDER BY s.MaSach";

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

                            dgvSach.Rows.Clear();

                            foreach (DataRow row in dt.Rows)
                            {
                                dgvSach.Rows.Add(
                                    row["MaSach"].ToString(),
                                    row["TenSach"].ToString(),
                                    row["TenTheLoai"].ToString(),
                                    row["TenTacGia"].ToString());
                            }

                            if (dt.Rows.Count == 0)
                            {
                                MessageBox.Show(
                                    "Không tìm thấy sách phù hợp.",
                                    "Thông báo",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Information);

                                return;
                            }

                            dgvSach.ClearSelection();

                            if (dgvSach.Rows.Count > 0)
                            {
                                dgvSach.Rows[0].Selected = true;
                                dgvSach.CurrentCell =
                                    dgvSach.Rows[0].Cells["colMaSach"];
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
                    MessageBoxIcon.Error);
            }
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            txtMaSach.Clear();
            txtTenSach.Clear();

            cboTheLoai.SelectedIndex = -1;
            cboTacGia.SelectedIndex = -1;

            dongDangChon = -1;

            LoadSach();

            dgvSach.ClearSelection();

            txtMaSach.Focus();
        }
    }
}