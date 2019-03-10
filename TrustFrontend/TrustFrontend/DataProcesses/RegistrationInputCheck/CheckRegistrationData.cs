using ServerLib;
using System.Threading.Tasks;

namespace TrustFrontend
{
    public static class CheckRegistrationData
    {
        #region Constants
        const int size = 9;
        const string eAlph = "qwertyuiopasdfghjklzxcvbnmQWERTYUIOPASDFGHJKLZXCVBNM";
        const string rAlph = "йцукенгшщзхъфывапролджэячсмитьбюЙЦУКЕНГШЩЗХЪФЫВАПРОЛДЖЭЯЧСМИТЬБЮ";
        const string numsStr = "1234567890";
        #endregion
        /// <summary>
        /// Method which checks every parametr of user's input from the first registration page
        /// </summary>
        /// <param name="data">
        /// user's input: 0 = login, 1 = pass, 2 = name, 3 = surname, 4 = family name, 5 = email,
        /// 6 = code word, 7 = passport series, 8 = passport number
        /// </param>
        /// <returns>
        /// Boolean array, for each element:  true - input is OK, false - input is incorrect
        /// </returns>
        public static async Task<RegistrationDataCheckResult> Check(string[] data, byte[] faceID,
            bool shouldCheckLogin)
        {
            return await Task.Run(async () =>
            {
                string nameBlockError = CheckName(data[2]) + CheckSurname(data[3])
                    + CheckFamilyName(data[4]) + CheckFaceID(faceID);
                string generalBlockError = (shouldCheckLogin) ? await CheckLogin(data[0]) : string.Empty
                    + CheckPassword(data[1]) + CheckEmail(data[5]);
                string confidentialBlockError = CheckPassportSeries(data[7]) +
                    CheckPassportNumber(data[8]) + CheckCodeWord(data[6]);
                bool checkResult = (string.Empty == nameBlockError &&
                    string.Empty == generalBlockError &&
                    string.Empty == confidentialBlockError);

                RegistrationDataCheckResult regCheckResult = new RegistrationDataCheckResult
                {
                    Result = checkResult,
                    NameBlockError = nameBlockError,
                    ConfidentialBlockError = confidentialBlockError,
                    GeneralBlockError = generalBlockError
                };

                return regCheckResult;
            });
        }
        #region Check data utility methods
        public static string CheckFaceID(byte[] faceID)
        {
            if (faceID.Length > 0 && FindFace.Find(faceID) !=
                "Сделайте фото только с Вашим лицом")
                return string.Empty;
            else
                return "Сделайте фото со своим лицом, как на паспорте";
        }
        public static string CheckName(string name)
        {
            if (name == null || name.Length == 0)
                return "Name's length must be greater then 0. ";
            else
                for (int i = 0; i < name.Length; i++)
                    if (rAlph.IndexOf(name[i]) > -1 || numsStr.IndexOf(name[i]) > -1)
                        return "Name must contain only English letters and must not contain numbers. ";
            //if (!FindNameInCSV.Find(name))
            //  return "This name is unregistered in the database. ";
            return string.Empty;
        }
        public static string CheckSurname(string surname)
        {
            if (surname == null || surname.Length == 0)
                return "Email's length must be greater than 0. ";
            else
                for (int i = 0; i < surname.Length; i++)
                    if (rAlph.IndexOf(surname[i]) > -1 || numsStr.IndexOf(surname[i]) > -1)
                        return "Surname must contain only English letters " +
                            "and must not contain numbers. ";
            // if (!FindSurnameInCSV.Find(surname))
            //   return "The given surname is unreal. ";
            return string.Empty;
        }
        public static string CheckFamilyName(string familyName)
        {
            if (familyName == null || familyName.Length == 0)
                return "Family name must be greater than 0. ";
            else
                for (int i = 0; i < familyName.Length; i++)
                    if (rAlph.IndexOf(familyName[i]) > -1 || numsStr.IndexOf(familyName[i]) > -1)
                        return "Family name must contain only English letters " +
                            "and must not contain numbers. ";
            return string.Empty;
        }
        public async static Task<string> CheckLogin(string login)
        {
            if (login == null || login.Length == 0 ||
                !(await UserService.CheckLoginAvailability(login)))
                return "Login is empty or already taken. ";
            else
                for (int i = 0; i < login.Length; i++)
                    if (rAlph.IndexOf(login[i]) > -1)
                        return "Login is a word consisting from the English letters. ";
            return string.Empty;
        }
        public static string CheckPassword(string password)
        {
            if (password == null || password.Length < 6)
                return "Password length must be greater than 6. ";
            else
                for (int i = 0; i < password.Length; i++)
                    if (rAlph.IndexOf(password[i]) > -1)
                        return "Password must contain only English letters";
            return string.Empty;
        }
        public static string CheckEmail(string email)
        {
            if (email == null || email.Length == 0)
                return "Email's length must be greater than 0. ";
            else
                for (int i = 0; i < email.Length; i++)
                    if (rAlph.IndexOf(email[i]) > -1)
                        return "Email must not contain Russian letters. ";
            return string.Empty;
        }
        public static string CheckPassportSeries(string passportS)
        {
            if (passportS == null || passportS.Length != 4)
                return "Passport series's length must be 4. ";
            else
                for (int i = 0; i < passportS.Length; i++)
                    if (numsStr.IndexOf(passportS[i]) < 0)
                        return "Passport series must contain only numbers. ";
            return string.Empty;
        }
        public static string CheckPassportNumber(string passportN)
        {
            if (passportN == null || passportN.Length != 6)
                return "Passport number's length must be 6. ";
            else
                for (int i = 0; i < passportN.Length; i++)
                    if (numsStr.IndexOf(passportN[i]) < 0)
                        return "Passport number must contain only numbers. ";
            return string.Empty;
        }
        public static string CheckCodeWord(string codeWord)
        {
            if (codeWord == null || codeWord.Length == 0)
                return "Code word's length must be greater than 0. ";
            else
                for (int i = 0; i < codeWord.Length; i++)
                    if (rAlph.IndexOf(codeWord[i]) > -1 || numsStr.IndexOf(codeWord[i]) > -1)
                        return "Code word must contain only English letters. ";
            return string.Empty;
        }
        #endregion
    }
}
