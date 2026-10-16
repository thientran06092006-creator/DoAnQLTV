using System;
using System.Data.SqlClient;

namespace DoAnQLTV
{
    /// <summary>
    /// Lỗi vi phạm quy định nghiệp vụ (hết sách, vượt giới hạn mượn, thẻ hết hạn...).
    /// Form bắt lỗi này và hiện cảnh báo cho người dùng. Truong cho biết nên đưa con trỏ vào ô nào
    /// ("sach", "docgia" hoặc "phieu").
    /// </summary>
    public class LoiNghiepVu : Exception
    {
        public string Truong { get; private set; }

        public LoiNghiepVu(string thongBao, string truong) : base(thongBao)
        {
            Truong = truong;
        }
    }

    /// <summary>
    /// Các thao tác mượn sách cần đảm bảo "kiểm tra rồi mới ghi" không bị lách khi 2 máy cùng thao tác.
    /// Mỗi thao tác chạy trong 1 transaction và khóa dòng Sach / DocGia / PhieuMuon (UPDLOCK, HOLDLOCK),
    /// nên 2 nhân viên cùng mượn bản cuối của 1 cuốn sẽ được xử lý lần lượt, người sau bị từ chối.
    /// </summary>
    public static class ThuVienDb
    {
        /// <summary>Biểu thức SQL: số bản của sách (alias s) đang được mượn chưa trả.</summary>
        public const string SqlDangMuon =
            "(SELECT COUNT(*) FROM PhieuMuon pmx WHERE pmx.MaSach = s.MaSach AND pmx.NgayTra IS NULL)";

        /// <summary>Biểu thức SQL: số bản còn lại = tổng số bản - số bản đang mượn.</summary>
        public const string SqlConLai = "(s.SoLuong - " + SqlDangMuon + ")";

        private static T Scalar<T>(SqlConnection conn, SqlTransaction tran, string sql, params SqlParameter[] ps)
        {
            using (SqlCommand cmd = new SqlCommand(sql, conn, tran))
            {
                if (ps != null && ps.Length > 0) cmd.Parameters.AddRange(ps);
                object kq = cmd.ExecuteScalar();
                if (kq == null || kq == DBNull.Value) return default(T);
                return (T)Convert.ChangeType(kq, typeof(T));
            }
        }

        /// <summary>
        /// Thẻ độc giả hết hạn chưa? Cột NgayHetHanThe để trống (NULL) nghĩa là không giới hạn.
        /// Trả về ngày hết hạn nếu thẻ ĐÃ hết hạn, ngược lại trả về null.
        /// </summary>
        private static DateTime? NgayTheHetHan(SqlConnection conn, SqlTransaction tran, string maDocGia)
        {
            DateTime het = Scalar<DateTime>(conn, tran,
                "SELECT NgayHetHanThe FROM DocGia WHERE MaDocGia = @d",
                DbConnection.Param("@d", maDocGia));

            if (het == default(DateTime)) return null;           // NULL -> không giới hạn
            return het.Date < DateTime.Today ? (DateTime?)het.Date : null;
        }

        private static string ThongBaoTheHetHan(DateTime ngayHet, string viec)
        {
            return "Thẻ của độc giả này đã hết hạn từ ngày " + ngayHet.ToString("dd/MM/yyyy") + ".\n" +
                   "Vui lòng gia hạn thẻ (mục Quản lý độc giả) trước khi " + viec + ".";
        }

