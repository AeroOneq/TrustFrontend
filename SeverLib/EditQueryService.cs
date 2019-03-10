using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ServerLib
{
    public static class EditQueryService
    {
        public static async Task<List<EditQuery>> GetContractQueriesAsync(ContractInfo contract) =>
            await Task.Run(() => EditQueryDB.GetContractQueries(contract));

        public static async Task CreateNewRecordAsync(EditQuery editQuery) =>
            await Task.Run(() => EditQueryDB.CreateNewRecord(editQuery));

    }
}
