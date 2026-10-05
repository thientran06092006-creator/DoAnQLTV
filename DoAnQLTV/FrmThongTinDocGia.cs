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
    public partial class FrmThongTinDocGia : Form
    {
        public FrmThongTinDocGia()
        {
            InitializeComponent();

            HienThiDuLieuMau();
        }

        private void HienThiDuLieuMau()
        {
            dgvKetQua.Rows.Clear();

            dgvKetQua.Rows.Add(
                "DG001",
                "Nguyễn Văn An",
                "01/01/2004",
                "TP. Hồ Chí Minh",
                "0900000001");

            dgvKetQua.Rows.Add(
                "DG002",
                "Trần Thị Bình",
                "15/03/2004",
                "TP. Hồ Chí Minh",
                "0900000002");

            dgvKetQua.Rows.Add(
                "DG003",
                "Lê Văn Cường",
                "20/05/2003",
                "Đồng Nai",
                "0900000003");

            dgvKetQua.Rows.Add(
                "DG004",
                "Phạm Thị Dung",
                "10/08/2004",
                "Bình Dương",
                "0900000004");

            dgvKetQua.Rows.Add(
                "DG005",
                "Hoàng Văn Em",
                "25/12/2003",
                "Long An",
                "0900000005");
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

                    dgvKetQua.CurrentCell =
                        row.Cells["colMaDocGia"];

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
            txtTuKhoa.Clear();

            dgvKetQua.ClearSelection();

            txtTuKhoa.Focus();
        }

        private void dgvKetQua_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            DataGridViewRow row = dgvKetQua.Rows[e.RowIndex];

            string maDocGia =
                row.Cells["colMaDocGia"].Value?.ToString() ?? "";

            string tenDocGia =
                row.Cells["colTenDocGia"].Value?.ToString() ?? "";

            txtTuKhoa.Text = tenDocGia;
        }
    }
}
