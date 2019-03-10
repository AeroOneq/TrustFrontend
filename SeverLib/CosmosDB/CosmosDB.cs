using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Newtonsoft.Json;

namespace ServerLib
{
    public class CosmosDB
    {
        #region Connection constants
        private static readonly string EndpointUrl = "https://trustcosmosdb.documents.azure.com:443/";
        private static readonly string PrimaryKey = "WmQj6N819bQVOow27tlwFLeU9pYhWbnYrFSNiKLtNv6zZodwBfSx73BfqzzkNRYsWZwZIMZbC3FtkzqViOTPDA==";
        private static readonly string DatabaseName = "TrustMessagesDB";
        #endregion
        #region Properties
        private  DocumentClient DocumentClient { get; set; }
        #endregion

        public void Connect() => Task.Run(() => CreateNewClient());
        public async Task<List<MessageInfo>> GetAllMessages(ContractInfo contract)
        {
            var messages = await DocumentClient.ReadDocumentFeedAsync(
                UriFactory.CreateDocumentCollectionUri(DatabaseName, $"contractChat{contract.Id}"));
            List<MessageInfo> messagesList = new List<MessageInfo>();
            foreach (var message in messages)
            {
                messagesList.Add(JsonConvert.DeserializeObject<MessageInfo>(
                    message.ToString()));
            }
            return messagesList;
        }
        public async void InsertMessageIntoTheDB(MessageInfo message)
        {
            await DocumentClient.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri(
                DatabaseName, $"{message.ContractChatId}"), message);
        }
        public async void CreateNewCollectionAsync(ContractInfo contract)
        {
            DocumentCollection documentCollection = new DocumentCollection
            {
                Id = $"contractChat{contract.Id}"
            };
            await DocumentClient.CreateDocumentCollectionIfNotExistsAsync(
                UriFactory.CreateDatabaseUri(DatabaseName), documentCollection);
        }

        #region Private utility methods
        private void CreateNewClient() =>
            DocumentClient = new DocumentClient(new Uri(EndpointUrl), PrimaryKey);
        #endregion
    }
}
