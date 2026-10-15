using System;
using System.Drawing;
using System.Windows.Forms;

namespace DoAnQLTV
{
    public partial class FrmHuongDanSuDung : Form
    {
        public FrmHuongDanSuDung()
        {
            InitializeComponent();

            AppTheme.ApplyForm(this);
            ApDungGiaoDien();
            NapNoiDung();
        }

        private void ApDungGiaoDien()
        {
            AppTheme.StyleCard(pnlNoiDung);

            AppTheme.StyleButton(btnDong,
                ColorTranslator.FromHtml("#E2E8F0"),
                ColorTranslator.FromHtml("#CBD5E1"),
                AppTheme.TextDark);
        }

        private void btnDong_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // ================= NỘI DUNG HƯỚNG DẪN =================
        // Nội dung được nạp bằng code (thay cho chữ nằm trong resource của Designer).
        // Các con số (mức phạt, giới hạn mượn, mật khẩu mặc định) lấy từ QuyDinh
        // nên đổi quy định thì hướng dẫn tự đổi theo.

        private Font fTieuDe;
        private Font fMuc;

        private void Them(string text, Font font, Color mau)
        {
            int batDau = rtbHuongDan.TextLength;
            rtbHuongDan.AppendText(text + "\n");
            rtbHuongDan.Select(batDau, text.Length);
            rtbHuongDan.SelectionFont = font;
            rtbHuongDan.SelectionColor = mau;
            rtbHuongDan.Select(rtbHuongDan.TextLength, 0);
        }

        private void TieuDe(string text)
        {
            rtbHuongDan.AppendText("\n");
            Them(text, fTieuDe, AppTheme.Primary);
        }

        private void Muc(string text)
        {
            Them(text, fMuc, AppTheme.TextDark);
        }

        private void Dong(string text)
        {
            Them("   •  " + text, rtbHuongDan.Font, AppTheme.TextDark);
        }

