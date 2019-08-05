using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Helseboka.Core.Common.Interfaces;
using Helseboka.Core.Common.Model;
using Helseboka.Core.Profile.Model;

namespace Helseboka.Core.Startup.Interface
{
	public interface ISignupPresenter : IBasePresenter
    {
		void RegisterBiometric();

		void ShowPINRegistration();

        void CancelPINBioRegister();
    }

    public interface IPINConfirmation : IBasePresenter
    {
        void PINSelectionCompleted(String pin);
    }

    public interface IDoctorSelectionPresenter : IBasePresenter
    {
        bool HasMoreData { get; }

        Task<Response<User>> GetCurrentUser();

        Task<List<Doctor>> SearchDoctor(String searchText);

        Task<List<Doctor>> LoadMore();

        Task SelectDoctor(Doctor doctor);

        void Cancel();
    }
}
