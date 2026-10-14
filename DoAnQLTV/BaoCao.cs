using System;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Printing;
using System.Globalization;
using System.IO;
using System.IO.Compression;   // cần Add Reference: System.IO.Compression
using System.Security;
using System.Text;
using System.Windows.Forms;

namespace DoAnQLTV
{
    /// <summary>Báo cáo danh sách phiếu mượn quá hạn: xuất Excel hoặc in.</summary>
    public static class BaoCao
    {
        private const string DinhDangNgay = "dd/MM/yyyy";

        /// <summary>Các phiếu chưa trả và đã quá hạn (kèm số ngày quá hạn, tiền phạt tạm tính).</summary>
        public static DataTable LayPhieuQuaHan()
        {
            DataTable nguon = DbConnection.ExecuteQuery(@"
                SELECT pm.MaPhieu,
                       pm.MaDocGia + ' - ' + dg.TenDocGia AS DocGia,
                       dg.SoDienThoai,
                       pm.MaSach + ' - ' + s.TenSach AS Sach,
                       pm.NgayMuon,
                       pm.HanTra
                FROM PhieuMuon pm
                INNER JOIN DocGia dg ON pm.MaDocGia = dg.MaDocGia
                INNER JOIN Sach s ON pm.MaSach = s.MaSach
                WHERE pm.NgayTra IS NULL
                  AND pm.HanTra < CAST(GETDATE() AS DATE)
                ORDER BY pm.HanTra, pm.MaPhieu");

            DataTable kq = new DataTable();
            kq.Columns.Add("Mã phiếu", typeof(string));
            kq.Columns.Add("Độc giả", typeof(string));
            kq.Columns.Add("Số điện thoại", typeof(string));
            kq.Columns.Add("Sách", typeof(string));
            kq.Columns.Add("Ngày mượn", typeof(string));
            kq.Columns.Add("Hạn trả", typeof(string));
            kq.Columns.Add("Số ngày quá hạn", typeof(int));
            kq.Columns.Add("Tiền phạt tạm tính", typeof(decimal));

            foreach (DataRow r in nguon.Rows)
            {
                DateTime hanTra = Convert.ToDateTime(r["HanTra"]).Date;
                int soNgay = (DateTime.Today - hanTra).Days;

                kq.Rows.Add(
                    r["MaPhieu"].ToString(),
                    r["DocGia"].ToString(),
                    r["SoDienThoai"] == DBNull.Value ? "" : r["SoDienThoai"].ToString(),
                    r["Sach"].ToString(),
                    Convert.ToDateTime(r["NgayMuon"]).ToString(DinhDangNgay),
                    hanTra.ToString(DinhDangNgay),
                    soNgay,
                    soNgay * QuyDinh.MucPhatMoiNgay);
            }
            return kq;
        }

        // ===================== XUẤT EXCEL =====================

        public static void XuatExcelQuaHan(IWin32Window owner)
        {
            DataTable dt = LayPhieuQuaHan();
            if (dt.Rows.Count == 0)
            {
                AppTheme.Info("Hiện không có phiếu mượn quá hạn.");
                return;
            }

            using (SaveFileDialog dlg = new SaveFileDialog())
            {
                dlg.Title = "Xuất danh sách phiếu quá hạn";
                dlg.Filter = "Excel (*.xlsx)|*.xlsx";
                dlg.FileName = "PhieuQuaHan_" + DateTime.Today.ToString("yyyyMMdd") + ".xlsx";
                if (dlg.ShowDialog(owner) != DialogResult.OK) return;

                try
                {
                    GhiXlsx(dt, "DANH SÁCH PHIẾU MƯỢN QUÁ HẠN", dlg.FileName);
                }
                catch (IOException)
                {
                    AppTheme.Warning("Không ghi được file.\nNếu file đang mở trong Excel, hãy đóng lại rồi thử lại.");
                    return;
                }

                NhatKy.Ghi("Xuất báo cáo", "Xuất " + dt.Rows.Count + " phiếu quá hạn ra Excel");

                if (AppTheme.Confirm("Xuất file thành công.\nBạn có muốn mở file ngay không?"))
                    Process.Start(dlg.FileName);
            }
        }

        private static string TenCot(int c) { return ((char)('A' + c)).ToString(); }   // tối đa 26 cột

        private static string OChu(int c, int hang, string text, int kieu)
        {
            return "<c r=\"" + TenCot(c) + hang + "\" t=\"inlineStr\" s=\"" + kieu + "\"><is><t>"
                   + SecurityElement.Escape(text ?? "") + "</t></is></c>";
        }

        private static string OSo(int c, int hang, object giaTri, int kieu)
        {
            return "<c r=\"" + TenCot(c) + hang + "\" s=\"" + kieu + "\"><v>"
                   + Convert.ToString(giaTri, CultureInfo.InvariantCulture) + "</v></c>";
        }

        private static void ThemFile(ZipArchive zip, string ten, string noiDung)
        {
            ZipArchiveEntry e = zip.CreateEntry(ten);
            using (StreamWriter w = new StreamWriter(e.Open(), new UTF8Encoding(false)))
                w.Write(noiDung);
        }

        /// <summary>Ghi file .xlsx tối giản (không cần thư viện ngoài).</summary>
        private static void GhiXlsx(DataTable dt, string tieuDe, string duongDan)
        {
            const string xml = "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>";

            StringBuilder sheet = new StringBuilder();
            sheet.Append(xml);
            sheet.Append("<worksheet xmlns=\"http://schemas.openxmlformats.org/spreadsheetml/2006/main\">");
            sheet.Append("<cols><col min=\"1\" max=\"" + dt.Columns.Count + "\" width=\"24\" customWidth=\"1\"/></cols>");
            sheet.Append("<sheetData>");

            int hang = 1;
            sheet.Append("<row r=\"" + hang + "\">" + OChu(0, hang, tieuDe, 1) + "</row>");
            hang++;
            sheet.Append("<row r=\"" + hang + "\">"
                + OChu(0, hang, "Ngày lập: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm")
                    + "  |  Mức phạt: " + QuyDinh.DinhDangTien(QuyDinh.MucPhatMoiNgay) + "/ngày", 0)
                + "</row>");
            hang += 2;   // chừa 1 dòng trống

            sheet.Append("<row r=\"" + hang + "\">");
            for (int c = 0; c < dt.Columns.Count; c++)
                sheet.Append(OChu(c, hang, dt.Columns[c].ColumnName, 1));
            sheet.Append("</row>");
            hang++;

            decimal tongPhat = 0;
            foreach (DataRow r in dt.Rows)
            {
                sheet.Append("<row r=\"" + hang + "\">");
                for (int c = 0; c < dt.Columns.Count; c++)
                {
                    object v = r[c];
                    if (v is decimal) { sheet.Append(OSo(c, hang, v, 2)); tongPhat += (decimal)v; }
                    else if (v is int) sheet.Append(OSo(c, hang, v, 0));
                    else sheet.Append(OChu(c, hang, Convert.ToString(v), 0));
                }
                sheet.Append("</row>");
                hang++;
            }

            // Dòng tổng
            hang++;
            sheet.Append("<row r=\"" + hang + "\">"
                + OChu(0, hang, "Tổng: " + dt.Rows.Count + " phiếu quá hạn", 1)
                + OChu(dt.Columns.Count - 2, hang, "Tổng tiền phạt:", 1)
                + OSo(dt.Columns.Count - 1, hang, tongPhat, 2)
                + "</row>");

            sheet.Append("</sheetData></worksheet>");

            string contentTypes = xml
                + "<Types xmlns=\"http://schemas.openxmlformats.org/package/2006/content-types\">"
                + "<Default Extension=\"rels\" ContentType=\"application/vnd.openxmlformats-package.relationships+xml\"/>"
                + "<Default Extension=\"xml\" ContentType=\"application/xml\"/>"
                + "<Override PartName=\"/xl/workbook.xml\" ContentType=\"application/vnd.openxmlformats-officedocument.spreadsheetml.sheet.main+xml\"/>"
                + "<Override PartName=\"/xl/worksheets/sheet1.xml\" ContentType=\"application/vnd.openxmlformats-officedocument.spreadsheetml.worksheet+xml\"/>"
                + "<Override PartName=\"/xl/styles.xml\" ContentType=\"application/vnd.openxmlformats-officedocument.spreadsheetml.styles+xml\"/>"
                + "</Types>";

            string rels = xml
                + "<Relationships xmlns=\"http://schemas.openxmlformats.org/package/2006/relationships\">"
                + "<Relationship Id=\"rId1\" Type=\"http://schemas.openxmlformats.org/officeDocument/2006/relationships/officeDocument\" Target=\"xl/workbook.xml\"/>"
                + "</Relationships>";

            string workbook = xml
                + "<workbook xmlns=\"http://schemas.openxmlformats.org/spreadsheetml/2006/main\" "
                + "xmlns:r=\"http://schemas.openxmlformats.org/officeDocument/2006/relationships\">"
                + "<sheets><sheet name=\"Phieu qua han\" sheetId=\"1\" r:id=\"rId1\"/></sheets></workbook>";

            string workbookRels = xml
                + "<Relationships xmlns=\"http://schemas.openxmlformats.org/package/2006/relationships\">"
                + "<Relationship Id=\"rId1\" Type=\"http://schemas.openxmlformats.org/officeDocument/2006/relationships/worksheet\" Target=\"worksheets/sheet1.xml\"/>"
                + "<Relationship Id=\"rId2\" Type=\"http://schemas.openxmlformats.org/officeDocument/2006/relationships/styles\" Target=\"styles.xml\"/>"
                + "</Relationships>";

            // 3 kiểu ô: 0 = thường, 1 = chữ đậm, 2 = số có dấu phân cách hàng nghìn
            string styles = xml
                + "<styleSheet xmlns=\"http://schemas.openxmlformats.org/spreadsheetml/2006/main\">"
                + "<fonts count=\"2\"><font><sz val=\"11\"/><name val=\"Calibri\"/></font>"
                + "<font><b/><sz val=\"11\"/><name val=\"Calibri\"/></font></fonts>"
                + "<fills count=\"2\"><fill><patternFill patternType=\"none\"/></fill><fill><patternFill patternType=\"gray125\"/></fill></fills>"
                + "<borders count=\"1\"><border><left/><right/><top/><bottom/><diagonal/></border></borders>"
                + "<cellStyleXfs count=\"1\"><xf numFmtId=\"0\" fontId=\"0\" fillId=\"0\" borderId=\"0\"/></cellStyleXfs>"
                + "<cellXfs count=\"3\">"
                + "<xf numFmtId=\"0\" fontId=\"0\" fillId=\"0\" borderId=\"0\" xfId=\"0\"/>"
                + "<xf numFmtId=\"0\" fontId=\"1\" fillId=\"0\" borderId=\"0\" xfId=\"0\" applyFont=\"1\"/>"
                + "<xf numFmtId=\"3\" fontId=\"0\" fillId=\"0\" borderId=\"0\" xfId=\"0\" applyNumberFormat=\"1\"/>"
                + "</cellXfs></styleSheet>";

            using (FileStream fs = new FileStream(duongDan, FileMode.Create))
            using (ZipArchive zip = new ZipArchive(fs, ZipArchiveMode.Create))
            {
                ThemFile(zip, "[Content_Types].xml", contentTypes);
                ThemFile(zip, "_rels/.rels", rels);
                ThemFile(zip, "xl/workbook.xml", workbook);
                ThemFile(zip, "xl/_rels/workbook.xml.rels", workbookRels);
                ThemFile(zip, "xl/styles.xml", styles);
                ThemFile(zip, "xl/worksheets/sheet1.xml", sheet.ToString());
            }
        }

