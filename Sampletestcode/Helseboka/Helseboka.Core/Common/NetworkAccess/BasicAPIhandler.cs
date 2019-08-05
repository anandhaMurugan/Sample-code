using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Helseboka.Core.Auth.Model;
using Helseboka.Core.Common.CommonImpl;
using Helseboka.Core.Common.Constant;
using Helseboka.Core.Common.EnumDefinitions;
using Helseboka.Core.Common.Interfaces;
using Helseboka.Core.Common.Model;

namespace Helseboka.Common.NetworkAccess
{
	public class BasicAPIhandler <TRequest, TResult> where TRequest : class, new() where TResult : class, new()
    {
        protected IHttpClient httpClient;
        protected ISerializer serializer;
        protected String requestString;
        protected String uri;
        protected TRequest request;
        protected HttpAction method;

        protected IConfig config = ApplicationCore.Container.Resolve<IConfig>();

        public Dictionary<String, String> Headers { get; set; }

		public BasicAPIhandler(IHttpClient httpClient, ISerializer serializer, String uri, HttpAction method, TRequest request = default(TRequest))
        {
            this.httpClient = httpClient;
            this.serializer = serializer;
            this.uri = uri;
            this.method = method;
            this.request = request;
        }

		public virtual async Task<Response<TResult>> Execute()
        {
			var preProcessResult = PreProcess();
			if (preProcessResult.IsSuccess)
			{
				(int httpCode, String responseString) apiResponse = await httpClient.ExecuteAPI(uri, method, Headers, requestString);
				return ProcessResponse(apiResponse.httpCode, apiResponse.responseString);
			}
			else
			{
				return preProcessResult;
			}
        }

		protected virtual Response<TResult> PreProcess()
		{
			if (request != null)
			{
				(bool isSuccess, String jsonString)? serializerResponse = serializer?.Serialize<TRequest>(request);

                if (serializerResponse.HasValue && serializerResponse.Value.isSuccess)
                {
                    requestString = serializerResponse.Value.jsonString;
                }
                else
                {
                    return new Response<TResult>(new BaseClientErrorResponseInfo(ClientError.SerializationError));
                }
			}

			SetHeader();
			return Response<TResult>.GetSuccessResponse(null);
		}
        
		protected virtual void SetHeader()
        {
			if (Headers == null)
			{
				Headers = new Dictionary<String, String>();
			}

            if (!Headers.ContainsKey(APIConstant.AcceptLanguageHeaderKey))
			{
                var deviceLanguage = ApplicationCore.Container.Resolve<IDeviceHandler>().GetLanguageCode();
				Headers.Add(APIConstant.AcceptLanguageHeaderKey, deviceLanguage);
			}
			if (!Headers.ContainsKey(APIConstant.AcceptHeaderKey))
            {
				Headers.Add(APIConstant.AcceptHeaderKey, APIConstant.AcceptHeaderValue);
            }
			if (!Headers.ContainsKey(APIConstant.AuthorizationHeaderKey))
            {
				Headers.Add(APIConstant.AuthorizationHeaderKey, AuthService.Instance.GetBasicAuthHeaderValue());
            }
			if ((!Headers.ContainsKey(APIConstant.APIKeyHeaderKey)) && (!String.IsNullOrEmpty(AuthService.Instance.APIKey)))
            {
				Headers.Add(APIConstant.APIKeyHeaderKey, AuthService.Instance.APIKey);
            }
            if ((!Headers.ContainsKey(APIConstant.AppVersionHeaderKey)) && (!String.IsNullOrEmpty(DeviceDetails.Instance.AppVersion)))
            {
                Headers.Add(APIConstant.AppVersionHeaderKey, DeviceDetails.Instance.AppVersion);
            }
        }

		protected virtual Response<TResult> ProcessResponse(int httpCode, String responseString)
        {
			if (!IsSuccess(httpCode))
			{
                return GetErrorResponse(httpCode, responseString);
			}

            // Need to think how to handle correctly empty JSON response like {} or [] etc.
            if (responseString == APIConstant.EmptyResponse)
            {
                return Response<TResult>.GetSuccessResponse(null);
            }

			(bool isSuccess, TResult result)? serializerResponse = serializer?.Deserialize<TResult>(responseString);

			if (serializerResponse.HasValue && serializerResponse.Value.isSuccess)
			{
				return Response<TResult>.GetSuccessResponse(serializerResponse.Value.result);
			}
			else
			{
				return new Response<TResult>(new BaseClientErrorResponseInfo(ClientError.SerializationError));
			}
        }

		protected bool IsSuccess(int statusCode)
		{
			return statusCode >= 200 && statusCode <= 299;
		}

        protected Response<TResult> GetErrorResponse(int httpCode, String responseString)
        {
            //TODO: Understand what are different error case API can return. Accordingly add more info here.
            if (httpCode == APIConstant.HTTPTimeout)
            {
                return Response<TResult>.GetAPIErrorResponse(APIError.TimeOut);
            }
            else if(httpCode == 404)
            {
                return Response<TResult>.GetAPIErrorResponse(APIError.NotFound);
            }
            else if (httpCode == 401 || httpCode == 403)
            {
                return Response<TResult>.GetAPIErrorResponse(APIError.UnAuthorized);
            }
            else
            {
                return Response<TResult>.GetGenericAPIErrorResponse();
            }
        }
    }
}
