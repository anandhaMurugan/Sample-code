using System;
using Helseboka.Core.Common.Model;

namespace Helseboka.Core.Common.Interfaces
{
    public interface IBasePresenter
    {
		event EventHandler<BaseErrorResponseInfo> ErrorOccured;

		event EventHandler<EventArgs> LoadingStarted;

		event EventHandler<EventArgs> LoadingCompleted;

        void ClearEventInvocationList();
    }
}