        /// <summary>
        /// Kiểm tra điều kiện được mượn. Ném LoiNghiepVu nếu vi phạm.
        /// boQuaMaPhieu: bỏ qua phiếu này khi đếm (dùng lúc sửa chính phiếu đó).
        /// laPhieuMoi: chỉ khi lập phiếu MỚI mới kiểm tra thẻ hết hạn và sách quá hạn.
        /// </summary>
        private static void KiemTraDieuKienMuon(SqlConnection conn, SqlTransaction tran,
            string maDocGia, string maSach, string boQuaMaPhieu, bool laPhieuMoi)
        {
            // ----- Sách: khóa dòng rồi đếm số bản đang mượn -----
            string tenSach = Scalar<string>(conn, tran,
                "SELECT TenSach FROM Sach WITH (UPDLOCK, HOLDLOCK) WHERE MaSach = @s",
                DbConnection.Param("@s", maSach));
            if (tenSach == null)
                throw new LoiNghiepVu("Sách được chọn không còn tồn tại.", "sach");

            int soLuong = Scalar<int>(conn, tran,
                "SELECT SoLuong FROM Sach WHERE MaSach = @s",
                DbConnection.Param("@s", maSach));

            int dangMuonSach = Scalar<int>(conn, tran,
                @"SELECT COUNT(*) FROM PhieuMuon
                  WHERE MaSach = @s AND NgayTra IS NULL
                    AND (@b IS NULL OR MaPhieu <> @b)",
                DbConnection.Param("@s", maSach),
                DbConnection.Param("@b", boQuaMaPhieu));

            if (dangMuonSach >= soLuong)
                throw new LoiNghiepVu(
                    "Sách \"" + tenSach + "\" đã được mượn hết (" + dangMuonSach + "/" + soLuong +
                    " bản).\nVui lòng chọn sách khác.", "sach");

            // ----- Độc giả: khóa dòng rồi kiểm tra thẻ, quá hạn và giới hạn -----
            string maTonTai = Scalar<string>(conn, tran,
                "SELECT MaDocGia FROM DocGia WITH (UPDLOCK, HOLDLOCK) WHERE MaDocGia = @d",
                DbConnection.Param("@d", maDocGia));
            if (maTonTai == null)
                throw new LoiNghiepVu("Độc giả được chọn không còn tồn tại.", "docgia");

            if (laPhieuMoi)
            {
                DateTime? theHetHan = NgayTheHetHan(conn, tran, maDocGia);
                if (theHetHan.HasValue)
                    throw new LoiNghiepVu(ThongBaoTheHetHan(theHetHan.Value, "mượn sách"), "docgia");

                int quaHan = Scalar<int>(conn, tran,
                    @"SELECT COUNT(*) FROM PhieuMuon
                      WHERE MaDocGia = @d AND NgayTra IS NULL
                        AND HanTra < CAST(GETDATE() AS DATE)",
                    DbConnection.Param("@d", maDocGia));

                if (quaHan > 0)
                    throw new LoiNghiepVu(
                        "Độc giả này đang có " + quaHan + " phiếu mượn quá hạn chưa trả.\n" +
                        "Vui lòng trả sách quá hạn trước khi mượn thêm.", "docgia");
            }

            int dangMuonDocGia = Scalar<int>(conn, tran,
                @"SELECT COUNT(*) FROM PhieuMuon
                  WHERE MaDocGia = @d AND NgayTra IS NULL
                    AND (@b IS NULL OR MaPhieu <> @b)",
                DbConnection.Param("@d", maDocGia),
                DbConnection.Param("@b", boQuaMaPhieu));

            if (dangMuonDocGia >= QuyDinh.SoSachToiDa)
                throw new LoiNghiepVu(
                    "Độc giả này đang mượn " + dangMuonDocGia + " cuốn chưa trả, đã đạt giới hạn tối đa " +
                    QuyDinh.SoSachToiDa + " cuốn.\nVui lòng trả bớt sách trước khi mượn thêm.", "docgia");
        }

