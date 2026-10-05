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
    public partial class FrmLapPhieuMuon : Form
    {
        private int dongDangChon = -1;
        public FrmLapPhieuMuon()
        {
            InitializeComponent();

            KhoiTaoDocGia();
            KhoiTaoNhanVien();
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

        private void KhoiTaoNhanVien()
        {
            cboNhanVien.Items.Clear();

            cboNhanVien.Items.Add("NV001 - Nguyễn Văn Minh");
            cboNhanVien.Items.Add("NV002 - Trần Thị Lan");
            cboNhanVien.Items.Add("NV003 - Lê Văn Hùng");
            cboNhanVien.Items.Add("NV004 - Phạm Thị Mai");
            cboNhanVien.Items.Add("NV005 - Hoàng Văn Nam");

            if (cboNhanVien.Items.Count > 0)
            {
                cboNhanVien.SelectedIndex = 0;
            }
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

        private void btnThem_Click(object sender, EventArgs e)
        {
            string maPhieu = txtMaPhieu.Text.Trim();
            string docGia = cboDocGia.Text.Trim();
            string nhanVien = cboNhanVien.Text.Trim();
            string ngayMuon = dtpNgayMuon.Value.ToString("dd/MM/yyyy");
            string hanTra = dtpHanTra.Value.ToString("dd/MM/yyyy");
            string sach = txtSach.Text.Trim();

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

            if (sach == "")
            {
                MessageBox.Show(
                    "Vui lòng nhập tên sách.",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                txtSach.Focus();
                return;
            }

            foreach (DataGridViewRow row in dgvPhieuMuon.Rows)
            {
                if (row.Cells["colMaPhieu"].Value != null &&
                    row.Cells["colMaPhieu"].Value.ToString()
                    .Equals(maPhieu, StringComparison.OrdinalIgnoreCase))
                {
                    MessageBox.Show(
                        "Mã phiếu đã tồn tại.",
                        "Thông báo",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    txtMaPhieu.Focus();
                    return;
                }
            }

            dgvPhieuMuon.Rows.Add(
                maPhieu,
                docGia,
                nhanVien,
                ngayMuon,
                hanTra,
                sach);

            MessageBox.Show(
                "Thêm phiếu mượn thành công.",
                "Thông báo",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);

            txtMaPhieu.Clear();
            txtSach.Clear();
            dongDangChon = -1;
            dgvPhieuMuon.ClearSelection();
            txtMaPhieu.Focus();
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

            txtSach.Text =
                row.Cells["colSach"].Value?.ToString() ?? "";

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
            if (dongDangChon < 0)
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
            string ngayMuon = dtpNgayMuon.Value.ToString("dd/MM/yyyy");
            string hanTra = dtpHanTra.Value.ToString("dd/MM/yyyy");
            string sach = txtSach.Text.Trim();

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

            if (sach == "")
            {
                MessageBox.Show(
                    "Vui lòng nhập tên sách.",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                txtSach.Focus();
                return;
            }

            for (int i = 0; i < dgvPhieuMuon.Rows.Count; i++)
            {
                if (i == dongDangChon)
                    continue;

                if (dgvPhieuMuon.Rows[i].Cells["colMaPhieu"].Value != null &&
                    dgvPhieuMuon.Rows[i].Cells["colMaPhieu"].Value.ToString()
                    .Equals(maPhieu, StringComparison.OrdinalIgnoreCase))
                {
                    MessageBox.Show(
                        "Mã phiếu đã tồn tại.",
                        "Thông báo",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    txtMaPhieu.Focus();
                    return;
                }
            }

            DataGridViewRow row = dgvPhieuMuon.Rows[dongDangChon];

            row.Cells["colMaPhieu"].Value = maPhieu;
            row.Cells["colDocGia"].Value = docGia;
            row.Cells["colNhanVien"].Value = nhanVien;
            row.Cells["colNgayMuon"].Value = ngayMuon;
            row.Cells["colHanTra"].Value = hanTra;
            row.Cells["colSach"].Value = sach;

            MessageBox.Show(
                "Sửa phiếu mượn thành công.",
                "Thông báo",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);

            dongDangChon = -1;
            dgvPhieuMuon.ClearSelection();

            txtMaPhieu.Clear();
            txtSach.Clear();
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (dongDangChon < 0)
            {
                MessageBox.Show(
                    "Vui lòng chọn phiếu mượn cần xóa.",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            DialogResult ketQua = MessageBox.Show(
                "Bạn có chắc chắn muốn xóa phiếu mượn này không?",
                "Xác nhận xóa",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (ketQua == DialogResult.Yes)
            {
                dgvPhieuMuon.Rows.RemoveAt(dongDangChon);

                MessageBox.Show(
                    "Xóa phiếu mượn thành công.",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                dongDangChon = -1;

                txtMaPhieu.Clear();
                txtSach.Clear();

                dgvPhieuMuon.ClearSelection();
            }
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            txtMaPhieu.Clear();
            txtSach.Clear();

            if (cboDocGia.Items.Count > 0)
            {
                cboDocGia.SelectedIndex = 0;
            }

            if (cboNhanVien.Items.Count > 0)
            {
                cboNhanVien.SelectedIndex = 0;
            }

            dtpNgayMuon.Value = DateTime.Now;
            dtpHanTra.Value = DateTime.Now.AddDays(7);

            dongDangChon = -1;
            dgvPhieuMuon.ClearSelection();

            txtMaPhieu.Focus();
        }

        private void btnLapPhieu_Click(object sender, EventArgs e)
        {
            string maPhieu = txtMaPhieu.Text.Trim();
            string docGia = cboDocGia.Text.Trim();
            string nhanVien = cboNhanVien.Text.Trim();
            string sach = txtSach.Text.Trim();

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

            if (sach == "")
            {
                MessageBox.Show(
                    "Vui lòng nhập tên sách.",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                txtSach.Focus();
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

            MessageBox.Show(
                "Lập phiếu mượn thành công.",
                "Thông báo",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }
    }
}
