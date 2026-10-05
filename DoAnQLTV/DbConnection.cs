using System.Data.SqlClient;

namespace DoAnQLTV
{
    public class DbConnection
    {
        private static string connectionString =
            @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=QuanLyThuVien;Integrated Security=True";

        public static SqlConnection GetConnection()
        {
            return new SqlConnection(connectionString);
        }
    }
}