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

            HienThiDuLieuMau();
        }

        private void HienThiDuLieuMau()
        {
            dgvTheLoai.Rows.Clear();

            dgvTheLoai.Rows.Add("TL001", "Công nghệ thông tin");
            dgvTheLoai.Rows.Add("TL002", "Giáo dục");
            dgvTheLoai.Rows.Add("TL003", "Toán học");
            dgvTheLoai.Rows.Add("TL004", "Kỹ năng");
            dgvTheLoai.Rows.Add("TL005", "Ngoại ngữ");
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            string maTheLoai = txtMaTheLoai.Text.Trim();
            string tenTheLoai = txtTenTheLoai.Text.Trim();

            if (maTheLoai == "")
            {
                MessageBox.Show(
                    "Vui lòng nhập mã thể loại.",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                txtMaTheLoai.Focus();
                return;
            }

            if (tenTheLoai == "")
            {
                MessageBox.Show(
                    "Vui lòng nhập tên thể loại.",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                txtTenTheLoai.Focus();
                return;
            }

            foreach (DataGridViewRow row in dgvTheLoai.Rows)
            {
                if (row.IsNewRow)
                    continue;

                string maCu = row.Cells["colMaTheLoai"].Value?.ToString() ?? "";

                if (maCu.Equals(maTheLoai, StringComparison.OrdinalIgnoreCase))
                {
                    MessageBox.Show(
                        "Mã thể loại đã tồn tại.",
                        "Thông báo",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    txtMaTheLoai.Focus();
                    return;
                }
            }

            dgvTheLoai.Rows.Add(maTheLoai, tenTheLoai);

            MessageBox.Show(
                "Thêm thể loại thành công.",
                "Thông báo",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);

            txtMaTheLoai.Clear();
            txtTenTheLoai.Clear();

            txtMaTheLoai.Focus();
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (dongDangChon == -1)
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

            if (maTheLoai == "")
            {
                MessageBox.Show(
                    "Vui lòng nhập mã thể loại.",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                txtMaTheLoai.Focus();
                return;
            }

            if (tenTheLoai == "")
            {
                MessageBox.Show(
                    "Vui lòng nhập tên thể loại.",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                txtTenTheLoai.Focus();
                return;
            }

            DataGridViewRow row = dgvTheLoai.Rows[dongDangChon];

            row.Cells["colMaTheLoai"].Value = maTheLoai;
            row.Cells["colTenTheLoai"].Value = tenTheLoai;

            MessageBox.Show(
                "Sửa thông tin thể loại thành công.",
                "Thông báo",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);

            dongDangChon = -1;
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
            if (dongDangChon == -1)
            {
                MessageBox.Show(
                    "Vui lòng chọn thể loại cần xóa.",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            DialogResult result = MessageBox.Show(
                "Bạn có chắc chắn muốn xóa thể loại này không?",
                "Xác nhận xóa",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.No)
            {
                return;
            }

            dgvTheLoai.Rows.RemoveAt(dongDangChon);

            dongDangChon = -1;

            MessageBox.Show(
                "Xóa thể loại thành công.",
                "Thông báo",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
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

            bool timThay = false;

            foreach (DataGridViewRow row in dgvTheLoai.Rows)
            {
                if (row.IsNewRow)
                    continue;

                string maTheLoai = row.Cells["colMaTheLoai"].Value?.ToString() ?? "";
                string tenTheLoai = row.Cells["colTenTheLoai"].Value?.ToString() ?? "";

                if (maTheLoai.IndexOf(tuKhoa, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    tenTheLoai.IndexOf(tuKhoa, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    row.Selected = true;
                    dgvTheLoai.CurrentCell = row.Cells["colMaTheLoai"];

                    txtMaTheLoai.Text = maTheLoai;
                    txtTenTheLoai.Text = tenTheLoai;

                    dongDangChon = row.Index;
                    timThay = true;

                    break;
                }
            }

            if (!timThay)
            {
                MessageBox.Show(
                    "Không tìm thấy thể loại phù hợp.",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            txtMaTheLoai.Clear();
            txtTenTheLoai.Clear();

            dongDangChon = -1;

            dgvTheLoai.ClearSelection();

            txtMaTheLoai.Focus();
        }
    }
}
