using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        private UserInfo CurrentUser { get; set; }
        public List<ContractInfo> Contracts { get; set; } = new List<ContractInfo>();
        public ObservableCollection<ContractModel> ContractsData { get; set; }
            = new ObservableCollection<ContractModel>();

        public ApprovedContractsPage(UserInfo user)
		{
			InitializeComponent();
            CurrentUser = user;

            UpdateApprovedContractsList(null, null);
		}

        private async void UpdateApprovedContractsList(object sender, EventArgs e)
        {
            try
            {
                approvedContractsListView.IsRefreshing = true;

                Contracts = await ContractService.GetAllUserContracts(CurrentUser.Id, true);
                ContractsData = new ObservableCollection<ContractModel>();
                foreach (ContractInfo contractInfo in Contracts)
                    ContractsData.Add(new ContractModel(contractInfo));
                approvedContractsListView.ItemsSource = ContractsData;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Ошибка", ex.Message, "OK");
            }
            finally
            {
                approvedContractsListView.IsRefreshing = false;
            }
        }
        private async void GoToContractViewPage(object sender, ItemTappedEventArgs e)
        {
            ContractModel contractModel = e.Item as ContractModel;
            ContractInfo tappedContract = Contracts.Find(c => c.Id == contractModel.ID);
            await Navigation.PushModalAsync(new ContractViewPage(tappedContract, CurrentUser));
        }
    }
}