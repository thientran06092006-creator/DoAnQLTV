using System;
using System.Data;

namespace DoAnQLTV
{
    /// <summary>
    /// Nhật ký hoạt động dùng chung cho mọi form.
    /// Ghi:  NhatKy.Ghi("Trả sách", "Trả sách \"Lập trình C#\" - Mã DG: DG025");
    /// Đọc:  NhatKy.LayMoiNhat(4)  (trang chủ dùng để hiện "Hoạt động gần đây")
    /// </summary>
    public static class NhatKy
    {
        /// <summary>Tên nhân viên đang đăng nhập. FrmLogin gán sau khi đăng nhập thành công.</summary>
        public static string NguoiThucHien = "";
        public static string MaNhanVien = "";

        /// <summary>
        /// Tạo bảng NhatKyHoatDong nếu chưa có (gọi 1 lần khi khởi động trong Program.cs).
        /// Có bảng rồi thì không làm gì, nên không cần chạy script SQL tạo bảng nữa.
        /// </summary>
        public static void DamBaoBang()
        {
            try
            {
                DbConnection.ExecuteNonQuery(@"
                    IF OBJECT_ID(N'dbo.NhatKyHoatDong', N'U') IS NULL
                    CREATE TABLE dbo.NhatKyHoatDong
                    (
                        MaNhatKy      INT IDENTITY(1,1) PRIMARY KEY,
                        ThoiGian      DATETIME NOT NULL DEFAULT GETDATE(),
                        LoaiHoatDong  NVARCHAR(100) NOT NULL,
                        ChiTiet       NVARCHAR(500) NULL,
                        NguoiThucHien NVARCHAR(100) NULL
                    )");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Không tạo được bảng nhật ký: " + ex.Message);
            }
        }

        /// <summary>
        /// Ghi 1 dòng nhật ký. Nếu ghi lỗi thì bỏ qua,
        /// để không làm hỏng chức năng chính như lập phiếu hay trả sách.
        /// </summary>
        public static void Ghi(string loaiHoatDong, string chiTiet)
        {
            try
            {
                DbConnection.ExecuteNonQuery(
                    "INSERT INTO NhatKyHoatDong (LoaiHoatDong, ChiTiet, NguoiThucHien) " +
                    "VALUES (@loai, @chiTiet, @nguoi)",
                    DbConnection.Param("@loai", loaiHoatDong),
                    DbConnection.Param("@chiTiet", chiTiet),
                    DbConnection.Param("@nguoi", string.IsNullOrWhiteSpace(NguoiThucHien) ? null : NguoiThucHien));
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Không ghi được nhật ký: " + ex.Message);
            }
        }

        /// <summary>Lấy các hoạt động mới nhất (mới nhất đứng trên cùng).</summary>
        public static DataTable LayMoiNhat(int soDong)
        {
            return DbConnection.ExecuteQuery(
                "SELECT TOP (@n) ThoiGian, LoaiHoatDong, ChiTiet, NguoiThucHien " +
                "FROM NhatKyHoatDong " +
                "ORDER BY ThoiGian DESC, MaNhatKy DESC",
                DbConnection.Param("@n", soDong));
        }
    }
}
