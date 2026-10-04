using System;
using System.Windows.Forms;

namespace DoAnQLTV
{
    public partial class FrmSach : Form
    {
        private int dongDangChon = -1;
        public FrmSach()
        {
            InitializeComponent();

            KhoiTaoTheLoai();
            KhoiTaoTacGia();
            HienThiDuLieuMau();
        }

        private void KhoiTaoTheLoai()
        {
            cboTheLoai.Items.Clear();

            cboTheLoai.Items.Add("Công nghệ thông tin");
            cboTheLoai.Items.Add("Giáo dục");
            cboTheLoai.Items.Add("Toán học");
            cboTheLoai.Items.Add("Kỹ năng");
            cboTheLoai.Items.Add("Ngoại ngữ");
        }

        private void KhoiTaoTacGia()
        {
            cboTacGia.Items.Clear();

            cboTacGia.Items.Add("Nguyễn Văn A");
            cboTacGia.Items.Add("Trần Văn B");
            cboTacGia.Items.Add("Lê Văn C");
            cboTacGia.Items.Add("Phạm Văn D");
            cboTacGia.Items.Add("Nguyễn Văn E");
            cboTacGia.Items.Add("Trần Văn F");
            cboTacGia.Items.Add("Lê Văn G");
            cboTacGia.Items.Add("Phạm Văn H");
            cboTacGia.Items.Add("Nguyễn Văn I");
            cboTacGia.Items.Add("Trần Văn K");
            cboTacGia.Items.Add("Lê Văn L");
            cboTacGia.Items.Add("Phạm Văn M");
            cboTacGia.Items.Add("Nguyễn Văn N");
            cboTacGia.Items.Add("Trần Văn P");
            cboTacGia.Items.Add("Lê Văn Q");
        }

