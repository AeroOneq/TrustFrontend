using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ServerLib
{
    /// <summary>
    /// Class, objects of which are serialized to Json string which is sent in POST query
    /// </summary>
    public class RequestBody
    {
        public string FaceId1 { get; set; }
        public string FaceId2 { get; set; }
    }
    /// <summary>
    /// Class which represents the response Json string
    /// </summary>
    public class ResponseBody
    {
        public bool IsIdentical { get; set; }
        public double Confidence { get; set; }
    }
    public class CompareFaceID
    {
        //Subscription details
        private const string subscriptionKey = "3f54505e01744c9690c18d82714b85a6";
        private const string regionConnectStr = "https://westeurope.api.cognitive.microsoft.com/face/v1.0/verify";
        /// <summary>
        /// Checks if the face of the given image is the same to the face in database
        /// </summary>
        /// <param name="userImage">
        /// User's photo
        /// </param>
        /// <param name="databaseImage">
        /// Photo in database
        /// </param>
        /// <returns>
        /// True if they are identical, false otherwise or if an exception accured
        /// </returns>
        public static bool Compare(byte[] userImage, byte[] databaseImage)
        {
            try
            {
                HttpClient httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", subscriptionKey);
                string queryString = string.Empty;
                //create a json request
                RequestBody requestBody = new RequestBody()
                {
                    FaceId1 = FindFace.Find(userImage),
                    FaceId2 = FindFace.Find(databaseImage)
                };
                byte[] requestByteData = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(requestBody));
                //send the request and get the results
                HttpResponseMessage response;
                using (ByteArrayContent content = new ByteArrayContent(requestByteData))
                {
                    content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    string uri = regionConnectStr + "?" + queryString;
                    response = httpClient.PostAsync(uri, content).Result;
                }
                string contentString = response.Content.ReadAsStringAsync().Result;
                ResponseBody responseBody = JsonConvert.DeserializeObject<ResponseBody>(contentString);
                return responseBody.IsIdentical;
            }
            catch
            {
                return false;
            }
        }
    }
}
