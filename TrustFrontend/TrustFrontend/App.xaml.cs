using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using System.Threading.Tasks;
using ServerLib;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace TrustFrontend
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            MainPage = new NavigationPage(new AuthorizationPage());
        }
        private AuthResult GetUser(int currentUserId)
        {
            UserInfo user = Login.GetUserById(currentUserId, out string mistakeMsg);
            return new AuthResult(user, mistakeMsg);
        }
        protected override void OnStart()
        {
            
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
