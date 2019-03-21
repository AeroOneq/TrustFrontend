using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ServerLib;

namespace TrustFrontend
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditQueryViewPage : ContentPage
    {
        public ObservableCollection<ContractTextModel> ContractText { get; set; } = 
            new ObservableCollection<ContractTextModel>();
        public string PageTitle { get; set; }

        private EditQuery EditQuery { get; set; }
        private ContractInfo Contract { get; set; }
        private UserInfo User { get; set; }

        public EditQueryViewPage(EditQuery editQuery, ContractInfo contract,
            UserInfo user)
        {
            InitializeComponent();

            User = user;
            Contract = contract;
            EditQuery = editQuery;
            SetBindingContext(editQuery);
        }

        private void SetBindingContext(EditQuery editQuery)
        {
            foreach (UserPart userPart in editQuery.ContractText.UserParts)
            {
                ContractText.Add(new ContractTextModel(userPart));
            }
            PageTitle = editQuery.QueryHeader;
            BindingContext = this;
        }

        private async void ApproveQuery(object sender, EventArgs e)
        {
            try
            {
                if (Array.FindIndex(EditQuery.ApprovedP, id => id == User.Id) > -1 ||
                    Array.FindIndex(EditQuery.DisapprovedP, id => id == User.Id) > -1)
                {
                    await DisplayAlert("Query status", "You have already voted for this query", "OK");
                }
                else
                {
                    int[] newApprovedP = new int[EditQuery.ApprovedP.Length + 1];
                    for (int i = 0; i < newApprovedP.Length - 1; i++)
                        newApprovedP[i] = EditQuery.ApprovedP[i];
                    newApprovedP[newApprovedP.Length - 1] = User.Id;

                    EditQuery.ApprovedP = newApprovedP;

                    if (EditQuery.ApprovedP.Length == Contract.ParticipantsId.Length)
                        EditQuery.Closed = true;

                    //ActivityIndicatorActions.SetActivityIndicatorOn(activityIndicator);
                    await EditQueryService.UpdateQueryStatus(EditQuery, Contract);
                    //ActivityIndicatorActions.SetActivityIndicatorOff(activityIndicator);

                    await DisplayAlert("Query status", "The query has been updated, you have approved " +
                        "this query", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Статус", ex.Message, "OK", "OK");
            }
        }

        private void DisapproveQuery(object sender, EventArgs e)
        {

        }
    }
}