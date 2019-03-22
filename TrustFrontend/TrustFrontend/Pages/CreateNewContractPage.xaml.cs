using ServerLib;
using System;
using System.Collections.Generic;
using System.Linq;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TrustFrontend
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CreateNewContractPage : ContentPage
    {
        public CreateNewContractPageModel PageModel { get; set; } =
            new CreateNewContractPageModel();
        private UserInfo User { get; set; }

        public CreateNewContractPage(UserInfo user)
        {
            InitializeComponent();
            User = user;

            CreateInitialBindingContext();
            BindingContext = PageModel;
        }

        private void CreateInitialBindingContext()
        {
            PageModel.UsersData.Add(new CreateNewContractUserData()
            {
                UserNameEditorText = string.Empty,
                HeadLabelText = "Введите имя пользователя:",
                IsEnabled = true,
            });
        }

        private void AddNewUser(object sender, EventArgs e)
        {
            List<CreateNewContractUserData> userData = usersDataListView.ItemsSource.
                Cast<CreateNewContractUserData>().ToList();
            PageModel.UsersData.RemoveAt(PageModel.UsersData.Count - 1);

            string userName = userData[userData.Count - 1].UserNameEditorText;
            if (string.IsNullOrEmpty(userName) || PageModel.UsersData.ToList().FindIndex(
                u => u.UserNameEditorText == userName) != -1)
            {
                DisplayAlert("Ошибка", "Невозможно создать пользователя с такими параметрами",
                    "OK");
                return;
            }

            PageModel.UsersData.Add(new CreateNewContractUserData()
            {
                UserNameEditorText = userName,
                HeadLabelText = "Имя пользователя",
                IsEnabled = false,
                DeleteButtonId = userName,
                IsDeleteButtonVisible = true
            });
            PageModel.UsersData.Add(new CreateNewContractUserData()
            {
                UserNameEditorText = string.Empty,
                HeadLabelText = "Имя нового пользователя:",
                IsEnabled = true,
                IsDeleteButtonVisible = false
            });
            PageModel.ContractText.Add(new ContractTextModel(new UserPart(userName, 
                string.Empty, string.Empty)));

            usersDataListView.HeightRequest += 65;
            BindingContext = PageModel;
        }

        private void DeleteUser(object sender, EventArgs e)
        {
            try
            {
                string userName = (sender as Button).ClassId;

                CreateNewContractUserData userData =
                    PageModel.UsersData.Single(u => u.UserNameEditorText == userName);
                PageModel.UsersData.Remove(userData);

                ContractTextModel contractTextModel = PageModel.ContractText.Single(
                    cm => cm.UserName == userName);
                PageModel.ContractText.Remove(contractTextModel);

                usersDataListView.HeightRequest -= 65;
                BindingContext = PageModel;
            }
            catch (Exception ex)
            {
                DisplayAlert("Ошибка", ex.Message, "OK");
            }
        }

        private async void CreateNewContract(object sender, EventArgs e)
        {
            try
            {
                ActivityIndicatorActions.SetActivityIndicatorOn(activitIndicator);
                string[] userLogins = GetUserLogins();
                ContractInfo contractInfo = CreateContractObject(
                    await UserService.GetUserIDs(userLogins));

                await ContractService.CreateNewRecord(contractInfo);
                await DisplayAlert("Статус", "Контракт создан", "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Ошибка при создании контракта", ex.Message, "OK");
            }
            finally
            {
                ActivityIndicatorActions.SetActivityIndicatorOff(activitIndicator);
            }
        }
        private string[] GetUserLogins()
        {
            string[] userLogins = new string[PageModel.UsersData.Count - 1];
            for (int i = 0; i < userLogins.Length; i++)
            {
                userLogins[i] = PageModel.UsersData[i].UserNameEditorText;
            }
            return userLogins;
        }
        private ContractInfo CreateContractObject(List<int> userIDsList)
        {
            UserPart[] userParts = new UserPart[PageModel.ContractText.Count];
            for (int i = 0; i < PageModel.ContractText.Count; i++)
            {
                userParts[i] = new UserPart(PageModel.ContractText[i].UserName,
                    PageModel.ContractText[i].Rights, PageModel.ContractText[i].Obligations);
            }

            return new ContractInfo
            {
                ApprovedP = new int[0],
                DisapprovedP = new int[0],
                UnsignedP = userIDsList.ToArray(),
                ParticipantsId = userIDsList.ToArray(),
                AuthorId = User.Id,
                Name = PageModel.ContractName,
                CreationDate = DateTime.Now,
                Photos = new byte[0],
                ContractText = new ContractText(userParts),
                Status = false
            };
        }
    }
}