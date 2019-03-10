using System;
using System.Data.SqlClient;
using Newtonsoft.Json;

namespace ServerLib
{
    public class CreateNewEdit
    {
        private static int GetNewQueryId(SqlConnection connection)
        {
            SqlCommand getRecordWithBiggestIdCommand = new SqlCommand
            {
                Connection = connection,
                CommandText = "SELECT TOP 1 * FROM editQueries ORDER BY queryId DESC"
            };

            SqlDataReader dataReader = getRecordWithBiggestIdCommand.ExecuteReader();

            if (!dataReader.HasRows)
            {
                dataReader.Close();
                return 0;
            }
            else
            {
                dataReader.Read();
                int biggestId = (int)dataReader.GetValue(10);
                dataReader.Close();
                return biggestId + 1;
            }
        }

        public static bool Create(EditQuery editQuery, out string mistakeMsg)
        {
            SqlConnection connection = Connect.Do(Constants.connectionStr);
            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand
                {
                    CommandText = "INSERT INTO editQueries VALUES(@id, @editText, @editPhotoes, @author," +
                    "@approvedP, @disapprovedP, @closed, @creationDate, @queryHeader, @authorName, " +
                    "@queryId)",
                    Connection = connection
                };

                int queryId = GetNewQueryId(connection);

                command.Parameters.AddWithValue("@id", editQuery.ContractId);
                command.Parameters.AddWithValue("@editText", JsonConvert.SerializeObject(editQuery.ContractText));
                command.Parameters.AddWithValue("@editPhotoes", new byte[0]);
                command.Parameters.AddWithValue("@author", editQuery.AuthorId);
                command.Parameters.AddWithValue("@approvedP", string.Empty);
                command.Parameters.AddWithValue("@disapprovedP", string.Empty);
                command.Parameters.AddWithValue("@closed", editQuery.Closed);
                command.Parameters.AddWithValue("@creationDate", editQuery.CreationDate);
                command.Parameters.AddWithValue("@queryHeader", editQuery.QueryHeader);
                command.Parameters.AddWithValue("@authorName", editQuery.AuthorName);
                command.Parameters.AddWithValue("@queryId", queryId);

                command.ExecuteNonQuery();
                mistakeMsg = "Success";
                return true;
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
