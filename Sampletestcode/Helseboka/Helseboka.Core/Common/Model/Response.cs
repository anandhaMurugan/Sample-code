using System;
using Helseboka.Core.Common.EnumDefinitions;

namespace Helseboka.Core.Common.Model
{
	public class Response
	{
		public BaseResponseInfo ResponseInfo { get; protected set; }

		public bool IsSuccess
        {
            get => ResponseInfo != null && ResponseInfo.IsSuccess;
        }

		protected Response() { }

		public Response(BaseErrorResponseInfo responseInfo)
        {
            ResponseInfo = responseInfo;
        }

		public Response(SuccessResponseInfo responseInfo)
        {
            ResponseInfo = responseInfo;
        }
        
		#region Helper Object Factory

		public static Response GetSuccessResponse()
        {
            return new Response(new SuccessResponseInfo());
        }

        public static Response GetGenericAPIErrorResponse()
        {
            return new Response(new BaseAPIErrorResponseInfo(APIError.GenericError));
        }

        public static Response GetAPIErrorResponse(APIError error)
        {
            return new Response(new BaseAPIErrorResponseInfo(error));
        }

        public static Response GetGenericClientErrorResponse()
        {
            return new Response(new BaseClientErrorResponseInfo(ClientError.GenericError));
        }

        public static Response GetClientErrorResponse(ClientError clientError)
        {
            return new Response(new BaseClientErrorResponseInfo(clientError));
        }

        public static Response GetClientErrorResponse(String errorMessage)
        {
            return new Response(new BaseClientErrorResponseInfo(errorMessage));
        }

        #endregion
    }

	public class Response<T> : Response where T : class, new()
	{
		public T Result { get; set; }

		public Response(BaseResponseInfo responseInfo, T result)
		{
			ResponseInfo = responseInfo;
			Result = result;
		}

		public Response(BaseErrorResponseInfo responseInfo) : base(responseInfo) { }

		#region Helper Object Factory

		public static Response<T> GetSuccessResponse(T result)
		{
			return new Response<T>(new SuccessResponseInfo(), result);
		}

		public static new Response<T> GetGenericAPIErrorResponse()
		{
			return new Response<T>(new BaseAPIErrorResponseInfo(APIError.GenericError), null);
		}

        public static new Response<T> GetAPIErrorResponse(APIError error)
        {
            return new Response<T>(new BaseAPIErrorResponseInfo(error));
        }

		public static new Response<T> GetGenericClientErrorResponse()
		{
			return new Response<T>(new BaseClientErrorResponseInfo(ClientError.GenericError), null);
		}

        public static new Response<T> GetClientErrorResponse(ClientError clientError)
        {
            return new Response<T>(new BaseClientErrorResponseInfo(clientError), null);
        }

        #endregion

	}
    
	public abstract class BaseResponseInfo
	{
		public abstract bool IsSuccess { get; }
	}

	public class SuccessResponseInfo : BaseResponseInfo
    {
        public override bool IsSuccess => true;
    }

	public abstract class BaseErrorResponseInfo : BaseResponseInfo
	{
		
	}

	public class BaseAPIErrorResponseInfo : BaseErrorResponseInfo 
	{
		public override bool IsSuccess => false;

		public APIError Error { get; protected set; }

		public BaseAPIErrorResponseInfo(APIError error) : base()
		{
			this.Error = error;
		}
	}

	public class BaseClientErrorResponseInfo : BaseErrorResponseInfo
    {
		public override bool IsSuccess => false;

		public ClientError Error { get; protected set; }

        public String Message 
        {
            get;
            private set;
        }

		public BaseClientErrorResponseInfo(ClientError error) : base()
        {
            this.Error = error;
        }

        public BaseClientErrorResponseInfo(String errorMessage) : base()
        {
            this.Message = errorMessage;
        }
    }

	public class Empty
	{
		
	}
}
