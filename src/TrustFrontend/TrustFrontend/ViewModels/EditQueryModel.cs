using System;
using System.Collections.Generic;
using System.Text;
using ServerLib;

namespace TrustFrontend
{
    public class EditQueryModel
    {
        public int ID { get; set; }
        public string QueryName { get; set; }
        public string AuthorName { get; set; } 
        public string CreationDate { get; set; } 
        public byte[] AuthorPhoto { get; set; }
        public string Status { get; set; }

        public EditQueryModel(EditQuery editQuery)
        {
            ID = editQuery.QueryId;
            QueryName = editQuery.QueryHeader;
            AuthorName = editQuery.AuthorName;
            CreationDate = editQuery.CreationDate.ToLongDateString();
            Status = editQuery.Closed ? "Закрыто" : "На обсуждении";
        }
    }
}