        /// <summary>Lập phiếu mượn mới. Ném LoiNghiepVu nếu vi phạm quy định, SqlException nếu lỗi CSDL.</summary>
        public static void LapPhieu(string maPhieu, string maDocGia, string maNhanVien,
            string maSach, DateTime ngayMuon, DateTime hanTra)
        {
            DbConnection.ExecuteInTransaction((conn, tran) =>
            {
                KiemTraDieuKienMuon(conn, tran, maDocGia, maSach, null, true);

                using (SqlCommand cmd = new SqlCommand(@"
                    INSERT INTO PhieuMuon (MaPhieu, MaDocGia, MaNhanVien, MaSach, NgayMuon, HanTra)
                    VALUES (@MaPhieu, @MaDocGia, @MaNhanVien, @MaSach, @NgayMuon, @HanTra)", conn, tran))
                {
                    cmd.Parameters.Add(DbConnection.Param("@MaPhieu", maPhieu));
                    cmd.Parameters.Add(DbConnection.Param("@MaDocGia", maDocGia));
                    cmd.Parameters.Add(DbConnection.Param("@MaNhanVien", maNhanVien));
                    cmd.Parameters.Add(DbConnection.Param("@MaSach", maSach));
                    cmd.Parameters.Add(DbConnection.Param("@NgayMuon", ngayMuon));
                    cmd.Parameters.Add(DbConnection.Param("@HanTra", hanTra));
                    cmd.ExecuteNonQuery();
                }
            });
        }

        /// <summary>
        /// Sửa phiếu mượn. Trả về số dòng được sửa (0 = không tìm thấy phiếu).
        /// Phiếu ĐÃ TRẢ là lịch sử cũ nên không bị kiểm tra số bản / giới hạn.
        /// </summary>
        public static int SuaPhieu(string maPhieu, string maDocGia, string maNhanVien,
            string maSach, DateTime ngayMuon, DateTime hanTra)
        {
            int soDong = 0;

            DbConnection.ExecuteInTransaction((conn, tran) =>
            {
                int dangMo = Scalar<int>(conn, tran,
                    "SELECT COUNT(*) FROM PhieuMuon WITH (UPDLOCK, HOLDLOCK) WHERE MaPhieu = @p AND NgayTra IS NULL",
                    DbConnection.Param("@p", maPhieu));

                if (dangMo > 0)
                    KiemTraDieuKienMuon(conn, tran, maDocGia, maSach, maPhieu, false);

                using (SqlCommand cmd = new SqlCommand(@"
                    UPDATE PhieuMuon
                    SET MaDocGia = @MaDocGia,
                        MaNhanVien = @MaNhanVien,
                        MaSach = @MaSach,
                        NgayMuon = @NgayMuon,
                        HanTra = @HanTra
                    WHERE MaPhieu = @MaPhieu", conn, tran))
                {
                    cmd.Parameters.Add(DbConnection.Param("@MaDocGia", maDocGia));
                    cmd.Parameters.Add(DbConnection.Param("@MaNhanVien", maNhanVien));
                    cmd.Parameters.Add(DbConnection.Param("@MaSach", maSach));
                    cmd.Parameters.Add(DbConnection.Param("@NgayMuon", ngayMuon));
                    cmd.Parameters.Add(DbConnection.Param("@HanTra", hanTra));
                    cmd.Parameters.Add(DbConnection.Param("@MaPhieu", maPhieu));
                    soDong = cmd.ExecuteNonQuery();
                }
            });

            return soDong;
        }

        /// <summary>
        /// Gia hạn phiếu: cộng thêm QuyDinh.SoNgayMoiLanGiaHan ngày vào hạn trả.
        /// Điều kiện: phiếu chưa trả, CHƯA quá hạn, chưa gia hạn quá QuyDinh.SoLanGiaHanToiDa lần,
        /// và thẻ độc giả còn hạn. Vi phạm thì ném LoiNghiepVu.
        /// </summary>
        public static void GiaHan(string maPhieu, out DateTime hanMoi, out int lanThu)
        {
            DateTime ketQuaHan = DateTime.MinValue;
            int ketQuaLan = 0;

            DbConnection.ExecuteInTransaction((conn, tran) =>
            {
                DateTime hanTra = DateTime.MinValue;
                bool daTra = false;
                int soLan = 0;
                string maDocGia = null;
                bool tonTai = false;

                // Khóa dòng phiếu rồi đọc trạng thái hiện tại
                using (SqlCommand cmd = new SqlCommand(
                    @"SELECT HanTra, NgayTra, SoLanGiaHan, MaDocGia
                      FROM PhieuMuon WITH (UPDLOCK, HOLDLOCK)
                      WHERE MaPhieu = @p", conn, tran))
                {
                    cmd.Parameters.Add(DbConnection.Param("@p", maPhieu));
                    using (SqlDataReader rd = cmd.ExecuteReader())
                    {
                        if (rd.Read())
                        {
                            tonTai = true;
                            hanTra = Convert.ToDateTime(rd["HanTra"]).Date;
                            daTra = rd["NgayTra"] != DBNull.Value;
                            soLan = Convert.ToInt32(rd["SoLanGiaHan"]);
                            maDocGia = rd["MaDocGia"].ToString();
                        }
                    }
                }

                if (!tonTai)
                    throw new LoiNghiepVu("Không tìm thấy phiếu mượn " + maPhieu + ".", "phieu");

                if (daTra)
                    throw new LoiNghiepVu("Phiếu " + maPhieu + " đã trả sách, không thể gia hạn.", "phieu");

                if (hanTra < DateTime.Today)
                    throw new LoiNghiepVu(
                        "Phiếu " + maPhieu + " đã quá hạn từ ngày " + hanTra.ToString("dd/MM/yyyy") +
                        " nên không thể gia hạn.\nHãy trả sách và nộp tiền phạt (nếu có).", "phieu");

                if (soLan >= QuyDinh.SoLanGiaHanToiDa)
                    throw new LoiNghiepVu(
                        "Phiếu " + maPhieu + " đã gia hạn " + soLan + " lần, đạt giới hạn tối đa " +
                        QuyDinh.SoLanGiaHanToiDa + " lần.", "phieu");

                DateTime? theHetHan = NgayTheHetHan(conn, tran, maDocGia);
                if (theHetHan.HasValue)
                    throw new LoiNghiepVu(ThongBaoTheHetHan(theHetHan.Value, "gia hạn phiếu"), "phieu");

                DateTime moi = hanTra.AddDays(QuyDinh.SoNgayMoiLanGiaHan);

                using (SqlCommand cmd = new SqlCommand(
                    @"UPDATE PhieuMuon
                      SET HanTra = @HanMoi,
                          SoLanGiaHan = SoLanGiaHan + 1
                      WHERE MaPhieu = @p", conn, tran))
                {
                    cmd.Parameters.Add(DbConnection.Param("@HanMoi", moi));
                    cmd.Parameters.Add(DbConnection.Param("@p", maPhieu));
                    cmd.ExecuteNonQuery();
                }

                ketQuaHan = moi;
                ketQuaLan = soLan + 1;
            });

            hanMoi = ketQuaHan;
            lanThu = ketQuaLan;
        }
    }
}
