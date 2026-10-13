namespace DoAnQLTV
{
    partial class FrmHuongDanSuDung
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmHuongDanSuDung));
            this.lblTieuDe = new System.Windows.Forms.Label();
            this.rtbHuongDan = new System.Windows.Forms.RichTextBox();
            this.btnDong = new System.Windows.Forms.Button();
            this.pnlNoiDung = new System.Windows.Forms.Panel();
            this.pnlNoiDung.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblTieuDe
            // 
            this.lblTieuDe.BackColor = System.Drawing.Color.Transparent;
            this.lblTieuDe.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTieuDe.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(58)))), ((int)(((byte)(138)))));
            this.lblTieuDe.Location = new System.Drawing.Point(30, 14);
            this.lblTieuDe.Name = "lblTieuDe";
            this.lblTieuDe.Size = new System.Drawing.Size(600, 40);
            this.lblTieuDe.TabIndex = 0;
            this.lblTieuDe.Text = "HƯỚNG DẪN SỬ DỤNG";
            this.lblTieuDe.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pnlNoiDung
            // 
            this.pnlNoiDung.BackColor = System.Drawing.Color.White;
            this.pnlNoiDung.Controls.Add(this.rtbHuongDan);
            this.pnlNoiDung.Location = new System.Drawing.Point(30, 64);
            this.pnlNoiDung.Name = "pnlNoiDung";
            this.pnlNoiDung.Padding = new System.Windows.Forms.Padding(18, 16, 12, 16);
            this.pnlNoiDung.Size = new System.Drawing.Size(840, 520);
            this.pnlNoiDung.TabIndex = 1;
            // 
            // rtbHuongDan
            // 
            this.rtbHuongDan.BackColor = System.Drawing.Color.White;
            this.rtbHuongDan.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtbHuongDan.DetectUrls = false;
            this.rtbHuongDan.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbHuongDan.Font = new System.Drawing.Font("Segoe UI", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtbHuongDan.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(41)))), ((int)(((byte)(55)))));
            this.rtbHuongDan.Location = new System.Drawing.Point(18, 16);
            this.rtbHuongDan.Name = "rtbHuongDan";
            this.rtbHuongDan.ReadOnly = true;
            this.rtbHuongDan.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.rtbHuongDan.Size = new System.Drawing.Size(810, 488);
            this.rtbHuongDan.TabIndex = 0;
            this.rtbHuongDan.Text = resources.GetString("rtbHuongDan.Text");
            // 
            // btnDong
            // 
            this.btnDong.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(232)))), ((int)(((byte)(240)))));
            this.btnDong.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnDong.FlatAppearance.BorderSize = 0;
            this.btnDong.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDong.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDong.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(41)))), ((int)(((byte)(55)))));
            this.btnDong.Location = new System.Drawing.Point(750, 604);
            this.btnDong.Name = "btnDong";
            this.btnDong.Size = new System.Drawing.Size(120, 40);
            this.btnDong.TabIndex = 2;
            this.btnDong.Text = "Đóng";
            this.btnDong.UseVisualStyleBackColor = false;
            this.btnDong.Click += new System.EventHandler(this.btnDong_Click);
            // 
            // FrmHuongDanSuDung
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(246)))), ((int)(((byte)(251)))));
            this.ClientSize = new System.Drawing.Size(900, 664);
            this.Controls.Add(this.btnDong);
            this.Controls.Add(this.pnlNoiDung);
            this.Controls.Add(this.lblTieuDe);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "FrmHuongDanSuDung";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Hướng Dẫn Sử Dụng";
            this.pnlNoiDung.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblTieuDe;
        private System.Windows.Forms.RichTextBox rtbHuongDan;
        private System.Windows.Forms.Button btnDong;
        private System.Windows.Forms.Panel pnlNoiDung;
    }
}