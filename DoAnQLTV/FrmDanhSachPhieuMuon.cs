using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace DoAnQLTV
{
    public partial class FrmDanhSachPhieuMuon : Form
    {
        public FrmDanhSachPhieuMuon()
        {
            InitializeComponent();

            LoadPhieuMuon();
        }

        private void LoadPhieuMuon()
        {
            try
            {
                using (SqlConnection conn = DbConnection.GetConnection())
                {
                    conn.Open();

                    string sql = @"
                SELECT
                    pm.MaPhieu,
                    pm.MaDocGia + ' - ' + dg.TenDocGia AS DocGia,
                    pm.MaNhanVien + ' - ' + nv.TenNhanVien AS NhanVien,
                    pm.NgayMuon,
                    pm.HanTra,
                    s.MaSach + ' - ' + s.TenSach AS Sach
                FROM PhieuMuon pm
                INNER JOIN DocGia dg
                    ON pm.MaDocGia = dg.MaDocGia
                INNER JOIN NhanVien nv
                    ON pm.MaNhanVien = nv.MaNhanVien
                INNER JOIN Sach s
                    ON pm.MaSach = s.MaSach
                ORDER BY pm.MaPhieu";

                    using (SqlDataAdapter adapter =
                        new SqlDataAdapter(sql, conn))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);

                        dgvPhieuMuon.Rows.Clear();

                        foreach (DataRow row in dt.Rows)
                        {
                            dgvPhieuMuon.Rows.Add(
                                row["MaPhieu"].ToString(),
                                row["DocGia"].ToString(),
                                row["NhanVien"].ToString(),
                                Convert.ToDateTime(
                                    row["NgayMuon"]).ToString("dd/MM/yyyy"),
                                Convert.ToDateTime(
                                    row["HanTra"]).ToString("dd/MM/yyyy"),
                                row["Sach"].ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Không thể tải dữ liệu phiếu mượn.\n\n" + ex.Message,
                    "Lỗi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
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

            try
            {
                using (SqlConnection conn = DbConnection.GetConnection())
                {
                    conn.Open();

                    string sql = @"
                SELECT
                    pm.MaPhieu,
                    pm.MaDocGia + ' - ' + dg.TenDocGia AS DocGia,
                    pm.MaNhanVien + ' - ' + nv.TenNhanVien AS NhanVien,
                    pm.NgayMuon,
                    pm.HanTra,
                    s.MaSach + ' - ' + s.TenSach AS Sach
                FROM PhieuMuon pm
                INNER JOIN DocGia dg
                    ON pm.MaDocGia = dg.MaDocGia
                INNER JOIN NhanVien nv
                    ON pm.MaNhanVien = nv.MaNhanVien
                INNER JOIN Sach s
                    ON pm.MaSach = s.MaSach
                WHERE pm.MaPhieu LIKE @TuKhoa
                   OR dg.TenDocGia LIKE @TuKhoa
                   OR nv.TenNhanVien LIKE @TuKhoa
                   OR s.TenSach LIKE @TuKhoa
                ORDER BY pm.MaPhieu";

                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue(
                            "@TuKhoa",
                            "%" + tuKhoa + "%");

                        using (SqlDataAdapter adapter =
                            new SqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            adapter.Fill(dt);

                            dgvPhieuMuon.Rows.Clear();

                            foreach (DataRow row in dt.Rows)
                            {
                                dgvPhieuMuon.Rows.Add(
                                    row["MaPhieu"].ToString(),
                                    row["DocGia"].ToString(),
                                    row["NhanVien"].ToString(),
                                    Convert.ToDateTime(
                                        row["NgayMuon"]).ToString("dd/MM/yyyy"),
                                    Convert.ToDateTime(
                                        row["HanTra"]).ToString("dd/MM/yyyy"),
                                    row["Sach"].ToString());
                            }

                            if (dt.Rows.Count == 0)
                            {
                                MessageBox.Show(
                                    "Không tìm thấy phiếu mượn phù hợp.",
                                    "Thông báo",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Information);

                                return;
                            }

                            dgvPhieuMuon.ClearSelection();

                            if (dgvPhieuMuon.Rows.Count > 0)
                            {
                                dgvPhieuMuon.Rows[0].Selected = true;

                                dgvPhieuMuon.CurrentCell =
                                    dgvPhieuMuon.Rows[0].Cells["colMaPhieu"];
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Không thể tìm kiếm phiếu mượn.\n\n" + ex.Message,
                    "Lỗi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            txtTuKhoa.Clear();

            LoadPhieuMuon();

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
