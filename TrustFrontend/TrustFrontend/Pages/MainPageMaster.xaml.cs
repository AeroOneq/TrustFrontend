using ServerLib;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TrustFrontend
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPageMaster : ContentPage
    {
        public ListView ListView { get; set; }
        public UserInfo User { get; set; }

        public MainPageMaster()
        {
            InitializeComponent();

            userPhotoFrame.Content = new Image()
            {
                Aspect = Aspect.Fill,
                Source = ImageSource.FromStream(() => new MemoryStream(User.FaceID))
            };
            ListView = menuItemsListView;
            BindingContext = new MainPageMasterViewModel();
        }

        class MainPageMasterViewModel
        {
            public ObservableCollection<MenuItem> MenuItems { get; set; }

            public MainPageMasterViewModel()
            {
                MenuItems = new ObservableCollection<MenuItem>(new[]
                {
                    new MenuItem
                    {
                        Id = 0,
                        MenuItemText = "Мой профиль",
                        MenuItemIconSource = ImageSource.FromFile("Resources/userIcon.png"),
                        TargetType = typeof(UserProfilePage)
                    },
                    new MenuItem
                    {
                        Id = 1,
                        MenuItemText = "Принятые контракты",
                        MenuItemIconSource = ImageSource.FromFile("Resources/acceptedContractsIcon.png"),
                        TargetType = typeof(ApprovedContractsPage)
                    },
                    new MenuItem
                    {
                        Id = 2,
                        MenuItemText = "Все контракты",
                        MenuItemIconSource = ImageSource.FromFile("Resources/sharedContractsIcon.png"),
                        TargetType = typeof(SharedContractsPage)
                    },
                });
            }
        }
    }
}