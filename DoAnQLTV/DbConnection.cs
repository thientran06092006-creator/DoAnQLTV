using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace DoAnQLTV
{
    /// <summary>
    /// Lớp truy cập dữ liệu dùng chung. Mọi form nên gọi qua các hàm bên dưới
    /// thay vì tự tạo SqlConnection / ghép chuỗi SQL.
    ///
    /// Ví dụ dùng:
    ///   var dt = DbConnection.ExecuteQuery(
    ///       "SELECT * FROM Sach WHERE TenSach LIKE @ten",
    ///       DbConnection.Param("@ten", "%" + txtTim.Text.Trim() + "%"));
    ///
    ///   int soDong = DbConnection.ExecuteNonQuery(
    ///       "DELETE FROM Sach WHERE MaSach = @ma",
    ///       DbConnection.Param("@ma", maSach));
    ///
    ///   int tongSach = DbConnection.ExecuteScalar&lt;int&gt;("SELECT COUNT(*) FROM Sach");
    /// </summary>
    public static class DbConnection
    {
        // Chuỗi kết nối nằm trong App.config (thẻ <connectionStrings>)
        private static readonly string _connectionString =
            ConfigurationManager.ConnectionStrings["QuanLyThuVien"].ConnectionString;

        /// <summary>Giữ lại hàm cũ để các form đang dùng không bị lỗi.</summary>
        public static SqlConnection GetConnection()
        {
            return new SqlConnection(_connectionString);
        }

        /// <summary>Tạo tham số, tự đổi null thành DBNull.</summary>
        public static SqlParameter Param(string name, object value)
        {
            return new SqlParameter(name, value ?? DBNull.Value);
        }

        /// <summary>Chạy SELECT, trả về DataTable (gán thẳng vào DataGridView.DataSource).</summary>
        public static DataTable ExecuteQuery(string sql, params SqlParameter[] parameters)
        {
            using (var conn = GetConnection())
            using (var cmd = new SqlCommand(sql, conn))
            using (var adapter = new SqlDataAdapter(cmd))
            {
                if (parameters != null && parameters.Length > 0)
                    cmd.Parameters.AddRange(parameters);

                var table = new DataTable();
                adapter.Fill(table);
                return table;
            }
        }

        /// <summary>Chạy INSERT / UPDATE / DELETE, trả về số dòng bị ảnh hưởng.</summary>
        public static int ExecuteNonQuery(string sql, params SqlParameter[] parameters)
        {
            using (var conn = GetConnection())
            using (var cmd = new SqlCommand(sql, conn))
            {
                if (parameters != null && parameters.Length > 0)
                    cmd.Parameters.AddRange(parameters);

                conn.Open();
                return cmd.ExecuteNonQuery();
            }
        }

        /// <summary>Chạy câu lệnh trả về 1 giá trị (COUNT, SUM, MAX...).</summary>
        public static T ExecuteScalar<T>(string sql, params SqlParameter[] parameters)
        {
            using (var conn = GetConnection())
            using (var cmd = new SqlCommand(sql, conn))
            {
                if (parameters != null && parameters.Length > 0)
                    cmd.Parameters.AddRange(parameters);

                conn.Open();
                object result = cmd.ExecuteScalar();
                if (result == null || result == DBNull.Value)
                    return default(T);

                return (T)Convert.ChangeType(result, typeof(T));
            }
        }

        /// <summary>
        /// Chạy nhiều lệnh trong 1 transaction: hoặc thành công hết, hoặc hoàn tác hết.
        /// Rất hợp cho "Lập phiếu mượn" (thêm phiếu + thêm chi tiết + trừ số lượng sách).
        ///
        ///   DbConnection.ExecuteInTransaction((conn, tran) =>
        ///   {
        ///       using (var cmd = new SqlCommand("INSERT ...", conn, tran)) { ... cmd.ExecuteNonQuery(); }
        ///       using (var cmd = new SqlCommand("UPDATE ...", conn, tran)) { ... cmd.ExecuteNonQuery(); }
        ///   });
        /// </summary>
        public static void ExecuteInTransaction(Action<SqlConnection, SqlTransaction> work)
        {
            using (var conn = GetConnection())
            {
                conn.Open();
                using (var tran = conn.BeginTransaction())
                {
                    try
                    {
                        work(conn, tran);
                        tran.Commit();
                    }
                    catch
                    {
                        tran.Rollback();
                        throw;
                    }
                }
            }
        }

        /// <summary>Thay cho FrmTestConnection: kiểm tra kết nối ngay khi khởi động.</summary>
        public static bool TestConnection(out string error)
        {
            try
            {
                using (var conn = GetConnection())
                {
                    conn.Open();
                    error = null;
                    return true;
                }
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }
    }
}
