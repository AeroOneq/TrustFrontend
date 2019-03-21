using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServerLib;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static TrustFrontend.CheckRegistrationData;

namespace TrustFrontend
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditProfilePage : ContentPage
    {
        private UserInfo User { get; set; }
        private EditProfilePageModel Model { get; set; }
        private byte[] NewFaceID { get; set; }

        public EditProfilePage(UserInfo user)
        {
            InitializeComponent();

            User = user;
            NewFaceID = User.FaceID;
            BindingContext = CreateEditPageModel();
        }

        private EditProfilePageModel CreateEditPageModel()
        {
            Model = new EditProfilePageModel(User);
            return Model;
        }

        private async void EditProfileData(object sender, EventArgs e)
        {
            try
            {
                UserInfo newUser = await CreateNewUserObject();

                await UserService.UpdateRecordAsync(User, newUser);
            }
            catch (ArgumentException ex)
            {
                await DisplayAlert("Ошибка в данных", ex.Message, "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Ошибка", ex.Message, "OK");
            }
        }

        private async Task<UserInfo> CreateNewUserObject()
        {
            UserInfo newUser = new UserInfo(User);

            string checkResultStirng = CheckName(Model.Name) + CheckSurname(Model.Surname) +
                CheckFamilyName(Model.FName) + ((Model.Login == User.Login) ? string.Empty : await CheckLogin(Model.Login))
                + CheckPassword(Model.Password) + ((NewFaceID == null) ? string.Empty :
                CompareFaceID.Compare(User.FaceID, NewFaceID) ? string.Empty : "Ошибка в Face ID");

            if (checkResultStirng == string.Empty)
            {
                newUser.Login = Model.Login;
                newUser.Password = Model.Password;
                newUser.Name = Model.Name;
                newUser.Surname = Model.Surname;
                newUser.FName = Model.FName;
                newUser.FaceID = NewFaceID ?? User.FaceID;

                return newUser;
            }
            else
                throw new ArgumentException(checkResultStirng);
        }
    }
}