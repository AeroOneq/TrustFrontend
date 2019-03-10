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
    public partial class CreateNewQueryPage : ContentPage
    {
        public ObservableCollection<ContractTextModel> ContractText { get; set; } =
            new ObservableCollection<ContractTextModel>();
        public string QueryName { get; set; } = string.Empty;

        private UserInfo CurrentUser { get; set; }
        private ContractInfo CurrentContract { get; set; }

        public CreateNewQueryPage(ContractInfo contract, UserInfo user)
        {
            InitializeComponent();
            SetBindingContext(contract);

            CurrentContract = contract;
            CurrentUser = user;

            BindingContext = this;
        }

        private void SetBindingContext(ContractInfo contract)
        {
            foreach (UserPart userPart in contract.ContractText.UserParts)
            {
                ContractText.Add(new ContractTextModel(userPart));
            }
        }

        private async void CreateNewQuery(object sender, EventArgs e)
        {
            try
            {
                EditQuery newQuery = CreateQueryObject();

                await EditQueryService.CreateNewRecordAsync(newQuery);

                await DisplayAlert("Статус создания", "Предложение успешно создано", "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Ошибка", ex.Message, "OK");
            }
        }

        private EditQuery CreateQueryObject()   
        {
            UserPart[] userParts = new UserPart[ContractText.Count];
            for (int i = 0; i < userParts.Length; i++)
            {
                userParts[i] = new UserPart(ContractText[i].UserName, ContractText[i].Rights,
                    ContractText[i].Obligations);
            }

            return new EditQuery
            {
                ApprovedP = new int[0],
                DisapprovedP = new int[0],
                AuthorId = CurrentUser.Id,
                AuthorName = CurrentUser.Name,
                Closed = false,
                ContractId = CurrentContract.Id,
                CreationDate = DateTime.Now,
                Photos = new byte[0],
                ContractText = new ContractText(userParts),
                QueryHeader = QueryName,
            };
        }
    }
}