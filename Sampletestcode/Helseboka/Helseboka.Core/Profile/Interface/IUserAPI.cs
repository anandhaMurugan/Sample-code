using System;
using System.Threading.Tasks;
using Helseboka.Core.Common.Model;
using Helseboka.Core.Profile.Model;

namespace Helseboka.Core.Profile.Interface
{
    public interface IUserAPI
    {
        Task<Response<User>> GetUserInfo();

        Task<Response> UpdateUserInfo(User user);

        Task<Response> UpdateNotificationToken(AddPushNotificationTokenRequest request);

        Task<Response> DeleteProfile();
    }
}
