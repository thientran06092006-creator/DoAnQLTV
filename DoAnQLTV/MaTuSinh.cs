using System;
using System.Collections.Generic;
using System.Data;
using System.Text.RegularExpressions;

namespace DoAnQLTV
{
    /// <summary>
    /// Tự sinh mã kế tiếp, ví dụ PM0001 -> PM0002.
    ///
    /// Cách hoạt động: đọc các mã đang có trong bảng, chỉ xét mã dạng "chữ + số" (PM0007, DG025...).
    /// Lấy tiền tố được dùng nhiều nhất, tìm số lớn nhất rồi cộng 1 và giữ nguyên độ rộng chữ số
    /// (nên bảng đang dùng "DG025" thì mã mới là "DG026", không bị lệch quy ước).
    /// Bảng chưa có mã nào thì dùng tiền tố và độ rộng mặc định.
    ///
    /// Mã chỉ là gợi ý điền sẵn vào ô, người dùng vẫn sửa được.
    /// Lưu ý: tên bảng / tên cột lấy từ code của chương trình, KHÔNG lấy từ người dùng nhập.
    /// </summary>
    public static class MaTuSinh
    {
        private class NhomTienTo
        {
            public string TienTo;
            public int Dem;
            public long LonNhat;
            public int DoRong;
        }

        public static string TiepTheo(string bang, string cot, string tienToMacDinh, int doRongMacDinh)
        {
            try
            {
                DataTable dt = DbConnection.ExecuteQuery("SELECT " + cot + " FROM " + bang);

                Regex mau = new Regex(@"^([A-Za-z]+)(\d{1,15})$");
                Dictionary<string, NhomTienTo> cacNhom =
                    new Dictionary<string, NhomTienTo>(StringComparer.OrdinalIgnoreCase);

                foreach (DataRow r in dt.Rows)
                {
                    if (r[0] == DBNull.Value) continue;

                    Match m = mau.Match(r[0].ToString().Trim());
                    if (!m.Success) continue;   // bỏ qua mã không theo dạng chữ + số (vd "admin")

                    string tienTo = m.Groups[1].Value;
                    string so = m.Groups[2].Value;
                    long giaTri = long.Parse(so);

                    NhomTienTo nhom;
                    if (!cacNhom.TryGetValue(tienTo, out nhom))
                    {
                        nhom = new NhomTienTo();
                        nhom.TienTo = tienTo;
                        cacNhom[tienTo] = nhom;
                    }

                    nhom.Dem++;
                    if (giaTri > nhom.LonNhat) nhom.LonNhat = giaTri;
                    if (so.Length > nhom.DoRong) nhom.DoRong = so.Length;
                }

                NhomTienTo chon = null;
                foreach (NhomTienTo nhom in cacNhom.Values)
                    if (chon == null || nhom.Dem > chon.Dem)
                        chon = nhom;

                if (chon == null)
                    return tienToMacDinh + "1".PadLeft(doRongMacDinh, '0');

                return chon.TienTo + (chon.LonNhat + 1).ToString().PadLeft(chon.DoRong, '0');
            }
            catch (Exception ex)
            {
                // Không sinh được thì để trống cho người dùng tự nhập
                System.Diagnostics.Debug.WriteLine("Không sinh được mã cho " + bang + ": " + ex.Message);
                return "";
            }
        }
    }
}
