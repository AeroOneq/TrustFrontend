using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace ServerLib
{
    public static class EditQueryDB
    {
        /// <summary>
        /// Gets all edit queries which a connected to the given contract
        /// </summary>
        public static List<EditQuery> GetContractQueries(ContractInfo contract)
        {
            SqlConnection connection = Connect.Do(Constants.connectionStr);
            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand
                {
                    CommandText = $"SELECT * FROM editQueries WHERE contractID like {contract.Id}",
                    Connection = connection
                };
                SqlDataReader reader = command.ExecuteReader();
                if (!reader.HasRows)
                {
                    return new List<EditQuery>();
                }
                else
                {
                    List<EditQuery> editQueriesList = new List<EditQuery>();
                    while (reader.Read())
                    {
                        editQueriesList.Add(CreateEditQueryObjcet(reader));
                    }
                    return editQueriesList;
                }
            }
            finally
            {
                connection.Close();
            }
        }
        private static EditQuery CreateEditQueryObjcet(SqlDataReader reader)
        {
            int id = (int)reader.GetValue(0);
            ContractText cText = JsonConvert.DeserializeObject<ContractText>(
                (string)reader.GetValue(1));
            byte[] photos = (byte[])reader.GetValue(2);
            int authorId = (int)reader.GetValue(3);
            int[] approvedP = TransferArrays.StringToInt((string)reader.GetValue(4));
            int[] disapprovedP = TransferArrays.StringToInt((string)reader.GetValue(5));
            bool closed = (bool)reader.GetValue(6);
            DateTime creationDate = (DateTime)reader.GetValue(7);
            string queryHeader = (string)reader.GetValue(8);
            string authorName = (string)reader.GetValue(9);
            int queryId = (int)reader.GetValue(10);

            return new EditQuery(id, cText, photos, authorId, approvedP,
                disapprovedP, closed, creationDate, queryHeader, authorName,
                queryId);
        }

        public static void CreateNewRecord(EditQuery editQuery)
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
            }
            finally
            {
                connection.Close();
            }
        }
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

        /// <summary>
        /// Updates the edit query record by changing the disapprovedP, approvedP and closed
        /// colls of the record.
        /// </summary>
        /// <returns>Error message if the exception occured</returns>
        public static async Task UpdateQueryStatus(EditQuery editQuery, ContractInfo contract)
        {
            SqlConnection connection = Connect.Do(Constants.connectionStr);
            try
            {
                connection.Open();
                SqlCommand updateQueryStatusCommand = new SqlCommand
                {
                    Connection = connection,
                    CommandText = @"UPDATE editQueries SET approvedP = @approvedP,
                        disapprovedP = @disapprovedP, closed = @closed WHERE 
                        queryId like @queryId"
                };
                updateQueryStatusCommand.Parameters.AddWithValue("@approvedP",
                    TransferArrays.IntToString(editQuery.ApprovedP));
                updateQueryStatusCommand.Parameters.AddWithValue("@disapprovedP",
                    TransferArrays.IntToString(editQuery.DisapprovedP));
                updateQueryStatusCommand.Parameters.AddWithValue("@closed", editQuery.Closed);
                updateQueryStatusCommand.Parameters.AddWithValue("@queryId",
                    editQuery.QueryId);

                if (editQuery.ApprovedP.Length == contract.ParticipantsId.Length)
                    await ContractService.UpdateContractText(contract, editQuery, connection);

                updateQueryStatusCommand.ExecuteNonQuery();
            }
            finally
            {
                connection.Close();
            }
        }
    }
}
