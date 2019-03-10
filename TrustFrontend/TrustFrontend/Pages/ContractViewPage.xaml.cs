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
    public partial class ContractViewPage : MasterDetailPage
    {
        private ContractInfo CurrentContract { get; set; }
        private UserInfo CurrentUser { get; set; }

        public ContractViewPage(ContractInfo contract, UserInfo user)
        {
            InitializeComponent();
            CurrentContract = contract;
            CurrentUser = user;
            ContractDataModel contractDataModel = CreateContractDataModel(CurrentContract);
            BindingContext = contractDataModel;
            MasterPage.ListView.ItemTapped += ListViewItemTapped;
        }

        private ContractDataModel CreateContractDataModel(ContractInfo contract)
        {
            ContractDataModel contractDataModel = new ContractDataModel();
            contractDataModel.ContractGeneralData.Add(new DataCell
            {
                PropertyName = "Имя контракта",
                PropertyValue = contract.Name
            });
            contractDataModel.ContractGeneralData.Add(new DataCell
            {
                PropertyName = "Дата создания контракта",
                PropertyValue = contract.CreationDate.ToLongDateString()
            });
            contractDataModel.ContractGeneralData.Add(new DataCell
            {
                PropertyName = "Число участнков",
                PropertyValue = contract.ParticipantsId.Length.ToString()
            });
            contractDataModel.ContractGeneralData.Add(new DataCell
            {
                PropertyName = "Статус контракта",
                PropertyValue = (contract.ParticipantsId.Length == contract.ApprovedP.Length) ?
                    "Одобрен" : (contract.DisapprovedP.Length > 0) ? "Отклонен" : "На согласовании"
            });

            for (int i = 0; i < contract.ContractText.UserParts.Length; i++)
            {
                contractDataModel.ContractText.Add(new ContractTextModel
                {
                    UserName = contract.ContractText.UserParts[i].UserName,
                    Rights = contract.ContractText.UserParts[i].Rights,
                    Obligations = contract.ContractText.UserParts[i].Obligations
                });
            }
            return contractDataModel;
        }

        private void ListViewItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (!(e.Item is MenuItem item))
                return;

            var page = (Page)Activator.CreateInstance(item.TargetType, CurrentContract, CurrentUser);
            Detail = new NavigationPage(page);
            IsPresented = false;
            MasterPage.ListView.SelectedItem = null;
        }
    }
}