        private void NapNoiDung()
        {
            string phat = QuyDinh.DinhDangTien(QuyDinh.MucPhatMoiNgay);

            rtbHuongDan.Clear();
            fTieuDe = new Font("Segoe UI", 12.5F, FontStyle.Bold);
            fMuc = new Font(rtbHuongDan.Font, FontStyle.Bold);

            Them("HƯỚNG DẪN SỬ DỤNG HỆ THỐNG QUẢN LÝ THƯ VIỆN", fTieuDe, AppTheme.Primary);

            // ---------- 1 ----------
            TieuDe("1. Đăng nhập và tài khoản");
            Dong("Đăng nhập bằng Mã nhân viên (làm tên đăng nhập) và mật khẩu. Nhân viên mới có mật khẩu mặc định là \""
                 + QuyDinh.MatKhauMacDinh + "\".");
            Dong("Nên đổi mật khẩu ngay: bấm vào tên hoặc ảnh đại diện ở góc phải trang chủ, chọn \"Đổi mật khẩu\". "
                 + "Mật khẩu mới phải có ít nhất 6 ký tự. Chương trình cũng tự nhắc khi bạn còn dùng mật khẩu mặc định.");
            Dong("Có 2 vai trò: Quản trị viên (dùng được mọi chức năng) và Nhân viên (không vào được \"Quản lý nhân viên\" "
                 + "và \"Xem tất cả nhật ký\").");
            Dong("Đăng xuất: mục \"Đăng xuất\" ở thanh bên hoặc ở menu khi bấm vào tên người dùng.");

            // ---------- 2 ----------
            TieuDe("2. Trang chủ");
            Dong("Bốn thẻ thống kê: tổng số đầu sách, số độc giả, số phiếu đang hiệu lực và số phiếu quá hạn.");
            Dong("Biểu đồ mượn - trả theo tháng và biểu đồ thể loại sách. Biểu tượng chuông hiện thông báo khi có phiếu quá hạn.");
            Dong("\"Truy cập nhanh\" mở nhanh các chức năng hay dùng.");
            Dong("\"Hoạt động gần đây\" hiện 4 thao tác mới nhất. Quản trị viên bấm \"Xem tất cả\" để xem toàn bộ nhật ký.");

            // ---------- 3 ----------
            TieuDe("3. Quản lý sách, thể loại, tác giả");
            Dong("Chọn một dòng trong danh sách để sửa hoặc xóa. Bấm \"Làm mới\" để quay về chế độ nhập mới. "
                 + "Mã là khóa chính nên không đổi được khi đang sửa.");
            Dong("Mỗi đầu sách có \"Số lượng\" (số bản). Cột \"Còn lại\" = số lượng trừ số bản đang được mượn chưa trả; "
                 + "hết sách thì hiện màu đỏ.");
            Dong("Không thể giảm số lượng thấp hơn số bản đang được mượn. Không thể xóa sách, thể loại hoặc tác giả "
                 + "đang được sử dụng.");
            Dong("Ô \"Tên\" cũng dùng làm từ khóa khi bấm \"Tìm kiếm\".");

            // ---------- 4 ----------
            TieuDe("4. Quản lý độc giả và tra cứu");
            Dong("Quản lý độc giả: thêm, sửa, xóa. Số điện thoại gồm 9 đến 11 chữ số.");
            Dong("Thông tin độc giả: tìm hoặc bấm vào một độc giả để xem lịch sử mượn, số cuốn đang mượn, số phiếu quá hạn "
                 + "và tiền phạt (đã thu / chưa thu).");
            Dong("Tìm kiếm sách: tìm theo mã, tên, thể loại hoặc tác giả; xem số lượng, số bản còn lại và tình trạng "
                 + "\"Có thể mượn\" / \"Hết sách\".");

            // ---------- 5 ----------
            TieuDe("5. Quản lý nhân viên (chỉ quản trị viên)");
            Dong("Thêm nhân viên và chọn vai trò. Nhân viên mới có mật khẩu mặc định \"" + QuyDinh.MatKhauMacDinh + "\".");
            Dong("Nút \"Đặt mật khẩu\" dùng khi nhân viên quên mật khẩu: chọn nhân viên, bấm nút và nhập mật khẩu mới.");
            Dong("Không thể xóa hoặc tự đổi vai trò của tài khoản đang đăng nhập.");

            // ---------- 6 ----------
            TieuDe("6. Lập phiếu mượn");
            Dong("Nhập mã phiếu, chọn độc giả, nhân viên (mặc định là người đang đăng nhập), sách, ngày mượn và hạn trả "
                 + "(mặc định 7 ngày). Bấm \"Lập phiếu\" hoặc \"Thêm\".");
            Dong("Danh sách sách hiện kèm số bản còn lại, ví dụ \"(còn 2/3)\". Sách hết bản thì không thể mượn.");
            Dong("Mỗi độc giả được mượn tối đa " + QuyDinh.SoSachToiDa + " cuốn cùng lúc (tính các phiếu chưa trả).");
            Dong("Độc giả còn sách quá hạn chưa trả thì không được mượn thêm.");

            // ---------- 7 ----------
            TieuDe("7. Trả sách và tiền phạt");
            Dong("Chọn phiếu trong danh sách (hoặc nhập mã phiếu), chọn ngày trả rồi bấm \"Trả sách\".");
            Dong("Trả muộn bị phạt " + phat + " cho mỗi ngày của mỗi cuốn. Hộp xác nhận cho biết số ngày muộn và tiền phạt.");
            Dong("Sau khi trả, chương trình hỏi đã thu tiền phạt chưa. Nếu chưa, bạn có thể thu sau ở Danh sách phiếu mượn.");

            // ---------- 8 ----------
            TieuDe("8. Danh sách phiếu mượn, thu phạt và báo cáo");
            Dong("Trạng thái: Đang mượn (xanh dương), Đã trả (xanh lá), Quá hạn (đỏ, kèm số ngày và tiền phạt tạm tính).");
            Dong("Thu phạt: chọn phiếu đã trả có tiền phạt, bấm \"Thu phạt\" và xác nhận. Cột \"Tiền phạt\" sẽ chuyển thành \"Đã thu\".");
            Dong("\"Xuất Excel quá hạn\" lưu danh sách phiếu quá hạn ra file .xlsx; \"In quá hạn\" mở bản xem trước để in.");

            // ---------- 9 ----------
            TieuDe("9. Nhật ký hoạt động");
            Dong("Mọi thao tác quan trọng (đăng nhập, thêm/sửa/xóa, mượn, trả, thu phạt, đổi mật khẩu...) đều được ghi lại "
                 + "cùng thời gian và người thực hiện.");
            Dong("Quản trị viên có thể lọc nhật ký theo ngày, nhân viên, loại hoạt động và từ khóa.");

            // ---------- Lưu ý ----------
            TieuDe("Lưu ý");
            Dong("Nhập đúng và đầy đủ thông tin trước khi lưu; chương trình sẽ báo lỗi nếu thiếu hoặc sai định dạng.");
            Dong("Không thể xóa dữ liệu đang được tham chiếu (ví dụ độc giả hoặc nhân viên đã có phiếu mượn).");
            Dong("Nếu gặp lỗi kết nối cơ sở dữ liệu, hãy kiểm tra SQL Server LocalDB và chuỗi kết nối trong App.config.");

            fTieuDe.Dispose();
            fMuc.Dispose();

            rtbHuongDan.Select(0, 0);
            rtbHuongDan.ScrollToCaret();
        }
    }
}
