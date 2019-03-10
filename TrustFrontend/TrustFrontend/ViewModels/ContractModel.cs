using System;
using System.Collections.Generic;
using System.Text;
using ServerLib;

namespace TrustFrontend
{
    public class ContractModel
    {
        public int ID { get; set; }
        public string ContractName { get; set; }
        public string CreationDate { get; set; }
        public byte[] AuthorPhoto { get; set; }
        public string AuthorName { get; set; }

        public ContractModel(ContractInfo contract)
        {
            ID = contract.Id;
            ContractName = contract.Name;
            CreationDate = contract.CreationDate.ToLongDateString();
            AuthorPhoto = contract.Photos;
            AuthorName = contract.AuthorId.ToString();
        }
    }
}
