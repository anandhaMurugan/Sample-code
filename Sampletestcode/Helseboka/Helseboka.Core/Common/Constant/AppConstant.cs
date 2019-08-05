using System;
using System.Text.RegularExpressions;

namespace Helseboka.Core.Common.Constant
{
	public static class AppConstant
    {
		public const String AppScheme = "helseboka://";
        public const String BankIDRedirectionUrl = AppScheme + "status=";
        public const String OkUrl = BankIDRedirectionUrl + "ok";
        public const String CancelUrl = BankIDRedirectionUrl + "cancel";
        public const String ErrorUrl = BankIDRedirectionUrl + "error";
        public const int apiVersion = 2;

        // Delay to start search after keystroke
        public const Double SearchDelay = 1000;

        public const String ReadMoreUrlFormat = "https://www.felleskatalogen.no/medisin/sok?sokord={0}";
        public const String ReadMoreUrl = "https://www.felleskatalogen.no";
        public const String TermsUrl = "https://frontend.helseboka.no/info/static-terms.html";

        public const int InactivityTimeOut = 600; // In seconds

        public const String FeedbackEmailAddress = "tilbakemelding@helseboka.no";

        public static readonly string[] VideoConfirmationInfos = { "video consultation", "videotime", "video time", "video konsultasjon", "videokonsultasjon", "videotelefon", "video telefon", "video samtale", "videosamtale" };

        public const String RegexUrlMatchPattern = @"(http|ftp|https):\/\/([\w\-_]+(?:(?:\.[\w\-_]+)+))([\w\-\.,@?^=%&amp;:/~\+#]*[\w\-\@?^=%&amp;/~\+#])?";

        public static bool Contains(this string source1, string source2, string toCheck, StringComparison comp)
        {
            return toCheck != null && ((!string.IsNullOrEmpty(source1) ? source1.IndexOf(toCheck, comp) >= 0 : false)  || (!string.IsNullOrEmpty(source2) ? source2.IndexOf(toCheck, comp) >= 0 : false)) ;
        }

        public static bool RegexIsMatch(this string source1, string source2, Regex regexPattern)
        {
            return (!string.IsNullOrEmpty(source1) ? regexPattern.IsMatch(source1) : false) || (!string.IsNullOrEmpty(source2) ? regexPattern.IsMatch(source2) : false);
        }
    }
}
