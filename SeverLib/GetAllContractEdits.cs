using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Newtonsoft.Json;

namespace ServerLib
{
    public class GetAllContractEdits
    {
        public static EditQuery CreateEditQueryObjcet(SqlDataReader reader)
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
        /// <summary>
        /// Gets all edit queries which a connected to the given contract
        /// </summary>
        public static List<EditQuery> Get(ContractInfo contract, out string mistakeMsg)
        {
            SqlConnection connection = Connect.Do(Constants.connectionStr);
            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand
                {
                    CommandText = $"SELECT * FROM editQueries WHERE id={contract.Id}",
                    Connection = connection
                };
                SqlDataReader reader = command.ExecuteReader();
                if (!reader.HasRows)
                {
                    mistakeMsg = "Нет предложений по изменению";
                    return null;
                }
                else
                {
                    List<EditQuery> editQueriesList = new List<EditQuery>();
                    while (reader.Read())
                    {
                        editQueriesList.Add(CreateEditQueryObjcet(reader));
                    }
                    mistakeMsg = "Success";
                    return editQueriesList;
                }
            }
            catch (Exception e)
            {
                mistakeMsg = e.Message;
                return null;
            }
            finally
            {
                connection.Close();
            }
        }
    }
}
