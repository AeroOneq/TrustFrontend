using System.Data.SqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace ServerLib
{
    public class CreateContract
    {
        public List<int> FindUserIDs(string[] userLogins)
        {
            SqlConnection connection = Connect.Do(Constants.connectionStr);
            connection.Open();
            SqlCommand findIDsCommand = new SqlCommand()
            {
                Connection = connection,
                CommandText = $"SELECT id FROM users WHERE "
            };

            for (int i = 0; i < userLogins.Length; i++)
            {
                findIDsCommand.CommandText += $"login like '{userLogins[i]}' OR ";
            }
            findIDsCommand.CommandText = findIDsCommand.CommandText.Remove(
                findIDsCommand.CommandText.Length - 5);

            SqlDataReader reader = findIDsCommand.ExecuteReader();

            List<int> userIDsList = new List<int>();
            while (reader.Read())
            {
                userIDsList.Add((int)reader.GetValue(1));
            }

            if (userIDsList.Count != userLogins.Length)
                throw new ArgumentException("An erros has occured");

            return userIDsList;
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
        public static bool Create(ContractInfo contract, out string mistakeMsg)
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
                    @participants, @unsignedP, @approvedP, @disapprovedP, @name, @creationDate)";

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
                sqlCommand.CommandText = $"UPDATE users SET userContracts='{userContracts}'" +
                    $" WHERE id={contract.AuthorId}";
                sqlCommand.ExecuteNonQuery();
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
                sqlConnection.Close();
            }
        }
    }
}
