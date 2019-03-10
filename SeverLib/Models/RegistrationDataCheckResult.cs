using System;
using System.Collections.Generic;
using System.Text;

namespace ServerLib
{
    public class RegistrationDataCheckResult
    {
        public bool Result { get; set; }
        public string NameBlockError { get; set; }
        public string GeneralBlockError { get; set; }
        public string ConfidentialBlockError { get; set; }
    }
}
