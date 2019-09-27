using System;
using System.Collections.Generic;
using System.Text;

namespace TrustFrontend
{
    public class CreateNewContractUserData
    {
        public string HeadLabelText { get; set; } 
        public string UserNameEditorText { get; set; }
        public bool IsEnabled { get; set; }
        public string DeleteButtonId { get; set; }
        public bool IsDeleteButtonVisible { get; set; }
    }
}
