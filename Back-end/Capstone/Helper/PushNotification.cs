using FirebaseAdmin.Messaging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Capstone.Helper
{
    public class PushNotification
    {
        private static string FireBasePushNotificationsURL = "https://fcm.googleapis.com/fcm/send";
        public static async Task<bool> SendMessageAsync(string[] deviceTokens, string title, string body)
        {
            //var registrationToken = token;

            // See documentation on defining a message payload.
            var messageInformation = new FirebaseMessage()
            {
                notification = new FirebaseNotification()
                {
                    title = title,
                    body = body
                },
                registration_ids = deviceTokens
            };
            //Object to JSON STRUCTURE => using Newtonsoft.Json;
            string jsonMessage = JsonConvert.SerializeObject(messageInformation);

            var request = new HttpRequestMessage(HttpMethod.Post, FireBasePushNotificationsURL);
            request.Headers.TryAddWithoutValidation("Authorization", "key=" + "AAAAdF94TYE:APA91bHC9GS-okTUsupFqT249FsRFeQ9oqn7GWLv4YiCmEGs-m3Wf5XVLAJqe95S3IeIcnTULxt1Yuw8devRoG7FLnqCHiD8f66FT70Ux4lt1vvG_kzR6kNZzUMuisNUZFOjHTFCKKXR");
            request.Content = new StringContent(jsonMessage, Encoding.UTF8, "application/json");
            HttpResponseMessage result;
            using (var client = new HttpClient())
            {
                result = await client.SendAsync(request);
            }
            return result.IsSuccessStatusCode;
        }
    }

    public class FirebaseMessage
    {
        public string[] registration_ids { get; set; }
        public FirebaseNotification notification { get; set; }
    }

    public class FirebaseNotification
    {
        public string title { get; set; }
        public string body { get; set; }
    }

    public class PushNotificationModel {
        public string[] registration_ids { get; set; }
        public string title { get; set; }
        public string body { get; set; }
    }
}
