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
    public partial class SharedContractsPage : ContentPage
    {
        public List<ContractInfo> Contracts { get; set; } = new List<ContractInfo>();
        public ObservableCollection<ContractModel> ContractsData { get; set; }
            = new ObservableCollection<ContractModel>();

        public UserInfo CurrentUser { get; set; }


        public SharedContractsPage(UserInfo user)
        {
            InitializeComponent();

            CurrentUser = user;
            UpdateSharedContractsList(sharedContractsListView, null);
        }

        private async void UpdateSharedContractsList(object sender, EventArgs e)
        {
            try
            {
                sharedContractsListView.IsRefreshing = true;

                Contracts = await ContractService.GetAllUserContracts(CurrentUser.Id, false);
                ContractsData = new ObservableCollection<ContractModel>();
                foreach (ContractInfo contractInfo in Contracts)
                    ContractsData.Add(new ContractModel(contractInfo));
                sharedContractsListView.ItemsSource = ContractsData;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Ошибка", ex.Message, "OK");
            }
            finally
            {
                sharedContractsListView.IsRefreshing = false;
            }
        }

        private async void GoToContractViewPage(object sender, ItemTappedEventArgs e)
        {
            ContractModel contractModel = e.Item as ContractModel;
            ContractInfo tappedContract = Contracts.Find(c => c.Id == contractModel.ID);
            await Navigation.PushModalAsync(new ContractViewPage(tappedContract, CurrentUser));
        }

        private async void GoToCreateNewContractPage(object sender, EventArgs e) =>
            await Navigation.PushAsync(new CreateNewContractPage(CurrentUser));

        private async void SearchForContracts(object sender, TextChangedEventArgs e)
        {
            await Task.Run(() =>
            {
                string searchText = searchEntry.Text;

                if (string.IsNullOrEmpty(searchText))
                {
                    Device.BeginInvokeOnMainThread(() => sharedContractsListView.ItemsSource =
                        ContractsData);
                    return;
                }

                ObservableCollection<ContractModel> contractModels = new ObservableCollection<ContractModel>();
                for (int i = 0; i < ContractsData.Count; i++)
                {
                    if (ContractsData[i].ContractName.IndexOf(searchText) > -1)
                        contractModels.Add(ContractsData[i]);
                }

                Device.BeginInvokeOnMainThread(() => sharedContractsListView.ItemsSource =
                    contractModels);
            });
        }
    }
}