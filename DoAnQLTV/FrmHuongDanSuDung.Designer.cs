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
            this.SuspendLayout();
            // 
            // lblTieuDe
            // 
            this.lblTieuDe.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTieuDe.ForeColor = System.Drawing.Color.DarkBlue;
            this.lblTieuDe.Location = new System.Drawing.Point(30, 25);
            this.lblTieuDe.Name = "lblTieuDe";
            this.lblTieuDe.Size = new System.Drawing.Size(840, 40);
            this.lblTieuDe.TabIndex = 0;
            this.lblTieuDe.Text = "HƯỚNG DẪN SỬ DỤNG";
            this.lblTieuDe.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // rtbHuongDan
            // 
            this.rtbHuongDan.BackColor = System.Drawing.Color.White;
            this.rtbHuongDan.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.rtbHuongDan.DetectUrls = false;
            this.rtbHuongDan.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtbHuongDan.ForeColor = System.Drawing.Color.Black;
            this.rtbHuongDan.Location = new System.Drawing.Point(40, 85);
            this.rtbHuongDan.Name = "rtbHuongDan";
            this.rtbHuongDan.ReadOnly = true;
            this.rtbHuongDan.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.rtbHuongDan.Size = new System.Drawing.Size(820, 510);
            this.rtbHuongDan.TabIndex = 1;
            this.rtbHuongDan.Text = resources.GetString("rtbHuongDan.Text");
            // 
            // btnDong
            // 
            this.btnDong.BackColor = System.Drawing.Color.IndianRed;
            this.btnDong.FlatAppearance.BorderSize = 0;
            this.btnDong.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDong.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDong.ForeColor = System.Drawing.Color.White;
            this.btnDong.Location = new System.Drawing.Point(390, 615);
            this.btnDong.Name = "btnDong";
            this.btnDong.Size = new System.Drawing.Size(120, 35);
            this.btnDong.TabIndex = 2;
            this.btnDong.Text = "Đóng";
            this.btnDong.UseVisualStyleBackColor = false;
            this.btnDong.Click += new System.EventHandler(this.btnDong_Click);
            // 
            // FrmHuongDanSuDung
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(884, 661);
            this.Controls.Add(this.btnDong);
            this.Controls.Add(this.rtbHuongDan);
            this.Controls.Add(this.lblTieuDe);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "FrmHuongDanSuDung";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "HƯỚNG DẪN SỬ DỤNG";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblTieuDe;
        private System.Windows.Forms.RichTextBox rtbHuongDan;
        private System.Windows.Forms.Button btnDong;
    }
}