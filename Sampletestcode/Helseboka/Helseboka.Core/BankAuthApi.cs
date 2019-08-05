using System;

using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Helseboka.Core.Models;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Helseboka.Core
{
    public class BankAuthApi
    {
        private static string baseAddressBankId = "https://frontend.helseboka.no/bankid-preprod";
        private static string baseAddressAuth = "https://frontend.helseboka.no/authentication-test";

        private string headerUserAgent { get; set; }
        public string sessionId { get; set; }

        private static string uname = "apiuser";
        private static string pword = "tZ5gS6n92T6q9FceBS8dUuEYndhz4pfkB7wgJRCuL47qc2xT8S";
        //private static string apikey = "abcd1234whatever";

        public BankAuthApi( string userAgent)
        {
            headerUserAgent = userAgent;
        }

        public string  getAuthValPair()
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes($"{uname}:{pword}"));
        }

        public async Task<string> TestConnect(string ssn)
        {
            var client = new HttpClient();

            //client.BaseAddress = new Uri(baseAddressBankId);
            client.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("application/json"));
            var basicAuthHeaderValue = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{uname}:{pword}"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", basicAuthHeaderValue);
            //client.DefaultRequestHeaders.Add("Host", )
            //client.DefaultRequestHeaders.Add("ssn", ssn );
            var response = await client.PostAsync(baseAddressBankId + "/testutils/createAuthorizedSession/", null);
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> AuthStart()
        {
            try
            {
                
                var client = new HttpClient();
                //client.BaseAddress = new Uri(baseAddressBankId);
                var basicAuthHeaderValue = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{uname}:{pword}"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", basicAuthHeaderValue);
                client.DefaultRequestHeaders.Add("User-Agent", headerUserAgent);
                //client.DefaultRequestHeaders.Add("Access-Control-Allow-Origin", "*");
                HttpResponseMessage response = await client.GetAsync(baseAddressBankId +
                                          "/authstart?BidType=BID&okUrl=helseboka://Status=Success&cancelUrl=helseboka://Status=Cancel&errorUrl=helseboka://Status=Error/");
                var json = await response.Content.ReadAsStringAsync();
                var jObject = JsonConvert.DeserializeObject<AuthStartModel>(json);
                return jObject.sid;

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("CAUGHT EXCEPTION:");
                System.Diagnostics.Debug.WriteLine(ex);
            }
            return null;

        }

        public  string ShowLoginStored(string sessionid)
        {
            var client = new HttpClient();
            
            var basicAuthHeaderValue = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{uname}:{pword}"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", basicAuthHeaderValue);
            client.DefaultRequestHeaders.Add("sessionid", sessionid);
            client.DefaultRequestHeaders.Add("User-Agent", headerUserAgent);
            var response = client.GetStringAsync(baseAddressBankId+"/stored").Result;
            return response;
        }

        public  string ShowLoginMobile(string sessionid)
        {
            var client = new HttpClient();
            var basicAuthHeaderValue = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{uname}:{pword}"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", basicAuthHeaderValue);
            client.DefaultRequestHeaders.Add("sessionid", sessionid);
            client.DefaultRequestHeaders.Add("User-Agent", headerUserAgent);
            var response = client.GetStringAsync(baseAddressBankId+"/mobile").Result;
            return response;
        }


        public  string Authenticate(string banksessid)
        {
            try
            {
                var client = new HttpClient();
                var basicAuthHeaderValue = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{uname}:{pword}"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", basicAuthHeaderValue);
                client.DefaultRequestHeaders.Add("User-Agent", headerUserAgent);
                var response = client.GetStringAsync(baseAddressAuth + "/authenticate?bankidsessionid=" + banksessid).Result;
                return response;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<string> Refresh(string banksessid)
        {
            var client = new HttpClient();

            var basicAuthHeaderValue = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{uname}:{pword}"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", basicAuthHeaderValue);
            var response = client.GetAsync(baseAddressAuth+"/refresh?bankidsessionid=" + banksessid);
            return await response.Result.Content.ReadAsStringAsync();
            //refreshes and returns apikey

        }

        public async Task<string> Validate()
        {
            var client = new HttpClient();
            var basicAuthHeaderValue = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{uname}:{pword}"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", basicAuthHeaderValue);
            var response = client.GetAsync(baseAddressAuth + "/validate");
            return await response.Result.Content.ReadAsStringAsync();

        }


    }
}
