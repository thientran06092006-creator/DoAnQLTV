using System;
using System.Globalization;

namespace DoAnQLTV
{
    /// <summary>
    /// Các quy định của thư viện. Muốn đổi mức phạt / số sách tối đa / số lần gia hạn...
    /// chỉ cần sửa ở đây.
    /// </summary>
    public static class QuyDinh
    {
        /// <summary>Tiền phạt cho mỗi ngày trả muộn (mỗi phiếu = 1 cuốn sách).</summary>
        public const decimal MucPhatMoiNgay = 2000m;

        /// <summary>Số sách tối đa một độc giả được mượn cùng lúc (tính các phiếu chưa trả).</summary>
        public const int SoSachToiDa = 5;

        /// <summary>Mật khẩu gán cho nhân viên mới / nhân viên cũ chưa có mật khẩu.</summary>
        public const string MatKhauMacDinh = "123456";

        /// <summary>Mỗi phiếu được gia hạn tối đa bao nhiêu lần.</summary>
        public const int SoLanGiaHanToiDa = 2;

        /// <summary>Mỗi lần gia hạn được cộng thêm bao nhiêu ngày vào hạn trả.</summary>
        public const int SoNgayMoiLanGiaHan = 7;

        /// <summary>Thẻ độc giả có hiệu lực bao nhiêu tháng (khi đăng ký mới hoặc gia hạn thẻ).</summary>
        public const int SoThangHanThe = 12;

        /// <summary>Số ngày trả muộn (0 nếu trả đúng hạn hoặc sớm).</summary>
        public static int SoNgayTre(DateTime hanTra, DateTime ngayTra)
        {
            int n = (ngayTra.Date - hanTra.Date).Days;
            return n > 0 ? n : 0;
        }

        public static decimal TinhTienPhat(DateTime hanTra, DateTime ngayTra)
        {
            return SoNgayTre(hanTra, ngayTra) * MucPhatMoiNgay;
        }

        /// <summary>Ví dụ: 6000 -> "6.000đ"</summary>
        public static string DinhDangTien(decimal soTien)
        {
            return soTien.ToString("N0", new CultureInfo("vi-VN")) + "đ";
        }
    }
}
