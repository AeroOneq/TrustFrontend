using System;
using System.Data.SqlClient;
using System.IO;
using Newtonsoft.Json;

namespace ServerLib
{
    public static class Update
    {
        /// <summary>
        /// UPdates the record of a user with a new parameters which are set
        /// in the newUser recrod
        /// </summary>
        /// <returns>
        /// Error message, if there was an exception
        /// </returns>
        public static string UpdateUserRecord(UserInfo user, UserInfo newUser)
        {
            SqlConnection connection = Connect.Do(Constants.connectionStr);
            try
            {
                connection.Open();
                SqlCommand sqlCommand = new SqlCommand
                {
                    CommandText = @"UPDATE users SET login = @login, password = @password,
                        faceID = @faceID, name = @name, surname = @surname, fname = @fname WHERE id like @id",
                    Connection = connection
                };

                sqlCommand.Parameters.AddWithValue("@login", newUser.Login);
                sqlCommand.Parameters.AddWithValue("@password", newUser.Password);
                sqlCommand.Parameters.AddWithValue("@faceID", newUser.FaceID);
                sqlCommand.Parameters.AddWithValue("@name", newUser.Name);
                sqlCommand.Parameters.AddWithValue("@surname", newUser.Surname);
                sqlCommand.Parameters.AddWithValue("@fname", newUser.FName);
                sqlCommand.Parameters.AddWithValue("@id", user.Id);

                sqlCommand.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                return ex.Message;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            finally
            {
                connection.Close();
            }
            return string.Empty;
        }
        /// <summary>
        /// Updates the edit query record by changing the disapprovedP, approvedP and closed
        /// colls of the record.
        /// </summary>
        /// <returns>Error message if the exception occured</returns>
        public static string UpdateQueryStatus(EditQuery editQuery, ContractInfo contract)
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
                    UpdateContractRecord(editQuery, contract, connection);

                updateQueryStatusCommand.ExecuteNonQuery();
                return string.Empty;
            }
            catch (SqlException ex)
            {
                return ex.Message;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            finally
            {
                connection.Close();
            }
        }
        public static string UpdateContractRecord(ContractInfo contract)
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

                return string.Empty;
            }
            catch (SqlException ex)
            {
                return ex.Message;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            finally
            {
                connection.Close();
            }
        }

        private static void UpdateContractRecord(EditQuery editQuery, ContractInfo contract,
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