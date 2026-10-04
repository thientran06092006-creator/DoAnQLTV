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
    public partial class FrmDocGia : Form
    {
        private int dongDangChon = -1;
        public FrmDocGia()
        {
            InitializeComponent();

            HienThiDuLieuMau();
        }

        private void HienThiDuLieuMau()
        {
            dgvDocGia.Rows.Clear();

            dgvDocGia.Rows.Add(
                "DG001",
                "Nguyễn Văn An",
                "01/01/2004",
                "TP. Hồ Chí Minh",
                "0900000001");

            dgvDocGia.Rows.Add(
                "DG002",
                "Trần Thị Bình",
                "15/03/2004",
                "TP. Hồ Chí Minh",
                "0900000002");

            dgvDocGia.Rows.Add(
                "DG003",
                "Lê Văn Cường",
                "20/05/2003",
                "Đồng Nai",
                "0900000003");

            dgvDocGia.Rows.Add(
                "DG004",
                "Phạm Thị Dung",
                "10/08/2004",
                "Bình Dương",
                "0900000004");

            dgvDocGia.Rows.Add(
                "DG005",
                "Hoàng Văn Em",
                "25/12/2003",
                "Long An",
                "0900000005");
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            string maDocGia = txtMaDocGia.Text.Trim();
            string tenDocGia = txtTenDocGia.Text.Trim();
            string ngaySinh = dtpNgaySinh.Value.ToString("dd/MM/yyyy");
            string diaChi = txtDiaChi.Text.Trim();
            string soDienThoai = txtSoDienThoai.Text.Trim();

            if (maDocGia == "")
            {
                MessageBox.Show(
                    "Vui lòng nhập mã độc giả.",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                txtMaDocGia.Focus();
                return;
            }

            if (tenDocGia == "")
            {
                MessageBox.Show(
                    "Vui lòng nhập tên độc giả.",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                txtTenDocGia.Focus();
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

            foreach (DataGridViewRow row in dgvDocGia.Rows)
            {
                if (row.IsNewRow)
                    continue;

                string maCu = row.Cells["colMaDocGia"].Value?.ToString() ?? "";

                if (maCu.Equals(maDocGia, StringComparison.OrdinalIgnoreCase))
                {
                    MessageBox.Show(
                        "Mã độc giả đã tồn tại.",
                        "Thông báo",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    txtMaDocGia.Focus();
                    return;
                }
            }

            dgvDocGia.Rows.Add(
                maDocGia,
                tenDocGia,
                ngaySinh,
                diaChi,
                soDienThoai);

            MessageBox.Show(
                "Thêm độc giả thành công.",
                "Thông báo",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);

            txtMaDocGia.Clear();
            txtTenDocGia.Clear();
            txtDiaChi.Clear();
            txtSoDienThoai.Clear();

            dtpNgaySinh.Value = DateTime.Now;

            txtMaDocGia.Focus();
        }

        private void dgvDocGia_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            dongDangChon = e.RowIndex;

            DataGridViewRow row = dgvDocGia.Rows[e.RowIndex];

            txtMaDocGia.Text = row.Cells["colMaDocGia"].Value?.ToString();
            txtTenDocGia.Text = row.Cells["colTenDocGia"].Value?.ToString();

            DateTime ngaySinh;
            if (DateTime.TryParse(
                row.Cells["colNgaySinh"].Value?.ToString(),
                out ngaySinh))
            {
                dtpNgaySinh.Value = ngaySinh;
            }

            txtDiaChi.Text = row.Cells["colDiaChi"].Value?.ToString();
            txtSoDienThoai.Text = row.Cells["colSoDienThoai"].Value?.ToString();
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (dongDangChon == -1)
            {
                MessageBox.Show(
                    "Vui lòng chọn độc giả cần sửa.",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            string maDocGia = txtMaDocGia.Text.Trim();
            string tenDocGia = txtTenDocGia.Text.Trim();
            string ngaySinh = dtpNgaySinh.Value.ToString("dd/MM/yyyy");
            string diaChi = txtDiaChi.Text.Trim();
            string soDienThoai = txtSoDienThoai.Text.Trim();

            if (maDocGia == "")
            {
                MessageBox.Show(
                    "Vui lòng nhập mã độc giả.",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                txtMaDocGia.Focus();
                return;
            }

            if (tenDocGia == "")
            {
                MessageBox.Show(
                    "Vui lòng nhập tên độc giả.",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                txtTenDocGia.Focus();
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

            DataGridViewRow row = dgvDocGia.Rows[dongDangChon];

            row.Cells["colMaDocGia"].Value = maDocGia;
            row.Cells["colTenDocGia"].Value = tenDocGia;
            row.Cells["colNgaySinh"].Value = ngaySinh;
            row.Cells["colDiaChi"].Value = diaChi;
            row.Cells["colSoDienThoai"].Value = soDienThoai;

            MessageBox.Show(
                "Sửa thông tin độc giả thành công.",
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
                    "Vui lòng chọn độc giả cần xóa.",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            DialogResult result = MessageBox.Show(
                "Bạn có chắc chắn muốn xóa độc giả này không?",
                "Xác nhận xóa",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.No)
            {
                return;
            }

            dgvDocGia.Rows.RemoveAt(dongDangChon);

            dongDangChon = -1;

            MessageBox.Show(
                "Xóa độc giả thành công.",
                "Thông báo",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            string tuKhoa = txtTenDocGia.Text.Trim();

            if (tuKhoa == "")
            {
                MessageBox.Show(
                    "Vui lòng nhập từ khóa cần tìm.",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                txtTenDocGia.Focus();
                return;
            }

            bool timThay = false;

            foreach (DataGridViewRow row in dgvDocGia.Rows)
            {
                if (row.IsNewRow)
                    continue;

                string maDocGia =
                    row.Cells["colMaDocGia"].Value?.ToString() ?? "";

                string tenDocGia =
                    row.Cells["colTenDocGia"].Value?.ToString() ?? "";

                string soDienThoai =
                    row.Cells["colSoDienThoai"].Value?.ToString() ?? "";

                if (maDocGia.IndexOf(
                        tuKhoa,
                        StringComparison.OrdinalIgnoreCase) >= 0 ||
                    tenDocGia.IndexOf(
                        tuKhoa,
                        StringComparison.OrdinalIgnoreCase) >= 0 ||
                    soDienThoai.IndexOf(
                        tuKhoa,
                        StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    row.Selected = true;

                    dgvDocGia.CurrentCell =
                        row.Cells["colMaDocGia"];

                    txtMaDocGia.Text = maDocGia;
                    txtTenDocGia.Text = tenDocGia;

                    DateTime ngaySinh;

                    if (DateTime.TryParse(
                        row.Cells["colNgaySinh"].Value?.ToString(),
                        out ngaySinh))
                    {
                        dtpNgaySinh.Value = ngaySinh;
                    }

                    txtDiaChi.Text =
                        row.Cells["colDiaChi"].Value?.ToString();

                    txtSoDienThoai.Text = soDienThoai;

                    dongDangChon = row.Index;

                    timThay = true;

                    break;
                }
            }

            if (!timThay)
            {
                MessageBox.Show(
                    "Không tìm thấy độc giả phù hợp.",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            txtMaDocGia.Clear();
            txtTenDocGia.Clear();
            txtDiaChi.Clear();
            txtSoDienThoai.Clear();

            dtpNgaySinh.Value = DateTime.Now;

            dongDangChon = -1;

            dgvDocGia.ClearSelection();

            txtMaDocGia.Focus();
        }
    }
}
