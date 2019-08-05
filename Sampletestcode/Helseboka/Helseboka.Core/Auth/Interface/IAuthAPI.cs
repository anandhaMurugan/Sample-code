using System;
using System.Threading.Tasks;
using Helseboka.Core.Auth.Model;
using Helseboka.Core.Common.Model;

namespace Helseboka.Core.Auth.Interface
{
	public interface IAuthAPI
    {
		Task<Response<AuthStartResponse>> AuthStart(String bidType);

        Task<Response<AuthenticateResponse>> Authenticate(String sessionID, String token);
    }
}
