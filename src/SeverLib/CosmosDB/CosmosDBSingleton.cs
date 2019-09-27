namespace ServerLib
{
    public static class CosmosDBSingleton 
    {
        private static CosmosDB CosmosDB { get; } = new CosmosDB();

        public static CosmosDB GetObj() => CosmosDB;
    }
}
