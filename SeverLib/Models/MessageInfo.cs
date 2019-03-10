using System;
using Newtonsoft.Json;

namespace ServerLib
{
    public class MessageInfo
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        public string MessageText { get; set; }
        public DateTime MessageSendDate { get; set; }
        public int UserId { get; set; }
        public string ContractChatId { get; set; }
        public string AuthorName { get; set; }

        public MessageInfo(string text, DateTime sendDate, string id,
            int userId, string chatId, string userName)
        {
            MessageText = text;
            MessageSendDate = sendDate;
            Id = id;
            UserId = userId;
            ContractChatId = chatId;
            AuthorName = userName;
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
