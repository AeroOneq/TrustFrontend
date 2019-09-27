using System;
using System.Collections.Generic;
using System.Text;

namespace ServerLib
{
    /// <summary>
    /// Class represents the result of authorization 
    /// </summary>
    public class AuthResult
    {
        public UserInfo User { get; set; }
        public string MistakeMsg { get; set; }

        public AuthResult(UserInfo user, string mistakeMsg)
        {
            User = user;
            MistakeMsg = mistakeMsg;
        }
    }
}
