using System;
using System.IO;
using System.Threading.Tasks;
using Plugin.Media.Abstractions;
using ServerLib;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TrustFrontend
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RegistrationPage : ContentPage
    {
        private UserInfo NewUser { get; set; } = new UserInfo();

        public RegistrationPage()
        {
            InitializeComponent();
            AddGestures();
        }

        private void AddGestures()
        {
            TapGestureRecognizer tapGestureRecognizer = new TapGestureRecognizer();
            tapGestureRecognizer.Tapped += async (sender, e) =>
            {
                try
                {
                    MediaFile faceID = await Photo.TakeFaceIDAsync();
                    if (faceID != null)
                    {
                        NewUser.FaceID = Photo.GetByteRepresentationOfFaceID(faceID);

                        Frame faceIDFrame = sender as Frame;
                        faceIDFrame.Content = new Image()
                        {
                            Source = ImageSource.FromStream(() => new MemoryStream(NewUser.FaceID)),
                            Aspect = Aspect.AspectFill
                        };
                    }
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Ошибка", ex.Message, "OK");
                }
            };

            faceIDFrame.GestureRecognizers.Add(tapGestureRecognizer);
        }

        private async void SendCodeAndGoToNextStage(object sender, EventArgs e)
        {
            try
            {
                (sender as Button).IsEnabled = false;
                ActivityIndicatorActions.SetActivityIndicatorOn(activityIndicator);
                if (await CheckInputData())
                {
                    await Task.Run(() => Email.SendEmailWIthCode(NewUser));
                    await Navigation.PushAsync(new ConfirmEmailPage(NewUser));
                }
                ActivityIndicatorActions.SetActivityIndicatorOff(activityIndicator);
            }
            catch (Exception ex)
            {
                await DisplayAlert("Ошибка", ex.Message, "OK");
            }
            finally
            {
                (sender as Button).IsEnabled = true;
            }
        }

        #region Data check
        private async Task<bool> CheckInputData()
        {
            SetNewUserProperties();
            string[] data = GetDataStringArray();

            if (NewUser.FaceID == null)
            {
                await DisplayAlert("Не найдено FaceID", "Сделайте faceID", "OK");
                return false;
            }

            RegistrationDataCheckResult checkResult = await CheckRegistrationData.Check(
                data, NewUser.FaceID, true);

            if (!checkResult.Result)
                await DisplayErrorMessage(checkResult);
            return checkResult.Result;
        }
        private void SetNewUserProperties()
        {
            NewUser.Login = loginEntry.Text;
            NewUser.Password = passwordEntry.Text;
            NewUser.Name = nameEntry.Text;
            NewUser.Surname = surnameEntry.Text;
            NewUser.FName = fNameEntry.Text;
            NewUser.Email = emailEntry.Text;
            NewUser.CodeWord = codewordEntry.Text;
            NewUser.Fingerprint = new byte[0];
        }
        private string[] GetDataStringArray()
        {
            return new string[]
            {
                loginEntry.Text,
                passwordEntry.Text,
                nameEntry.Text,
                surnameEntry.Text,
                fNameEntry.Text,
                emailEntry.Text,
                codewordEntry.Text,
                "1111",
                "111111",
            };
        }
        private async Task DisplayErrorMessage(RegistrationDataCheckResult checkResult)
        {
            if (!string.IsNullOrEmpty(checkResult.NameBlockError))
            {
                await DisplayAlert("Ошибка в данных", checkResult.NameBlockError, "OK");
                return;
            }
            if (!string.IsNullOrEmpty(checkResult.ConfidentialBlockError))
            {
                await DisplayAlert("Ошибка в данных", checkResult.NameBlockError, "OK");
                return;
            }
            if (!string.IsNullOrEmpty(checkResult.GeneralBlockError))
            {
                await DisplayAlert("Ошибка в данных", checkResult.NameBlockError, "OK");
                return;
            }
        }
        #endregion
    }
}