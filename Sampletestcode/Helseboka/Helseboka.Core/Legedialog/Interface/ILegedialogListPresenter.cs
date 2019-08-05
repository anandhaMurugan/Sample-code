using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Helseboka.Core.Common.Interfaces;
using Helseboka.Core.Common.Model;
using Helseboka.Core.Legedialog.Model;

namespace Helseboka.Core.Legedialog.Interface
{
	public interface ILegedialogListPresenter : IBasePresenter
    {
        bool HasMoreData { get; }

        Task<List<MessageThread>> GetThreads();

        Task<List<MessageThread>> LoadMore();

        void DidSelectThread(MessageThread messageThread);

        Task<Response> DidTapOnNewDialog();
    }
}
