using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Capstone.Service
{
    public interface IWebHookService
    {
        Task<HttpResponseMessage> WebHook(string url, string message);
    }

    public class WebHookService : IWebHookService
    {
        public Task<HttpResponseMessage> WebHook(string url, string message)
        {
            var client = new HttpClient();
            // Create the HttpContent for the form to be posted.
            var requestContent = new FormUrlEncodedContent(new[] {new KeyValuePair<string, string>("message", message)});
            return client.PostAsync(url, requestContent);
        }
    }
}
