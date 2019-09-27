using System;
using System.Collections.Generic;
using System.Text;

namespace ServerLib
{
    //edit query class
    public class EditQuery
    {
        public int ContractId { get; set; }
        public ContractText ContractText { get; set; }
        public byte[] Photos { get; set; }
        public int AuthorId { get; set; }
        public int[] ApprovedP { get; set; }
        public int[] DisapprovedP { get; set; }
        public bool Closed { get; set; }
        public DateTime CreationDate { get; set; }
        public string QueryHeader { get; set; }
        public string AuthorName { get; set; }
        public int QueryId { get; set; }

        public EditQuery() { }
        public EditQuery(int contractId, ContractText contractText, byte[] photos,
            int authorId, int[] approvedP, int[] disapprovedP, bool closed,
            DateTime creationDate, string queryHeader, string authorName, int queryId)
        {
            ContractId = contractId;
            ContractText = contractText;
            Photos = photos;
            AuthorId = authorId;
            ApprovedP = approvedP;
            DisapprovedP = disapprovedP;
            Closed = closed;
            CreationDate = creationDate;
            QueryHeader = queryHeader;
            AuthorName = authorName;
            QueryId = queryId;
        }
    }
}
