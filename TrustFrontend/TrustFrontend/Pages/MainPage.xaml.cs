using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ServerLib;

namespace TrustFrontend
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : MasterDetailPage
    {
        public UserInfo CurrentUser { get; set; }

        public MainPage(UserInfo user)
        {
            InitializeComponent();
            CurrentUser = user;
            MasterPage.ListView.ItemTapped += ListViewItemTapped;
            Detail = new SharedContractsPage(user);
        }

        private void ListViewItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (!(e.Item is MenuItem item))
                return;
            var page = (Page)Activator.CreateInstance(item.TargetType, CurrentUser);
            Detail = new NavigationPage(page);
            IsPresented = false;

            MasterPage.ListView.SelectedItem = null;
        }
    }
}