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
            ((System.ComponentModel.ISupportInitialize)(this.dgvTraSach)).BeginInit();
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
            // lblSach
            // 
            this.lblSach.AutoSize = true;
            this.lblSach.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSach.ForeColor = System.Drawing.Color.DarkBlue;
            this.lblSach.Location = new System.Drawing.Point(40, 135);
            this.lblSach.Name = "lblSach";
            this.lblSach.Size = new System.Drawing.Size(33, 15);
            this.lblSach.TabIndex = 2;
            this.lblSach.Text = "Sách";
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
            // lblNgayTra
            // 
            this.lblNgayTra.AutoSize = true;
            this.lblNgayTra.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNgayTra.ForeColor = System.Drawing.Color.DarkBlue;
            this.lblNgayTra.Location = new System.Drawing.Point(500, 180);
            this.lblNgayTra.Name = "lblNgayTra";
            this.lblNgayTra.Size = new System.Drawing.Size(54, 15);
            this.lblNgayTra.TabIndex = 5;
            this.lblNgayTra.Text = "Ngày trả";
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
            // cboSach
            // 
            this.cboSach.BackColor = System.Drawing.Color.White;
            this.cboSach.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSach.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboSach.ForeColor = System.Drawing.Color.Black;
            this.cboSach.FormattingEnabled = true;
            this.cboSach.Location = new System.Drawing.Point(160, 133);
            this.cboSach.Name = "cboSach";
            this.cboSach.Size = new System.Drawing.Size(260, 23);
            this.cboSach.TabIndex = 8;
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
            // dtpNgayTra
            // 
            this.dtpNgayTra.CalendarFont = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtpNgayTra.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpNgayTra.Location = new System.Drawing.Point(620, 178);
            this.dtpNgayTra.Name = "dtpNgayTra";
            this.dtpNgayTra.Size = new System.Drawing.Size(300, 20);
            this.dtpNgayTra.TabIndex = 11;
            // 
            // btnTimKiem
            // 
            this.btnTimKiem.BackColor = System.Drawing.Color.DarkOrange;
            this.btnTimKiem.FlatAppearance.BorderSize = 0;
            this.btnTimKiem.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTimKiem.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTimKiem.ForeColor = System.Drawing.Color.White;
            this.btnTimKiem.Location = new System.Drawing.Point(40, 225);
            this.btnTimKiem.Name = "btnTimKiem";
            this.btnTimKiem.Size = new System.Drawing.Size(130, 35);
            this.btnTimKiem.TabIndex = 12;
            this.btnTimKiem.Text = "Tìm kiếm";
            this.btnTimKiem.UseVisualStyleBackColor = false;
            this.btnTimKiem.Click += new System.EventHandler(this.btnTimKiem_Click);
            // 
            // btnTraSach
            // 
            this.btnTraSach.BackColor = System.Drawing.Color.RoyalBlue;
            this.btnTraSach.FlatAppearance.BorderSize = 0;
            this.btnTraSach.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTraSach.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTraSach.ForeColor = System.Drawing.Color.White;
            this.btnTraSach.Location = new System.Drawing.Point(190, 225);
            this.btnTraSach.Name = "btnTraSach";
            this.btnTraSach.Size = new System.Drawing.Size(120, 35);
            this.btnTraSach.TabIndex = 13;
            this.btnTraSach.Text = "Trả sách";
            this.btnTraSach.UseVisualStyleBackColor = false;
            this.btnTraSach.Click += new System.EventHandler(this.btnTraSach_Click);
            // 
            // btnLamMoi
            // 
            this.btnLamMoi.BackColor = System.Drawing.Color.SeaGreen;
            this.btnLamMoi.FlatAppearance.BorderSize = 0;
            this.btnLamMoi.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLamMoi.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLamMoi.ForeColor = System.Drawing.Color.White;
            this.btnLamMoi.Location = new System.Drawing.Point(330, 225);
            this.btnLamMoi.Name = "btnLamMoi";
            this.btnLamMoi.Size = new System.Drawing.Size(120, 35);
            this.btnLamMoi.TabIndex = 14;
            this.btnLamMoi.Text = "Làm mới";
            this.btnLamMoi.UseVisualStyleBackColor = false;
            this.btnLamMoi.Click += new System.EventHandler(this.btnLamMoi_Click);
            // 
            // dgvTraSach
            // 
            this.dgvTraSach.AllowUserToAddRows = false;
            this.dgvTraSach.AllowUserToDeleteRows = false;
            this.dgvTraSach.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvTraSach.BackgroundColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.RoyalBlue;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvTraSach.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvTraSach.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvTraSach.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colMaPhieu,
            this.colDocGia,
            this.colSach,
            this.colNgayMuon,
            this.colHanTra,
            this.colNgayTra});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.LightBlue;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.DarkBlue;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvTraSach.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgvTraSach.Location = new System.Drawing.Point(40, 285);
            this.dgvTraSach.MultiSelect = false;
            this.dgvTraSach.Name = "dgvTraSach";
            this.dgvTraSach.ReadOnly = true;
            this.dgvTraSach.RowHeadersVisible = false;
            this.dgvTraSach.RowHeadersWidth = 51;
            this.dgvTraSach.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvTraSach.Size = new System.Drawing.Size(880, 340);
            this.dgvTraSach.TabIndex = 15;
            this.dgvTraSach.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvTraSach_CellClick);
            // 
            // colMaPhieu
            // 
            this.colMaPhieu.HeaderText = "Mã phiếu";
            this.colMaPhieu.MinimumWidth = 6;
            this.colMaPhieu.Name = "colMaPhieu";
            this.colMaPhieu.ReadOnly = true;
            // 
            // colDocGia
            // 
            this.colDocGia.HeaderText = "Độc giả";
            this.colDocGia.MinimumWidth = 6;
            this.colDocGia.Name = "colDocGia";
            this.colDocGia.ReadOnly = true;
            // 
            // colSach
            // 
            this.colSach.HeaderText = "Sách";
            this.colSach.MinimumWidth = 6;
            this.colSach.Name = "colSach";
            this.colSach.ReadOnly = true;
            // 
            // colNgayMuon
            // 
            this.colNgayMuon.HeaderText = "Ngày mượn";
            this.colNgayMuon.MinimumWidth = 6;
            this.colNgayMuon.Name = "colNgayMuon";
            this.colNgayMuon.ReadOnly = true;
            // 
            // colHanTra
            // 
            this.colHanTra.HeaderText = "Hạn trả";
            this.colHanTra.MinimumWidth = 6;
            this.colHanTra.Name = "colHanTra";
            this.colHanTra.ReadOnly = true;
            // 
            // colNgayTra
            // 
            this.colNgayTra.HeaderText = "Ngày trả";
            this.colNgayTra.MinimumWidth = 6;
            this.colNgayTra.Name = "colNgayTra";
            this.colNgayTra.ReadOnly = true;
            // 
            // lblTieuDeTraSach
            // 
            this.lblTieuDeTraSach.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTieuDeTraSach.ForeColor = System.Drawing.Color.DarkBlue;
            this.lblTieuDeTraSach.Location = new System.Drawing.Point(30, 25);
            this.lblTieuDeTraSach.Name = "lblTieuDeTraSach";
            this.lblTieuDeTraSach.Size = new System.Drawing.Size(940, 35);
            this.lblTieuDeTraSach.TabIndex = 16;
            this.lblTieuDeTraSach.Text = "TRẢ SÁCH";
            this.lblTieuDeTraSach.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // FrmTraSach
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(984, 661);
            this.Controls.Add(this.lblTieuDeTraSach);
            this.Controls.Add(this.dgvTraSach);
            this.Controls.Add(this.btnLamMoi);
            this.Controls.Add(this.btnTraSach);
            this.Controls.Add(this.btnTimKiem);
            this.Controls.Add(this.dtpNgayTra);
            this.Controls.Add(this.dtpHanTra);
            this.Controls.Add(this.dtpNgayMuon);
            this.Controls.Add(this.cboSach);
            this.Controls.Add(this.cboDocGia);
            this.Controls.Add(this.txtMaPhieu);
            this.Controls.Add(this.lblNgayTra);
            this.Controls.Add(this.lblHanTra);
            this.Controls.Add(this.lblNgayMuon);
            this.Controls.Add(this.lblSach);
            this.Controls.Add(this.lblDocGia);
            this.Controls.Add(this.lblMaPhieu);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "FrmTraSach";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "TRẢ SÁCH";
            ((System.ComponentModel.ISupportInitialize)(this.dgvTraSach)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

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
    }
}