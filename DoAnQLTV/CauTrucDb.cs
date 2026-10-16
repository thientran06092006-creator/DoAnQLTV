using System;
using System.Data;

namespace DoAnQLTV
{
    /// <summary>
    /// Gọi 1 lần khi khởi động (Program.cs). Chạy lại nhiều lần vẫn an toàn:
    ///  - NhanVien : MatKhau, VaiTro
    ///  - PhieuMuon: TienPhat, DaThuPhat, NgayThuPhat, SoLanGiaHan
    ///  - Sach     : SoLuong, NhaXuatBan, NamXuatBan, ViTriKe
    ///  - DocGia   : NgayHetHanThe
    ///  - đảm bảo có nhân viên "admin" và luôn có ít nhất 1 quản trị viên
    ///  - gán mật khẩu mặc định cho nhân viên nào chưa có mật khẩu
    /// </summary>
    public static class CauTrucDb
    {
        private static void ThemCot(string bang, string cot, string dinhNghia)
        {
            DbConnection.ExecuteNonQuery(
                "IF COL_LENGTH('dbo." + bang + "', '" + cot + "') IS NULL " +
                "ALTER TABLE dbo." + bang + " ADD " + cot + " " + dinhNghia);
        }

        public static void DamBao()
        {
            // ----- Nhân viên -----
            ThemCot("NhanVien", "MatKhau", "NVARCHAR(100) NULL");
            ThemCot("NhanVien", "VaiTro", "NVARCHAR(20) NULL");

            // ----- Phiếu mượn -----
            ThemCot("PhieuMuon", "TienPhat", "DECIMAL(18,0) NULL");
            ThemCot("PhieuMuon", "DaThuPhat", "BIT NOT NULL CONSTRAINT DF_PhieuMuon_DaThuPhat DEFAULT 0");
            ThemCot("PhieuMuon", "NgayThuPhat", "DATETIME NULL");
            ThemCot("PhieuMuon", "SoLanGiaHan", "INT NOT NULL CONSTRAINT DF_PhieuMuon_SoLanGiaHan DEFAULT 0");

            // ----- Sách -----
            // Sách cũ mặc định có 1 bản (giữ đúng hành vi trước đây)
            ThemCot("Sach", "SoLuong", "INT NOT NULL CONSTRAINT DF_Sach_SoLuong DEFAULT 1");
            ThemCot("Sach", "NhaXuatBan", "NVARCHAR(100) NULL");
            ThemCot("Sach", "NamXuatBan", "INT NULL");
            ThemCot("Sach", "ViTriKe", "NVARCHAR(50) NULL");

            // ----- Độc giả -----
            ThemCot("DocGia", "NgayHetHanThe", "DATE NULL");

            // Độc giả cũ chưa có hạn thẻ: cấp thẻ có hiệu lực từ hôm nay
            DbConnection.ExecuteNonQuery(
                "UPDATE DocGia SET NgayHetHanThe = DATEADD(MONTH, " + QuyDinh.SoThangHanThe +
                ", CAST(GETDATE() AS DATE)) WHERE NgayHetHanThe IS NULL");

            // ----- Tài khoản quản trị -----
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
