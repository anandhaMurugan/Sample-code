using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Helseboka.Core.Common.Interfaces;
using Helseboka.Core.Common.Model;
using Helseboka.Core.Legedialog.Model;

namespace Helseboka.Core.Legedialog.Interface
{
    public interface IChatPresenter : IBasePresenter
    {
        MessageThread Thread { get; }
        bool HasMoreData { get; }

        Task<List<ChatMessage>> LoadMore();
        Task<List<ChatMessage>> RefreshMessage();
        Task<Response> SendMessage(String message);

        void GoBack();
    }
}