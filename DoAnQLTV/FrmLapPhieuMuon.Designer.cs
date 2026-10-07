namespace DoAnQLTV
{
    partial class FrmLapPhieuMuon
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.lblMaPhieu = new System.Windows.Forms.Label();
            this.lblDocGia = new System.Windows.Forms.Label();
            this.lblNhanVien = new System.Windows.Forms.Label();
            this.lblNgayMuon = new System.Windows.Forms.Label();
            this.lblHanTra = new System.Windows.Forms.Label();
            this.lblSach = new System.Windows.Forms.Label();
            this.txtMaPhieu = new System.Windows.Forms.TextBox();
            this.cboDocGia = new System.Windows.Forms.ComboBox();
            this.cboNhanVien = new System.Windows.Forms.ComboBox();
            this.dtpNgayMuon = new System.Windows.Forms.DateTimePicker();
            this.dtpHanTra = new System.Windows.Forms.DateTimePicker();
            this.txtSach = new System.Windows.Forms.TextBox();
            this.btnThem = new System.Windows.Forms.Button();
            this.btnSua = new System.Windows.Forms.Button();
            this.btnXoa = new System.Windows.Forms.Button();
            this.btnLamMoi = new System.Windows.Forms.Button();
            this.btnLapPhieu = new System.Windows.Forms.Button();
            this.dgvPhieuMuon = new System.Windows.Forms.DataGridView();
            this.colMaPhieu = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDocGia = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colNhanVien = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colNgayMuon = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colHanTra = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSach = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lblTieuDeLapPhieuMuon = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPhieuMuon)).BeginInit();
            this.SuspendLayout();
            // 
            // lblMaPhieu
            // 
            this.lblMaPhieu.AutoSize = true;
            this.lblMaPhieu.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMaPhieu.ForeColor = System.Drawing.Color.DarkBlue;
            this.lblMaPhieu.Location = new System.Drawing.Point(40, 90);
            this.lblMaPhieu.Name = "lblMaPhieu";
            this.lblMaPhieu.Size = new System.Drawing.Size(58, 15);
            this.lblMaPhieu.TabIndex = 0;
            this.lblMaPhieu.Text = "Mã phiếu";
            // 
            // lblDocGia
            // 
            this.lblDocGia.AutoSize = true;
            this.lblDocGia.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDocGia.ForeColor = System.Drawing.Color.DarkBlue;
            this.lblDocGia.Location = new System.Drawing.Point(500, 90);
            this.lblDocGia.Name = "lblDocGia";
            this.lblDocGia.Size = new System.Drawing.Size(48, 15);
            this.lblDocGia.TabIndex = 1;
            this.lblDocGia.Text = "Độc giả";
            // 
            // lblNhanVien
            // 
            this.lblNhanVien.AutoSize = true;
            this.lblNhanVien.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNhanVien.ForeColor = System.Drawing.Color.DarkBlue;
            this.lblNhanVien.Location = new System.Drawing.Point(40, 135);
            this.lblNhanVien.Name = "lblNhanVien";
            this.lblNhanVien.Size = new System.Drawing.Size(63, 15);
            this.lblNhanVien.TabIndex = 2;
            this.lblNhanVien.Text = "Nhân viên";
            // 
            // lblNgayMuon
            // 
            this.lblNgayMuon.AutoSize = true;
            this.lblNgayMuon.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNgayMuon.ForeColor = System.Drawing.Color.DarkBlue;
            this.lblNgayMuon.Location = new System.Drawing.Point(500, 135);
            this.lblNgayMuon.Name = "lblNgayMuon";
            this.lblNgayMuon.Size = new System.Drawing.Size(72, 15);
            this.lblNgayMuon.TabIndex = 3;
            this.lblNgayMuon.Text = "Ngày mượn";
            // 
            // lblHanTra
            // 
            this.lblHanTra.AutoSize = true;
            this.lblHanTra.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHanTra.ForeColor = System.Drawing.Color.DarkBlue;
            this.lblHanTra.Location = new System.Drawing.Point(40, 180);
            this.lblHanTra.Name = "lblHanTra";
            this.lblHanTra.Size = new System.Drawing.Size(48, 15);
            this.lblHanTra.TabIndex = 4;
            this.lblHanTra.Text = "Hạn trả";
            // 
            // lblSach
            // 
            this.lblSach.AutoSize = true;
            this.lblSach.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSach.ForeColor = System.Drawing.Color.DarkBlue;
            this.lblSach.Location = new System.Drawing.Point(500, 180);
            this.lblSach.Name = "lblSach";
            this.lblSach.Size = new System.Drawing.Size(33, 15);
            this.lblSach.TabIndex = 5;
            this.lblSach.Text = "Sách";
            // 
            // txtMaPhieu
            // 
            this.txtMaPhieu.BackColor = System.Drawing.Color.White;
            this.txtMaPhieu.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtMaPhieu.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMaPhieu.ForeColor = System.Drawing.Color.Black;
            this.txtMaPhieu.Location = new System.Drawing.Point(160, 88);
            this.txtMaPhieu.Name = "txtMaPhieu";
            this.txtMaPhieu.Size = new System.Drawing.Size(260, 23);
            this.txtMaPhieu.TabIndex = 6;
            // 
            // cboDocGia
            // 
            this.cboDocGia.BackColor = System.Drawing.Color.White;
            this.cboDocGia.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboDocGia.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboDocGia.ForeColor = System.Drawing.Color.Black;
            this.cboDocGia.FormattingEnabled = true;
            this.cboDocGia.Location = new System.Drawing.Point(620, 88);
            this.cboDocGia.Name = "cboDocGia";
            this.cboDocGia.Size = new System.Drawing.Size(300, 23);
            this.cboDocGia.TabIndex = 7;
            // 
            // cboNhanVien
            // 
            this.cboNhanVien.BackColor = System.Drawing.Color.White;
            this.cboNhanVien.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboNhanVien.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboNhanVien.ForeColor = System.Drawing.Color.Black;
            this.cboNhanVien.FormattingEnabled = true;
            this.cboNhanVien.Location = new System.Drawing.Point(160, 133);
            this.cboNhanVien.Name = "cboNhanVien";
            this.cboNhanVien.Size = new System.Drawing.Size(260, 23);
            this.cboNhanVien.TabIndex = 8;
            // 
            // dtpNgayMuon
            // 
            this.dtpNgayMuon.CalendarFont = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtpNgayMuon.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpNgayMuon.Location = new System.Drawing.Point(620, 133);
            this.dtpNgayMuon.Name = "dtpNgayMuon";
            this.dtpNgayMuon.Size = new System.Drawing.Size(300, 20);
            this.dtpNgayMuon.TabIndex = 9;
            // 
            // dtpHanTra
            // 
            this.dtpHanTra.CalendarFont = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtpHanTra.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpHanTra.Location = new System.Drawing.Point(160, 178);
            this.dtpHanTra.Name = "dtpHanTra";
            this.dtpHanTra.Size = new System.Drawing.Size(260, 20);
            this.dtpHanTra.TabIndex = 10;
            // 
            // txtSach
            // 
            this.txtSach.BackColor = System.Drawing.Color.White;
            this.txtSach.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSach.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSach.ForeColor = System.Drawing.Color.Black;
            this.txtSach.Location = new System.Drawing.Point(620, 178);
            this.txtSach.Name = "txtSach";
            this.txtSach.Size = new System.Drawing.Size(300, 23);
            this.txtSach.TabIndex = 11;
            // 
            // btnThem
            // 
            this.btnThem.BackColor = System.Drawing.Color.RoyalBlue;
            this.btnThem.FlatAppearance.BorderSize = 0;
            this.btnThem.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnThem.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnThem.ForeColor = System.Drawing.Color.White;
            this.btnThem.Location = new System.Drawing.Point(40, 225);
            this.btnThem.Name = "btnThem";
            this.btnThem.Size = new System.Drawing.Size(120, 35);
            this.btnThem.TabIndex = 12;
            this.btnThem.Text = "Thêm";
            this.btnThem.UseVisualStyleBackColor = false;
            this.btnThem.Click += new System.EventHandler(this.btnThem_Click);
            // 
            // btnSua
            // 
            this.btnSua.BackColor = System.Drawing.Color.SteelBlue;
            this.btnSua.FlatAppearance.BorderSize = 0;
            this.btnSua.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSua.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSua.ForeColor = System.Drawing.Color.White;
            this.btnSua.Location = new System.Drawing.Point(180, 225);
            this.btnSua.Name = "btnSua";
            this.btnSua.Size = new System.Drawing.Size(120, 35);
            this.btnSua.TabIndex = 13;
            this.btnSua.Text = "Sửa";
            this.btnSua.UseVisualStyleBackColor = false;
            this.btnSua.Click += new System.EventHandler(this.btnSua_Click);
            // 
            // btnXoa
            // 
            this.btnXoa.BackColor = System.Drawing.Color.IndianRed;
            this.btnXoa.FlatAppearance.BorderSize = 0;
            this.btnXoa.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnXoa.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnXoa.ForeColor = System.Drawing.Color.White;
            this.btnXoa.Location = new System.Drawing.Point(320, 225);
            this.btnXoa.Name = "btnXoa";
            this.btnXoa.Size = new System.Drawing.Size(120, 35);
            this.btnXoa.TabIndex = 14;
            this.btnXoa.Text = "Xóa";
            this.btnXoa.UseVisualStyleBackColor = false;
            this.btnXoa.Click += new System.EventHandler(this.btnXoa_Click);
            // 
            // btnLamMoi
            // 
            this.btnLamMoi.BackColor = System.Drawing.Color.SeaGreen;
            this.btnLamMoi.FlatAppearance.BorderSize = 0;
            this.btnLamMoi.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLamMoi.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLamMoi.ForeColor = System.Drawing.Color.White;
            this.btnLamMoi.Location = new System.Drawing.Point(460, 225);
            this.btnLamMoi.Name = "btnLamMoi";
            this.btnLamMoi.Size = new System.Drawing.Size(120, 35);
            this.btnLamMoi.TabIndex = 15;
            this.btnLamMoi.Text = "Làm mới";
            this.btnLamMoi.UseVisualStyleBackColor = false;
            this.btnLamMoi.Click += new System.EventHandler(this.btnLamMoi_Click);
            // 
            // btnLapPhieu
            // 
            this.btnLapPhieu.BackColor = System.Drawing.Color.DarkOrange;
            this.btnLapPhieu.FlatAppearance.BorderSize = 0;
            this.btnLapPhieu.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLapPhieu.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLapPhieu.ForeColor = System.Drawing.Color.White;
            this.btnLapPhieu.Location = new System.Drawing.Point(600, 225);
            this.btnLapPhieu.Name = "btnLapPhieu";
            this.btnLapPhieu.Size = new System.Drawing.Size(130, 35);
            this.btnLapPhieu.TabIndex = 16;
            this.btnLapPhieu.Text = "Lập phiếu";
            this.btnLapPhieu.UseVisualStyleBackColor = false;
            this.btnLapPhieu.Click += new System.EventHandler(this.btnLapPhieu_Click);
            // 
            // dgvPhieuMuon
            // 
            this.dgvPhieuMuon.AllowUserToAddRows = false;
            this.dgvPhieuMuon.AllowUserToDeleteRows = false;
            this.dgvPhieuMuon.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvPhieuMuon.BackgroundColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.RoyalBlue;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvPhieuMuon.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvPhieuMuon.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPhieuMuon.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colMaPhieu,
            this.colDocGia,
            this.colNhanVien,
            this.colNgayMuon,
            this.colHanTra,
            this.colSach});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.LightBlue;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.DarkBlue;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvPhieuMuon.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgvPhieuMuon.Location = new System.Drawing.Point(40, 285);
            this.dgvPhieuMuon.MultiSelect = false;
            this.dgvPhieuMuon.Name = "dgvPhieuMuon";
            this.dgvPhieuMuon.ReadOnly = true;
            this.dgvPhieuMuon.RowHeadersVisible = false;
            this.dgvPhieuMuon.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvPhieuMuon.Size = new System.Drawing.Size(880, 340);
            this.dgvPhieuMuon.TabIndex = 17;
            this.dgvPhieuMuon.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvPhieuMuon_CellClick);
            // 
            // colMaPhieu
            // 
            this.colMaPhieu.HeaderText = "Mã phiếu";
            this.colMaPhieu.Name = "colMaPhieu";
            this.colMaPhieu.ReadOnly = true;
            // 
            // colDocGia
            // 
            this.colDocGia.HeaderText = "Độc giả";
            this.colDocGia.Name = "colDocGia";
            this.colDocGia.ReadOnly = true;
            // 
            // colNhanVien
            // 
            this.colNhanVien.HeaderText = "Nhân viên";
            this.colNhanVien.Name = "colNhanVien";
            this.colNhanVien.ReadOnly = true;
            // 
            // colNgayMuon
            // 
            this.colNgayMuon.HeaderText = "Ngày mượn";
            this.colNgayMuon.Name = "colNgayMuon";
            this.colNgayMuon.ReadOnly = true;
            // 
            // colHanTra
            // 
            this.colHanTra.HeaderText = "Hạn trả";
            this.colHanTra.Name = "colHanTra";
            this.colHanTra.ReadOnly = true;
            // 
            // colSach
            // 
            this.colSach.HeaderText = "Sách";
            this.colSach.Name = "colSach";
            this.colSach.ReadOnly = true;
            // 
            // lblTieuDeLapPhieuMuon
            // 
            this.lblTieuDeLapPhieuMuon.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTieuDeLapPhieuMuon.ForeColor = System.Drawing.Color.DarkBlue;
            this.lblTieuDeLapPhieuMuon.Location = new System.Drawing.Point(30, 25);
            this.lblTieuDeLapPhieuMuon.Name = "lblTieuDeLapPhieuMuon";
            this.lblTieuDeLapPhieuMuon.Size = new System.Drawing.Size(940, 35);
            this.lblTieuDeLapPhieuMuon.TabIndex = 18;
            this.lblTieuDeLapPhieuMuon.Text = "LẬP PHIẾU MƯỢN";
            this.lblTieuDeLapPhieuMuon.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // FrmLapPhieuMuon
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(984, 661);
            this.Controls.Add(this.lblTieuDeLapPhieuMuon);
            this.Controls.Add(this.dgvPhieuMuon);
            this.Controls.Add(this.btnLapPhieu);
            this.Controls.Add(this.btnLamMoi);
            this.Controls.Add(this.btnXoa);
            this.Controls.Add(this.btnSua);
            this.Controls.Add(this.btnThem);
            this.Controls.Add(this.txtSach);
            this.Controls.Add(this.dtpHanTra);
            this.Controls.Add(this.dtpNgayMuon);
            this.Controls.Add(this.cboNhanVien);
            this.Controls.Add(this.cboDocGia);
            this.Controls.Add(this.txtMaPhieu);
            this.Controls.Add(this.lblSach);
            this.Controls.Add(this.lblHanTra);
            this.Controls.Add(this.lblNgayMuon);
            this.Controls.Add(this.lblNhanVien);
            this.Controls.Add(this.lblDocGia);
            this.Controls.Add(this.lblMaPhieu);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "FrmLapPhieuMuon";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "LẬP PHIẾU MƯỢN";
            ((System.ComponentModel.ISupportInitialize)(this.dgvPhieuMuon)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblMaPhieu;
        private System.Windows.Forms.Label lblDocGia;
        private System.Windows.Forms.Label lblNhanVien;
        private System.Windows.Forms.Label lblNgayMuon;
        private System.Windows.Forms.Label lblHanTra;
        private System.Windows.Forms.Label lblSach;
        private System.Windows.Forms.TextBox txtMaPhieu;
        private System.Windows.Forms.ComboBox cboDocGia;
        private System.Windows.Forms.ComboBox cboNhanVien;
        private System.Windows.Forms.DateTimePicker dtpNgayMuon;
        private System.Windows.Forms.DateTimePicker dtpHanTra;
        private System.Windows.Forms.TextBox txtSach;
        private System.Windows.Forms.Button btnThem;
        private System.Windows.Forms.Button btnSua;
        private System.Windows.Forms.Button btnXoa;
        private System.Windows.Forms.Button btnLamMoi;
        private System.Windows.Forms.Button btnLapPhieu;
        private System.Windows.Forms.DataGridView dgvPhieuMuon;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMaPhieu;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDocGia;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNhanVien;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNgayMuon;
        private System.Windows.Forms.DataGridViewTextBoxColumn colHanTra;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSach;
        private System.Windows.Forms.Label lblTieuDeLapPhieuMuon;
    }
}