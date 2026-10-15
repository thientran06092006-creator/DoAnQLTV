using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace DoAnQLTV
{
    /// <summary>
    /// Xem toàn bộ nhật ký hoạt động, lọc theo ngày / nhân viên / loại hoạt động / từ khóa.
    /// Form tạo hoàn toàn bằng code, không có file Designer.
    /// </summary>
    public class FrmNhatKy : Form
    {
        private const string TatCa = "(Tất cả)";
        private const int ToiDa = 1000;
        private const string DinhDangGio = "HH:mm:ss dd/MM/yyyy";

        private Panel pnlLoc;
        private Panel pnlDanhSach;
        private DateTimePicker dtpTu;
        private DateTimePicker dtpDen;
        private ComboBox cboNguoi;
        private ComboBox cboLoai;
        private TextBox txtTuKhoa;
        private Button btnLoc;
        private Button btnLamMoi;
        private DataGridView dgv;
        private Label lblKetQua;

        public FrmNhatKy()
        {
            Name = "FrmNhatKy";
            Text = "Nhật Ký Hoạt Động";
            AutoScaleDimensions = new SizeF(96F, 96F);
            AutoScaleMode = AutoScaleMode.Dpi;
            ClientSize = new Size(1000, 630);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            StartPosition = FormStartPosition.CenterScreen;

            AppTheme.ApplyForm(this);
            KhoiTaoGiaoDien();

            try
            {
                NapBoLoc();
                Loc();
            }
            catch (Exception ex)
            {
                AppTheme.Error("Không thể tải nhật ký hoạt động.\n\n" + ex.Message);
            }
        }

        // ================= GIAO DIỆN =================

        private Label TaoNhan(Control cha, string text, int x, int y)
        {
            Label lbl = new Label();
            lbl.Text = text;
            lbl.AutoSize = true;
            lbl.Font = AppTheme.Bold;
            lbl.ForeColor = AppTheme.Primary;
            lbl.BackColor = Color.Transparent;
            lbl.Location = new Point(x, y);
            cha.Controls.Add(lbl);
            return lbl;
        }

        private DateTimePicker TaoNgay(Control cha, int x, int y, int rong)
        {
            DateTimePicker dtp = new DateTimePicker();
            dtp.Format = DateTimePickerFormat.Custom;
            dtp.CustomFormat = "dd/MM/yyyy";
            dtp.Font = new Font("Segoe UI", 11F);
            dtp.SetBounds(x, y, rong, 27);
            cha.Controls.Add(dtp);
            return dtp;
        }

        private ComboBox TaoCombo(Control cha, int x, int y, int rong)
        {
            ComboBox cbo = new ComboBox();
            cbo.DropDownStyle = ComboBoxStyle.DropDownList;
            cbo.SetBounds(x, y, rong, 28);
            cha.Controls.Add(cbo);
            AppTheme.StyleComboBox(cbo);   // phải thêm vào panel trước khi style (vẽ viền lên panel)
            return cbo;
        }

        private void KhoiTaoGiaoDien()
        {
            Label tieuDe = new Label();
            tieuDe.Text = "NHẬT KÝ HOẠT ĐỘNG";
            tieuDe.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            tieuDe.ForeColor = AppTheme.Primary;
            tieuDe.BackColor = Color.Transparent;
            tieuDe.TextAlign = ContentAlignment.MiddleLeft;
            tieuDe.SetBounds(30, 14, 600, 40);
            Controls.Add(tieuDe);

            // ----- Khung bộ lọc -----
            pnlLoc = new Panel();
            pnlLoc.SetBounds(30, 64, 940, 116);
            Controls.Add(pnlLoc);
            AppTheme.StyleCard(pnlLoc);

            TaoNhan(pnlLoc, "Từ ngày", 24, 28);
            dtpTu = TaoNgay(pnlLoc, 100, 24, 150);

            TaoNhan(pnlLoc, "Đến ngày", 270, 28);
            dtpDen = TaoNgay(pnlLoc, 350, 24, 150);

            TaoNhan(pnlLoc, "Nhân viên", 520, 28);
            cboNguoi = TaoCombo(pnlLoc, 600, 24, 300);

            TaoNhan(pnlLoc, "Loại", 24, 76);
            cboLoai = TaoCombo(pnlLoc, 100, 72, 200);

            TaoNhan(pnlLoc, "Từ khóa", 320, 76);
            txtTuKhoa = new TextBox();
            txtTuKhoa.SetBounds(400, 72, 220, 27);
            pnlLoc.Controls.Add(txtTuKhoa);
            AppTheme.StyleTextBox(txtTuKhoa);

            btnLoc = new Button();
            btnLoc.Text = "Lọc";
            btnLoc.SetBounds(640, 66, 120, 40);
            btnLoc.Click += btnLoc_Click;
            pnlLoc.Controls.Add(btnLoc);
            AppTheme.StyleSuccessButton(btnLoc);

            btnLamMoi = new Button();
            btnLamMoi.Text = "Làm mới";
            btnLamMoi.SetBounds(772, 66, 120, 40);
            btnLamMoi.Click += btnLamMoi_Click;
            pnlLoc.Controls.Add(btnLamMoi);
            AppTheme.StyleButton(btnLamMoi,
                ColorTranslator.FromHtml("#E2E8F0"),
                ColorTranslator.FromHtml("#CBD5E1"),
                AppTheme.TextDark);

            DatMacDinhLoc();

            // ----- Bảng kết quả -----
            pnlDanhSach = new Panel();
            pnlDanhSach.SetBounds(30, 200, 940, 390);
            pnlDanhSach.Padding = new Padding(1);
            Controls.Add(pnlDanhSach);
            AppTheme.StyleCard(pnlDanhSach);

            dgv = new DataGridView();
            dgv.Name = "dgvNhatKy";
            dgv.Dock = DockStyle.Fill;
            pnlDanhSach.Controls.Add(dgv);

            ThemCot("colThoiGian", "Thời gian", 22F);
            ThemCot("colLoai", "Loại hoạt động", 18F);
            ThemCot("colChiTiet", "Thông tin chi tiết", 42F);
            ThemCot("colNguoi", "Người thực hiện", 20F);
            AppTheme.StyleGrid(dgv);

            lblKetQua = new Label();
            lblKetQua.AutoSize = false;
            lblKetQua.Font = AppTheme.Base;
            lblKetQua.ForeColor = AppTheme.TextMuted;
            lblKetQua.BackColor = Color.Transparent;
            lblKetQua.TextAlign = ContentAlignment.MiddleLeft;
            lblKetQua.SetBounds(30, 596, 940, 24);
            Controls.Add(lblKetQua);

            AcceptButton = btnLoc;   // Enter trong ô từ khóa = Lọc
        }

        private void ThemCot(string ten, string tieuDe, float trongSo)
        {
            DataGridViewTextBoxColumn col = new DataGridViewTextBoxColumn();
            col.Name = ten;
            col.HeaderText = tieuDe;
            col.FillWeight = trongSo;
            col.ReadOnly = true;
            dgv.Columns.Add(col);
        }

        private void DatMacDinhLoc()
        {
            dtpTu.Value = DateTime.Today.AddDays(-30);
            dtpDen.Value = DateTime.Today;
            txtTuKhoa.Clear();
        }

        // ================= NẠP DỮ LIỆU =================

        private void NapCombo(ComboBox cbo, string sql, string cot)
        {
            string cu = cbo.SelectedItem as string;

            cbo.Items.Clear();
            cbo.Items.Add(TatCa);
            foreach (DataRow r in DbConnection.ExecuteQuery(sql).Rows)
                cbo.Items.Add(r[cot].ToString());

            int viTri = cu == null ? -1 : cbo.Items.IndexOf(cu);
            cbo.SelectedIndex = viTri >= 0 ? viTri : 0;
        }

        private void NapBoLoc()
        {
            NapCombo(cboNguoi,
                "SELECT DISTINCT NguoiThucHien FROM NhatKyHoatDong " +
                "WHERE NguoiThucHien IS NOT NULL ORDER BY NguoiThucHien",
                "NguoiThucHien");

            NapCombo(cboLoai,
                "SELECT DISTINCT LoaiHoatDong FROM NhatKyHoatDong ORDER BY LoaiHoatDong",
                "LoaiHoatDong");
        }

        private void Loc()
        {
            if (dtpTu.Value.Date > dtpDen.Value.Date)
            {
                AppTheme.Warning("Ngày bắt đầu không được sau ngày kết thúc.");
                dtpTu.Focus();
                return;
            }

            string nguoi = cboNguoi.SelectedIndex <= 0 ? null : cboNguoi.SelectedItem.ToString();
            string loai = cboLoai.SelectedIndex <= 0 ? null : cboLoai.SelectedItem.ToString();
            string tuKhoa = txtTuKhoa.Text.Trim();
            object tuKhoaLike = tuKhoa == "" ? null : "%" + tuKhoa + "%";

            DataTable dt = DbConnection.ExecuteQuery(
                "SELECT TOP (" + ToiDa + ") ThoiGian, LoaiHoatDong, ChiTiet, NguoiThucHien " +
                "FROM NhatKyHoatDong " +
                "WHERE ThoiGian >= @Tu AND ThoiGian < @Den " +
                "  AND (@Nguoi IS NULL OR NguoiThucHien = @Nguoi) " +
                "  AND (@Loai IS NULL OR LoaiHoatDong = @Loai) " +
                "  AND (@TuKhoa IS NULL OR ChiTiet LIKE @TuKhoa) " +
                "ORDER BY ThoiGian DESC, MaNhatKy DESC",
                DbConnection.Param("@Tu", dtpTu.Value.Date),
                DbConnection.Param("@Den", dtpDen.Value.Date.AddDays(1)),
                DbConnection.Param("@Nguoi", nguoi),
                DbConnection.Param("@Loai", loai),
                DbConnection.Param("@TuKhoa", tuKhoaLike));

            dgv.Rows.Clear();
            foreach (DataRow r in dt.Rows)
            {
                dgv.Rows.Add(
                    Convert.ToDateTime(r["ThoiGian"]).ToString(DinhDangGio),
                    r["LoaiHoatDong"],
                    r["ChiTiet"],
                    r["NguoiThucHien"]);
            }
            dgv.ClearSelection();
            dgv.CurrentCell = null;

            lblKetQua.Text = "Tìm thấy " + dt.Rows.Count + " hoạt động" +
                (dt.Rows.Count >= ToiDa
                    ? " (chỉ hiển thị " + ToiDa + " dòng mới nhất, hãy thu hẹp bộ lọc để xem thêm)"
                    : "");
        }

        // ================= SỰ KIỆN =================

        private void btnLoc_Click(object sender, EventArgs e)
        {
            try
            {
                Loc();
            }
            catch (Exception ex)
            {
                AppTheme.Error("Không thể lọc nhật ký.\n\n" + ex.Message);
            }
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            try
            {
                DatMacDinhLoc();
                NapBoLoc();            // có thể đã có nhân viên / loại hoạt động mới
                cboNguoi.SelectedIndex = 0;
                cboLoai.SelectedIndex = 0;
                Loc();
                txtTuKhoa.Focus();
            }
            catch (Exception ex)
            {
                AppTheme.Error("Không thể làm mới nhật ký.\n\n" + ex.Message);
            }
        }
    }
}
