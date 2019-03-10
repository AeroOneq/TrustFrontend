using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServerLib;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TrustFrontend
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class UserProfilePage : ContentPage
	{
        private UserInfo User { get; set; }

		public UserProfilePage(UserInfo user)
		{
			InitializeComponent ();

            User = user;
            BindingContext = new UserProfilePageModel(User);

            userPhotoFrame.Content = new Image()
            {
                Source = ImageSource.FromStream(() => new MemoryStream(User.FaceID)),
                Aspect = Aspect.Fill
            };
		}

        private async void GoToEditProfilePage(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new EditProfilePage(User));
        }
    }
}