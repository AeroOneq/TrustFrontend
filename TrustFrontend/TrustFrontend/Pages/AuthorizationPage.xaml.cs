using System;
using Xamarin.Forms;
using ServerLib;
using Plugin.Media.Abstractions;
using Plugin.Media;
using Plugin.Permissions.Abstractions;
using Plugin.Permissions;
using System.Threading.Tasks;
using System.IO;

namespace TrustFrontend
{
    public partial class AuthorizationPage : ContentPage
    {
        //face id which can be taken on the second page
        private byte[] FaceID { get; set; }
        private bool AuthMode { get; set; }

        public AuthorizationPage()
        {
            InitializeComponent();
        }

        private async void SignUp(object sender, EventArgs e) =>
            await Navigation.PushAsync(new RegistrationPage());

        private void SwitchToLoginPassMode(object sender, EventArgs e)
        {
            passwordEntry.IsVisible = true;
            takeFaceIDBtn.IsVisible = false;
            switchToLoginPassModeBtn.Style = Resources["authOptionsActiveBtnStyle"] as Style;
            switchToLoginFaceIDModeBtn.Style = Resources["authOptionsPassiveBtnStyle"] as Style;
            AuthMode = false;
        }
        private void SwitchToLoginFaceIDMode(object sender, EventArgs e)
        {
            passwordEntry.IsVisible = false;
            takeFaceIDBtn.IsVisible = true;
            switchToLoginPassModeBtn.Style = Resources["authOptionsPassiveBtnStyle"] as Style;
            switchToLoginFaceIDModeBtn.Style = Resources["authOptionsActiveBtnStyle"] as Style;
            AuthMode = true;
        }
        private async void SignIn(object sender, EventArgs e)
        {
            ActivityIndicatorActions.SetActivityIndicatorOn(authActivityIndicator);
            AuthResult authResult = await CheckLoginAndPass();
            if (authResult.User == null)
                await DisplayAlert("Ошибка при авторизации", authResult.MistakeMsg, "OK");
            else
            {
                App.Current.Properties.Clear();
                App.Current.Properties.Add("currentUserId", authResult.User.Id);
                await Navigation.PushAsync(new MainPage(authResult.User));
            }
            ActivityIndicatorActions.SetActivityIndicatorOff(authActivityIndicator);
        }
        /// <summary>
        /// Method for a Task in AuthorizeUser, checks the correctness of input data
        /// </summary>
        /// <returns>
        /// AuthResult object which is the combination od UserInfo object and mistake message
        /// </returns>
        private async Task<AuthResult> CheckLoginAndPass()
        {
            try
            {
                UserInfo user = await UserService.AuthorizeUserAsync(loginEntry.Text, passwordEntry.Text);
                return new AuthResult(user, string.Empty);
            }
            catch (Exception ex)
            {
                return new AuthResult(null, ex.Message);
            }
        }
        /// <summary>
        /// Activates camera and takes face ID, in the end changes the button content
        /// </summary>
        private async void TakeFaceID(object sender, EventArgs e)
        {
            if (await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Photos)
                == PermissionStatus.Granted)
            {
                var image = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions());
                if (image != null)
                {
                    FileStream fileStream = new FileStream(image.Path, FileMode.Open);
                    FaceID = new byte[fileStream.Length];
                    fileStream.Read(FaceID, 0, (int)fileStream.Length);
                }
            }
        }
        /// <summary>
        /// Authorizes user with login and Face ID
        /// </summary>
        private async void AuthorizeUserWithFaceID(object sender, EventArgs e)
        {
            ActivityIndicatorActions.SetActivityIndicatorOn(authActivityIndicator);
            AuthResult authResult = await CheckdLoginAndFaceID();
            if (authResult.User == null)
                await DisplayAlert("Ошибка при авторизации", authResult.MistakeMsg, "OK");
            else
            {
                FaceID = null;
                App.Current.Properties.Clear();
                App.Current.Properties.Add("currentUser", authResult.User.Id);
                await Navigation.PushAsync(new MainPage(authResult.User));
            }
            ActivityIndicatorActions.SetActivityIndicatorOff(authActivityIndicator);
        }
        /// <summary>
        /// Method for a Task in AuthorizeUserWithFaceID, checks the correctness of input data
        /// </summary>
        /// <returns>
        /// AuthResult object which is the combination od UserInfo object and mistake message
        /// </returns>
        private async Task<AuthResult> CheckdLoginAndFaceID()
        {
            if (FaceID == null)
                return new AuthResult(null, "Сделайте Face ID");
            else
            {
                try
                {
                    UserInfo user = await UserService.AuthorizeUserAsync(loginEntry.Text, FaceID);
                    return new AuthResult(user, string.Empty);
                }
                catch (Exception ex)
                {
                    return new AuthResult(null, ex.Message);
                }
            }
        }
    }
}
