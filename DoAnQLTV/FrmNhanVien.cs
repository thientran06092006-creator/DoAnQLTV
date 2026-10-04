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
    public partial class FrmNhanVien : Form
    {
        public FrmNhanVien()
        {
            InitializeComponent();

            HienThiDuLieuMau();
        }

        private void HienThiDuLieuMau()
        {
            dgvNhanVien.Rows.Clear();

            dgvNhanVien.Rows.Add(
                "NV001",
                "Nguyễn Văn Minh",
                "0900000011",
                "TP. Hồ Chí Minh");

            dgvNhanVien.Rows.Add(
                "NV002",
                "Trần Thị Lan",
                "0900000012",
                "TP. Hồ Chí Minh");

            dgvNhanVien.Rows.Add(
                "NV003",
                "Lê Văn Hùng",
                "0900000013",
                "Đồng Nai");

            dgvNhanVien.Rows.Add(
                "NV004",
                "Phạm Thị Mai",
                "0900000014",
                "Bình Dương");

            dgvNhanVien.Rows.Add(
                "NV005",
                "Hoàng Văn Nam",
                "0900000015",
                "Long An");
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
    }
}