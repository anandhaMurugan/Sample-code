using System;
using System.Threading;
using System.Threading.Tasks;
using Helseboka.Core.Auth.Model;
using Helseboka.Core.Common.Interfaces;
using Helseboka.Core.Common.Model;
using Helseboka.Core.Common.Presenter;
using Helseboka.Core.Startup.Interface;

namespace Helseboka.Core.Startup.Presenter
{
	public class SplashPresenter : BasePresenter, ISplashPresenter
    {
		private IStartupRouter Router { get => router as IStartupRouter; }

		public SplashPresenter(IStartupRouter startupRouter)
        {
			router = startupRouter;
        }
        
		public void DecideAndNavigateAppFlow()
		{
            if (AuthService.Instance.IsFirstTimeUser())
            {
                Router.ShowOnboardingView();
            }
            else
            {
                if (AuthService.Instance.CanLoginWithBio())
                {
                    Router.StartBioLogin();
                }
                else if (AuthService.Instance.CanLoginWithPIN())
                {
                    Router.StartPINLogin();
                }
                else
                {
                    StartRegistrationFlow();
                }
            }
		}

        public void StartRegistrationFlow()
        {
            Router.StartRegistrationFlow();
        }
	}
}
