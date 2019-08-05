using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;
using System.Threading.Tasks;
using Helseboka.Core.Models;
using System.IO;

namespace Helseboka.Core
{
    public class MessageApi
    {

        private static string messageService = "https://frontend.helseboka.no/message";

        public async Task<string> GetMessage( string ApiUserId )
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("X-USER-ID", ApiUserId);
            var response = client.GetAsync(messageService);
            return await response.Result.Content.ReadAsStringAsync();
        }

        public async Task<string> SendMessage(string Title, string Message, string ApiUserId)
        {

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("X-USER-ID", ApiUserId);
            var message = new MessageModel
            {
                title = Title,
                message = Message,
            };
            var jsonPayload = JsonConvert.SerializeObject(message);
            var response = client.PostAsync(messageService, new StringContent(jsonPayload));
            return await response.Result.Content.ReadAsStringAsync();

        }
    }
}
