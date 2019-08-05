using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Helseboka.Core.Common.Interfaces
{
    public interface IHttpClient
    {
		Task<(int httpCode, String responseString)> ExecuteAPI(String uri, HttpAction method, Dictionary<String, String> headers = null, String requestBody = null);
    }

	public enum HttpAction
	{
		GET,
        POST,
        PUT,
        DELETE
	}
}
