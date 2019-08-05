using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Helseboka.Core.Common.Constant;
using Helseboka.Core.Common.Interfaces;
using Helseboka.Core.Common.Model;

namespace Helseboka.Core.Common.CommonImpl
{
    public class RESTClient : IHttpClient
    {
        private ILogger Logger => ApplicationCore.Container.Resolve<ILogger>();
        private IAnalytics EventTracker => ApplicationCore.Container.Resolve<IAnalytics>();


        public async Task<(int httpCode, string responseString)> ExecuteAPI(string uri, HttpAction method, Dictionary<string, string> headers = null, string requestBody = null)
        {
            var client = new HttpClient();
            if (headers != null && headers.Count > 0)
            {
                client.DefaultRequestHeaders.Clear();
                foreach(var header in headers)
                {
                    client.DefaultRequestHeaders.Add(header.Key, header.Value);
                }
            }

            Logger.Log($"{method.ToString()}: {uri} \nHeaders: {JSONSerializer.LogObject(headers)} \nRequest: {JSONSerializer.PrettyPrint(requestBody)}");

            try
            {
                switch (method)
                {
                    case HttpAction.GET: return await GetResponse(await client.GetAsync(uri), requestBody);
                    case HttpAction.POST: return await GetResponse(await client.PostAsync(uri, new StringContent(requestBody, Encoding.UTF8, "application/json")), requestBody);
                    case HttpAction.PUT: return await GetResponse(await client.PutAsync(uri, new StringContent(requestBody, Encoding.UTF8, "application/json")), requestBody);
                    case HttpAction.DELETE: return await GetResponse(await client.DeleteAsync(uri), requestBody);
                    default: return (999, null);
                }
            }
            catch (TaskCanceledException ex)
            {
                string path = uri;
                if(Uri.TryCreate(uri, UriKind.Absolute, out var url))
                {
                    path = String.Format($"{url.Scheme}://{url.Authority}{url.AbsolutePath}");
                }
                EventTracker.TrackError(ex, new Dictionary<String, String>
                {
                    {"URL", path},
                    {"Response", "Request Timeout"}
                });
                return (APIConstant.HTTPTimeout, String.Empty);
            }
            catch (Exception ex)
            {
                string path = uri;
                if (Uri.TryCreate(uri, UriKind.Absolute, out var url))
                {
                    path = String.Format($"{url.Scheme}://{url.Authority}{url.AbsolutePath}");
                }
                EventTracker.TrackError(ex, new Dictionary<String, String>
                {
                    {"URL", path},
                    {"Response", "General Exception"}
                });
                return (999, String.Empty);
            }
        }

        private async Task<(int httpCode, string responseString)> GetResponse(HttpResponseMessage response, String requestBody)
        {
            var responseString = await response.Content.ReadAsStringAsync();

            try
            {
                if(!response.IsSuccessStatusCode)
                {
                    var url = response.RequestMessage.RequestUri;
                    string path = String.Format($"{url.Scheme}://{url.Authority}{url.AbsolutePath}");

                    if (!ExcludeSomeUrlException((int)response.StatusCode, path))
                    {
                        EventTracker.TrackError(GetExceptionForHttp((int)response.StatusCode, path), new Dictionary<string, string>
                        {
                            {"URL", path},
                            {"Request", requestBody},
                            {"Response", responseString}
                        });
                    }
                }

                Logger.Log($"Response {(int)response.StatusCode} for {response.RequestMessage.RequestUri.AbsoluteUri} \n{JSONSerializer.PrettyPrint(responseString)}");
            }
            catch
            {
                Logger.Log("Exception while loggin");
            }

            return ((int)response.StatusCode, responseString);
        }

        private Exception GetExceptionForHttp(int httpCode, String url)
        {
            switch(httpCode)
            {
                case 400: return new BadRequestException(url);
                case 404: return new NotFoundException(url);
                case 405: return new MethodNotAllowedException(url);
                case 401:
                case 403: return new UnAuthorizedException(url);
                case 301:
                case 302: return new MovedTempException(url);
                case 304: return new NotModifiedException(url);
                case 500:
                case 598: return new InternalServerException(url);
                default: return new APIErrorException(url);
            }
        }

        private bool ExcludeSomeUrlException(int httpCode, string url)
        {
            if (url.Equals(APIEndPoints.NextAppointment) && httpCode == 404)
            {
                return true;
            }
            return false;
        }
    }

    public class BadRequestException : Exception
    {
        public BadRequestException(String url) : base(url) { }
    }

    public class NotFoundException : Exception
    {
        public NotFoundException(String url) : base(url) { }
    }

    public class MethodNotAllowedException : Exception
    {
        public MethodNotAllowedException(String url) : base(url) { }
    }

    public class UnAuthorizedException : Exception
    {
        public UnAuthorizedException(String url) : base(url) { }
    }

    public class MovedTempException : Exception
    {
        public MovedTempException(String url) : base(url) { }
    }

    public class NotModifiedException : Exception
    {
        public NotModifiedException(String url) : base(url) { }
    }

    public class InternalServerException : Exception
    {
        public InternalServerException(String url) : base(url) { }
    }

    public class APIErrorException : Exception
    {
        public APIErrorException(String url) : base(url) { }
    }
}
