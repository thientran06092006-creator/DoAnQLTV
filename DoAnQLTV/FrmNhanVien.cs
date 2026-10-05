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

            foreach (DataGridViewRow row in dgvNhanVien.Rows)
            {
                if (row.IsNewRow)
                    continue;

                string maCu =
                    row.Cells["colMaNhanVien"].Value?.ToString() ?? "";

                if (maCu.Equals(
                    maNhanVien,
                    StringComparison.OrdinalIgnoreCase))
                {
                    MessageBox.Show(
                        "Mã nhân viên đã tồn tại.",
                        "Thông báo",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    txtMaNhanVien.Focus();
                    return;
                }
            }

            dgvNhanVien.Rows.Add(
                maNhanVien,
                tenNhanVien,
                soDienThoai,
                diaChi);

            MessageBox.Show(
                "Thêm nhân viên thành công.",
                "Thông báo",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);

            txtMaNhanVien.Clear();
            txtTenNhanVien.Clear();
            txtSoDienThoai.Clear();
            txtDiaChi.Clear();

            txtMaNhanVien.Focus();
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

            DataGridViewRow row = dgvNhanVien.Rows[dongDangChon];

            row.Cells["colMaNhanVien"].Value = maNhanVien;
            row.Cells["colTenNhanVien"].Value = tenNhanVien;
            row.Cells["colSoDienThoai"].Value = soDienThoai;
            row.Cells["colDiaChi"].Value = diaChi;

            MessageBox.Show(
                "Sửa thông tin nhân viên thành công.",
                "Thông báo",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);

            dongDangChon = -1;
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

            DialogResult result = MessageBox.Show(
                "Bạn có chắc chắn muốn xóa nhân viên này không?",
                "Xác nhận xóa",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.No)
            {
                return;
            }

            dgvNhanVien.Rows.RemoveAt(dongDangChon);

            dongDangChon = -1;

            MessageBox.Show(
                "Xóa nhân viên thành công.",
                "Thông báo",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
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

            bool timThay = false;

            foreach (DataGridViewRow row in dgvNhanVien.Rows)
            {
                if (row.IsNewRow)
                    continue;

                string maNhanVien =
                    row.Cells["colMaNhanVien"].Value?.ToString() ?? "";

                string tenNhanVien =
                    row.Cells["colTenNhanVien"].Value?.ToString() ?? "";

                string soDienThoai =
                    row.Cells["colSoDienThoai"].Value?.ToString() ?? "";

                if (maNhanVien.IndexOf(
                        tuKhoa,
                        StringComparison.OrdinalIgnoreCase) >= 0 ||
                    tenNhanVien.IndexOf(
                        tuKhoa,
                        StringComparison.OrdinalIgnoreCase) >= 0 ||
                    soDienThoai.IndexOf(
                        tuKhoa,
                        StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    row.Selected = true;

                    dgvNhanVien.CurrentCell =
                        row.Cells["colMaNhanVien"];

                    txtMaNhanVien.Text = maNhanVien;
                    txtTenNhanVien.Text = tenNhanVien;
                    txtSoDienThoai.Text = soDienThoai;

                    txtDiaChi.Text =
                        row.Cells["colDiaChi"].Value?.ToString();

                    dongDangChon = row.Index;

                    timThay = true;

                    break;
                }
            }

            if (!timThay)
            {
                MessageBox.Show(
                    "Không tìm thấy nhân viên phù hợp.",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            txtMaNhanVien.Clear();
            txtTenNhanVien.Clear();
            txtSoDienThoai.Clear();
            txtDiaChi.Clear();

            dongDangChon = -1;

            dgvNhanVien.ClearSelection();

            txtMaNhanVien.Focus();
        }
    }
}