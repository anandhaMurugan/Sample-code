using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Helseboka.Core.Common.Model;
using Helseboka.Core.Common.Presenter;
using Helseboka.Core.Legedialog.Interface;
using Helseboka.Core.Legedialog.Model;

namespace Helseboka.Core.Legedialog.Presenter
{
    public class ChatPresenter : BasePresenter, IChatPresenter
    {
        private const int pageSize = 15;
        private int nextPageToLoad = 0;

        public MessageThread Thread { get; private set; }

        public bool HasMoreData { get; private set; }

        private IChatRouter Router { get => router as IChatRouter; }

        public ChatPresenter(IChatRouter legedialogRouter, MessageThread messageThread)
        {
            this.router = legedialogRouter;
            this.Thread = messageThread;
        }

        public async Task<List<ChatMessage>> RefreshMessage()
        {
            nextPageToLoad = 0;
            return await LoadMore();
        }

        public async Task<List<ChatMessage>> LoadMore()
        {
            var response = await Thread.GetThreadMessages(nextPageToLoad++, pageSize);
            if (response.IsSuccess && response.Result.Content != null)
            {
                HasMoreData = nextPageToLoad < response.Result.TotalPages;
                var resultList = response.Result.Content;
                resultList.Sort((x, y) => x.Created.CompareTo(y.Created));
                return resultList;
            }
            else
            {
                RaiseError(response.ResponseInfo);
                return new List<ChatMessage>();
            }
        }

        public async Task<Response> SendMessage(String message)
        {
            return await Thread.SendMessage(message);
        }

        public void GoBack()
        {
            Router.GoBackToHome();
        }
    }
}
