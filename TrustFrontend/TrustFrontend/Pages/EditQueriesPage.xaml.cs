using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServerLib;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Collections.ObjectModel;

namespace TrustFrontend
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditQueriesPage : ContentPage
    {
        #region Properties
        public ObservableCollection<ListViewGrouping<string, EditQueryModel>>
            QueriesGroups { get; set; }
        public List<EditQuery> EditQueriesList { get; set; } = new List<EditQuery>();
        private ContractInfo CurrentContract { get; set; }
        private UserInfo CurrentUser { get; set; }
        #endregion

        public EditQueriesPage(ContractInfo contract, UserInfo user)
        {
            InitializeComponent();

            CurrentContract = contract;
            CurrentUser = user;
        }

        private async void GoToEditQueryViewPage(object sender, ItemTappedEventArgs e)
        {
            EditQueryModel editQueryModel = e.Item as EditQueryModel;
            EditQuery editQuery = EditQueriesList.Find(eq => eq.QueryId == editQueryModel.ID);
            await Navigation.PushAsync(new EditQueryViewPage(editQuery));
        }

        private async void GoToCreateQueryPage(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CreateNewQueryPage(CurrentContract, CurrentUser));
        }

        private async void UpdateEditQueries(object sender, EventArgs e)
        {
            editQueriesListView.IsRefreshing = true;

            await CreateBindingContext();

            editQueriesListView.IsRefreshing = false;
        }

        private async Task CreateBindingContext()
        {
            EditQueriesList = await EditQueryService.GetContractQueriesAsync(CurrentContract);

            List<EditQueryModel> editQueryModels = new List<EditQueryModel>();
            foreach (EditQuery editQuery in EditQueriesList)
                editQueryModels.Add(new EditQueryModel(editQuery));

            var queriesGroups = editQueryModels.GroupBy(q => q.Status).Select(
                g => new ListViewGrouping<string, EditQueryModel>(g.Key, g));
            QueriesGroups = new ObservableCollection<ListViewGrouping<string, EditQueryModel>>
                (queriesGroups);

            editQueriesListView.ItemsSource = QueriesGroups;
        }
    }
}