        private void HienThiDuLieuMau()
        {
            dgvSach.Rows.Clear();

            dgvSach.Rows.Add(
                "S001",
                "Lập trình C#",
                "Công nghệ thông tin",
                "Nguyễn Văn A");

            dgvSach.Rows.Add(
                "S002",
                "Cơ sở dữ liệu",
                "Công nghệ thông tin",
                "Trần Văn B");

            dgvSach.Rows.Add(
                "S003",
                "Kỹ năng học tập",
                "Giáo dục",
                "Lê Văn C");

            dgvSach.Rows.Add(
                "S004",
                "Lập trình hướng đối tượng",
                "Công nghệ thông tin",
                "Phạm Văn D");

            dgvSach.Rows.Add(
                "S005",
                "Cấu trúc dữ liệu và giải thuật",
                "Công nghệ thông tin",
                "Nguyễn Văn E");

            dgvSach.Rows.Add(
                "S006",
                "Hệ điều hành",
                "Công nghệ thông tin",
                "Trần Văn F");

            dgvSach.Rows.Add(
                "S007",
                "Mạng máy tính",
                "Công nghệ thông tin",
                "Lê Văn G");

            dgvSach.Rows.Add(
                "S008",
                "Phân tích và thiết kế hệ thống",
                "Công nghệ thông tin",
                "Phạm Văn H");

            dgvSach.Rows.Add(
                "S009",
                "Toán cao cấp",
                "Toán học",
                "Nguyễn Văn I");

            dgvSach.Rows.Add(
                "S010",
                "Xác suất thống kê",
                "Toán học",
                "Trần Văn K");

            dgvSach.Rows.Add(
                "S011",
                "Kỹ năng giao tiếp",
                "Kỹ năng",
                "Lê Văn L");

            dgvSach.Rows.Add(
                "S012",
                "Tiếng Anh chuyên ngành CNTT",
                "Ngoại ngữ",
                "Phạm Văn M");

            dgvSach.Rows.Add(
                "S013",
                "Lập trình Windows Forms",
                "Công nghệ thông tin",
                "Nguyễn Văn N");

            dgvSach.Rows.Add(
                "S014",
                "Phát triển ứng dụng C#",
                "Công nghệ thông tin",
                "Trần Văn P");

            dgvSach.Rows.Add(
                "S015",
                "Nhập môn lập trình",
                "Công nghệ thông tin",
                "Lê Văn Q");
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            string maSach = txtMaSach.Text.Trim();
            string tenSach = txtTenSach.Text.Trim();
            string theLoai = cboTheLoai.Text.Trim();
            string tacGia = cboTacGia.Text.Trim();

            // Kiểm tra mã sách
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

            // Kiểm tra tên sách
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

            // Kiểm tra thể loại
            if (theLoai == "")
            {
                MessageBox.Show(
                    "Vui lòng chọn thể loại.",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                cboTheLoai.Focus();
                return;
            }

            // Kiểm tra tác giả
            if (tacGia == "")
            {
                MessageBox.Show(
                    "Vui lòng chọn tác giả.",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                cboTacGia.Focus();
                return;
            }

            // Kiểm tra mã sách trùng
            foreach (DataGridViewRow row in dgvSach.Rows)
            {
                if (row.IsNewRow)
                    continue;

                if (row.Cells["colMaSach"].Value != null &&
                    row.Cells["colMaSach"].Value.ToString()
                    .Equals(maSach, StringComparison.OrdinalIgnoreCase))
                {
                    MessageBox.Show(
                        "Mã sách đã tồn tại. Vui lòng nhập mã khác.",
                        "Thông báo",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    txtMaSach.Focus();
                    return;
                }
            }

            // Thêm sách vào DataGridView
            dgvSach.Rows.Add(
                maSach,
                tenSach,
                theLoai,
                tacGia);

            MessageBox.Show(
                "Thêm sách thành công.",
                "Thông báo",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);

            // Xóa dữ liệu nhập
            txtMaSach.Clear();
            txtTenSach.Clear();
            cboTheLoai.SelectedIndex = -1;
            cboTacGia.SelectedIndex = -1;

            txtMaSach.Focus();
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
            string theLoai = cboTheLoai.Text.Trim();
            string tacGia = cboTacGia.Text.Trim();

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

            if (theLoai == "")
            {
                MessageBox.Show(
                    "Vui lòng chọn thể loại.",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                cboTheLoai.Focus();
                return;
            }

            if (tacGia == "")
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

            row.Cells["colMaSach"].Value = maSach;
            row.Cells["colTenSach"].Value = tenSach;
            row.Cells["colTheLoai"].Value = theLoai;
            row.Cells["colTacGia"].Value = tacGia;

            MessageBox.Show(
                "Sửa thông tin sách thành công.",
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
                    "Vui lòng chọn sách cần xóa.",
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

            dgvSach.Rows.RemoveAt(dongDangChon);

            dongDangChon = -1;

            MessageBox.Show(
                "Xóa sách thành công.",
                "Thông báo",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            string tuKhoa = txtTenSach.Text.Trim();
            bool timThay = false;

            foreach (DataGridViewRow row in dgvSach.Rows)
            {
                if (row.IsNewRow)
                    continue;

                string maSach = row.Cells["colMaSach"].Value?.ToString() ?? "";
                string tenSach = row.Cells["colTenSach"].Value?.ToString() ?? "";

                if (maSach.IndexOf(tuKhoa, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    tenSach.IndexOf(tuKhoa, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    timThay = true;

                    row.Selected = true;
                    dgvSach.CurrentCell = row.Cells["colMaSach"];
                    return;
                }
            }

            if (!timThay)
            {
                MessageBox.Show(
                    "Không tìm thấy sách phù hợp.",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            txtMaSach.Clear();
            txtTenSach.Clear();
            cboTheLoai.SelectedIndex = -1;
            cboTacGia.SelectedIndex = -1;

            dongDangChon = -1;
            dgvSach.ClearSelection();
        }
    }
}