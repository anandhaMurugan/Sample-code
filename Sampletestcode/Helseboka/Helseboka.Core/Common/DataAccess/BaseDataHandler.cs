using System;
using Helseboka.Common.NetworkAccess;
using Helseboka.Core.Common.CommonImpl;
using Helseboka.Core.Common.Interfaces;
using Helseboka.Core.Common.Model;

namespace Helseboka.Core.Common.DataAccess
{
    public class BaseDataHandler
    {
		protected ISerializer Serializer
		{
            get => ApplicationCore.Container.Resolve<ISerializer>();
        }

		protected IHttpClient HttpHandler
		{
            get => ApplicationCore.Container.Resolve<IHttpClient>();
        }

		protected BasicAPIhandler<Empty, TResponse> GetAPIhandlerForGet<TResponse>(String url) where TResponse :  class, new()
		{
			return new BasicAPIhandler<Empty, TResponse>(HttpHandler, Serializer, url, HttpAction.GET, null);
		}

		protected BasicAPIhandler<Empty, TResponse> GetAPIhandlerForDelete<TResponse>(String url) where TResponse : class, new()
        {
			return new BasicAPIhandler<Empty, TResponse>(HttpHandler, Serializer, url, HttpAction.DELETE, null);
        }

		protected BasicAPIhandler<TRequest, TResponse> GetAPIhandlerForPost<TRequest, TResponse>(String url, TRequest request) where TResponse : class, new() where TRequest : class, new()
        {
			return new BasicAPIhandler<TRequest, TResponse>(HttpHandler, Serializer, url, HttpAction.POST, request);
        }

		protected BasicAPIhandler<TRequest, TResponse> GetAPIhandlerForPut<TRequest, TResponse>(String url, TRequest request) where TResponse : class, new() where TRequest : class, new()
        {
			return new BasicAPIhandler<TRequest, TResponse>(HttpHandler, Serializer, url, HttpAction.PUT, request);
        }

    }
}
