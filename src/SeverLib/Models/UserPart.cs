using System;
using System.Collections.Generic;
using System.Text;

namespace ServerLib
{
    public class UserPart
    {
        public string UserName { get; set; }
        public string Rights { get; set; }
        public string Obligations { get; set; }

        public UserPart(string userName, string rights, string obligations)
        {
            UserName = userName;
            Rights = rights;
            Obligations = obligations;
        }
    }
}
