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

            HienThiDuLieuMau();
        }

        private void HienThiDuLieuMau()
        {
            dgvTacGia.Rows.Clear();

            dgvTacGia.Rows.Add("TG001", "Nguyễn Văn A");
            dgvTacGia.Rows.Add("TG002", "Trần Văn B");
            dgvTacGia.Rows.Add("TG003", "Lê Văn C");
            dgvTacGia.Rows.Add("TG004", "Phạm Văn D");
            dgvTacGia.Rows.Add("TG005", "Nguyễn Văn E");
            dgvTacGia.Rows.Add("TG006", "Trần Văn F");
            dgvTacGia.Rows.Add("TG007", "Lê Văn G");
            dgvTacGia.Rows.Add("TG008", "Phạm Văn H");
            dgvTacGia.Rows.Add("TG009", "Nguyễn Văn I");
            dgvTacGia.Rows.Add("TG010", "Trần Văn K");
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            string maTacGia = txtMaTacGia.Text.Trim();
            string tenTacGia = txtTenTacGia.Text.Trim();

            if (maTacGia == "")
            {
                MessageBox.Show(
                    "Vui lòng nhập mã tác giả.",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                txtMaTacGia.Focus();
                return;
            }

            if (tenTacGia == "")
            {
                MessageBox.Show(
                    "Vui lòng nhập tên tác giả.",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                txtTenTacGia.Focus();
                return;
            }

            foreach (DataGridViewRow row in dgvTacGia.Rows)
            {
                if (row.IsNewRow)
                    continue;

                string maCu = row.Cells["colMaTacGia"].Value?.ToString() ?? "";

                if (maCu.Equals(maTacGia, StringComparison.OrdinalIgnoreCase))
                {
                    MessageBox.Show(
                        "Mã tác giả đã tồn tại.",
                        "Thông báo",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    txtMaTacGia.Focus();
                    return;
                }
            }

            dgvTacGia.Rows.Add(maTacGia, tenTacGia);

            MessageBox.Show(
                "Thêm tác giả thành công.",
                "Thông báo",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);

            txtMaTacGia.Clear();
            txtTenTacGia.Clear();

            txtMaTacGia.Focus();
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
            if (dongDangChon == -1)
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

            if (maTacGia == "")
            {
                MessageBox.Show(
                    "Vui lòng nhập mã tác giả.",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                txtMaTacGia.Focus();
                return;
            }

            if (tenTacGia == "")
            {
                MessageBox.Show(
                    "Vui lòng nhập tên tác giả.",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                txtTenTacGia.Focus();
                return;
            }

            DataGridViewRow row = dgvTacGia.Rows[dongDangChon];

            row.Cells["colMaTacGia"].Value = maTacGia;
            row.Cells["colTenTacGia"].Value = tenTacGia;

            MessageBox.Show(
                "Sửa thông tin tác giả thành công.",
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
                    "Vui lòng chọn tác giả cần xóa.",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            DialogResult result = MessageBox.Show(
                "Bạn có chắc chắn muốn xóa tác giả này không?",
                "Xác nhận xóa",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.No)
            {
                return;
            }

            dgvTacGia.Rows.RemoveAt(dongDangChon);

            dongDangChon = -1;

            MessageBox.Show(
                "Xóa tác giả thành công.",
                "Thông báo",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
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

            bool timThay = false;

            foreach (DataGridViewRow row in dgvTacGia.Rows)
            {
                if (row.IsNewRow)
                    continue;

                string maTacGia = row.Cells["colMaTacGia"].Value?.ToString() ?? "";
                string tenTacGia = row.Cells["colTenTacGia"].Value?.ToString() ?? "";

                if (maTacGia.IndexOf(tuKhoa, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    tenTacGia.IndexOf(tuKhoa, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    row.Selected = true;
                    dgvTacGia.CurrentCell = row.Cells["colMaTacGia"];

                    txtMaTacGia.Text = maTacGia;
                    txtTenTacGia.Text = tenTacGia;

                    dongDangChon = row.Index;
                    timThay = true;

                    break;
                }
            }

            if (!timThay)
            {
                MessageBox.Show(
                    "Không tìm thấy tác giả phù hợp.",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            txtMaTacGia.Clear();
            txtTenTacGia.Clear();

            dongDangChon = -1;

            dgvTacGia.ClearSelection();

            txtMaTacGia.Focus();
        }
    }
}
