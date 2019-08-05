using System;
using Helseboka.Core.Common.Interfaces;
using Helseboka.Core.Common.Model;

namespace Helseboka.Core.Common.Presenter
{
	public abstract class BasePresenter
    {
		protected IBaseRouter router;
		public event EventHandler<BaseErrorResponseInfo> ErrorOccured;
		public event EventHandler<EventArgs> LoadingStarted;
        public event EventHandler<EventArgs> LoadingCompleted;

		protected void Loading()
		{
			LoadingStarted?.Invoke(this, EventArgs.Empty);
		}

		protected void HideLoading()
        {
			LoadingCompleted?.Invoke(this, EventArgs.Empty);
        }

        protected void RaiseError(BaseResponseInfo error)
		{
            if (error is BaseErrorResponseInfo)
            {
                ErrorOccured?.Invoke(this, error as BaseErrorResponseInfo);
            }
		}

        public void ClearEventInvocationList()
        {
            if (ErrorOccured != null)
            {
                foreach (var subscribedDelegate in ErrorOccured.GetInvocationList())
                {
                    ErrorOccured -= (EventHandler<BaseErrorResponseInfo>)subscribedDelegate;
                }
            }

            if (LoadingStarted != null)
            {
                foreach (var subscribedDelegate in LoadingStarted.GetInvocationList())
                {
                    LoadingStarted -= (EventHandler<EventArgs>)subscribedDelegate;
                }
            }

            if (LoadingCompleted != null)
            {
                foreach (var subscribedDelegate in LoadingCompleted.GetInvocationList())
                {
                    LoadingCompleted -= (EventHandler<EventArgs>)subscribedDelegate;
                }
            }
        }
    }
}
