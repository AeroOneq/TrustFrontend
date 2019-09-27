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
            try
            {
                (sender as Button).IsEnabled = false;
                if (!AuthMode)
                    await AuthorizeUserWithLoginAndPass();
                else
                    await AuthorizeUserWithFaceID();
            }
            finally
            {
                (sender as Button).IsEnabled = true;
            }
        }

        private async Task AuthorizeUserWithLoginAndPass()
        {
            try
            {
                ActivityIndicatorActions.SetActivityIndicatorOn(authActivityIndicator);
                AuthResult authResult = await CheckLoginAndPass();
                if (authResult.User == null)
                    await DisplayAlert("Ошибка при авторизации", authResult.MistakeMsg, "OK");
                else
                {
                    await Navigation.PushAsync(new MainPage(authResult.User));
                }
                ActivityIndicatorActions.SetActivityIndicatorOff(authActivityIndicator);
            }
            catch (Exception)
            {
                await DisplayAlert("Ошибка", "Ошибка авторизации", "OK");
            }
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
            try
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
            catch (NullReferenceException)
            {
                await DisplayAlert("Ошибка", "Ошибка при фотографировании", "OK");
            }
            catch (Exception)
            {
                await DisplayAlert("Ошибка", "Ошибка при фотографировании", "OK");
            }

        }
        /// <summary>
        /// Authorizes user with login and Face ID
        /// </summary>
        private async Task AuthorizeUserWithFaceID()
        {
            try
            {
                ActivityIndicatorActions.SetActivityIndicatorOn(authActivityIndicator);
                AuthResult authResult = await CheckdLoginAndFaceID();
                if (authResult.User == null)
                    await DisplayAlert("Ошибка при авторизации", authResult.MistakeMsg, "OK");
                else
                {
                    FaceID = null;
                    await Navigation.PushAsync(new MainPage(authResult.User));
                }
                ActivityIndicatorActions.SetActivityIndicatorOff(authActivityIndicator);
            }
            catch (Exception)
            {
                await DisplayAlert("Ошибка", "Ошибка при авторизации", "OK");
            }
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
