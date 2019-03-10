using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ServerLib;

namespace TrustFrontend
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditQueryViewPage : ContentPage
    {
        public ObservableCollection<ContractTextModel> ContractText { get; set; } = 
            new ObservableCollection<ContractTextModel>();
        public string PageTitle { get; set; }

        public EditQueryViewPage(EditQuery editQuery)
        {
            InitializeComponent();
            SetBindingContext(editQuery);
            BindingContext = this;
        }

        private void SetBindingContext(EditQuery editQuery)
        {
            foreach (UserPart userPart in editQuery.ContractText.UserParts)
            {
                ContractText.Add(new ContractTextModel(userPart));
            }
            PageTitle = editQuery.QueryHeader;
        }
    }
}