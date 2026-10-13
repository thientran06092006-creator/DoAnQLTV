namespace DoAnQLTV
{
    partial class FrmTraSach
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
            this.lblSach = new System.Windows.Forms.Label();
            this.lblNgayMuon = new System.Windows.Forms.Label();
            this.lblHanTra = new System.Windows.Forms.Label();
            this.lblNgayTra = new System.Windows.Forms.Label();
            this.txtMaPhieu = new System.Windows.Forms.TextBox();
            this.cboDocGia = new System.Windows.Forms.ComboBox();
            this.cboSach = new System.Windows.Forms.ComboBox();
            this.dtpNgayMuon = new System.Windows.Forms.DateTimePicker();
            this.dtpHanTra = new System.Windows.Forms.DateTimePicker();
            this.dtpNgayTra = new System.Windows.Forms.DateTimePicker();
            this.btnTimKiem = new System.Windows.Forms.Button();
            this.btnTraSach = new System.Windows.Forms.Button();
            this.btnLamMoi = new System.Windows.Forms.Button();
            this.dgvTraSach = new System.Windows.Forms.DataGridView();
            this.colMaPhieu = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDocGia = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSach = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colNgayMuon = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colHanTra = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colNgayTra = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lblTieuDeTraSach = new System.Windows.Forms.Label();
            this.pnlThongTin = new System.Windows.Forms.Panel();
            this.pnlDanhSach = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTraSach)).BeginInit();
            this.pnlThongTin.SuspendLayout();
            this.pnlDanhSach.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblTieuDeTraSach
            // 
            this.lblTieuDeTraSach.BackColor = System.Drawing.Color.Transparent;
            this.lblTieuDeTraSach.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTieuDeTraSach.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(58)))), ((int)(((byte)(138)))));
            this.lblTieuDeTraSach.Location = new System.Drawing.Point(30, 14);
            this.lblTieuDeTraSach.Name = "lblTieuDeTraSach";
            this.lblTieuDeTraSach.Size = new System.Drawing.Size(600, 40);
            this.lblTieuDeTraSach.TabIndex = 0;
            this.lblTieuDeTraSach.Text = "TRẢ SÁCH";
            this.lblTieuDeTraSach.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pnlThongTin
            // 
            this.pnlThongTin.BackColor = System.Drawing.Color.White;
            this.pnlThongTin.Controls.Add(this.lblMaPhieu);
            this.pnlThongTin.Controls.Add(this.txtMaPhieu);
            this.pnlThongTin.Controls.Add(this.lblDocGia);
            this.pnlThongTin.Controls.Add(this.cboDocGia);
            this.pnlThongTin.Controls.Add(this.lblSach);
            this.pnlThongTin.Controls.Add(this.cboSach);
            this.pnlThongTin.Controls.Add(this.lblNgayMuon);
            this.pnlThongTin.Controls.Add(this.dtpNgayMuon);
            this.pnlThongTin.Controls.Add(this.lblHanTra);
            this.pnlThongTin.Controls.Add(this.dtpHanTra);
            this.pnlThongTin.Controls.Add(this.lblNgayTra);
            this.pnlThongTin.Controls.Add(this.dtpNgayTra);
            this.pnlThongTin.Controls.Add(this.btnTraSach);
            this.pnlThongTin.Controls.Add(this.btnTimKiem);
            this.pnlThongTin.Controls.Add(this.btnLamMoi);
            this.pnlThongTin.Location = new System.Drawing.Point(30, 64);
            this.pnlThongTin.Name = "pnlThongTin";
            this.pnlThongTin.Size = new System.Drawing.Size(940, 252);
            this.pnlThongTin.TabIndex = 1;
            // 
            // lblMaPhieu
            // 
            this.lblMaPhieu.AutoSize = true;
            this.lblMaPhieu.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMaPhieu.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(58)))), ((int)(((byte)(138)))));
            this.lblMaPhieu.Location = new System.Drawing.Point(24, 28);
            this.lblMaPhieu.Name = "lblMaPhieu";
            this.lblMaPhieu.Size = new System.Drawing.Size(65, 19);
            this.lblMaPhieu.TabIndex = 9;
            this.lblMaPhieu.Text = "Mã phiếu";
            // 
            // txtMaPhieu
            // 
            this.txtMaPhieu.BackColor = System.Drawing.Color.White;
            this.txtMaPhieu.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtMaPhieu.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMaPhieu.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(41)))), ((int)(((byte)(55)))));
            this.txtMaPhieu.Location = new System.Drawing.Point(110, 24);
            this.txtMaPhieu.Name = "txtMaPhieu";
            this.txtMaPhieu.Size = new System.Drawing.Size(340, 27);
            this.txtMaPhieu.TabIndex = 0;
            // 
            // lblDocGia
            // 
            this.lblDocGia.AutoSize = true;
            this.lblDocGia.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDocGia.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(58)))), ((int)(((byte)(138)))));
            this.lblDocGia.Location = new System.Drawing.Point(476, 28);
            this.lblDocGia.Name = "lblDocGia";
            this.lblDocGia.Size = new System.Drawing.Size(53, 19);
            this.lblDocGia.TabIndex = 10;
            this.lblDocGia.Text = "Độc giả";
            // 
            // cboDocGia
            // 
            this.cboDocGia.BackColor = System.Drawing.Color.White;
            this.cboDocGia.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboDocGia.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cboDocGia.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboDocGia.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(41)))), ((int)(((byte)(55)))));
            this.cboDocGia.FormattingEnabled = true;
            this.cboDocGia.Location = new System.Drawing.Point(570, 24);
            this.cboDocGia.Name = "cboDocGia";
            this.cboDocGia.Size = new System.Drawing.Size(330, 28);
            this.cboDocGia.TabIndex = 1;
            // 
            // lblSach
            // 
            this.lblSach.AutoSize = true;
            this.lblSach.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSach.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(58)))), ((int)(((byte)(138)))));
            this.lblSach.Location = new System.Drawing.Point(24, 80);
            this.lblSach.Name = "lblSach";
            this.lblSach.Size = new System.Drawing.Size(40, 19);
            this.lblSach.TabIndex = 11;
            this.lblSach.Text = "Sách";
            // 
            // cboSach
            // 
            this.cboSach.BackColor = System.Drawing.Color.White;
            this.cboSach.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSach.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cboSach.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboSach.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(41)))), ((int)(((byte)(55)))));
            this.cboSach.FormattingEnabled = true;
            this.cboSach.Location = new System.Drawing.Point(110, 76);
            this.cboSach.Name = "cboSach";
            this.cboSach.Size = new System.Drawing.Size(340, 28);
            this.cboSach.TabIndex = 2;
            // 
            // lblNgayMuon
            // 
            this.lblNgayMuon.AutoSize = true;
            this.lblNgayMuon.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNgayMuon.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(58)))), ((int)(((byte)(138)))));
            this.lblNgayMuon.Location = new System.Drawing.Point(476, 80);
            this.lblNgayMuon.Name = "lblNgayMuon";
            this.lblNgayMuon.Size = new System.Drawing.Size(77, 19);
            this.lblNgayMuon.TabIndex = 12;
            this.lblNgayMuon.Text = "Ngày mượn";
            // 
            // dtpNgayMuon
            // 
            this.dtpNgayMuon.CalendarFont = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtpNgayMuon.CustomFormat = "dd/MM/yyyy";
            this.dtpNgayMuon.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtpNgayMuon.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpNgayMuon.Location = new System.Drawing.Point(570, 76);
            this.dtpNgayMuon.Name = "dtpNgayMuon";
            this.dtpNgayMuon.Size = new System.Drawing.Size(330, 27);
            this.dtpNgayMuon.TabIndex = 3;
            // 
            // lblHanTra
            // 
            this.lblHanTra.AutoSize = true;
            this.lblHanTra.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHanTra.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(58)))), ((int)(((byte)(138)))));
            this.lblHanTra.Location = new System.Drawing.Point(24, 132);
            this.lblHanTra.Name = "lblHanTra";
            this.lblHanTra.Size = new System.Drawing.Size(54, 19);
            this.lblHanTra.TabIndex = 13;
            this.lblHanTra.Text = "Hạn trả";
            // 
            // dtpHanTra
            // 
            this.dtpHanTra.CalendarFont = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtpHanTra.CustomFormat = "dd/MM/yyyy";
            this.dtpHanTra.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtpHanTra.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpHanTra.Location = new System.Drawing.Point(110, 128);
            this.dtpHanTra.Name = "dtpHanTra";
            this.dtpHanTra.Size = new System.Drawing.Size(340, 27);
            this.dtpHanTra.TabIndex = 4;
            // 
            // lblNgayTra
            // 
            this.lblNgayTra.AutoSize = true;
            this.lblNgayTra.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNgayTra.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(58)))), ((int)(((byte)(138)))));
            this.lblNgayTra.Location = new System.Drawing.Point(476, 132);
            this.lblNgayTra.Name = "lblNgayTra";
            this.lblNgayTra.Size = new System.Drawing.Size(63, 19);
            this.lblNgayTra.TabIndex = 14;
            this.lblNgayTra.Text = "Ngày trả";
            // 
            // dtpNgayTra
            // 
            this.dtpNgayTra.CalendarFont = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtpNgayTra.CustomFormat = "dd/MM/yyyy";
            this.dtpNgayTra.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtpNgayTra.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpNgayTra.Location = new System.Drawing.Point(570, 128);
            this.dtpNgayTra.Name = "dtpNgayTra";
            this.dtpNgayTra.Size = new System.Drawing.Size(330, 27);
            this.dtpNgayTra.TabIndex = 5;
            // 
            // btnTraSach
            // 
            this.btnTraSach.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(99)))), ((int)(((byte)(235)))));
            this.btnTraSach.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnTraSach.FlatAppearance.BorderSize = 0;
            this.btnTraSach.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTraSach.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTraSach.ForeColor = System.Drawing.Color.White;
            this.btnTraSach.Location = new System.Drawing.Point(24, 188);
            this.btnTraSach.Name = "btnTraSach";
            this.btnTraSach.Size = new System.Drawing.Size(120, 40);
            this.btnTraSach.TabIndex = 6;
            this.btnTraSach.Text = "Trả sách";
            this.btnTraSach.UseVisualStyleBackColor = false;
            this.btnTraSach.Click += new System.EventHandler(this.btnTraSach_Click);
            // 
            // btnTimKiem
            // 
            this.btnTimKiem.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(163)))), ((int)(((byte)(74)))));
            this.btnTimKiem.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnTimKiem.FlatAppearance.BorderSize = 0;
            this.btnTimKiem.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTimKiem.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTimKiem.ForeColor = System.Drawing.Color.White;
            this.btnTimKiem.Location = new System.Drawing.Point(156, 188);
            this.btnTimKiem.Name = "btnTimKiem";
            this.btnTimKiem.Size = new System.Drawing.Size(120, 40);
            this.btnTimKiem.TabIndex = 7;
            this.btnTimKiem.Text = "Tìm kiếm";
            this.btnTimKiem.UseVisualStyleBackColor = false;
            this.btnTimKiem.Click += new System.EventHandler(this.btnTimKiem_Click);
            // 
            // btnLamMoi
            // 
            this.btnLamMoi.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(232)))), ((int)(((byte)(240)))));
            this.btnLamMoi.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnLamMoi.FlatAppearance.BorderSize = 0;
            this.btnLamMoi.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLamMoi.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLamMoi.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(41)))), ((int)(((byte)(55)))));
            this.btnLamMoi.Location = new System.Drawing.Point(288, 188);
            this.btnLamMoi.Name = "btnLamMoi";
            this.btnLamMoi.Size = new System.Drawing.Size(120, 40);
            this.btnLamMoi.TabIndex = 8;
            this.btnLamMoi.Text = "Làm mới";
            this.btnLamMoi.UseVisualStyleBackColor = false;
            this.btnLamMoi.Click += new System.EventHandler(this.btnLamMoi_Click);
            // 
            // pnlDanhSach
            // 
            this.pnlDanhSach.BackColor = System.Drawing.Color.White;
            this.pnlDanhSach.Controls.Add(this.dgvTraSach);
            this.pnlDanhSach.Location = new System.Drawing.Point(30, 336);
            this.pnlDanhSach.Name = "pnlDanhSach";
            this.pnlDanhSach.Padding = new System.Windows.Forms.Padding(1);
            this.pnlDanhSach.Size = new System.Drawing.Size(940, 304);
            this.pnlDanhSach.TabIndex = 2;
            // 
            // dgvTraSach
            // 
            this.dgvTraSach.AllowUserToAddRows = false;
            this.dgvTraSach.AllowUserToDeleteRows = false;
            this.dgvTraSach.AllowUserToResizeRows = false;
            this.dgvTraSach.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvTraSach.BackgroundColor = System.Drawing.Color.White;
            this.dgvTraSach.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(58)))), ((int)(((byte)(138)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(58)))), ((int)(((byte)(138)))));
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvTraSach.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvTraSach.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvTraSach.ColumnHeadersHeight = 40;
            this.dgvTraSach.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colMaPhieu,
            this.colDocGia,
            this.colSach,
            this.colNgayMuon,
            this.colHanTra,
            this.colNgayTra});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(41)))), ((int)(((byte)(55)))));
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(219)))), ((int)(((byte)(234)))), ((int)(((byte)(254)))));
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(41)))), ((int)(((byte)(55)))));
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvTraSach.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgvTraSach.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvTraSach.Location = new System.Drawing.Point(1, 1);
            this.dgvTraSach.MultiSelect = false;
            this.dgvTraSach.Name = "dgvTraSach";
            this.dgvTraSach.ReadOnly = true;
            this.dgvTraSach.RowHeadersVisible = false;
            this.dgvTraSach.RowHeadersWidth = 51;
            this.dgvTraSach.RowTemplate.Height = 34;
            this.dgvTraSach.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvTraSach.Size = new System.Drawing.Size(938, 302);
            this.dgvTraSach.TabIndex = 0;
            this.dgvTraSach.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvTraSach_CellClick);
            // 
            // colMaPhieu
            // 
            this.colMaPhieu.FillWeight = 60F;
            this.colMaPhieu.HeaderText = "Mã phiếu";
            this.colMaPhieu.MinimumWidth = 6;
            this.colMaPhieu.Name = "colMaPhieu";
            this.colMaPhieu.ReadOnly = true;
            // 
            // colDocGia
            // 
            this.colDocGia.FillWeight = 130F;
            this.colDocGia.HeaderText = "Độc giả";
            this.colDocGia.MinimumWidth = 6;
            this.colDocGia.Name = "colDocGia";
            this.colDocGia.ReadOnly = true;
            // 
            // colSach
            // 
            this.colSach.FillWeight = 150F;
            this.colSach.HeaderText = "Sách";
            this.colSach.MinimumWidth = 6;
            this.colSach.Name = "colSach";
            this.colSach.ReadOnly = true;
            // 
            // colNgayMuon
            // 
            this.colNgayMuon.FillWeight = 80F;
            this.colNgayMuon.HeaderText = "Ngày mượn";
            this.colNgayMuon.MinimumWidth = 6;
            this.colNgayMuon.Name = "colNgayMuon";
            this.colNgayMuon.ReadOnly = true;
            // 
            // colHanTra
            // 
            this.colHanTra.FillWeight = 80F;
            this.colHanTra.HeaderText = "Hạn trả";
            this.colHanTra.MinimumWidth = 6;
            this.colHanTra.Name = "colHanTra";
            this.colHanTra.ReadOnly = true;
            // 
            // colNgayTra
            // 
            this.colNgayTra.FillWeight = 80F;
            this.colNgayTra.HeaderText = "Ngày trả";
            this.colNgayTra.MinimumWidth = 6;
            this.colNgayTra.Name = "colNgayTra";
            this.colNgayTra.ReadOnly = true;
            // 
            // FrmTraSach
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(246)))), ((int)(((byte)(251)))));
            this.ClientSize = new System.Drawing.Size(1000, 670);
            this.Controls.Add(this.pnlDanhSach);
            this.Controls.Add(this.pnlThongTin);
            this.Controls.Add(this.lblTieuDeTraSach);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "FrmTraSach";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Trả Sách";
            ((System.ComponentModel.ISupportInitialize)(this.dgvTraSach)).EndInit();
            this.pnlThongTin.ResumeLayout(false);
            this.pnlThongTin.PerformLayout();
            this.pnlDanhSach.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblMaPhieu;
        private System.Windows.Forms.Label lblDocGia;
        private System.Windows.Forms.Label lblSach;
        private System.Windows.Forms.Label lblNgayMuon;
        private System.Windows.Forms.Label lblHanTra;
        private System.Windows.Forms.Label lblNgayTra;
        private System.Windows.Forms.TextBox txtMaPhieu;
        private System.Windows.Forms.ComboBox cboDocGia;
        private System.Windows.Forms.ComboBox cboSach;
        private System.Windows.Forms.DateTimePicker dtpNgayMuon;
        private System.Windows.Forms.DateTimePicker dtpHanTra;
        private System.Windows.Forms.DateTimePicker dtpNgayTra;
        private System.Windows.Forms.Button btnTimKiem;
        private System.Windows.Forms.Button btnTraSach;
        private System.Windows.Forms.Button btnLamMoi;
        private System.Windows.Forms.DataGridView dgvTraSach;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMaPhieu;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDocGia;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSach;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNgayMuon;
        private System.Windows.Forms.DataGridViewTextBoxColumn colHanTra;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNgayTra;
        private System.Windows.Forms.Label lblTieuDeTraSach;
        private System.Windows.Forms.Panel pnlThongTin;
        private System.Windows.Forms.Panel pnlDanhSach;
    }
}