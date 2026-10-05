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
    public partial class FrmTimKiemSach : Form
    {
        public FrmTimKiemSach()
        {
            InitializeComponent();

            HienThiDuLieuMau();
        }

        private void HienThiDuLieuMau()
        {
            dgvKetQua.Rows.Clear();

            dgvKetQua.Rows.Add(
                "S001",
                "Lập trình C#",
                "Công nghệ thông tin",
                "Nguyễn Văn A");

            dgvKetQua.Rows.Add(
                "S002",
                "Cơ sở dữ liệu",
                "Công nghệ thông tin",
                "Trần Văn B");

            dgvKetQua.Rows.Add(
                "S003",
                "Kỹ năng học tập",
                "Giáo dục",
                "Lê Văn C");

            dgvKetQua.Rows.Add(
                "S004",
                "Lập trình hướng đối tượng",
                "Công nghệ thông tin",
                "Phạm Văn D");

            dgvKetQua.Rows.Add(
                "S005",
                "Cấu trúc dữ liệu và giải thuật",
                "Công nghệ thông tin",
                "Nguyễn Văn E");
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            string tuKhoa = txtTuKhoa.Text.Trim();

            if (tuKhoa == "")
            {
                MessageBox.Show(
                    "Vui lòng nhập từ khóa cần tìm.",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                txtTuKhoa.Focus();
                return;
            }

            bool timThay = false;

            foreach (DataGridViewRow row in dgvKetQua.Rows)
            {
                if (row.IsNewRow)
                    continue;

                string maSach =
                    row.Cells["colMaSach"].Value?.ToString() ?? "";

                string tenSach =
                    row.Cells["colTenSach"].Value?.ToString() ?? "";

                string theLoai =
                    row.Cells["colTheLoai"].Value?.ToString() ?? "";

                string tacGia =
                    row.Cells["colTacGia"].Value?.ToString() ?? "";

                if (maSach.IndexOf(
                        tuKhoa,
                        StringComparison.OrdinalIgnoreCase) >= 0 ||
                    tenSach.IndexOf(
                        tuKhoa,
                        StringComparison.OrdinalIgnoreCase) >= 0 ||
                    theLoai.IndexOf(
                        tuKhoa,
                        StringComparison.OrdinalIgnoreCase) >= 0 ||
                    tacGia.IndexOf(
                        tuKhoa,
                        StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    row.Selected = true;

                    dgvKetQua.CurrentCell =
                        row.Cells["colMaSach"];

                    timThay = true;

                    break;
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
            txtTuKhoa.Clear();

            dgvKetQua.ClearSelection();

            txtTuKhoa.Focus();
        }
    }
}
