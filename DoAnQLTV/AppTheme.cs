using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Reflection;
using System.Windows.Forms;

namespace DoAnQLTV
{
    /// <summary>
    /// Bảng màu + hàm làm đẹp dùng chung cho TẤT CẢ form.
    /// Muốn đổi giao diện cả phần mềm chỉ cần sửa màu ở đây.
    /// </summary>
    public static class AppTheme
    {
        // ===== BẢNG MÀU =====
        public static readonly Color Primary = ColorTranslator.FromHtml("#1E3A8A"); // navy: sidebar, header bảng
        public static readonly Color PrimaryDark = ColorTranslator.FromHtml("#172554");
        public static readonly Color Accent = ColorTranslator.FromHtml("#2563EB"); // nút chính, mục đang chọn
        public static readonly Color AccentHover = ColorTranslator.FromHtml("#1D4ED8");
        public static readonly Color AccentLight = ColorTranslator.FromHtml("#DBEAFE"); // dòng đang chọn
        public static readonly Color Background = ColorTranslator.FromHtml("#F3F6FB"); // nền form
        public static readonly Color Card = Color.White;
        public static readonly Color Border = ColorTranslator.FromHtml("#E2E8F0");
        public static readonly Color TextDark = ColorTranslator.FromHtml("#1F2937");
        public static readonly Color TextMuted = ColorTranslator.FromHtml("#6B7280");
        public static readonly Color Success = ColorTranslator.FromHtml("#16A34A");
        public static readonly Color Amber = ColorTranslator.FromHtml("#F59E0B");
        public static readonly Color Danger = ColorTranslator.FromHtml("#DC2626");
        public static readonly Color DangerHover = ColorTranslator.FromHtml("#B91C1C");
        public static readonly Color RowAlt = ColorTranslator.FromHtml("#F8FAFC");

        /// <summary>Dãy màu dùng cho biểu đồ (Chart) để đồng bộ với giao diện.</summary>
        public static readonly Color[] ChartPalette =
        {
            ColorTranslator.FromHtml("#2563EB"),
            ColorTranslator.FromHtml("#16A34A"),
            ColorTranslator.FromHtml("#F59E0B"),
            ColorTranslator.FromHtml("#8B5CF6"),
            ColorTranslator.FromHtml("#EC4899"),
            ColorTranslator.FromHtml("#94A3B8")
        };

        // ===== FONT =====
        public static readonly Font Base = new Font("Segoe UI", 10F);
        public static readonly Font Bold = new Font("Segoe UI", 10F, FontStyle.Bold);
        public static readonly Font Title = new Font("Segoe UI", 20F, FontStyle.Bold);
        public static readonly Font Heading = new Font("Segoe UI", 13F, FontStyle.Bold);
        public static readonly Font Number = new Font("Segoe UI", 24F, FontStyle.Bold);

        // ===== FORM =====
        /// <summary>Gọi 1 dòng trong constructor mỗi form: AppTheme.ApplyForm(this);</summary>
        public static void ApplyForm(Form form)
        {
            form.Font = Base;
            form.BackColor = Background;
            form.ForeColor = TextDark;
            SetDoubleBuffered(form);
        }

        /// <summary>Bật double buffer cho form/control để giảm giật, nhấp nháy.</summary>
        public static void SetDoubleBuffered(Control control)
        {
            typeof(Control)
                .GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic)
                .SetValue(control, true, null);
        }

        // ===== NÚT BẤM =====
        public static void StyleButton(Button btn, Color back, Color hover, Color? fore = null, int radius = 6)
        {
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.FlatAppearance.MouseOverBackColor = hover;
            btn.FlatAppearance.MouseDownBackColor = hover;
            btn.BackColor = back;
            btn.ForeColor = fore ?? Color.White;
            btn.Font = Bold;
            btn.Cursor = Cursors.Hand;
            btn.UseVisualStyleBackColor = false;
            if (radius > 0) Round(btn, radius);
        }

        public static void StylePrimaryButton(Button btn) { StyleButton(btn, Accent, AccentHover); }
        public static void StyleDangerButton(Button btn) { StyleButton(btn, Danger, DangerHover); }
        public static void StyleSuccessButton(Button btn) { StyleButton(btn, Success, ColorTranslator.FromHtml("#15803D")); }

        /// <summary>Nút viền, nền trắng (Hủy, Làm mới...).</summary>
        public static void StyleOutlineButton(Button btn)
        {
            StyleButton(btn, Color.White, AccentLight, Accent);
            btn.FlatAppearance.BorderSize = 1;
            btn.FlatAppearance.BorderColor = Accent;
        }

        // ===== Ô NHẬP LIỆU =====
        public static void StyleTextBox(TextBox txt)
        {
            txt.BorderStyle = BorderStyle.FixedSingle;
            txt.Font = new Font("Segoe UI", 11F);
            txt.BackColor = Color.White;
            txt.ForeColor = TextDark;
        }

