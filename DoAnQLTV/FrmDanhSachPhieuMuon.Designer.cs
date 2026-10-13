namespace DoAnQLTV
{
    partial class FrmDanhSachPhieuMuon
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
            this.lblTuKhoa = new System.Windows.Forms.Label();
            this.txtTuKhoa = new System.Windows.Forms.TextBox();
            this.btnTimKiem = new System.Windows.Forms.Button();
            this.btnLamMoi = new System.Windows.Forms.Button();
            this.dgvPhieuMuon = new System.Windows.Forms.DataGridView();
            this.colMaPhieu = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDocGia = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colNhanVien = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colNgayMuon = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colHanTra = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSach = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lblTieuDeDanhSachPhieuMuon = new System.Windows.Forms.Label();
            this.pnlThongTin = new System.Windows.Forms.Panel();
            this.pnlDanhSach = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPhieuMuon)).BeginInit();
            this.pnlThongTin.SuspendLayout();
            this.pnlDanhSach.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblTieuDeDanhSachPhieuMuon
            // 
            this.lblTieuDeDanhSachPhieuMuon.BackColor = System.Drawing.Color.Transparent;
            this.lblTieuDeDanhSachPhieuMuon.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTieuDeDanhSachPhieuMuon.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(58)))), ((int)(((byte)(138)))));
            this.lblTieuDeDanhSachPhieuMuon.Location = new System.Drawing.Point(30, 14);
            this.lblTieuDeDanhSachPhieuMuon.Name = "lblTieuDeDanhSachPhieuMuon";
            this.lblTieuDeDanhSachPhieuMuon.Size = new System.Drawing.Size(600, 40);
            this.lblTieuDeDanhSachPhieuMuon.TabIndex = 0;
            this.lblTieuDeDanhSachPhieuMuon.Text = "DANH SÁCH PHIẾU MƯỢN";
            this.lblTieuDeDanhSachPhieuMuon.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pnlThongTin
            // 
            this.pnlThongTin.BackColor = System.Drawing.Color.White;
            this.pnlThongTin.Controls.Add(this.lblTuKhoa);
            this.pnlThongTin.Controls.Add(this.txtTuKhoa);
            this.pnlThongTin.Controls.Add(this.btnTimKiem);
            this.pnlThongTin.Controls.Add(this.btnLamMoi);
            this.pnlThongTin.Location = new System.Drawing.Point(30, 64);
            this.pnlThongTin.Name = "pnlThongTin";
            this.pnlThongTin.Size = new System.Drawing.Size(940, 76);
            this.pnlThongTin.TabIndex = 1;
            // 
            // lblTuKhoa
            // 
            this.lblTuKhoa.AutoSize = true;
            this.lblTuKhoa.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTuKhoa.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(58)))), ((int)(((byte)(138)))));
            this.lblTuKhoa.Location = new System.Drawing.Point(24, 28);
            this.lblTuKhoa.Name = "lblTuKhoa";
            this.lblTuKhoa.Size = new System.Drawing.Size(60, 19);
            this.lblTuKhoa.TabIndex = 4;
            this.lblTuKhoa.Text = "Từ khóa";
            // 
            // txtTuKhoa
            // 
            this.txtTuKhoa.BackColor = System.Drawing.Color.White;
            this.txtTuKhoa.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtTuKhoa.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTuKhoa.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(41)))), ((int)(((byte)(55)))));
            this.txtTuKhoa.Location = new System.Drawing.Point(110, 24);
            this.txtTuKhoa.Name = "txtTuKhoa";
            this.txtTuKhoa.Size = new System.Drawing.Size(520, 27);
            this.txtTuKhoa.TabIndex = 0;
            // 
            // btnTimKiem
            // 
            this.btnTimKiem.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(163)))), ((int)(((byte)(74)))));
            this.btnTimKiem.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnTimKiem.FlatAppearance.BorderSize = 0;
            this.btnTimKiem.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTimKiem.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTimKiem.ForeColor = System.Drawing.Color.White;
            this.btnTimKiem.Location = new System.Drawing.Point(650, 18);
            this.btnTimKiem.Name = "btnTimKiem";
            this.btnTimKiem.Size = new System.Drawing.Size(120, 40);
            this.btnTimKiem.TabIndex = 1;
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
            this.btnLamMoi.Location = new System.Drawing.Point(782, 18);
            this.btnLamMoi.Name = "btnLamMoi";
            this.btnLamMoi.Size = new System.Drawing.Size(120, 40);
            this.btnLamMoi.TabIndex = 2;
            this.btnLamMoi.Text = "Làm mới";
            this.btnLamMoi.UseVisualStyleBackColor = false;
            this.btnLamMoi.Click += new System.EventHandler(this.btnLamMoi_Click);
            // 
            // pnlDanhSach
            // 
            this.pnlDanhSach.BackColor = System.Drawing.Color.White;
            this.pnlDanhSach.Controls.Add(this.dgvPhieuMuon);
            this.pnlDanhSach.Location = new System.Drawing.Point(30, 160);
            this.pnlDanhSach.Name = "pnlDanhSach";
            this.pnlDanhSach.Padding = new System.Windows.Forms.Padding(1);
            this.pnlDanhSach.Size = new System.Drawing.Size(940, 440);
            this.pnlDanhSach.TabIndex = 2;
            // 
            // dgvPhieuMuon
            // 
            this.dgvPhieuMuon.AllowUserToAddRows = false;
            this.dgvPhieuMuon.AllowUserToDeleteRows = false;
            this.dgvPhieuMuon.AllowUserToResizeRows = false;
            this.dgvPhieuMuon.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvPhieuMuon.BackgroundColor = System.Drawing.Color.White;
            this.dgvPhieuMuon.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(58)))), ((int)(((byte)(138)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(58)))), ((int)(((byte)(138)))));
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvPhieuMuon.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvPhieuMuon.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvPhieuMuon.ColumnHeadersHeight = 40;
            this.dgvPhieuMuon.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colMaPhieu,
            this.colDocGia,
            this.colNhanVien,
            this.colNgayMuon,
            this.colHanTra,
            this.colSach});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(41)))), ((int)(((byte)(55)))));
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(219)))), ((int)(((byte)(234)))), ((int)(((byte)(254)))));
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(41)))), ((int)(((byte)(55)))));
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvPhieuMuon.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgvPhieuMuon.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvPhieuMuon.Location = new System.Drawing.Point(1, 1);
            this.dgvPhieuMuon.MultiSelect = false;
            this.dgvPhieuMuon.Name = "dgvPhieuMuon";
            this.dgvPhieuMuon.ReadOnly = true;
            this.dgvPhieuMuon.RowHeadersVisible = false;
            this.dgvPhieuMuon.RowHeadersWidth = 51;
            this.dgvPhieuMuon.RowTemplate.Height = 34;
            this.dgvPhieuMuon.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvPhieuMuon.Size = new System.Drawing.Size(938, 438);
            this.dgvPhieuMuon.TabIndex = 0;
            this.dgvPhieuMuon.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvPhieuMuon_CellClick);
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
            // colNhanVien
            // 
            this.colNhanVien.FillWeight = 110F;
            this.colNhanVien.HeaderText = "Nhân viên";
            this.colNhanVien.MinimumWidth = 6;
            this.colNhanVien.Name = "colNhanVien";
            this.colNhanVien.ReadOnly = true;
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
            // colSach
            // 
            this.colSach.FillWeight = 150F;
            this.colSach.HeaderText = "Sách";
            this.colSach.MinimumWidth = 6;
            this.colSach.Name = "colSach";
            this.colSach.ReadOnly = true;
            // 
            // FrmDanhSachPhieuMuon
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(246)))), ((int)(((byte)(251)))));
            this.ClientSize = new System.Drawing.Size(1000, 630);
            this.Controls.Add(this.pnlDanhSach);
            this.Controls.Add(this.pnlThongTin);
            this.Controls.Add(this.lblTieuDeDanhSachPhieuMuon);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "FrmDanhSachPhieuMuon";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Danh Sách Phiếu Mượn";
            ((System.ComponentModel.ISupportInitialize)(this.dgvPhieuMuon)).EndInit();
            this.pnlThongTin.ResumeLayout(false);
            this.pnlThongTin.PerformLayout();
            this.pnlDanhSach.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblTuKhoa;
        private System.Windows.Forms.TextBox txtTuKhoa;
        private System.Windows.Forms.Button btnTimKiem;
        private System.Windows.Forms.Button btnLamMoi;
        private System.Windows.Forms.DataGridView dgvPhieuMuon;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMaPhieu;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDocGia;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNhanVien;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNgayMuon;
        private System.Windows.Forms.DataGridViewTextBoxColumn colHanTra;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSach;
        private System.Windows.Forms.Label lblTieuDeDanhSachPhieuMuon;
        private System.Windows.Forms.Panel pnlThongTin;
        private System.Windows.Forms.Panel pnlDanhSach;
    }
}