using System;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace ServerLib
{
    public class Login
    {
        public static UserInfo GetUserById(int id, out string mistakeMsg)
        {
            SqlConnection connection = Connect.Do(Constants.connectionStr);
            mistakeMsg = string.Empty;
            try
            { 
                connection.Open();

                SqlCommand getUserByIdCommand = new SqlCommand
                {
                    Connection = connection,
                    CommandText = @"SELECT * FROM users WHERE id like @id"
                };
                getUserByIdCommand.Parameters.AddWithValue("@id", id);

                SqlDataReader dataReader = getUserByIdCommand.ExecuteReader();
                if (dataReader.HasRows)
                {
                    dataReader.Read();
                    return CreateUserObj(dataReader);
                }
                else
                {
                    mistakeMsg = "FATAL ERROR";
                    return new UserInfo();
                }
            }
            catch (SqlException ex)
            {
                mistakeMsg = ex.Message;
                return new UserInfo();
            }
            catch (Exception ex)
            {
                mistakeMsg = ex.Message;
                return new UserInfo();
            }
            finally
            {
                connection.Close();
            }
        }
        /// <summary>
        /// Creates a UserInfo objecr from SqlDataReader
        /// </summary>
        /// <returns>
        /// UserInfo object
        /// </returns>
        public static UserInfo CreateUserObj(SqlDataReader reader)
        {
            //arrays for user's contracts and shared contracts
            int[] shareadContractsArr = new int[0];
            int[] contractsArr = new int[0];
            //transfer string representation of an integer array to an integer array
            string s = (string)reader.GetValue(7);
            if (s.Length > 0)
            {
                contractsArr = new int[s.Split(Constants.Devider).Length];
                for (int j = 0; j < contractsArr.Length; j++)
                {
                    contractsArr[j] = int.Parse(s.Split(Constants.Devider)[j]);
                }
            }
            //transfer string representation of an integer array to an integer array
            s = (string)reader.GetValue(8);
            if (s.Length > 0)
            {
                shareadContractsArr = new int[s.Split(Constants.Devider).Length];
                for (int j = 0; j < shareadContractsArr.Length; j++)
                {
                    shareadContractsArr[j] = int.Parse(s.Split(Constants.Devider)[j]);
                }
            }
            //user object
            return new UserInfo((int)reader.GetValue(0), (string)reader.GetValue(1),
                (string)reader.GetValue(2), (string)reader.GetValue(3), (string)reader.GetValue(4),
                (string)reader.GetValue(5), (string)reader.GetValue(6), contractsArr, shareadContractsArr,
                 (byte[])reader.GetValue(9), (byte[])reader.GetValue(10), (string)reader.GetValue(11), 
                 (string)reader.GetValue(12), (string)reader.GetValue(13));
        }
        /// <summary>
        /// Authorizes user with login and hos Face ID
        /// </summary>
        /// <param name="user">
        /// A UserInfo object which is generated if authorization was successfull
        /// </param>
        /// <param name="mistakeMsg">
        /// Error message
        /// </param>
        /// <returns>
        /// True if everything is OK, false otherwise
        /// </returns>
        public static bool AuthorizeUserWithFaceID(string login, byte[] userFaceId, out UserInfo user, out string mistakeMsg)
        {
            user = null;
            mistakeMsg = string.Empty;
            SqlConnection connection = Connect.Do(Constants.connectionStr);
            try
            {
                connection.Open();
                SqlCommand sqlCommand = new SqlCommand()
                {
                    CommandText = $"SELECT * FROM users WHERE login='{login}'",
                    Connection = connection
                };
                SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
                if (!sqlDataReader.HasRows)
                {
                    mistakeMsg = "Неправильный логин или Face ID";
                    return false;
                }
                else
                {
                    while (sqlDataReader.Read())
                    {
                        if (CompareFaceID.Compare((byte[])sqlDataReader.GetValue(9), userFaceId))
                        {
                            user = CreateUserObj(sqlDataReader);
                            mistakeMsg = "Success";
                            return true;
                        }
                    }
                    mistakeMsg = "Неправильный логин или Face ID";
                    return false;
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
        /// <summary>
        /// Метод который проводит авторизацию пользователя по логину или паролю
        /// </summary>
        /// <param name="login">Логин </param>
        /// <param name="password">Пароль</param>
        /// <param name="user">
        /// При удачной авторизации создается объект UserInfo в котором хранится вся информация о пользователе
        /// При неудачной попытке возвращается null значение
        /// </param>
        /// <param name="mistake">
        /// Сообщение об ошибке при ошибке аторизации/коннекта к бд
        /// При удачной авторизации возвращается строка "Success"
        /// </param>
        /// <returns>
        /// True при успешной авторизации, False при провале авторизации
        /// </returns>
        public static bool AuthorizeUser(string login, string password, out UserInfo user, out string mistake)
        {
            user = null;
            mistake = string.Empty;
            SqlConnection connection = Connect.Do(Constants.connectionStr);
            try
            {
                //Открытие коннекта и инициализация команды для поиска пользователя по логину
                connection.Open();
                SqlCommand command = new SqlCommand($"SELECT * FROM users WHERE login='{login}'", connection);
                SqlDataReader reader = command.ExecuteReader();
                if (!reader.HasRows)
                {
                    mistake = "Неправильный логин или пароль";
                    connection.Close();
                    return false;
                }
                else
                {
                    while (reader.Read())
                    {
                        if ((string)reader.GetValue(2) == password)
                        {
                            user = CreateUserObj(reader);
                            mistake = "Success";
                            return true;
                        }
                    }
                    mistake = "Неправильный логин или пароль";
                    return false;
                }
            }
            catch (Exception e)
            {
                mistake = e.Message;
                return false;
            }
            finally
            {
                connection.Close();
            }
        }
    }
}
