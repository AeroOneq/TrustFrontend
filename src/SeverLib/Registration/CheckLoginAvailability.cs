using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace ServerLib
{
    public class CheckLoginAvailability
    {
        public static int Check(string login, out string mistakeMsg)
        {
            mistakeMsg = string.Empty;
            SqlConnection connection = Connect.Do(Constants.connectionStr);
            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand()
                {
                    CommandText = $"SELECT * FROM users WHERE login = '{login}'",
                    Connection = connection
                };
                SqlDataReader dataReader = command.ExecuteReader();
                return (dataReader.HasRows) ? (0) : (1);
            }
            catch (Exception e)
            {
                mistakeMsg = e.Message;
                return 2;
            }
        }
    }
}
