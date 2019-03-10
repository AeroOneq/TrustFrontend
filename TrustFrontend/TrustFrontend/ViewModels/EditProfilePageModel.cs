using System;
using System.Collections.Generic;
using System.Text;
using ServerLib;

namespace TrustFrontend
{
    public class EditProfilePageModel
    {
        public string Name { get; set; } 
        public string Surname { get; set; } 
        public string FName { get; set; } 

        public string Login { get; set; } 
        public string Password { get; set; }

        public byte[] FaceID { get; set; }

        public EditProfilePageModel(UserInfo user)
        {
            Name = user.Name;
            Surname = user.Surname;
            FName = user.FName;
            Login = user.Login;
            Password = user.Password;
            FaceID = user.FaceID;
        }
    }
}
