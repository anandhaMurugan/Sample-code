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
    public class UserApi
    {

        private static string userService = "https://frontend.helseboka.no/user";

        public async Task<string> GetUser(string ApiUserId)
        {
            HttpClient client = new HttpClient();
            //client.DefaultRequestHeaders.Add("X-USER-ID", ApiUserId);


            var response = client.GetAsync(userService);
            return await response.Result.Content.ReadAsStringAsync();
        }

        public async Task<string> UpdateUser(UserModel userModel, string ApiUserId)
        {

            HttpClient client = new HttpClient();
            //client.DefaultRequestHeaders.Add("X-USER-ID", ApiUserId);
            
            var jsonPayload = JsonConvert.SerializeObject(userModel);
            var response = client.PutAsync(userService, new StringContent(jsonPayload));
            return await response.Result.Content.ReadAsStringAsync();

        }
    }
}
