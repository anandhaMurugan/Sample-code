using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Helseboka.Core.Models;

namespace Helseboka.Core
{
    public class DataApi
    {
        private static string dataService = "https://frontend.helseboka.no/data";


        private static string uname = "apiuser";
        private static string pword = "tZ5gS6n92T6q9FceBS8dUuEYndhz4pfkB7wgJRCuL47qc2xT8S";
        private static string apikey = "abcd1234whatever";


        public void GetAllMedicines()
        {
            var client = new HttpClient();
            var basicAuthHeaderValue = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{uname}:{pword}"));
            client.DefaultRequestHeaders.Add("X-API-KEY", apikey);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", basicAuthHeaderValue);
            var jsonRaw = client.GetStringAsync(dataService + "/medicine/all").Result;
            
            var jObject = JsonConvert.DeserializeObject<Medicine>(jsonRaw);
        }



    }
}
