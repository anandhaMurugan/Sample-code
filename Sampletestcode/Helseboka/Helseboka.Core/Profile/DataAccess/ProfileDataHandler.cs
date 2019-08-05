using System;
using System.Threading.Tasks;
using Helseboka.Core.Common.Constant;
using Helseboka.Core.Common.DataAccess;
using Helseboka.Core.Common.Interfaces;
using Helseboka.Core.Common.Model;
using Helseboka.Core.Profile.Interface;
using Helseboka.Core.Profile.Model;

namespace Helseboka.Core.Profile.DataAccess
{
    public class ProfileDataHandler : BaseDataHandler, IUserAPI
    {
        public async Task<Response<User>> GetUserInfo()
        {
            var apiHandler = GetAPIhandlerForGet<User>(APIEndPoints.UserInfo);
            return await apiHandler.Execute();
        }

        public async Task<Response> UpdateUserInfo(User user)
        {
            var apiHandler = GetAPIhandlerForPut<User, Empty>(APIEndPoints.UserInfo, user);
            return await apiHandler.Execute();
        }

        public async Task<Response> UpdateNotificationToken(AddPushNotificationTokenRequest request)
        {
            var apiHandler = GetAPIhandlerForPost<AddPushNotificationTokenRequest, Empty>(APIEndPoints.UpdateNotificationToken, request);
            return await apiHandler.Execute();
        }

        public async Task<Response> DeleteProfile()
        {
            var apiHandler = GetAPIhandlerForDelete<Empty>(APIEndPoints.DeleteProfile);
            return await apiHandler.Execute();
        }
    }
}
