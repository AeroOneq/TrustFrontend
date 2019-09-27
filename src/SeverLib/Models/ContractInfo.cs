using System;
using System.Collections.Generic;
using System.Text;

namespace ServerLib
{
    public class ContractInfo
    {
        public int Id { get; set; }
        public ContractText ContractText { get; set; }
        public byte[] Photos { get; set; }
        public int AuthorId { get; set; }
        public int[] ParticipantsId { get; set; }
        public int[] UnsignedP { get; set; }
        public int[] DisapprovedP { get; set; }
        public int[] ApprovedP { get; set; }
        public string Name { get; set; }
        public DateTime CreationDate { get; set; }
        public bool Status { get; set; }

        public ContractInfo() { }
        public ContractInfo(int id, ContractText contractText, byte[] photos, int authorId, int[] participantsId,
            int[] unsignedP, int[] disapprovedP, int[] approvedP, string name, DateTime creationDate, bool status)
        {
            Id = id;
            ContractText = contractText;
            Photos = photos;
            AuthorId = authorId;
            ParticipantsId = participantsId;
            UnsignedP = unsignedP;
            DisapprovedP = disapprovedP;
            ApprovedP = approvedP;
            Name = name;
            CreationDate = creationDate;
            Status = status;
        }
    }
}
