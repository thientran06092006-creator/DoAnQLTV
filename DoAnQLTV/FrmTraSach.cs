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
    public partial class FrmTraSach : Form
    {
        public FrmTraSach()
        {
            InitializeComponent();

            LoadDocGia();
            LoadSach();
            LoadTraSach();
        }

        private void LoadDocGia()
        {
            try
            {
                using (SqlConnection conn = DbConnection.GetConnection())
                {
                    conn.Open();

                    string sql = @"
                SELECT MaDocGia, TenDocGia
                FROM DocGia
                ORDER BY MaDocGia";

                    using (SqlDataAdapter adapter =
                        new SqlDataAdapter(sql, conn))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);

                        cboDocGia.Items.Clear();

                        foreach (DataRow row in dt.Rows)
                        {
                            cboDocGia.Items.Add(
                                row["MaDocGia"].ToString()
                                + " - "
                                + row["TenDocGia"].ToString());
                        }

                        if (cboDocGia.Items.Count > 0)
                        {
                            cboDocGia.SelectedIndex = 0;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Không thể tải danh sách độc giả.\n\n" + ex.Message,
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
                SELECT MaSach, TenSach
                FROM Sach
                ORDER BY MaSach";

                    using (SqlDataAdapter adapter =
                        new SqlDataAdapter(sql, conn))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);

                        cboSach.Items.Clear();

                        foreach (DataRow row in dt.Rows)
                        {
                            cboSach.Items.Add(
                                row["MaSach"].ToString()
                                + " - "
                                + row["TenSach"].ToString());
                        }

                        if (cboSach.Items.Count > 0)
                        {
                            cboSach.SelectedIndex = 0;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Không thể tải danh sách sách.\n\n" + ex.Message,
                    "Lỗi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void LoadTraSach()
        {
            try
            {
                using (SqlConnection conn = DbConnection.GetConnection())
                {
                    conn.Open();

                    string sql = @"
                SELECT
                    pm.MaPhieu,
                    pm.MaDocGia + ' - ' + dg.TenDocGia AS DocGia,
                    pm.MaSach + ' - ' + s.TenSach AS Sach,
                    pm.NgayMuon,
                    pm.HanTra,
                    pm.NgayTra
                FROM PhieuMuon pm
                INNER JOIN DocGia dg
                    ON pm.MaDocGia = dg.MaDocGia
                INNER JOIN Sach s
                    ON pm.MaSach = s.MaSach
                WHERE pm.NgayTra IS NULL
                ORDER BY pm.MaPhieu";

                    using (SqlDataAdapter adapter =
                        new SqlDataAdapter(sql, conn))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);

                        dgvTraSach.Rows.Clear();

                        foreach (DataRow row in dt.Rows)
                        {
                            dgvTraSach.Rows.Add(
                                row["MaPhieu"].ToString(),
                                row["DocGia"].ToString(),
                                row["Sach"].ToString(),
                                Convert.ToDateTime(
                                    row["NgayMuon"]).ToString("dd/MM/yyyy"),
                                Convert.ToDateTime(
                                    row["HanTra"]).ToString("dd/MM/yyyy"),
                                row["NgayTra"] == DBNull.Value
                                    ? ""
                                    : Convert.ToDateTime(
                                        row["NgayTra"])
                                        .ToString("dd/MM/yyyy"));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Không thể tải dữ liệu trả sách.\n\n" + ex.Message,
                    "Lỗi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            string tuKhoa = txtMaPhieu.Text.Trim();

            if (tuKhoa == "")
            {
                MessageBox.Show(
                    "Vui lòng nhập mã phiếu cần tìm.",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                txtMaPhieu.Focus();
                return;
            }

            try
            {
                using (SqlConnection conn = DbConnection.GetConnection())
                {
                    conn.Open();

                    string sql = @"
                SELECT
                    pm.MaPhieu,
                    pm.MaDocGia + ' - ' + dg.TenDocGia AS DocGia,
                    pm.MaSach + ' - ' + s.TenSach AS Sach,
                    pm.NgayMuon,
                    pm.HanTra,
                    pm.NgayTra
                FROM PhieuMuon pm
                INNER JOIN DocGia dg
                    ON pm.MaDocGia = dg.MaDocGia
                INNER JOIN Sach s
                    ON pm.MaSach = s.MaSach
                WHERE pm.MaPhieu LIKE @TuKhoa
                  AND pm.NgayTra IS NULL
                ORDER BY pm.MaPhieu";

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

                            dgvTraSach.Rows.Clear();

                            foreach (DataRow row in dt.Rows)
                            {
                                dgvTraSach.Rows.Add(
                                    row["MaPhieu"].ToString(),
                                    row["DocGia"].ToString(),
                                    row["Sach"].ToString(),
                                    Convert.ToDateTime(
                                        row["NgayMuon"]).ToString("dd/MM/yyyy"),
                                    Convert.ToDateTime(
                                        row["HanTra"]).ToString("dd/MM/yyyy"),
                                    row["NgayTra"] == DBNull.Value
                                        ? ""
                                        : Convert.ToDateTime(
                                            row["NgayTra"])
                                            .ToString("dd/MM/yyyy"));
                            }

                            if (dt.Rows.Count == 0)
                            {
                                MessageBox.Show(
                                    "Không tìm thấy phiếu mượn chưa trả phù hợp.",
                                    "Thông báo",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Information);

                                return;
                            }

                            dgvTraSach.ClearSelection();

                            if (dgvTraSach.Rows.Count > 0)
                            {
                                dgvTraSach.Rows[0].Selected = true;

                                dgvTraSach.CurrentCell =
                                    dgvTraSach.Rows[0].Cells["colMaPhieu"];
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Không thể tìm kiếm phiếu trả sách.\n\n" + ex.Message,
                    "Lỗi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void dgvTraSach_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            DataGridViewRow row = dgvTraSach.Rows[e.RowIndex];

            txtMaPhieu.Text =
                row.Cells["colMaPhieu"].Value?.ToString() ?? "";

            cboDocGia.Text =
                row.Cells["colDocGia"].Value?.ToString() ?? "";

            cboSach.Text =
                row.Cells["colSach"].Value?.ToString() ?? "";

            string ngayMuon =
                row.Cells["colNgayMuon"].Value?.ToString() ?? "";

            string hanTra =
                row.Cells["colHanTra"].Value?.ToString() ?? "";

            string ngayTra =
                row.Cells["colNgayTra"].Value?.ToString() ?? "";

            DateTime ngayMuonValue;
            if (DateTime.TryParse(ngayMuon, out ngayMuonValue))
            {
                dtpNgayMuon.Value = ngayMuonValue;
            }

            DateTime hanTraValue;
            if (DateTime.TryParse(hanTra, out hanTraValue))
            {
                dtpHanTra.Value = hanTraValue;
            }

            DateTime ngayTraValue;
            if (DateTime.TryParse(ngayTra, out ngayTraValue))
            {
                dtpNgayTra.Value = ngayTraValue;
            }
        }

        private void btnTraSach_Click(object sender, EventArgs e)
        {
            string maPhieu = txtMaPhieu.Text.Trim();

            if (maPhieu == "")
            {
                MessageBox.Show(
                    "Vui lòng chọn phiếu cần trả sách.",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            if (dtpNgayTra.Value.Date < dtpNgayMuon.Value.Date)
            {
                MessageBox.Show(
                    "Ngày trả không được trước ngày mượn.",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            DialogResult result = MessageBox.Show(
                "Bạn có chắc chắn muốn xác nhận trả sách cho phiếu này không?",
                "Xác nhận trả sách",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.No)
                return;

            try
            {
                using (SqlConnection conn = DbConnection.GetConnection())
                {
                    conn.Open();

                    string sql = @"
                UPDATE PhieuMuon
                SET NgayTra = @NgayTra
                WHERE MaPhieu = @MaPhieu
                  AND NgayTra IS NULL";

                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue(
                            "@NgayTra",
                            dtpNgayTra.Value.Date);

                        cmd.Parameters.AddWithValue(
                            "@MaPhieu",
                            maPhieu);

                        int soDong = cmd.ExecuteNonQuery();

                        if (soDong == 0)
                        {
                            MessageBox.Show(
                                "Không tìm thấy phiếu chưa trả hoặc phiếu đã được trả.",
                                "Thông báo",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);

                            return;
                        }
                    }
                }

                MessageBox.Show(
                    "Trả sách thành công.",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                LoadTraSach();

                txtMaPhieu.Clear();

                if (cboDocGia.Items.Count > 0)
                    cboDocGia.SelectedIndex = 0;

                if (cboSach.Items.Count > 0)
                    cboSach.SelectedIndex = 0;

                dtpNgayMuon.Value = DateTime.Now;
                dtpHanTra.Value = DateTime.Now.AddDays(7);
                dtpNgayTra.Value = DateTime.Now;

                dgvTraSach.ClearSelection();

                txtMaPhieu.Focus();
            }
            catch (SqlException ex)
            {
                MessageBox.Show(
                    "Không thể cập nhật trạng thái trả sách.\n\n" + ex.Message,
                    "Lỗi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
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

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            txtMaPhieu.Clear();

            if (cboDocGia.Items.Count > 0)
            {
                cboDocGia.SelectedIndex = 0;
            }

            if (cboSach.Items.Count > 0)
            {
                cboSach.SelectedIndex = 0;
            }

            dtpNgayMuon.Value = DateTime.Now;
            dtpHanTra.Value = DateTime.Now.AddDays(7);
            dtpNgayTra.Value = DateTime.Now;

            LoadTraSach();

            dgvTraSach.ClearSelection();

            txtMaPhieu.Focus();
        }
    }
}
