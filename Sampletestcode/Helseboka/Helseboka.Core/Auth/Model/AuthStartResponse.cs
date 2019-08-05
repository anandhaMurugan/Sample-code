using System;

namespace Helseboka.Core.Auth.Model
{
    public class AuthStartResponse
    {
        public string SID { get; set; }

		public string URI { get; set; }

        public String Token { get; set; }

		public AuthStartResponse()
		{

		}
	}
}
