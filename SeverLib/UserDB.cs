using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace ServerLib
{
    public class UserDB
    {
        /// <summary>
        /// UPdates the record of a user with a new parameters which are set
        /// in the newUser recrod
        /// </summary>
        /// <returns>
        /// Error message, if there was an exception
        /// </returns>
        public static void UpdateUserRecord(UserInfo user, UserInfo newUser)
        {
            SqlConnection connection = Connect.Do(Constants.connectionStr);
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

        public static UserInfo GetUserById(int id)
        {
            SqlConnection connection = Connect.Do(Constants.connectionStr);
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
                    return new UserInfo();
                }
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
        public static UserInfo AuthorizeUserWithFaceID(string login, byte[] userFaceId)
        {
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
                    throw new ArgumentException("Incorrect login or password");
                }
                else
                {
                    while (sqlDataReader.Read())
                    {
                        if (CompareFaceID.Compare((byte[])sqlDataReader.GetValue(9), userFaceId))
                        {
                            return CreateUserObj(sqlDataReader);
                        }
                    }
                    throw new ArgumentException("Incorrect login or password");
                }
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
        /// <exception cref="ArgumentException">When input data is incorrect or the user 
        /// was not found</exception>
        /// <returns>
        /// True при успешной авторизации, False при провале авторизации
        /// </returns>
        public static UserInfo AuthorizeUser(string login, string password)
        {
            SqlConnection connection = Connect.Do(Constants.connectionStr);
            try
            {
                //Открытие коннекта и инициализация команды для поиска пользователя по логину
                connection.Open();
                SqlCommand command = new SqlCommand($"SELECT * FROM users WHERE login='{login}'", connection);
                SqlDataReader reader = command.ExecuteReader();
                if (!reader.HasRows)
                {
                    throw new ArgumentException("Incorrect login or password");
                }
                else
                {
                    while (reader.Read())
                    {
                        if ((string)reader.GetValue(2) == password)
                        {
                            return CreateUserObj(reader);
                        }
                    }
                    throw new ArgumentException("Incorrect login or password");
                }
            }
            finally
            {
                connection.Close();
            }
        }

        public static void CreateNewUser(UserInfo user)
        {
            Create(0, user.Login, user.Password, user.Name, user.Surname, user.FName, user.Email,
                string.Empty, string.Empty, user.FaceID, user.Fingerprint, user.CodeWord, user.PassportS,
                user.PassportN);
        }
        /// <summary>
        /// Создает новую учетную запись
        /// </summary>
        /// <param name="fName">Отчество</param>
        /// <param name="contracts">Всегда пустая строка для контрактов</param>
        /// <param name="sharedContracts">Всегда пустая строка для shared контрактов</param>
        /// <param name="faceID">Массив байтов с картинкой</param>
        /// <param name="fingerPrint">Массив байтов с отпечатком пальца</param>
        /// <param name="codeWord">Кодовое слово для потдтверждения действий в приложении</param>
        /// <param name="passportS">Серия паспорта</param>
        /// <param name="passportN">Номер паспорта</param>
        /// <returns>
        /// Строку "Пользователь успешно зарегестрирован!" при успешной регистрации,
        /// Сообщение об ошибке при ошибке в регистрации
        /// </returns>
        private static string Create(int id, string login, string password, string name, string surname, string fName, string email, string contracts,
            string sharedContracts, byte[] faceID, byte[] fingerPrint, string codeWord, string passportS, string passportN)
        {
            SqlConnection connection = Connect.Do(Constants.connectionStr);
            try
            {
                connection.Open();
                //find the last id
                SqlCommand command = new SqlCommand
                {
                    Connection = connection,
                    CommandText = @"SELECT top 1 * FROM users ORDER BY id DESC"
                };
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    reader.Read();
                    id = (int)reader.GetValue(0);
                    ++id;
                }
                else
                {
                    id = 0;
                }
                reader.Close();
                //add user
                command = new SqlCommand
                {
                    Connection = connection,
                    CommandText = @"INSERT INTO users VALUES(@id, @login, @password, @name, @surname, @fName, @email, @contracts,
                    @sharedContracts, @faceID, @fingerprint, @codeWord, @passportS, @passportN)"
                };
                //initializing parameters
                command.Parameters.AddWithValue("@id", id);
                command.Parameters.AddWithValue("@login", login);
                command.Parameters.AddWithValue("@password", password);
                command.Parameters.AddWithValue("@name", name);
                command.Parameters.AddWithValue("@surname", surname);
                command.Parameters.AddWithValue("@fName", fName);
                command.Parameters.AddWithValue("@email", email);
                command.Parameters.AddWithValue("@contracts", contracts);
                command.Parameters.AddWithValue("@sharedContracts", sharedContracts);
                command.Parameters.AddWithValue("@faceID", faceID);
                command.Parameters.AddWithValue("@fingerPrint", fingerPrint);
                command.Parameters.AddWithValue("@codeWord", codeWord);
                command.Parameters.AddWithValue("@passportS", string.Empty);
                command.Parameters.AddWithValue("@passportN", string.Empty);
                //execute query
                command.ExecuteNonQuery();
                return "Пользователь успешно зарегестрирован!";
            }
            finally
            {
                connection.Close();
            }
        }

        public static bool CheckLoginAvailability(string login)
        {
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
                return (dataReader.HasRows) ? false : true;
            }
            finally
            {
                connection.Close();
            }
        }

        /// <summary>
        /// Gets the list of IDs which are the ID's of users with given logins
        /// </summary>
        public static List<int> GetUserIDs(string[] userLogins)
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
                findIDsCommand.CommandText.Length - 4);

            SqlDataReader reader = findIDsCommand.ExecuteReader();

            List<int> userIDsList = new List<int>();
            while (reader.Read())
            {
                userIDsList.Add((int)reader.GetValue(0));
            }

            if (userIDsList.Count != userLogins.Length)
                throw new ArgumentException("An erros has occured");

            return userIDsList;
        }
    }
}
