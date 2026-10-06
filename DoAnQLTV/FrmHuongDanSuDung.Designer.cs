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
            this.lblTieuDe.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTieuDe.Location = new System.Drawing.Point(13, 13);
            this.lblTieuDe.Name = "lblTieuDe";
            this.lblTieuDe.Size = new System.Drawing.Size(500, 45);
            this.lblTieuDe.TabIndex = 0;
            this.lblTieuDe.Text = "HƯỚNG DẪN SỬ DỤNG";
            this.lblTieuDe.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // rtbHuongDan
            // 
            this.rtbHuongDan.Location = new System.Drawing.Point(29, 62);
            this.rtbHuongDan.Name = "rtbHuongDan";
            this.rtbHuongDan.ReadOnly = true;
            this.rtbHuongDan.Size = new System.Drawing.Size(520, 387);
            this.rtbHuongDan.TabIndex = 1;
            this.rtbHuongDan.Text = resources.GetString("rtbHuongDan.Text");
            // 
            // btnDong
            // 
            this.btnDong.Location = new System.Drawing.Point(12, 12);
            this.btnDong.Name = "btnDong";
            this.btnDong.Size = new System.Drawing.Size(75, 23);
            this.btnDong.TabIndex = 2;
            this.btnDong.Text = "Đóng";
            this.btnDong.UseVisualStyleBackColor = true;
            this.btnDong.Click += new System.EventHandler(this.btnDong_Click);
            // 
            // FrmHuongDanSuDung
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 461);
            this.Controls.Add(this.btnDong);
            this.Controls.Add(this.rtbHuongDan);
            this.Controls.Add(this.lblTieuDe);
            this.Name = "FrmHuongDanSuDung";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Hướng dẫn sử dụng";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblTieuDe;
        private System.Windows.Forms.RichTextBox rtbHuongDan;
        private System.Windows.Forms.Button btnDong;
    }
}