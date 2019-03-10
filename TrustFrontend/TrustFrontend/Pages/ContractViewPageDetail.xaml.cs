using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServerLib;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TrustFrontend
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ContractViewPageDetail : ContentPage
    {
        public ListView ContractListView { get; set; }

        public ContractViewPageDetail()
        {
            InitializeComponent();
        }

        public ContractViewPageDetail(ContractInfo contract, UserInfo user)
        {
            ContractListView = contractInfoListView;
            InitializeComponent();
        }
    }
}