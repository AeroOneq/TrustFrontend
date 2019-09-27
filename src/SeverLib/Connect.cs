using System.Data.SqlClient;

namespace ServerLib
{
    class Connect
    {
        public static SqlConnection Do(string connectionStr)
        {
            return new SqlConnection(connectionStr);
        }
    }
}
