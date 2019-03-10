using System;
using ServerLib;
namespace ConsoleApp2
{
    class Program
    {
        static void Main(string[] args)
        {
            s();
            Console.ReadKey();
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
