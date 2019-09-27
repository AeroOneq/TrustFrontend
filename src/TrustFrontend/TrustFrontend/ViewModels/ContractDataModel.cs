using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace TrustFrontend
{
    public class ContractDataModel
    {
        public ObservableCollection<DataCell> ContractGeneralData { get; set; } =
    new ObservableCollection<DataCell>();
        public ObservableCollection<ContractTextModel> ContractText { get; set; } =
            new ObservableCollection<ContractTextModel>();

    }
}
