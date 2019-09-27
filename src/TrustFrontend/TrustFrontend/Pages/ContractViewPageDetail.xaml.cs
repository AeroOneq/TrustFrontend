using System;
using System.Collections.Generic;
using System.Linq;
using ServerLib;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TrustFrontend
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ContractViewPageDetail : ContentPage
    {
        public ListView ContractListView { get; set; }

        public ContractInfo CurrentContract { get; set; } 
        public UserInfo CurrentUser { get; set; } 

        public ContractViewPageDetail()
        {
            InitializeComponent();
            ContractListView = contractInfoListView;
        }

        public ContractViewPageDetail(ContractInfo contract, UserInfo user)
        {
            ContractListView = contractInfoListView;
            InitializeComponent();

            CurrentContract = contract;
            CurrentUser = user;
        }

        private async void ApproveContract(object sender, EventArgs e)
        {
            try
            {
                if (Array.FindIndex(CurrentContract.UnsignedP, p => p == CurrentUser.Id) == -1)
                {
                    throw new ArgumentException("Вы уже высказали свое мнение по поводу контракта");
                }
                else
                {
                    List<int> unsignedPList = CurrentContract.UnsignedP.ToList();
                    List<int> approvedPList = CurrentContract.ApprovedP.ToList();

                    unsignedPList.Remove(CurrentUser.Id);
                    approvedPList.Add(CurrentUser.Id);

                    CurrentContract.UnsignedP = unsignedPList.ToArray();
                    CurrentContract.ApprovedP = approvedPList.ToArray();

                    if (CurrentContract.ApprovedP.Length == CurrentContract.ParticipantsId.Length)
                        CurrentContract.Status = true;

                    await ContractService.UpdateRecord(CurrentContract);

                    await DisplayAlert("Статус", "Вы одобрили этот контракт", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Ошибка", ex.Message, "OK");
            }
        }

        private async void DisapproveContract(object sender, EventArgs e)
        {
            try
            {
                if (Array.FindIndex(CurrentContract.UnsignedP, p => p == CurrentUser.Id) == -1)
                {
                    throw new ArgumentException("Вы уже высказали свое мнение по поводу контракта");
                }
                else
                {
                    List<int> unsignedPList = CurrentContract.UnsignedP.ToList();
                    List<int> disapprovedPList = CurrentContract.DisapprovedP.ToList();

                    unsignedPList.Remove(CurrentUser.Id);
                    disapprovedPList.Add(CurrentUser.Id);

                    CurrentContract.UnsignedP = unsignedPList.ToArray();
                    CurrentContract.ApprovedP = disapprovedPList.ToArray();

                    if (CurrentContract.DisapprovedP.Length == CurrentContract.ParticipantsId.Length)
                        CurrentContract.Status = true;

                    await ContractService.UpdateRecord(CurrentContract);

                    await DisplayAlert("Статус", "Вы неодобрили этот контракт", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Ошибка", ex.Message, "OK");
            }
        }
    }
}