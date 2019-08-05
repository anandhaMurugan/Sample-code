using System;
namespace Helseboka.Core.Common.Constant
{
	public static class APIConstant
    {
		// const vs static readoly decision
		// 1. Whenever core assembly changes, app needs to recompile any way. Thus there is no difference between runtime intialization and compile time initialization.
		// 2. Major decision point is 1. Now because of using const we get micro (probably nano) performance advantage.

		public const String ContentTypeHeaderKey = "Content-Type";
		public const String ContentTypeHeaderValue = "application/json; charset=utf-8";

		public const String AcceptHeaderKey = "Accept";
		public const String AcceptHeaderValue = "application/json";

        public const String AcceptLanguageHeaderKey = "Accept-Language";

        public const String AuthorizationHeaderKey = "Authorization";
		public const String AuthorizationHeaderValueFormat = "Basic {0}";

		public const String APIKeyHeaderKey = "X-API-KEY";
		public const String SessionIDHeaderKey = "sessionid";
        public const String AppVersionHeaderKey = "X-APP-VERSION";

        public const String PartnerTypeDoctor = "DOCTOR";
        public const String PartnerTypeMedicalCenter = "OFFICE";

        public const String ChatTypeDoctor = "PRIVATE_CHAT";
        public const String ChatTypeOffice = "GENERAL_REQUEST";

        public const String SentMessageDirection = "SENT";
        public const String ReceivedMessageDirection = "RECEIVED";

        public const String ChatStatusSent = "SENT";
        public const String ChatStatusDelivered = "DELIVERED";
        public const String ChatStatusError = "ERROR";
        public const String ChatStatusReceived = "RECEIVED";

        public const int HTTPTimeout = 900;

        public const String EmptyResponse = "{}";
    }
}
