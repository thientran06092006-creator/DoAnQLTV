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
    public partial class FrmLapPhieuMuon : Form
    {
        private int dongDangChon = -1;
        public FrmLapPhieuMuon()
        {
            InitializeComponent();

            LoadSach();

            LoadDocGia();
            LoadNhanVien();
            LoadPhieuMuon();
        }


        private void LoadSach()
        {
            try
            {
                using (SqlConnection conn = DbConnection.GetConnection())
                {
                    conn.Open();

                    string sql = "SELECT MaSach, TenSach FROM Sach ORDER BY TenSach";

                    using (SqlDataAdapter da = new SqlDataAdapter(sql, conn))
                    {
                        DataTable dt = new DataTable();
                        da.Fill(dt);

                        cboSach.DataSource = dt;
                        cboSach.DisplayMember = "TenSach";
                        cboSach.ValueMember = "MaSach";
                        cboSach.SelectedIndex = -1;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể tải danh sách sách: " + ex.Message);
            }
        }


        private void CapNhatThongKeTrangChu()
        {
            // Không còn cập nhật thống kê trên FrmMain.
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

        private void LoadNhanVien()
        {
            try
            {
                using (SqlConnection conn = DbConnection.GetConnection())
                {
                    conn.Open();

                    string sql = @"
                SELECT MaNhanVien, TenNhanVien
                FROM NhanVien
                ORDER BY MaNhanVien";

                    using (SqlDataAdapter adapter =
                        new SqlDataAdapter(sql, conn))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);

                        cboNhanVien.Items.Clear();

                        foreach (DataRow row in dt.Rows)
                        {
                            cboNhanVien.Items.Add(
                                row["MaNhanVien"].ToString()
                                + " - "
                                + row["TenNhanVien"].ToString());
                        }

                        if (cboNhanVien.Items.Count > 0)
                        {
                            cboNhanVien.SelectedIndex = 0;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Không thể tải danh sách nhân viên.\n\n" + ex.Message,
                    "Lỗi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void LoadPhieuMuon()
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
                    pm.MaNhanVien + ' - ' + nv.TenNhanVien AS NhanVien,
                    pm.NgayMuon,
                    pm.HanTra,
                    s.TenSach
                FROM PhieuMuon pm
                INNER JOIN DocGia dg
                    ON pm.MaDocGia = dg.MaDocGia
                INNER JOIN NhanVien nv
                    ON pm.MaNhanVien = nv.MaNhanVien
                INNER JOIN Sach s
                    ON pm.MaSach = s.MaSach
                ORDER BY pm.MaPhieu";

                    using (SqlDataAdapter adapter =
                        new SqlDataAdapter(sql, conn))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);

                        dgvPhieuMuon.Rows.Clear();

                        foreach (DataRow row in dt.Rows)
                        {
                            dgvPhieuMuon.Rows.Add(
                                row["MaPhieu"].ToString(),
                                row["DocGia"].ToString(),
                                row["NhanVien"].ToString(),
                                Convert.ToDateTime(
                                    row["NgayMuon"]).ToString("dd/MM/yyyy"),
                                Convert.ToDateTime(
                                    row["HanTra"]).ToString("dd/MM/yyyy"),
                                row["TenSach"].ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Không thể tải dữ liệu phiếu mượn.\n\n" + ex.Message,
                    "Lỗi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            string maPhieu = txtMaPhieu.Text.Trim();
            string docGia = cboDocGia.Text.Trim();
            string nhanVien = cboNhanVien.Text.Trim();
            if (cboSach.SelectedIndex == -1)
            {
                MessageBox.Show(
                    "Vui lòng chọn sách.",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                cboSach.Focus();
                return;
            }

            string maSach = cboSach.SelectedValue.ToString();

            if (maPhieu == "")
            {
                MessageBox.Show(
                    "Vui lòng nhập mã phiếu.",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                txtMaPhieu.Focus();
                return;
            }

            if (docGia == "")
            {
                MessageBox.Show(
                    "Vui lòng chọn độc giả.",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                cboDocGia.Focus();
                return;
            }

            if (nhanVien == "")
            {
                MessageBox.Show(
                    "Vui lòng chọn nhân viên.",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                cboNhanVien.Focus();
                return;
            }


            if (dtpHanTra.Value.Date < dtpNgayMuon.Value.Date)
            {
                MessageBox.Show(
                    "Hạn trả không được trước ngày mượn.",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            string maDocGia = docGia.Split('-')[0].Trim();
            string maNhanVien = nhanVien.Split('-')[0].Trim();

            try
            {
                using (SqlConnection conn = DbConnection.GetConnection())
                {
                    conn.Open();

                    string sql = @"
    INSERT INTO PhieuMuon
        (MaPhieu, MaDocGia, MaNhanVien, MaSach, NgayMuon, HanTra)
    VALUES
        (@MaPhieu, @MaDocGia, @MaNhanVien, @MaSach, @NgayMuon, @HanTra)";

                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@MaPhieu", maPhieu);
                        cmd.Parameters.AddWithValue("@MaDocGia", maDocGia);
                        cmd.Parameters.AddWithValue("@MaNhanVien", maNhanVien);
                        cmd.Parameters.AddWithValue("@MaSach", maSach);
                        cmd.Parameters.AddWithValue(
                            "@NgayMuon",
                            dtpNgayMuon.Value.Date);
                        cmd.Parameters.AddWithValue(
                            "@HanTra",
                            dtpHanTra.Value.Date);

                        int soDong = cmd.ExecuteNonQuery();

                        if (soDong <= 0)
                        {
                            MessageBox.Show(
                                "Không thể thêm phiếu mượn.",
                                "Thông báo",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);

                            return;
                        }
                    }
                }

                MessageBox.Show(
                    "Thêm phiếu mượn thành công.",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                LoadPhieuMuon();
                CapNhatThongKeTrangChu();

                txtMaPhieu.Clear();
                cboSach.SelectedIndex = -1;

                if (cboDocGia.Items.Count > 0)
                    cboDocGia.SelectedIndex = 0;

                if (cboNhanVien.Items.Count > 0)
                    cboNhanVien.SelectedIndex = 0;

                dtpNgayMuon.Value = DateTime.Now;
                dtpHanTra.Value = DateTime.Now.AddDays(7);

                dongDangChon = -1;
                dgvPhieuMuon.ClearSelection();

                txtMaPhieu.Focus();
            }
            catch (SqlException ex)
            {
                if (ex.Number == 2627 || ex.Number == 2601)
                {
                    MessageBox.Show(
                        "Mã phiếu đã tồn tại. Vui lòng nhập mã khác.",
                        "Thông báo",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                }
                else if (ex.Number == 547)
                {
                    MessageBox.Show(
                        "Dữ liệu độc giả, nhân viên hoặc sách không hợp lệ.",
                        "Thông báo",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                }
                else
                {
                    MessageBox.Show(
                        "Không thể thêm phiếu mượn.\n\n" + ex.Message,
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

        private void dgvPhieuMuon_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            DataGridViewRow row = dgvPhieuMuon.Rows[e.RowIndex];

            txtMaPhieu.Text =
                row.Cells["colMaPhieu"].Value?.ToString() ?? "";

            cboDocGia.Text =
                row.Cells["colDocGia"].Value?.ToString() ?? "";

            cboNhanVien.Text =
                row.Cells["colNhanVien"].Value?.ToString() ?? "";

            string ngayMuon =
                row.Cells["colNgayMuon"].Value?.ToString() ?? "";

            string hanTra =
                row.Cells["colHanTra"].Value?.ToString() ?? "";

            string maPhieu = row.Cells["colMaPhieu"].Value?.ToString() ?? "";

            using (SqlConnection conn = DbConnection.GetConnection())
            {
                conn.Open();

                string sql = "SELECT MaSach FROM PhieuMuon WHERE MaPhieu = @MaPhieu";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@MaPhieu", maPhieu);

                    object result = cmd.ExecuteScalar();

                    if (result != null)
                        cboSach.SelectedValue = result.ToString();
                    else
                        cboSach.SelectedIndex = -1;
                }
            }

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

            dongDangChon = e.RowIndex;
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (dongDangChon < 0 ||
    dongDangChon >= dgvPhieuMuon.Rows.Count)
            {
                MessageBox.Show(
                    "Vui lòng chọn phiếu mượn cần sửa.",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            string maPhieu = txtMaPhieu.Text.Trim();
            string docGia = cboDocGia.Text.Trim();
            string nhanVien = cboNhanVien.Text.Trim();
            if (cboSach.SelectedIndex == -1)
            {
                MessageBox.Show("Vui lòng chọn sách.");
                cboSach.Focus();
                return;
            }

            string maSach = cboSach.SelectedValue.ToString();

            if (maPhieu == "")
            {
                MessageBox.Show(
                    "Vui lòng nhập mã phiếu.",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                txtMaPhieu.Focus();
                return;
            }

            if (docGia == "")
            {
                MessageBox.Show(
                    "Vui lòng chọn độc giả.",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                cboDocGia.Focus();
                return;
            }

            if (nhanVien == "")
            {
                MessageBox.Show(
                    "Vui lòng chọn nhân viên.",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                cboNhanVien.Focus();
                return;
            }

            if (dtpHanTra.Value.Date < dtpNgayMuon.Value.Date)
            {
                MessageBox.Show(
                    "Hạn trả không được trước ngày mượn.",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            DataGridViewRow row = dgvPhieuMuon.Rows[dongDangChon];

            string maPhieuCu =
                row.Cells["colMaPhieu"].Value?.ToString() ?? "";

            if (maPhieuCu == "")
            {
                MessageBox.Show(
                    "Không xác định được mã phiếu cần sửa.",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            string maDocGia = docGia.Split('-')[0].Trim();
            string maNhanVien = nhanVien.Split('-')[0].Trim();

            try
            {
                using (SqlConnection conn = DbConnection.GetConnection())
                {
                    conn.Open();

                    string sql = @"
                UPDATE PhieuMuon
                SET
                    MaPhieu = @MaPhieuMoi,
                    MaDocGia = @MaDocGia,
                    MaNhanVien = @MaNhanVien,
                    MaSach = @MaSach,
                    NgayMuon = @NgayMuon,
                    HanTra = @HanTra
                WHERE MaPhieu = @MaPhieuCu";

                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue(
                            "@MaPhieuMoi", maPhieu);

                        cmd.Parameters.AddWithValue(
                            "@MaDocGia", maDocGia);

                        cmd.Parameters.AddWithValue(
                            "@MaNhanVien", maNhanVien);

                        cmd.Parameters.AddWithValue("@MaSach", maSach);

                        cmd.Parameters.AddWithValue(
                            "@NgayMuon", dtpNgayMuon.Value.Date);

                        cmd.Parameters.AddWithValue(
                            "@HanTra", dtpHanTra.Value.Date);

                        cmd.Parameters.AddWithValue(
                            "@MaPhieuCu", maPhieuCu);

                        int soDong = cmd.ExecuteNonQuery();

                        if (soDong == 0)
                        {
                            MessageBox.Show(
                                "Không tìm thấy phiếu mượn cần sửa.",
                                "Thông báo",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);

                            return;
                        }
                    }
                }

                MessageBox.Show(
                    "Sửa phiếu mượn thành công.",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                LoadPhieuMuon();
                CapNhatThongKeTrangChu();

                txtMaPhieu.Clear();
                cboSach.SelectedIndex = -1;

                if (cboDocGia.Items.Count > 0)
                    cboDocGia.SelectedIndex = 0;

                if (cboNhanVien.Items.Count > 0)
                    cboNhanVien.SelectedIndex = 0;

                dtpNgayMuon.Value = DateTime.Now;
                dtpHanTra.Value = DateTime.Now.AddDays(7);

                dongDangChon = -1;
                dgvPhieuMuon.ClearSelection();

                txtMaPhieu.Focus();
            }
            catch (SqlException ex)
            {
                if (ex.Number == 2627 || ex.Number == 2601)
                {
                    MessageBox.Show(
                        "Mã phiếu mới đã tồn tại. Vui lòng nhập mã khác.",
                        "Thông báo",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                }
                else if (ex.Number == 547)
                {
                    MessageBox.Show(
                        "Dữ liệu độc giả, nhân viên hoặc sách không hợp lệ.",
                        "Thông báo",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                }
                else
                {
                    MessageBox.Show(
                        "Không thể sửa phiếu mượn.\n\n" + ex.Message,
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
            if (dongDangChon < 0 ||
    dongDangChon >= dgvPhieuMuon.Rows.Count)
            {
                MessageBox.Show(
                    "Vui lòng chọn phiếu mượn cần xóa.",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            DataGridViewRow row = dgvPhieuMuon.Rows[dongDangChon];

            string maPhieu =
                row.Cells["colMaPhieu"].Value?.ToString() ?? "";

            if (maPhieu == "")
            {
                MessageBox.Show(
                    "Không xác định được mã phiếu cần xóa.",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            DialogResult result = MessageBox.Show(
                "Bạn có chắc chắn muốn xóa phiếu mượn này không?",
                "Xác nhận xóa",
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
                DELETE FROM PhieuMuon
                WHERE MaPhieu = @MaPhieu";

                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue(
                            "@MaPhieu", maPhieu);

                        int soDong = cmd.ExecuteNonQuery();

                        if (soDong == 0)
                        {
                            MessageBox.Show(
                                "Không tìm thấy phiếu mượn cần xóa.",
                                "Thông báo",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);

                            return;
                        }
                    }
                }

                MessageBox.Show(
                    "Xóa phiếu mượn thành công.",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                LoadPhieuMuon();

                txtMaPhieu.Clear();
                cboSach.SelectedIndex = -1;

                if (cboDocGia.Items.Count > 0)
                    cboDocGia.SelectedIndex = 0;

                if (cboNhanVien.Items.Count > 0)
                    cboNhanVien.SelectedIndex = 0;

                dtpNgayMuon.Value = DateTime.Now;
                dtpHanTra.Value = DateTime.Now.AddDays(7);

                dongDangChon = -1;

                dgvPhieuMuon.ClearSelection();

                txtMaPhieu.Focus();
            }
            catch (SqlException ex)
            {
                if (ex.Number == 547)
                {
                    MessageBox.Show(
                        "Không thể xóa phiếu mượn vì dữ liệu đang được tham chiếu.",
                        "Không thể xóa",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                }
                else
                {
                    MessageBox.Show(
                        "Không thể xóa phiếu mượn.\n\n" + ex.Message,
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

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            txtMaPhieu.Clear();
            cboSach.SelectedIndex = -1;

            if (cboDocGia.Items.Count > 0)
                cboDocGia.SelectedIndex = 0;

            if (cboNhanVien.Items.Count > 0)
                cboNhanVien.SelectedIndex = 0;

            dtpNgayMuon.Value = DateTime.Now;
            dtpHanTra.Value = DateTime.Now.AddDays(7);

            LoadPhieuMuon();

            dongDangChon = -1;
            dgvPhieuMuon.ClearSelection();
            dgvPhieuMuon.CurrentCell = null;

            txtMaPhieu.Focus();
        }


        private void btnLapPhieu_Click(object sender, EventArgs e)
        {
            string maPhieu = txtMaPhieu.Text.Trim();
            string docGia = cboDocGia.Text.Trim();
            string nhanVien = cboNhanVien.Text.Trim();

            // 1. Kiểm tra mã phiếu
            if (maPhieu == "")
            {
                MessageBox.Show(
                    "Vui lòng nhập mã phiếu.",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                txtMaPhieu.Focus();
                return;
            }

            // 2. Kiểm tra độc giả
            if (cboDocGia.SelectedIndex == -1 || docGia == "")
            {
                MessageBox.Show(
                    "Vui lòng chọn độc giả.",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                cboDocGia.Focus();
                return;
            }

            // 3. Kiểm tra nhân viên
            if (cboNhanVien.SelectedIndex == -1 || nhanVien == "")
            {
                MessageBox.Show(
                    "Vui lòng chọn nhân viên.",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                cboNhanVien.Focus();
                return;
            }

            // 4. Kiểm tra sách
            if (cboSach.SelectedIndex == -1 || cboSach.SelectedValue == null)
            {
                MessageBox.Show(
                    "Vui lòng chọn sách.",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                cboSach.Focus();
                return;
            }

            // Lấy mã độc giả, nhân viên và sách
            string maDocGia = docGia.Split('-')[0].Trim();
            string maNhanVien = nhanVien.Split('-')[0].Trim();
            string maSach = cboSach.SelectedValue.ToString();

            // 5. Kiểm tra ngày mượn và hạn trả
            if (dtpHanTra.Value.Date < dtpNgayMuon.Value.Date)
            {
                MessageBox.Show(
                    "Hạn trả không được trước ngày mượn.",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                dtpHanTra.Focus();
                return;
            }

            try
            {
                using (SqlConnection conn = DbConnection.GetConnection())
                {
                    conn.Open();

                    // 6. Thêm phiếu mượn bằng mã sách đã chọn
                    string sql = @"
                INSERT INTO PhieuMuon
                    (MaPhieu, MaDocGia, MaNhanVien, MaSach,
                     NgayMuon, HanTra)
                VALUES
                    (@MaPhieu, @MaDocGia, @MaNhanVien, @MaSach,
                     @NgayMuon, @HanTra)";

                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@MaPhieu", maPhieu);
                        cmd.Parameters.AddWithValue("@MaDocGia", maDocGia);
                        cmd.Parameters.AddWithValue("@MaNhanVien", maNhanVien);
                        cmd.Parameters.AddWithValue("@MaSach", maSach);
                        cmd.Parameters.AddWithValue(
                            "@NgayMuon", dtpNgayMuon.Value.Date);
                        cmd.Parameters.AddWithValue(
                            "@HanTra", dtpHanTra.Value.Date);

                        int soDong = cmd.ExecuteNonQuery();

                        if (soDong <= 0)
                        {
                            MessageBox.Show(
                                "Không thể lập phiếu mượn.",
                                "Thông báo",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);

                            return;
                        }
                    }
                }

                // 7. Thông báo thành công
                MessageBox.Show(
                    "Lập phiếu mượn thành công.",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                // 8. Cập nhật lại danh sách phiếu mượn
                LoadPhieuMuon();

                // 9. Làm sạch dữ liệu sau khi lập phiếu
                txtMaPhieu.Clear();
                cboSach.SelectedIndex = -1;

                if (cboDocGia.Items.Count > 0)
                    cboDocGia.SelectedIndex = 0;

                if (cboNhanVien.Items.Count > 0)
                    cboNhanVien.SelectedIndex = 0;

                dtpNgayMuon.Value = DateTime.Now;
                dtpHanTra.Value = DateTime.Now.AddDays(7);

                dongDangChon = -1;
                dgvPhieuMuon.ClearSelection();

                txtMaPhieu.Focus();
            }
            catch (SqlException ex)
            {
                // Trùng khóa chính hoặc khóa duy nhất
                if (ex.Number == 2627 || ex.Number == 2601)
                {
                    MessageBox.Show(
                        "Mã phiếu đã tồn tại. Vui lòng nhập mã khác.",
                        "Thông báo",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    txtMaPhieu.Focus();
                }
                // Vi phạm khóa ngoại
                else if (ex.Number == 547)
                {
                    MessageBox.Show(
                        "Độc giả, nhân viên hoặc sách không hợp lệ. " +
                        "Vui lòng kiểm tra lại dữ liệu.",
                        "Thông báo",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                }
                else
                {
                    MessageBox.Show(
                        "Không thể lập phiếu mượn.\n\n" + ex.Message,
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

    }
}