using System;
using System.Data.SqlClient;
using System.IO;

namespace ServerLib
{
    public class CreateUser
    {
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
            catch (Exception e)
            {
                return e.Message;
            }
            finally
            {
                connection.Close();
            }
        }
    }
}
