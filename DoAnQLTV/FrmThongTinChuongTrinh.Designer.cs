namespace DoAnQLTV
{
    partial class FrmThongTinChuongTrinh
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
            this.lblTieuDe = new System.Windows.Forms.Label();
            this.lblTruong = new System.Windows.Forms.Label();
            this.lblMonHoc = new System.Windows.Forms.Label();
            this.lblDeTai = new System.Windows.Forms.Label();
            this.lblNhom = new System.Windows.Forms.Label();
            this.lblGiangVien = new System.Windows.Forms.Label();
            this.btnDong = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblTieuDe
            // 
            this.lblTieuDe.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTieuDe.Location = new System.Drawing.Point(10, 11);
            this.lblTieuDe.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblTieuDe.Name = "lblTieuDe";
            this.lblTieuDe.Size = new System.Drawing.Size(300, 37);
            this.lblTieuDe.TabIndex = 0;
            this.lblTieuDe.Text = "QUẢN LÝ THƯ VIỆN";
            this.lblTieuDe.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblTruong
            // 
            this.lblTruong.AutoSize = true;
            this.lblTruong.Location = new System.Drawing.Point(28, 71);
            this.lblTruong.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblTruong.Name = "lblTruong";
            this.lblTruong.Size = new System.Drawing.Size(234, 13);
            this.lblTruong.TabIndex = 1;
            this.lblTruong.Text = "Trường Đại học Công nghệ TP.HCM (HUTECH)";
            // 
            // lblMonHoc
            // 
            this.lblMonHoc.AutoSize = true;
            this.lblMonHoc.Location = new System.Drawing.Point(28, 103);
            this.lblMonHoc.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblMonHoc.Name = "lblMonHoc";
            this.lblMonHoc.Size = new System.Drawing.Size(195, 13);
            this.lblMonHoc.TabIndex = 2;
            this.lblMonHoc.Text = "Môn: Lập trình trên môi trường Windows";
            // 
            // lblDeTai
            // 
            this.lblDeTai.AutoSize = true;
            this.lblDeTai.Location = new System.Drawing.Point(28, 137);
            this.lblDeTai.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblDeTai.Name = "lblDeTai";
            this.lblDeTai.Size = new System.Drawing.Size(118, 13);
            this.lblDeTai.TabIndex = 3;
            this.lblDeTai.Text = "Đề tài: Quản lý thư viện";
            // 
            // lblNhom
            // 
            this.lblNhom.AutoSize = true;
            this.lblNhom.Location = new System.Drawing.Point(28, 167);
            this.lblNhom.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblNhom.Name = "lblNhom";
            this.lblNhom.Size = new System.Drawing.Size(68, 13);
            this.lblNhom.TabIndex = 4;
            this.lblNhom.Text = "Nhóm: 3H1T";
            // 
            // lblGiangVien
            // 
            this.lblGiangVien.AutoSize = true;
            this.lblGiangVien.Location = new System.Drawing.Point(28, 201);
            this.lblGiangVien.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblGiangVien.Name = "lblGiangVien";
            this.lblGiangVien.Size = new System.Drawing.Size(174, 13);
            this.lblGiangVien.TabIndex = 5;
            this.lblGiangVien.Text = "Giảng viên: Th.S Nguyễn Đình Ánh";
            // 
            // btnDong
            // 
            this.btnDong.Location = new System.Drawing.Point(146, 244);
            this.btnDong.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnDong.Name = "btnDong";
            this.btnDong.Size = new System.Drawing.Size(56, 19);
            this.btnDong.TabIndex = 6;
            this.btnDong.Text = "Đóng";
            this.btnDong.UseVisualStyleBackColor = true;
            this.btnDong.Click += new System.EventHandler(this.btnDong_Click);
            // 
            // FrmThongTinChuongTrinh
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(362, 287);
            this.Controls.Add(this.btnDong);
            this.Controls.Add(this.lblGiangVien);
            this.Controls.Add(this.lblNhom);
            this.Controls.Add(this.lblDeTai);
            this.Controls.Add(this.lblMonHoc);
            this.Controls.Add(this.lblTruong);
            this.Controls.Add(this.lblTieuDe);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "FrmThongTinChuongTrinh";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Thông tin chương trình";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblTieuDe;
        private System.Windows.Forms.Label lblTruong;
        private System.Windows.Forms.Label lblMonHoc;
        private System.Windows.Forms.Label lblDeTai;
        private System.Windows.Forms.Label lblNhom;
        private System.Windows.Forms.Label lblGiangVien;
        private System.Windows.Forms.Button btnDong;
    }
}