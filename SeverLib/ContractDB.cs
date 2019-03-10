using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace ServerLib
{
    public class ContractDB
    {
        /// <summary>
        /// Updates the contract record with it's primary key
        /// </summary>
        public static void UpdateContractRecord(ContractInfo contract)
        {
            SqlConnection connection = Connect.Do(Constants.connectionStr);
            try
            {
                connection.Open();
                SqlCommand updateCommand = new SqlCommand()
                {
                    Connection = connection,
                    CommandText = @"UPDATE TABLE contracts SET cText = @cText, photos = @photos, authorId = @authorId, 
                    participants = @participants, unsignedP = @unsignedP, approvedP = @approvedP, 
                    disapprovedP = @disapprovedP, name = @name, creationDate = @creationDate WHERE id like @id"
                };

                string cText = JsonConvert.SerializeObject(contract.ContractText);
                //transfer integer array of all participants to string representation
                string participantsId = TransferArrays.IntToString(contract.ParticipantsId);
                //transfer integer array of unsigned participants to string representation
                string unsignedP = TransferArrays.IntToString(contract.UnsignedP);
                //transfer integer array of approved participants to string representation
                string approvedP = TransferArrays.IntToString(contract.ApprovedP);
                //transfer integer array of disapproved participants to string representation
                string disapprovedP = TransferArrays.IntToString(contract.DisapprovedP);

                updateCommand.Parameters.AddWithValue("@id", contract.Id);
                updateCommand.Parameters.AddWithValue("@cText", cText);
                updateCommand.Parameters.AddWithValue("@photos", contract.Photos);
                updateCommand.Parameters.AddWithValue("@authorId", contract.AuthorId);
                updateCommand.Parameters.AddWithValue("@participants", participantsId);
                updateCommand.Parameters.AddWithValue("@unsignedP", unsignedP);
                updateCommand.Parameters.AddWithValue("@approvedP", approvedP);
                updateCommand.Parameters.AddWithValue("@disapprovedP", disapprovedP);
                updateCommand.Parameters.AddWithValue("@name", contract.Name);
                updateCommand.Parameters.AddWithValue("@creationDate", contract.CreationDate);

                updateCommand.ExecuteNonQuery();
            }
            finally
            {
                connection.Close();
            }
        }

        public static void CreateCosmosDBForChat(ContractInfo contract)
        {
            CosmosDB cosmosDB = new CosmosDB();
            //cosmosDB.Connect();
            cosmosDB.CreateNewCollectionAsync(contract);
        }
        /// <summary>
        /// Creates a contract in database and writes the id of this contract to all participants
        /// </summary>
        /// <param name="contract">ContractInfo object</param>
        /// <param name="mistakeMsg">mistake message, returns "Success" if everything is alright</param>
        /// <returns>
        /// True if creation of a contract was successful, false otherwise
        /// </returns>
        public static void CreateRecord(ContractInfo contract)
        {
            SqlConnection sqlConnection = Connect.Do(Constants.connectionStr);
            try
            {
                //open connection and select next id
                sqlConnection.Open();
                SqlCommand sqlCommand = new SqlCommand
                {
                    CommandText = "SELECT TOP 1 * FROM contracts ORDER BY id DESC",
                    Connection = sqlConnection
                };
                SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
                int id;
                if (!sqlDataReader.HasRows)
                {
                    id = 0;
                }
                else
                {
                    sqlDataReader.Read();
                    id = (int)sqlDataReader.GetValue(0);
                    ++id;
                }
                sqlDataReader.Close();
                //transfer variables into the apropriate format
                string cText = JsonConvert.SerializeObject(contract.ContractText);
                //transfer integer array of all participants to string representation
                string participantsId = TransferArrays.IntToString(contract.ParticipantsId);
                //transfer integer array of unsigned participants to string representation
                string unsignedP = TransferArrays.IntToString(contract.UnsignedP);
                //transfer integer array of approved participants to string representation
                string approvedP = TransferArrays.IntToString(contract.ApprovedP);
                //transfer integer array of disapproved participants to string representation
                string disapprovedP = TransferArrays.IntToString(contract.DisapprovedP);

                sqlCommand.CommandText = @"INSERT INTO contracts VALUES(@id, @cText, @photos, @authorId, 
                    @participants, @unsignedP, @approvedP, @disapprovedP, @name, @creationDate, @status)";

                sqlCommand.Parameters.AddWithValue("@id", id);
                sqlCommand.Parameters.AddWithValue("@cText", cText);
                sqlCommand.Parameters.AddWithValue("@photos", contract.Photos);
                sqlCommand.Parameters.AddWithValue("@authorId", contract.AuthorId);
                sqlCommand.Parameters.AddWithValue("@participants", participantsId);
                sqlCommand.Parameters.AddWithValue("@unsignedP", unsignedP);
                sqlCommand.Parameters.AddWithValue("@approvedP", approvedP);
                sqlCommand.Parameters.AddWithValue("@disapprovedP", disapprovedP);
                sqlCommand.Parameters.AddWithValue("@name", contract.Name);
                sqlCommand.Parameters.AddWithValue("@creationDate", contract.CreationDate);
                sqlCommand.Parameters.AddWithValue("@status", contract.Status);

                sqlCommand.ExecuteNonQuery();
                //add this contract to all participants contract's list
                for (int i = 0; i < contract.ParticipantsId.Length; i++)
                {
                    sqlCommand.CommandText = $"SELECT * FROM users WHERE id={contract.ParticipantsId[i]}";
                    sqlDataReader = sqlCommand.ExecuteReader();
                    sqlDataReader.Read();
                    string contractsId = (string)sqlDataReader.GetValue(8);
                    if (contractsId.Length == 0)
                    {
                        contractsId = id.ToString();
                    }
                    else
                    {
                        contractsId += Constants.Devider + id.ToString();
                    }
                    sqlDataReader.Close();
                    sqlCommand.CommandText = $"UPDATE users SET sharedContracts='{contractsId}'" +
                        $" WHERE id={contract.ParticipantsId[i]}";
                    sqlCommand.ExecuteNonQuery();
                }
                //add this contract to the usersContract of the author of the contract
                sqlCommand.CommandText = $"SELECT * FROM users WHERE id={contract.AuthorId}";
                sqlDataReader = sqlCommand.ExecuteReader();
                sqlDataReader.Read();
                string userContracts = (string)sqlDataReader.GetValue(7);
                if (userContracts.Length == 0)
                {
                    userContracts = id.ToString();
                }
                else
                {
                    userContracts += Constants.Devider + id.ToString();
                }
                sqlDataReader.Close();
                sqlCommand.CommandText = $"UPDATE users SET contracts='{userContracts}'" +
                    $" WHERE id={contract.AuthorId}";
                sqlCommand.ExecuteNonQuery();
            }
            finally
            {
                sqlConnection.Close();
            }
        }
        public static List<ContractInfo> GetUserContracts(int userId, bool status)
        {
            SqlConnection sqlConnection = Connect.Do(Constants.connectionStr);
            try
            {
                sqlConnection.Open();
                SqlCommand sqlCommand = new SqlCommand
                {
                    CommandText = $"SELECT * FROM users WHERE id like {userId}",
                    Connection = sqlConnection
                };
                SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
                if (sqlDataReader.HasRows)
                {
                    sqlDataReader.Read();
                    int[] contractsId = TransferArrays.StringToInt((string)sqlDataReader.GetValue(8));
                    sqlDataReader.Close();
                    List<ContractInfo> userContracts = new List<ContractInfo>();
                    for (int i = 0; i < contractsId.Length; i++)
                    {
                        sqlCommand.CommandText = $"SELECT * FROM contracts WHERE id like '{contractsId[i]}' AND CONTRACTSTATUS like '{(status ? 1 : 0)}'";
                        sqlDataReader = sqlCommand.ExecuteReader();
                        sqlDataReader.Read();
                        ContractText contractText = JsonConvert.DeserializeObject<ContractText>(
                            (string)sqlDataReader.GetValue(1));
                        int[] participantsId = TransferArrays.StringToInt((string)sqlDataReader.GetValue(4));
                        int[] unsignedP = TransferArrays.StringToInt((string)sqlDataReader.GetValue(5));
                        int[] approvedP = TransferArrays.StringToInt((string)sqlDataReader.GetValue(6));
                        int[] disapprovedP = TransferArrays.StringToInt((string)sqlDataReader.GetValue(7));
                        userContracts.Add(new ContractInfo((int)sqlDataReader.GetValue(0), contractText,
                           (byte[])sqlDataReader.GetValue(2), (int)sqlDataReader.GetValue(3), participantsId,
                           unsignedP, approvedP, disapprovedP, (string)sqlDataReader.GetValue(8),
                           (DateTime)sqlDataReader.GetValue(9), (bool)sqlDataReader.GetValue(10)));
                        sqlDataReader.Close();
                    }
                    return userContracts;
                }
                else
                {
                    return new List<ContractInfo>();
                }
            }
            finally
            {
                sqlConnection.Close();
            }
        }

        private static void UpdateContractText(EditQuery editQuery, ContractInfo contract,
            SqlConnection connection)
        {
            SqlCommand updateContractRecordCommand = new SqlCommand
            {
                Connection = connection,
                CommandText = @"UPDATE contracts SET cText = @contractText WHERE
                    id like @contractId"
            };

            updateContractRecordCommand.Parameters.AddWithValue("@contractText",
                JsonConvert.SerializeObject(editQuery.ContractText));
            updateContractRecordCommand.Parameters.AddWithValue("@contractId",
                contract.Id);

            updateContractRecordCommand.ExecuteNonQuery();
        }
    }
}
