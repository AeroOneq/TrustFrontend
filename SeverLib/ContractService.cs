using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ServerLib
{
    public static class ContractService
    {
        public static async Task CreateNewRecord(ContractInfo contract) =>
            await Task.Run(() => ContractDB.CreateRecord(contract));

        public static async Task<List<ContractInfo>> GetAllUserContracts(int userID, bool status) =>
            await Task.Run(() => ContractDB.GetUserContracts(userID, status));
    }
}
