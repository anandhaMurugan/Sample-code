using System;
using Helseboka.Core.Common.Interfaces;
using Helseboka.Core.Common.Model;
using Helseboka.Core.Profile.Model;
using System.Threading.Tasks;

namespace Helseboka.Core.MobilephoneNumber.Interface
{
   public interface IMobilePhoneNumberPresenter : IBasePresenter
    {
        Task<User> GetCurrentUserProfile();

        Task<Response> UpdateMobilePhoneNumber(String mobile);

        void UpdateSuccess();
    }
}
