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
	public partial class ApprovedContractsPage : ContentPage
	{
        private UserInfo User { get; set; }

		public ApprovedContractsPage(UserInfo user)
		{
			InitializeComponent();
            User = user;
		}

        private void UpdateApprovedContractsList(object sender, EventArgs e)
        {

        }
        private void GoToContractViewPage(object sender, EventArgs e)
        {

        }
    }
}