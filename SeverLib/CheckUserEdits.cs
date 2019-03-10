using System.Data.SqlClient;
using System;

namespace ServerLib
{
    public class CheckUserEdits
    {
        public static bool Check(UserInfo user, out string mistakeMsg)
        {
            SqlConnection connection = Connect.Do(Constants.connectionStr);
            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand
                {
                    CommandText = $"SELECT * FROM editQueries WHERE author={user.Id} AND closed='False'",
                    Connection = connection
                };
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    mistakeMsg = "У вас уже есть незакрытые изменения";
                    return false;
                }
                else
                {
                    mistakeMsg = "Success";
                    return true;
                }
            }
            catch (Exception e)
            {
                mistakeMsg = e.Message;
                return false;
            }
            finally
            {
                connection.Close();
            }
        }
    }
}
