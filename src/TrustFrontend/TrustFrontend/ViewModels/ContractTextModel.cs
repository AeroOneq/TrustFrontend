using System;
using System.Collections.Generic;
using System.Text;
using ServerLib;

namespace TrustFrontend
{
    public class ContractTextModel
    {
        public string UserName { get; set; }
        public string Rights { get; set; }
        public string Obligations { get; set; }

        public ContractTextModel() { }
        public ContractTextModel(UserPart userPart)
        {
            UserName = userPart.UserName;
            Rights = userPart.Rights;
            Obligations = userPart.Obligations;
        }
    }
}
