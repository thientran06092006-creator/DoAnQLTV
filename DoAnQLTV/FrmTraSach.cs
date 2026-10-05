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
    public partial class FrmTraSach : Form
    {
        public FrmTraSach()
        {
            InitializeComponent();

            KhoiTaoDocGia();
            KhoiTaoSach();
            HienThiDuLieuMau();
        }

        private void KhoiTaoDocGia()
        {
            cboDocGia.Items.Clear();

            cboDocGia.Items.Add("DG001 - Nguyễn Văn An");
            cboDocGia.Items.Add("DG002 - Trần Thị Bình");
            cboDocGia.Items.Add("DG003 - Lê Văn Cường");
            cboDocGia.Items.Add("DG004 - Phạm Thị Dung");
            cboDocGia.Items.Add("DG005 - Hoàng Văn Em");

            if (cboDocGia.Items.Count > 0)
            {
                cboDocGia.SelectedIndex = 0;
            }
        }

        private void KhoiTaoSach()
        {
            cboSach.Items.Clear();

            cboSach.Items.Add("S001 - Lập trình C#");
            cboSach.Items.Add("S002 - Cơ sở dữ liệu");
            cboSach.Items.Add("S003 - Kỹ năng học tập");
            cboSach.Items.Add("S004 - Lập trình hướng đối tượng");
            cboSach.Items.Add("S005 - Cấu trúc dữ liệu và giải thuật");

            if (cboSach.Items.Count > 0)
            {
                cboSach.SelectedIndex = 0;
            }
        }

        private void HienThiDuLieuMau()
        {
            dgvTraSach.Rows.Clear();

            dgvTraSach.Rows.Add(
                "PM001",
                "DG001 - Nguyễn Văn An",
                "S001 - Lập trình C#",
                "05/10/2026",
                "12/10/2026",
                "");

            dgvTraSach.Rows.Add(
                "PM002",
                "DG002 - Trần Thị Bình",
                "S002 - Cơ sở dữ liệu",
                "05/10/2026",
                "12/10/2026",
                "");

            dgvTraSach.Rows.Add(
                "PM003",
                "DG003 - Lê Văn Cường",
                "S003 - Kỹ năng học tập",
                "04/10/2026",
                "11/10/2026",
                "");
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

            bool timThay = false;

            foreach (DataGridViewRow row in dgvTraSach.Rows)
            {
                if (row.Cells["colMaPhieu"].Value != null &&
                    row.Cells["colMaPhieu"].Value.ToString()
                    .Equals(tuKhoa, StringComparison.OrdinalIgnoreCase))
                {
                    row.Selected = true;
                    dgvTraSach.CurrentCell = row.Cells["colMaPhieu"];

                    timThay = true;
                    break;
                }
            }

            if (!timThay)
            {
                MessageBox.Show(
                    "Không tìm thấy mã phiếu.",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
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

            DataGridViewRow dongTimThay = null;

            foreach (DataGridViewRow row in dgvTraSach.Rows)
            {
                if (row.Cells["colMaPhieu"].Value != null &&
                    row.Cells["colMaPhieu"].Value.ToString()
                    .Equals(maPhieu, StringComparison.OrdinalIgnoreCase))
                {
                    dongTimThay = row;
                    break;
                }
            }

            if (dongTimThay == null)
            {
                MessageBox.Show(
                    "Không tìm thấy mã phiếu.",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            string ngayTra =
                dtpNgayTra.Value.ToString("dd/MM/yyyy");

            dongTimThay.Cells["colNgayTra"].Value = ngayTra;

            MessageBox.Show(
                "Trả sách thành công.",
                "Thông báo",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
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

            dgvTraSach.ClearSelection();

            txtMaPhieu.Focus();
        }
    }
}
