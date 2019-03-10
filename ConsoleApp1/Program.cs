using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServerLib;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            s();
        }
        static async void s()
        {
            CosmosDB cosmos = new CosmosDB();
            cosmos.Connect();
            await cosmos.GetAllMessages(new ContractInfo());
            Console.ReadKey(true);
        }
    }
}
