using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace DoAnQLTV
{
    public partial class FrmSach : Form
    {
        // Mã sách đang được chọn để sửa/xóa. null = đang ở chế độ nhập sách mới.
        private string maSachDangChon = null;

        // Thành phần thêm bằng code (không sửa Designer)
        private NumericUpDown nudSoLuong;
        private TextBox txtNhaXuatBan;
        private TextBox txtNamXuatBan;
        private TextBox txtViTriKe;

        public FrmSach()
        {
            InitializeComponent();

            txtMaSach.MaxLength = 20;
            txtTenSach.MaxLength = 200;

            AppTheme.ApplyForm(this);
            ThemO();
            ApDungGiaoDien();
            ThemCotMoi();

            LoadComboBox(cboTheLoai,
                "SELECT MaTheLoai, TenTheLoai FROM TheLoai ORDER BY TenTheLoai",
                "TenTheLoai", "MaTheLoai", "thể loại");

            LoadComboBox(cboTacGia,
                "SELECT MaTacGia, TenTacGia FROM TacGia ORDER BY TenTacGia",
                "TenTacGia", "MaTacGia", "tác giả");

            LoadSach();
            DatMaMoi();
        }

        // ================= GIAO DIỆN =================

        private Label TaoNhan(string text, int x, int y)
        {
            Label lbl = new Label();
            lbl.AutoSize = true;
            lbl.Font = lblMaSach.Font;
            lbl.ForeColor = lblMaSach.ForeColor;
            lbl.Text = text;
            lbl.Location = new Point(x, y);
            pnlThongTin.Controls.Add(lbl);
            return lbl;
        }

        private TextBox TaoOTexto(int x, int y, int rong, int toiDa)
        {
            TextBox txt = new TextBox();
            txt.MaxLength = toiDa;
            txt.SetBounds(x, y, rong, 27);
            pnlThongTin.Controls.Add(txt);
            return txt;
        }

        /// <summary>
        /// Thêm 2 hàng ô nhập: [Số lượng | Nhà XB] và [Năm XB | Vị trí kệ].
        /// Đẩy hàng nút và danh sách xuống, đồng thời thu bớt chiều cao danh sách cho vừa màn hình.
        /// </summary>
        private void ThemO()
        {
            const int doi = 96;       // 2 hàng x 48
            const int thuBotDS = 56;  // thu chiều cao danh sách

            pnlThongTin.Height += doi;
            foreach (Button b in new Button[] { btnThem, btnSua, btnXoa, btnTimKiem, btnLamMoi })
                b.Top += doi;
            pnlDanhSach.Top += doi;
            pnlDanhSach.Height -= thuBotDS;
            ClientSize = new Size(ClientSize.Width, ClientSize.Height + doi - thuBotDS);

            // Hàng 3
            TaoNhan("Số lượng", 24, 132);
            nudSoLuong = new NumericUpDown();
            nudSoLuong.Minimum = 1;
            nudSoLuong.Maximum = 1000;
            nudSoLuong.Value = 1;
            nudSoLuong.Font = new Font("Segoe UI", 11F);
            nudSoLuong.SetBounds(110, 128, 110, 27);
            pnlThongTin.Controls.Add(nudSoLuong);

            Label donVi = TaoNhan("bản", 230, 132);
            donVi.Font = AppTheme.Base;
            donVi.ForeColor = AppTheme.TextMuted;

            TaoNhan("Nhà XB", 476, 132);
            txtNhaXuatBan = TaoOTexto(560, 128, 340, 100);

            // Hàng 4
            TaoNhan("Năm XB", 24, 180);
            txtNamXuatBan = TaoOTexto(110, 176, 110, 4);

            TaoNhan("Vị trí kệ", 476, 180);
            txtViTriKe = TaoOTexto(560, 176, 340, 50);
        }

        private void ApDungGiaoDien()
        {
            // Hai thẻ trắng có viền mảnh
            AppTheme.StyleCard(pnlThongTin);
            AppTheme.StyleCard(pnlDanhSach);

            AppTheme.StyleGrid(dgvSach);

            AppTheme.StyleTextBox(txtMaSach);
            AppTheme.StyleTextBox(txtTenSach);
            AppTheme.StyleTextBox(txtNhaXuatBan);
            AppTheme.StyleTextBox(txtNamXuatBan);
            AppTheme.StyleTextBox(txtViTriKe);

            AppTheme.StyleComboBox(cboTheLoai);
            AppTheme.StyleComboBox(cboTacGia);

            AppTheme.StylePrimaryButton(btnThem);
            AppTheme.StyleButton(btnSua, AppTheme.Amber, ColorTranslator.FromHtml("#D97706"));
            AppTheme.StyleDangerButton(btnXoa);
            AppTheme.StyleSuccessButton(btnTimKiem);
            AppTheme.StyleButton(btnLamMoi,
                ColorTranslator.FromHtml("#E2E8F0"),
                ColorTranslator.FromHtml("#CBD5E1"),
                AppTheme.TextDark);   // nút phụ màu xám nhạt
        }

        private void ThemCot(string ten, string tieuDe, float trongSo)
        {
            if (dgvSach.Columns.Contains(ten)) return;

            DataGridViewTextBoxColumn col = new DataGridViewTextBoxColumn();
            col.Name = ten;
            col.HeaderText = tieuDe;
            col.FillWeight = trongSo;
            col.ReadOnly = true;
            dgvSach.Columns.Add(col);
        }

        private void ThemCotMoi()
        {
            ThemCot("colSoLuong", "Số lượng", 20F);
            ThemCot("colConLai", "Còn lại", 20F);
            ThemCot("colNXB", "Nhà XB", 90F);
            ThemCot("colNam", "Năm XB", 22F);
            ThemCot("colViTri", "Vị trí kệ", 50F);

            // Tối ưu tỷ lệ độ rộng để tên sách và thông tin hiển thị trọn vẹn, không bị cắt dấu "..."
            if (dgvSach.Columns.Contains("colTenSach"))
                dgvSach.Columns["colTenSach"].FillWeight = 180F;
            if (dgvSach.Columns.Contains("colTheLoai"))
                dgvSach.Columns["colTheLoai"].FillWeight = 85F;
            if (dgvSach.Columns.Contains("colTacGia"))
                dgvSach.Columns["colTacGia"].FillWeight = 85F;
            if (dgvSach.Columns.Contains("colMaSach"))
                dgvSach.Columns["colMaSach"].FillWeight = 35F;
        }

        // ================= NẠP DỮ LIỆU =================

        private void LoadComboBox(ComboBox cbo, string sql, string display, string value, string tenDanhSach)
        {
            try
            {
                cbo.DropDownStyle = ComboBoxStyle.DropDownList;
                cbo.DisplayMember = display;
                cbo.ValueMember = value;
                cbo.DataSource = DbConnection.ExecuteQuery(sql);
                cbo.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                AppTheme.Error("Không thể tải danh sách " + tenDanhSach + ".\n\n" + ex.Message);
            }
        }

        /// <summary>Lấy danh sách sách (kèm số lượng, số bản còn lại, thông tin xuất bản); tuKhoa rỗng/null = lấy tất cả.</summary>
        private DataTable TruyVanSach(string tuKhoa)
        {
            const string sql = @"
                SELECT s.MaSach, s.TenSach, tl.TenTheLoai, tg.TenTacGia,
                       s.SoLuong, " + ThuVienDb.SqlConLai + @" AS ConLai,
                       s.NhaXuatBan, s.NamXuatBan, s.ViTriKe
                FROM Sach s
                INNER JOIN TheLoai tl ON s.MaTheLoai = tl.MaTheLoai
                INNER JOIN TacGia  tg ON s.MaTacGia  = tg.MaTacGia
                WHERE (@TuKhoa IS NULL OR s.MaSach LIKE @TuKhoa OR s.TenSach LIKE @TuKhoa)
                ORDER BY s.MaSach";

            object tuKhoaLike = string.IsNullOrEmpty(tuKhoa) ? null : "%" + tuKhoa + "%";
            return DbConnection.ExecuteQuery(sql, DbConnection.Param("@TuKhoa", tuKhoaLike));
        }

        private void LoadSach()
        {
            try
            {
                HienThiLuoi(TruyVanSach(null));
            }
            catch (Exception ex)
            {
                AppTheme.Error("Không thể tải dữ liệu sách.\n\n" + ex.Message);
            }
        }

        private static string Chuoi(object giaTri)
        {
            return giaTri == null || giaTri == DBNull.Value ? "" : giaTri.ToString();
        }

        private void HienThiLuoi(DataTable dt)
        {
            dgvSach.Rows.Clear();
            foreach (DataRow r in dt.Rows)
            {
                int conLai = Convert.ToInt32(r["ConLai"]);

                int idx = dgvSach.Rows.Add(
                    r["MaSach"], r["TenSach"], r["TenTheLoai"], r["TenTacGia"],
                    r["SoLuong"], conLai,
                    Chuoi(r["NhaXuatBan"]), Chuoi(r["NamXuatBan"]), Chuoi(r["ViTriKe"]));

                // Hết sách thì tô đỏ cột "Còn lại"
                if (conLai <= 0)
                {
                    DataGridViewCell o = dgvSach.Rows[idx].Cells["colConLai"];
                    o.Style.ForeColor = AppTheme.Danger;
                    o.Style.SelectionForeColor = AppTheme.Danger;
                    o.Style.Font = AppTheme.Bold;
                }
            }

            dgvSach.ClearSelection();
            dgvSach.CurrentCell = null;
        }

        // ================= HÀM DÙNG CHUNG TRÊN FORM =================

        /// <summary>Điền sẵn mã sách kế tiếp (vẫn sửa được).</summary>
        private void DatMaMoi()
        {
            txtMaSach.Text = MaTuSinh.TiepTheo("Sach", "MaSach", "S", 4);
        }

        private void DoLenForm(string maSach, string tenSach, string tenTheLoai, string tenTacGia,
                               int soLuong, string nhaXuatBan, string namXuatBan, string viTriKe)
        {
            txtMaSach.Text = maSach;
            txtTenSach.Text = tenSach;
            cboTheLoai.SelectedIndex = cboTheLoai.FindStringExact(tenTheLoai);
            cboTacGia.SelectedIndex = cboTacGia.FindStringExact(tenTacGia);
            nudSoLuong.Value = Math.Max(nudSoLuong.Minimum, Math.Min(nudSoLuong.Maximum, soLuong));
            txtNhaXuatBan.Text = nhaXuatBan ?? "";
            txtNamXuatBan.Text = namXuatBan ?? "";
            txtViTriKe.Text = viTriKe ?? "";

            maSachDangChon = maSach;
            txtMaSach.ReadOnly = true;   // Không cho đổi mã khi đang sửa (mã là khóa chính)
        }

        private void XoaForm()
        {
            txtTenSach.Clear();
            cboTheLoai.SelectedIndex = -1;
            cboTacGia.SelectedIndex = -1;
            nudSoLuong.Value = 1;
            txtNhaXuatBan.Clear();
            txtNamXuatBan.Clear();
            txtViTriKe.Clear();

            maSachDangChon = null;
            txtMaSach.ReadOnly = false;
            DatMaMoi();

            dgvSach.ClearSelection();
            dgvSach.CurrentCell = null;
            txtTenSach.Focus();
        }

        private bool CanhBao(string thongBao, Control control)
        {
            AppTheme.Warning(thongBao);
            control.Focus();
            return false;
        }

        private bool KiemTraDuLieu()
        {
            if (txtMaSach.Text.Trim() == "")
                return CanhBao("Vui lòng nhập mã sách.", txtMaSach);
            if (txtTenSach.Text.Trim() == "")
                return CanhBao("Vui lòng nhập tên sách.", txtTenSach);
            if (cboTheLoai.SelectedValue == null)
                return CanhBao("Vui lòng chọn thể loại.", cboTheLoai);
            if (cboTacGia.SelectedValue == null)
                return CanhBao("Vui lòng chọn tác giả.", cboTacGia);

            // Năm xuất bản: có thể để trống
            string nam = txtNamXuatBan.Text.Trim();
            if (nam != "")
            {
                int n;
                if (!int.TryParse(nam, out n) || n < 1000 || n > DateTime.Today.Year + 1)
                    return CanhBao("Năm xuất bản không hợp lệ (nhập 4 chữ số, từ 1000 đến " +
                                   (DateTime.Today.Year + 1) + ").", txtNamXuatBan);
            }

            return true;
        }

        /// <summary>Chuỗi rỗng thì lưu NULL.</summary>
        private static object NullNeuRong(string s)
        {
            s = (s ?? "").Trim();
            return s == "" ? null : s;
        }

        private object NamXuatBanDeLuu()
        {
            string nam = txtNamXuatBan.Text.Trim();
            return nam == "" ? null : (object)int.Parse(nam);
        }

        private static int DocSo(object giaTri)
        {
            int n;
            return int.TryParse(giaTri == null ? "" : giaTri.ToString(), out n) ? n : 1;
        }

        // ================= SỰ KIỆN (giữ nguyên tên để Designer không bị lỗi) =================

        private void dgvSach_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            DataGridViewRow row = dgvSach.Rows[e.RowIndex];
            DoLenForm(
                row.Cells["colMaSach"].Value?.ToString(),
                row.Cells["colTenSach"].Value?.ToString(),
                row.Cells["colTheLoai"].Value?.ToString(),
                row.Cells["colTacGia"].Value?.ToString(),
                DocSo(row.Cells["colSoLuong"].Value),
                row.Cells["colNXB"].Value?.ToString(),
                row.Cells["colNam"].Value?.ToString(),
                row.Cells["colViTri"].Value?.ToString());
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            if (maSachDangChon != null)
            {
                AppTheme.Warning("Bạn đang chọn một sách có sẵn.\nBấm \"Làm mới\" để nhập sách mới.");
                return;
            }

            if (!KiemTraDuLieu()) return;

            try
            {
                DbConnection.ExecuteNonQuery(@"
                    INSERT INTO Sach (MaSach, TenSach, MaTheLoai, MaTacGia, SoLuong, NhaXuatBan, NamXuatBan, ViTriKe)
                    VALUES (@MaSach, @TenSach, @MaTheLoai, @MaTacGia, @SoLuong, @NhaXuatBan, @NamXuatBan, @ViTriKe)",
                    DbConnection.Param("@MaSach", txtMaSach.Text.Trim()),
                    DbConnection.Param("@TenSach", txtTenSach.Text.Trim()),
                    DbConnection.Param("@MaTheLoai", cboTheLoai.SelectedValue),
                    DbConnection.Param("@MaTacGia", cboTacGia.SelectedValue),
                    DbConnection.Param("@SoLuong", (int)nudSoLuong.Value),
                    DbConnection.Param("@NhaXuatBan", NullNeuRong(txtNhaXuatBan.Text)),
                    DbConnection.Param("@NamXuatBan", NamXuatBanDeLuu()),
                    DbConnection.Param("@ViTriKe", NullNeuRong(txtViTriKe.Text)));

                NhatKy.Ghi("Thêm sách", "Thêm sách \"" + txtTenSach.Text.Trim() + "\" - Mã sách: " +
                    txtMaSach.Text.Trim() + " (" + (int)nudSoLuong.Value + " bản)");

                AppTheme.Info("Thêm sách thành công.");
                LoadSach();
                XoaForm();
            }
            catch (SqlException ex) when (ex.Number == 2627 || ex.Number == 2601)
            {
                AppTheme.Warning("Mã sách đã tồn tại. Vui lòng nhập mã khác.");
            }
            catch (Exception ex)
            {
                AppTheme.Error("Không thể thêm sách.\n\n" + ex.Message);
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (maSachDangChon == null)
            {
                AppTheme.Warning("Vui lòng chọn sách cần sửa.");
                return;
            }

            if (!KiemTraDuLieu()) return;

            try
            {
                // Không cho giảm số bản xuống thấp hơn số bản đang được mượn chưa trả.
                // Kiểm tra và cập nhật trong 1 transaction, khóa dòng sách (giống lúc lập phiếu)
                // để không bị lập phiếu xen vào giữa hai bước.
                int soLuongMoi = (int)nudSoLuong.Value;
                int dangMuon = 0;
                int soDong = 0;
                bool biChan = false;

                DbConnection.ExecuteInTransaction((conn, tran) =>
                {
                    using (SqlCommand khoa = new SqlCommand(
                        "SELECT MaSach FROM Sach WITH (UPDLOCK, HOLDLOCK) WHERE MaSach = @MaSach", conn, tran))
                    {
                        khoa.Parameters.Add(DbConnection.Param("@MaSach", maSachDangChon));
                        khoa.ExecuteScalar();
                    }

                    using (SqlCommand dem = new SqlCommand(
                        "SELECT COUNT(*) FROM PhieuMuon WHERE MaSach = @MaSach AND NgayTra IS NULL", conn, tran))
                    {
                        dem.Parameters.Add(DbConnection.Param("@MaSach", maSachDangChon));
                        dangMuon = Convert.ToInt32(dem.ExecuteScalar());
                    }

                    if (soLuongMoi < dangMuon)
                    {
                        biChan = true;
                        return;
                    }

                    using (SqlCommand sua = new SqlCommand(@"
                        UPDATE Sach
                        SET TenSach    = @TenSach,
                            MaTheLoai  = @MaTheLoai,
                            MaTacGia   = @MaTacGia,
                            SoLuong    = @SoLuong,
                            NhaXuatBan = @NhaXuatBan,
                            NamXuatBan = @NamXuatBan,
                            ViTriKe    = @ViTriKe
                        WHERE MaSach = @MaSach", conn, tran))
                    {
                        sua.Parameters.Add(DbConnection.Param("@TenSach", txtTenSach.Text.Trim()));
                        sua.Parameters.Add(DbConnection.Param("@MaTheLoai", cboTheLoai.SelectedValue));
                        sua.Parameters.Add(DbConnection.Param("@MaTacGia", cboTacGia.SelectedValue));
                        sua.Parameters.Add(DbConnection.Param("@SoLuong", soLuongMoi));
                        sua.Parameters.Add(DbConnection.Param("@NhaXuatBan", NullNeuRong(txtNhaXuatBan.Text)));
                        sua.Parameters.Add(DbConnection.Param("@NamXuatBan", NamXuatBanDeLuu()));
                        sua.Parameters.Add(DbConnection.Param("@ViTriKe", NullNeuRong(txtViTriKe.Text)));
                        sua.Parameters.Add(DbConnection.Param("@MaSach", maSachDangChon));
                        soDong = sua.ExecuteNonQuery();
                    }
                });

                if (biChan)
                {
                    AppTheme.Warning("Sách này đang có " + dangMuon + " bản được mượn chưa trả.\n" +
                                     "Số lượng không được nhỏ hơn " + dangMuon + ".");
                    nudSoLuong.Focus();
                    return;
                }

                if (soDong == 0)
                {
                    AppTheme.Warning("Không tìm thấy sách cần sửa.");
                    return;
                }

                NhatKy.Ghi("Sửa sách", "Sửa sách \"" + txtTenSach.Text.Trim() + "\" - Mã sách: " +
                    maSachDangChon + " (" + (int)nudSoLuong.Value + " bản)");

                AppTheme.Info("Sửa thông tin sách thành công.");
                LoadSach();
                XoaForm();
            }
            catch (Exception ex)
            {
                AppTheme.Error("Không thể sửa sách.\n\n" + ex.Message);
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (maSachDangChon == null)
            {
                AppTheme.Warning("Vui lòng chọn sách cần xóa.");
                return;
            }

            if (!AppTheme.Confirm("Bạn có chắc chắn muốn xóa sách này không?"))
                return;

            try
            {
                int soDong = DbConnection.ExecuteNonQuery(
                    "DELETE FROM Sach WHERE MaSach = @MaSach",
                    DbConnection.Param("@MaSach", maSachDangChon));

                if (soDong == 0)
                {
                    AppTheme.Warning("Không tìm thấy sách cần xóa.");
                    return;
                }

                NhatKy.Ghi("Xóa sách", "Xóa sách \"" + txtTenSach.Text.Trim() + "\" - Mã sách: " + maSachDangChon);

                AppTheme.Info("Xóa sách thành công.");
                LoadSach();
                XoaForm();
            }
            catch (SqlException ex) when (ex.Number == 547)
            {
                AppTheme.Warning("Không thể xóa sách này vì sách đang được sử dụng trong phiếu mượn.");
            }
            catch (Exception ex)
            {
                AppTheme.Error("Không thể xóa sách.\n\n" + ex.Message);
            }
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            string tuKhoa = txtTenSach.Text.Trim();
            if (tuKhoa == "")
            {
                CanhBao("Vui lòng nhập từ khóa cần tìm.", txtTenSach);
                return;
            }

            try
            {
                DataTable dt = TruyVanSach(tuKhoa);
                HienThiLuoi(dt);

                if (dt.Rows.Count == 0)
                {
                    maSachDangChon = null;
                    txtMaSach.ReadOnly = false;
                    AppTheme.Info("Không tìm thấy sách phù hợp.");
                }
                else if (dt.Rows.Count == 1)
                {
                    DataRow r = dt.Rows[0];
                    DoLenForm(
                        r["MaSach"].ToString(),
                        r["TenSach"].ToString(),
                        r["TenTheLoai"].ToString(),
                        r["TenTacGia"].ToString(),
                        Convert.ToInt32(r["SoLuong"]),
                        Chuoi(r["NhaXuatBan"]),
                        Chuoi(r["NamXuatBan"]),
                        Chuoi(r["ViTriKe"]));
                }
            }
            catch (Exception ex)
            {
                AppTheme.Error("Không thể tìm kiếm sách.\n\n" + ex.Message);
            }
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            LoadSach();
            XoaForm();
        }
    }
}
