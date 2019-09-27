using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerLib
{
    public class NewUserData
    {
        public static string Login { get; set; }
        public static string Password { get; set; }
        public static string Name { get; set; }
        public static string Surname { get; set; }
        public static string FName { get; set; }
        public static string Email { get; set; }
        public static string CodeWord { get; set; }
        public static string PassportS { get; set; }
        public static string PassportN { get; set; }
        public static byte[] FaceID { get; set; }
        public static byte[] Fingerprint { get; set; }
    }
}
