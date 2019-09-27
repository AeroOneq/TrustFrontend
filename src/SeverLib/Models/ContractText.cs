using System;
using System.Collections.Generic;
using System.Text;

namespace ServerLib
{
    //contract text - main part of the contract
    public class ContractText
    {
        public UserPart[] UserParts { get; set; }

        public ContractText(UserPart[] userParts)
        {
            UserParts = userParts;
        }
    }
}
