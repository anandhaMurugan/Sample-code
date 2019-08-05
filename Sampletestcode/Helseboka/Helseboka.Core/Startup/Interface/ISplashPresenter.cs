using System;
using Helseboka.Core.Common.Interfaces;

namespace Helseboka.Core.Startup.Interface
{
	public interface ISplashPresenter : IBasePresenter
    {
        void DecideAndNavigateAppFlow();

        void StartRegistrationFlow();
    }
}
