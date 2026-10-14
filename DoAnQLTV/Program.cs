using System;
using System.Windows.Forms;

namespace DoAnQLTV
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Kiểm tra kết nối CSDL ngay từ đầu để báo lỗi rõ ràng,
            // thay vì để từng form báo lỗi lẻ tẻ sau khi đăng nhập.
            try
            {
                string loi;
                if (!DbConnection.TestConnection(out loi))
                {
                    AppTheme.Error(
                        "Không kết nối được cơ sở dữ liệu QuanLyThuVien.\n\n" + loi +
                        "\n\nHãy kiểm tra SQL Server LocalDB và chuỗi kết nối trong App.config.");
                    return;
                }
            }
            catch (Exception ex)
            {
                AppTheme.Error("Không đọc được cấu hình kết nối (App.config).\n\n" + ex.Message);
                return;
            }

            // Tạo bảng nhật ký, thêm cột MatKhau / TienPhat nếu chưa có
            try
            {
                NhatKy.DamBaoBang();
                CauTrucDb.DamBao();
            }
            catch (Exception ex)
            {
                AppTheme.Error("Không cập nhật được cấu trúc cơ sở dữ liệu.\n\n" + ex.Message);
                return;
            }

            // Khởi chạy form đăng nhập trước
            Application.Run(new FrmLogin());
        }
    }
}
