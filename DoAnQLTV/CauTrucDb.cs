using System;
using System.Data;

namespace DoAnQLTV
{
    /// <summary>
    /// Gọi 1 lần khi khởi động (Program.cs). Chạy lại nhiều lần vẫn an toàn:
    ///  - thêm cột NhanVien.MatKhau, NhanVien.VaiTro, PhieuMuon.TienPhat (nếu chưa có)
    ///  - đảm bảo có nhân viên "admin"
    ///  - đảm bảo luôn có ít nhất 1 quản trị viên
    ///  - gán mật khẩu mặc định cho nhân viên nào chưa có mật khẩu
    /// </summary>
    public static class CauTrucDb
    {
        public static void DamBao()
        {
            DbConnection.ExecuteNonQuery(
                "IF COL_LENGTH('dbo.NhanVien', 'MatKhau') IS NULL " +
                "ALTER TABLE dbo.NhanVien ADD MatKhau NVARCHAR(100) NULL");

            DbConnection.ExecuteNonQuery(
                "IF COL_LENGTH('dbo.NhanVien', 'VaiTro') IS NULL " +
                "ALTER TABLE dbo.NhanVien ADD VaiTro NVARCHAR(20) NULL");

            DbConnection.ExecuteNonQuery(
                "IF COL_LENGTH('dbo.PhieuMuon', 'TienPhat') IS NULL " +
                "ALTER TABLE dbo.PhieuMuon ADD TienPhat DECIMAL(18,0) NULL");

            // Giữ được tài khoản cũ admin / 123456: nếu chưa có nhân viên mã "admin" thì tạo.
            int coAdmin = DbConnection.ExecuteScalar<int>(
                "SELECT COUNT(*) FROM NhanVien WHERE MaNhanVien = 'admin'");
            if (coAdmin == 0)
            {
                try
                {
                    DbConnection.ExecuteNonQuery(
                        "INSERT INTO NhanVien (MaNhanVien, TenNhanVien, SoDienThoai, DiaChi) " +
                        "VALUES ('admin', N'Admin', '0000000000', N'Thư viện')");
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("Không tạo được nhân viên admin: " + ex.Message);
                }
            }

            // Nhân viên chưa có vai trò -> nhân viên thường
            DbConnection.ExecuteNonQuery(
                "UPDATE NhanVien SET VaiTro = 'NhanVien' WHERE VaiTro IS NULL OR LTRIM(RTRIM(VaiTro)) = ''");

            // Nếu chưa có quản trị viên nào thì cấp cho "admin"
            DbConnection.ExecuteNonQuery(
                "UPDATE NhanVien SET VaiTro = 'Admin' " +
                "WHERE MaNhanVien = 'admin' AND NOT EXISTS (SELECT 1 FROM NhanVien WHERE VaiTro = 'Admin')");

            // Phòng trường hợp không có mã "admin": cấp cho nhân viên đầu tiên để hệ thống không bị khóa
            DbConnection.ExecuteNonQuery(
                "UPDATE NhanVien SET VaiTro = 'Admin' " +
                "WHERE MaNhanVien = (SELECT TOP 1 MaNhanVien FROM NhanVien ORDER BY MaNhanVien) " +
                "AND NOT EXISTS (SELECT 1 FROM NhanVien WHERE VaiTro = 'Admin')");

            DataTable chuaCo = DbConnection.ExecuteQuery(
                "SELECT MaNhanVien FROM NhanVien WHERE MatKhau IS NULL OR LTRIM(RTRIM(MatKhau)) = ''");
            foreach (DataRow r in chuaCo.Rows)
                TaiKhoan.DatMatKhau(r["MaNhanVien"].ToString(), QuyDinh.MatKhauMacDinh);
        }
    }
}
