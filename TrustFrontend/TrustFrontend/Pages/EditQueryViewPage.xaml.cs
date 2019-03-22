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
                ActivityIndicatorActions.SetActivityIndicatorOn(activityIndicator);
                if (Array.FindIndex(EditQuery.ApprovedP, id => id == User.Id) > -1 ||
                    Array.FindIndex(EditQuery.DisapprovedP, id => id == User.Id) > -1)
                {
                    await DisplayAlert("Статус", "Вы уже высказали свое мнение", "OK");
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

                    await EditQueryService.UpdateQueryStatus(EditQuery, Contract);

                    await DisplayAlert("Статус", "Вы одобрили изменение", "OK");
                }
            }
            catch (Exception)
            {
                await DisplayAlert("Статус", "Произошла внутренняя ошибка", "OK");
            }
            finally
            {
                ActivityIndicatorActions.SetActivityIndicatorOff(activityIndicator);
            }
        }

        private async void DisapproveQuery(object sender, EventArgs e)
        {
            try
            {
                ActivityIndicatorActions.SetActivityIndicatorOn(activityIndicator);
                if (Array.FindIndex(EditQuery.ApprovedP, id => id == User.Id) > -1 ||
                    Array.FindIndex(EditQuery.DisapprovedP, id => id == User.Id) > -1)
                {
                    await DisplayAlert("Статус", "Вы уже высказали свое мнение", "OK");
                }
                else
                {
                    int[] newDispprovedP = new int[EditQuery.DisapprovedP.Length + 1];
                    for (int i = 0; i < newDispprovedP.Length - 1; i++)
                        newDispprovedP[i] = EditQuery.DisapprovedP[i];
                    newDispprovedP[newDispprovedP.Length - 1] = User.Id;

                    EditQuery.DisapprovedP = newDispprovedP;
                    EditQuery.Closed = true;

                    await EditQueryService.UpdateQueryStatus(EditQuery, Contract);

                    await DisplayAlert("Статус", "Вы наложили вето на изменение", "OK");
                }
            }
            catch (Exception)
            {
                await DisplayAlert("Статус", "Произошла внутренняя ошибка", "OK");
            }
            finally
            {
                ActivityIndicatorActions.SetActivityIndicatorOff(activityIndicator);
            }
        }
    }
}