using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Capstone.Helper
{
    public class PushNotification
    {
        private static readonly Uri FireBasePushNotificationsURL = new Uri("https://fcm.googleapis.com/fcm/send");
        private static readonly string ServerKey = "AAAAdF94TYE:APA91bHC9GS-okTUsupFqT249FsRFeQ9oqn7GWLv4YiCmEGs-m3Wf5XVLAJqe95S3IeIcnTULxt1Yuw8devRoG7FLnqCHiD8f66FT70Ux4lt1vvG_kzR6kNZzUMuisNUZFOjHTFCKKXR";

        public static async Task<bool> SendMessageAsync(string[] deviceTokens, string title, string body)
        {

            bool sent = false;

            // Send a message to the device corresponding to the provided
            // registration token.

            var number = from device in deviceTokens
                         select device;
            if (number.Count() > 0)
            {
                var messageInformation = new FirebaseMessage()
                {
                    data = new FirebaseNotification()
                    {
                        title = title,
                        body = body
                    },
                    registration_ids = deviceTokens
                };

                //chuyển từ object sang dạng json
                string jsonMessage = JsonConvert.SerializeObject(messageInformation);

                //Tạo request đến Firebase API
                var request = new HttpRequestMessage(HttpMethod.Post, FireBasePushNotificationsURL);

                request.Headers.TryAddWithoutValidation("Authorization", "key=" + ServerKey);
                request.Content = new StringContent(jsonMessage, Encoding.UTF8, "application/json");

                HttpResponseMessage result;
                using (var client = new HttpClient())
                {
                    result = await client.SendAsync(request);
                    var model = await result.Content.ReadAsStreamAsync();
                    sent = result.IsSuccessStatusCode;
                }

            }
            return sent;
        }
    }

    public class FirebaseMessage
    {
        public string[] registration_ids { get; set; }
        public FirebaseNotification data { get; set; }
    }

    public class FirebaseNotification
    {
        public string title { get; set; }
        public string body { get; set; }
    }

    public class PushNotificationModel
    {
        public string[] registration_ids { get; set; }
        public string title { get; set; }
        public string body { get; set; }
    }
}
