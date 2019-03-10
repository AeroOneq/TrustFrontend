using System.Collections.ObjectModel;

namespace TrustFrontend
{
    public class CreateNewContractPageModel
    {
        public ObservableCollection<ContractTextModel> ContractText { get; set; } =
            new ObservableCollection<ContractTextModel>();
        public ObservableCollection<CreateNewContractUserData> UsersData { get; set; } =
            new ObservableCollection<CreateNewContractUserData>();
        public string ContractName { get; set; } = string.Empty;
    }
}