        // ===================== IN =====================

        /// <summary>Mở cửa sổ xem trước khi in (trong đó có nút In).</summary>
        public static void InQuaHan()
        {
            DataTable dt = LayPhieuQuaHan();
            if (dt.Rows.Count == 0)
            {
                AppTheme.Info("Hiện không có phiếu mượn quá hạn.");
                return;
            }

            float[] rong = { 80, 190, 110, 250, 95, 95, 85, 110 };
            float tongRong = 0;
            foreach (float w in rong) tongRong += w;

            decimal tongPhat = 0;
            foreach (DataRow r in dt.Rows) tongPhat += (decimal)r["Tiền phạt tạm tính"];

            int dong = 0;
            int trang = 0;
            bool daInTong = false;

            PrintDocument pd = new PrintDocument();
            pd.DocumentName = "Danh sách phiếu quá hạn";
            pd.DefaultPageSettings.Landscape = true;
            pd.DefaultPageSettings.Margins = new Margins(50, 50, 50, 50);

            pd.BeginPrint += (s, e) => { dong = 0; trang = 0; daInTong = false; };

            pd.PrintPage += (s, e) =>
            {
                trang++;
                Graphics g = e.Graphics;
                float x0 = e.MarginBounds.Left;
                float y = e.MarginBounds.Top;
                float k = e.MarginBounds.Width / tongRong;   // co giãn vừa khổ giấy
                const float cao = 26f;

                using (Font fTieuDe = new Font("Segoe UI", 16F, FontStyle.Bold))
                using (Font fDam = new Font("Segoe UI", 10F, FontStyle.Bold))
                using (Font fThuong = new Font("Segoe UI", 10F))
                using (Brush bDen = new SolidBrush(Color.Black))
                using (Brush bTrang = new SolidBrush(Color.White))
                using (Brush bNen = new SolidBrush(AppTheme.Primary))
                using (Pen but = new Pen(Color.Silver))
                using (StringFormat trai = new StringFormat())
                using (StringFormat phai = new StringFormat())
                {
                    trai.LineAlignment = StringAlignment.Center;
                    trai.Trimming = StringTrimming.EllipsisCharacter;
                    trai.FormatFlags = StringFormatFlags.NoWrap;
                    phai.LineAlignment = StringAlignment.Center;
                    phai.Alignment = StringAlignment.Far;
                    phai.Trimming = StringTrimming.EllipsisCharacter;
                    phai.FormatFlags = StringFormatFlags.NoWrap;

                    if (trang == 1)
                    {
                        g.DrawString("DANH SÁCH PHIẾU MƯỢN QUÁ HẠN", fTieuDe, bNen, x0, y);
                        y += 36;
                        g.DrawString("Ngày in: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm")
                            + "   |   Mức phạt: " + QuyDinh.DinhDangTien(QuyDinh.MucPhatMoiNgay) + "/ngày",
                            fThuong, bDen, x0, y);
                        y += 30;
                    }

                    // Hàng tiêu đề (in lại ở mỗi trang)
                    float x = x0;
                    for (int c = 0; c < rong.Length; c++)
                    {
                        float w = rong[c] * k;
                        g.FillRectangle(bNen, x, y, w, cao);
                        g.DrawString(dt.Columns[c].ColumnName, fDam, bTrang,
                            new RectangleF(x + 4, y, w - 8, cao), c >= 6 ? phai : trai);
                        x += w;
                    }
                    y += cao;

                    // Các hàng dữ liệu
                    while (dong < dt.Rows.Count)
                    {
                        if (y + cao > e.MarginBounds.Bottom)
                        {
                            e.HasMorePages = true;
                            return;
                        }

                        DataRow r = dt.Rows[dong];
                        x = x0;
                        for (int c = 0; c < rong.Length; c++)
                        {
                            float w = rong[c] * k;
                            object v = r[c];
                            string text = v is decimal ? QuyDinh.DinhDangTien((decimal)v) : Convert.ToString(v);
                            g.DrawString(text, fThuong, bDen, new RectangleF(x + 4, y, w - 8, cao), c >= 6 ? phai : trai);
                            x += w;
                        }
                        g.DrawLine(but, x0, y + cao, x0 + e.MarginBounds.Width, y + cao);
                        y += cao;
                        dong++;
                    }

                    // Dòng tổng ở cuối
                    if (!daInTong)
                    {
                        if (y + cao > e.MarginBounds.Bottom)
                        {
                            e.HasMorePages = true;
                            return;
                        }
                        y += 8;
                        g.DrawString("Tổng cộng: " + dt.Rows.Count + " phiếu quá hạn  -  Tiền phạt tạm tính: "
                            + QuyDinh.DinhDangTien(tongPhat), fDam, bDen, x0, y);
                        daInTong = true;
                    }
                }

                e.HasMorePages = false;
            };

            try
            {
                using (PrintPreviewDialog pv = new PrintPreviewDialog())
                {
                    pv.Document = pd;
                    pv.Width = 1100;
                    pv.Height = 750;
                    pv.StartPosition = FormStartPosition.CenterScreen;
                    pv.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                AppTheme.Error("Không thể mở bản in (máy có thể chưa cài máy in).\n\n" + ex.Message);
            }
        }
    }
}
