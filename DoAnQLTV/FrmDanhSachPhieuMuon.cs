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
    public partial class FrmDanhSachPhieuMuon : Form
    {
        public FrmDanhSachPhieuMuon()
        {
            InitializeComponent();

            HienThiDuLieuMau();
        }

        private void HienThiDuLieuMau()
        {
            dgvPhieuMuon.Rows.Clear();

            dgvPhieuMuon.Rows.Add(
                "PM001",
                "DG001 - Nguyễn Văn An",
                "NV001 - Nguyễn Văn Minh",
                "05/10/2026",
                "12/10/2026",
                "Lập trình C#");

            dgvPhieuMuon.Rows.Add(
                "PM002",
                "DG002 - Trần Thị Bình",
                "NV002 - Trần Thị Lan",
                "05/10/2026",
                "12/10/2026",
                "Cơ sở dữ liệu");

            dgvPhieuMuon.Rows.Add(
                "PM003",
                "DG003 - Lê Văn Cường",
                "NV003 - Lê Văn Hùng",
                "04/10/2026",
                "11/10/2026",
                "Kỹ năng học tập");
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

            foreach (DataGridViewRow row in dgvPhieuMuon.Rows)
            {
                if (row.Cells["colMaPhieu"].Value != null &&
                    row.Cells["colMaPhieu"].Value.ToString()
                    .IndexOf(tuKhoa, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    row.Selected = true;
                    dgvPhieuMuon.CurrentCell = row.Cells["colMaPhieu"];

                    timThay = true;
                    break;
                }

                if (row.Cells["colDocGia"].Value != null &&
                    row.Cells["colDocGia"].Value.ToString()
                    .IndexOf(tuKhoa, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    row.Selected = true;
                    dgvPhieuMuon.CurrentCell = row.Cells["colDocGia"];

                    timThay = true;
                    break;
                }

                if (row.Cells["colNhanVien"].Value != null &&
                    row.Cells["colNhanVien"].Value.ToString()
                    .IndexOf(tuKhoa, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    row.Selected = true;
                    dgvPhieuMuon.CurrentCell = row.Cells["colNhanVien"];

                    timThay = true;
                    break;
                }

                if (row.Cells["colSach"].Value != null &&
                    row.Cells["colSach"].Value.ToString()
                    .IndexOf(tuKhoa, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    row.Selected = true;
                    dgvPhieuMuon.CurrentCell = row.Cells["colSach"];

                    timThay = true;
                    break;
                }
            }

            if (!timThay)
            {
                MessageBox.Show(
                    "Không tìm thấy phiếu mượn.",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            txtTuKhoa.Clear();

            dgvPhieuMuon.ClearSelection();

            txtTuKhoa.Focus();
        }

        private void dgvPhieuMuon_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            DataGridViewRow row = dgvPhieuMuon.Rows[e.RowIndex];

            txtTuKhoa.Text =
                row.Cells["colMaPhieu"].Value?.ToString() ?? "";
        }
    }
}
