using System;
using System.Threading.Tasks;
using Helseboka.Core.Auth.Interface;
using Helseboka.Core.Auth.Model;
using Helseboka.Core.Common.Constant;
using Helseboka.Core.Common.DataAccess;
using Helseboka.Core.Common.Model;

namespace Helseboka.Core.Auth.DataAccess
{
	public class AuthDataHandler : BaseDataHandler, IAuthAPI
	{
		public async Task<Response<AuthStartResponse>> AuthStart(string bidType)
		{
			var url = String.Format(APIEndPoints.AuthStart, bidType, AppConstant.OkUrl, AppConstant.CancelUrl, AppConstant.ErrorUrl, AppConstant.apiVersion);
			var apiHandler = GetAPIhandlerForGet<AuthStartResponse>(url);
			return await apiHandler.Execute();
		}

        public async Task<Response<AuthenticateResponse>> Authenticate(string sessionID, String token)
        {
            var url = String.Format(APIEndPoints.Authenticate, sessionID, token);
            var apiHandler = GetAPIhandlerForGet<AuthenticateResponse>(url);
            return await apiHandler.Execute();
        }
	}
}
