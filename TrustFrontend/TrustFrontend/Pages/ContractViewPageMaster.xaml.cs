using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TrustFrontend
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ContractViewPageMaster : ContentPage
    {
        public ListView ListView { get; set; }

        public ContractViewPageMaster()
        {
            InitializeComponent();

            BindingContext = new ContractViewPageMasterViewModel();
            ListView = menuItemsListView;
        }

        class ContractViewPageMasterViewModel
        {
            public ObservableCollection<MenuItem> MenuItems { get; set; }

            public ContractViewPageMasterViewModel()
            {
                MenuItems = new ObservableCollection<MenuItem>(new[]
                {
                    new MenuItem
                    {
                        Id = 0,
                        MenuItemText = "Текст контракта",
                        MenuItemIconSource = ImageSource.FromFile("Resources/contractTextIcon.png"),
                        TargetType = typeof(ContractViewPageDetail)
                    },
                    new MenuItem
                    {
                        Id = 1,
                        MenuItemText = "Чат",
                        MenuItemIconSource = ImageSource.FromFile("Resources/chatIcon.png"),
                        TargetType = typeof(ContractChatPage)
                    },
                    new MenuItem
                    {
                        Id = 2,
                        MenuItemText = "Предложения по изменению",
                        MenuItemIconSource = ImageSource.FromFile("Resources/editQueriesIcon.png"),
                        TargetType = typeof(EditQueriesPage)
                    },
                });
            }
        }
    }
}