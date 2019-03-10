using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Net.Http.Headers;
using System.Text;
using System.Net.Http;
using System.Web;
using System.Threading.Tasks;

namespace ServerLib
{
    public class RootClass
    {
        public string FaceID { get; set; }
        public FaceRectangle FaceRectangle { get; set; }
    }
    public class FaceRectangle
    {
        public double Top { get; set; }
        public double Left { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
    }
    public class FindFace
    {
        //Subscription details
        private const string subscriptionKey = "3f54505e01744c9690c18d82714b85a6";
        private const string regionConnectStr = "https://westeurope.api.cognitive.microsoft.com/face/v1.0/detect";
        /// <summary>
        /// Checks if the face is in the picture
        /// </summary>
        /// <param name="imageByteArr">
        /// byte array representation of an image
        /// </param>
        /// <returns>
        /// -2 if an exception accured, 
        /// -1 if there is no face in the picture, 
        /// positive number (or 0) otherwise
        /// </returns>
        public static string Find(byte[] imageByteArr)
        {
            try
            {
                HttpClient httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", subscriptionKey);
                string queryString = "returnFaceId=true";
                HttpResponseMessage response;
                using (ByteArrayContent content = new ByteArrayContent(imageByteArr))
                {
                    content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                    string uri = regionConnectStr + "?" + queryString;
                    response = httpClient.PostAsync(uri, content).Result;
                }
                string contentString = response.Content.ReadAsStringAsync().Result;
                RootClass[] rootClass = JsonConvert.DeserializeObject<RootClass[]>(contentString);
                //if there is more than one face or there are no faces then photo is incorrect
                if (rootClass.Length != 1)
                {
                    throw new ArgumentException("Сделайте фото только с Вашим лицом");
                }
                return rootClass[0].FaceID;
            }
            catch (ArgumentException e)
            {
                return e.Message;
            }
        }
    }
}
