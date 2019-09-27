using System;
namespace ServerLib
{
    /// <summary>
    /// Классы для хранения информации о логических объектах проеграммы
    /// </summary>
    public class UserInfo
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string FName { get; set; }
        public string Email { get; set; }
        public int[] Contracts { get; set; }
        public int[] SharedContracts { get; set; }
        public byte[] FaceID { get; set; }
        public byte[] Fingerprint { get; set; }
        public string CodeWord { get; set; }
        public string PassportS { get; set; }
        public string PassportN { get; set; }

        public UserInfo() { }
        public UserInfo(int id, string login, string password, string name, string surname, string fName, 
                        string email, int[] contracts, int[] sharedContracts, byte[] faceID, byte[] fingerprint,
                        string codeWord, string passportS, string passportN)
        {
            Id = id;
            Login = login;
            Password = password;
            Name = name;
            Surname = surname;
            FName = fName;
            Email = email;
            Contracts = contracts;
            SharedContracts = sharedContracts;
            CodeWord = codeWord;
            PassportS = passportS;
            PassportN = passportN;
            FaceID = faceID;
            Fingerprint = fingerprint;
        }
        public UserInfo(UserInfo userInfo) 
            :this(userInfo.Id, userInfo.Login, userInfo.Password, userInfo.Name, userInfo.Surname,
                 userInfo.FName, userInfo.Email, userInfo.Contracts, userInfo.SharedContracts,
                 userInfo.FaceID, userInfo.Fingerprint, userInfo.CodeWord, userInfo.PassportS,
                 userInfo.PassportN) { }
    }
}
