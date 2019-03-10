using Newtonsoft.Json;
using System.Data.SqlClient;
using System;

namespace ServerLib
{
    public class GetAllUserContracts
    {
        public static ContractInfo[] Get(int userId, out string mistakeMsg)
        {
            /*SqlConnection sqlConnection = Connect.Do(Constants.connectionStr);
            try
            {
                sqlConnection.Open();
                SqlCommand sqlCommand = new SqlCommand
                {
                    CommandText = $"SELECT * FROM users WHERE id={userId}",
                    Connection = sqlConnection
                };
                SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
                if (sqlDataReader.HasRows)
                {
                    sqlDataReader.Read();
                    int[] contractsId = TransferArrays.StringToInt((string)sqlDataReader.GetValue(8));
                    sqlDataReader.Close();
                    ContractInfo[] contracts = new ContractInfo[contractsId.Length];
                    for (int i = 0; i < contractsId.Length; i++)
                    {
                        sqlCommand.CommandText = $"SELECT * FROM contracts WHERE id={contractsId[i]}";
                        sqlDataReader = sqlCommand.ExecuteReader();
                        sqlDataReader.Read();
                        ContractText contractText = JsonConvert.DeserializeObject<ContractText>(
                            (string)sqlDataReader.GetValue(1));
                        int[] participantsId = TransferArrays.StringToInt((string)sqlDataReader.GetValue(4));
                        int[] unsignedP = TransferArrays.StringToInt((string)sqlDataReader.GetValue(5));
                        int[] approvedP = TransferArrays.StringToInt((string)sqlDataReader.GetValue(6));
                        int[] disapprovedP = TransferArrays.StringToInt((string)sqlDataReader.GetValue(7));
                        contracts[i] = new ContractInfo((int)sqlDataReader.GetValue(0), contractText,
                           (byte[])sqlDataReader.GetValue(2), (int)sqlDataReader.GetValue(3), participantsId,
                           unsignedP, approvedP, disapprovedP, (string)sqlDataReader.GetValue(8),
                           (DateTime)sqlDataReader.GetValue(9));
                        sqlDataReader.Close();
                    }
                    mistakeMsg = "Success";
                    return contracts;
                }
                else
                {
                    mistakeMsg = "Произошла фатальная ошибка";
                    return null;
                }
            }
            catch (Exception e)
            {
                mistakeMsg = e.Message;
                return null;
            }
            finally
            {
                sqlConnection.Close();
            }*/
            mistakeMsg = null;
            return null;
        }
    }
}