        public static void StyleComboBox(ComboBox cbo)
        {
            cbo.FlatStyle = FlatStyle.Flat;
            cbo.BackColor = Color.White;
            cbo.ForeColor = TextDark;
            cbo.Font = new Font("Segoe UI", 11F);

            // Flat ComboBox không có viền, nên vẽ viền lên panel chứa nó
            if (cbo.Parent != null)
            {
                cbo.Parent.Paint += (s, e) =>
                {
                    Rectangle r = cbo.Bounds;
                    using (var pen = new Pen(ColorTranslator.FromHtml("#7A7A7A")))
                        e.Graphics.DrawRectangle(pen, r.X - 1, r.Y - 1, r.Width + 1, r.Height + 1);
                };
                cbo.Parent.Invalidate();
            }
        }


        // ===== BẢNG DỮ LIỆU =====
        public static void StyleGrid(DataGridView dgv)
        {
            dgv.BorderStyle = BorderStyle.None;
            dgv.BackgroundColor = Color.White;
            dgv.GridColor = Border;
            dgv.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgv.EnableHeadersVisualStyles = false;

            dgv.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgv.ColumnHeadersHeight = 40;
            dgv.ColumnHeadersDefaultCellStyle.BackColor = Primary;
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgv.ColumnHeadersDefaultCellStyle.Font = Bold;
            dgv.ColumnHeadersDefaultCellStyle.SelectionBackColor = Primary;
            dgv.ColumnHeadersDefaultCellStyle.Padding = new Padding(8, 0, 0, 0);

            dgv.DefaultCellStyle.BackColor = Color.White;
            dgv.DefaultCellStyle.ForeColor = TextDark;
            dgv.DefaultCellStyle.Font = Base;
            dgv.DefaultCellStyle.SelectionBackColor = AccentLight;
            dgv.DefaultCellStyle.SelectionForeColor = TextDark;
            dgv.DefaultCellStyle.Padding = new Padding(8, 0, 0, 0);
            dgv.AlternatingRowsDefaultCellStyle.BackColor = RowAlt;

            dgv.RowTemplate.Height = 34;
            dgv.RowHeadersVisible = false;
            dgv.AllowUserToAddRows = false;
            dgv.AllowUserToDeleteRows = false;
            dgv.AllowUserToResizeRows = false;
            dgv.ReadOnly = true;
            dgv.MultiSelect = false;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            SetDoubleBuffered(dgv);
        }

        // ===== THẺ (CARD) =====
        /// <summary>Panel nền trắng + viền mảnh nhẹ, thay cho viền đen mặc định.</summary>
        public static void StyleCard(Panel panel)
        {
            panel.BorderStyle = BorderStyle.None;
            panel.BackColor = Card;
            panel.Paint += (s, e) =>
            {
                using (var pen = new Pen(Border))
                    e.Graphics.DrawRectangle(pen, 0, 0, panel.Width - 1, panel.Height - 1);
            };
        }

        /// <summary>Đổi màu nền khi rê chuột vào card (kể cả khi chuột nằm trên control con).</summary>
        public static void AddHover(Control card, Color normal, Color hover)
        {
            card.Cursor = Cursors.Hand;
            EventHandler enter = (s, e) => card.BackColor = hover;
            EventHandler leave = (s, e) =>
            {
                if (!card.ClientRectangle.Contains(card.PointToClient(Cursor.Position)))
                    card.BackColor = normal;
            };

            card.MouseEnter += enter;
            card.MouseLeave += leave;
            foreach (Control child in card.Controls)
            {
                child.Cursor = Cursors.Hand;
                child.MouseEnter += enter;
                child.MouseLeave += leave;
            }
        }

        // ===== BO GÓC =====
        public static void Round(Control control, int radius)
        {
            Action apply = () =>
            {
                if (control.Width <= 0 || control.Height <= 0) return;
                using (var path = RoundedRect(new Rectangle(0, 0, control.Width, control.Height), radius))
                    control.Region = new Region(path);
            };
            apply();
            control.SizeChanged += (s, e) => apply();
        }

        private static GraphicsPath RoundedRect(Rectangle r, int radius)
        {
            int d = radius * 2;
            var path = new GraphicsPath();
            path.AddArc(r.X, r.Y, d, d, 180, 90);
            path.AddArc(r.Right - d, r.Y, d, d, 270, 90);
            path.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
            path.AddArc(r.X, r.Bottom - d, d, d, 90, 90);
            path.CloseFigure();
            return path;
        }

        // ===== HỘP THOẠI THÔNG BÁO NGẮN GỌN =====
        public static void Info(string message)
        {
            MessageBox.Show(message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public static void Warning(string message)
        {
            MessageBox.Show(message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        public static void Error(string message)
        {
            MessageBox.Show(message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public static bool Confirm(string message)
        {
            return MessageBox.Show(message, "Xác nhận", MessageBoxButtons.YesNo,
                MessageBoxIcon.Question) == DialogResult.Yes;
        }
    }
}