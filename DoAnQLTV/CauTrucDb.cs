using System;
using System.Data;

namespace DoAnQLTV
{
    /// <summary>
    /// Gọi 1 lần khi khởi động (Program.cs). Chạy lại nhiều lần vẫn an toàn:
    ///  - thêm cột NhanVien.MatKhau  (nếu chưa có)
    ///  - thêm cột PhieuMuon.TienPhat (nếu chưa có)
    ///  - đảm bảo có nhân viên "admin"
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

            DataTable chuaCo = DbConnection.ExecuteQuery(
                "SELECT MaNhanVien FROM NhanVien WHERE MatKhau IS NULL OR LTRIM(RTRIM(MatKhau)) = ''");
            foreach (DataRow r in chuaCo.Rows)
                TaiKhoan.DatMatKhau(r["MaNhanVien"].ToString(), QuyDinh.MatKhauMacDinh);
        }
    }
}
