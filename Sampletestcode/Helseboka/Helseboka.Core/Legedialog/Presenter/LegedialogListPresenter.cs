using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Helseboka.Core.Common.Model;
using Helseboka.Core.Common.Presenter;
using Helseboka.Core.Legedialog.Interface;
using Helseboka.Core.Legedialog.Model;

namespace Helseboka.Core.Legedialog.Presenter
{
	public class LegedialogListPresenter : BasePresenter, ILegedialogListPresenter
    {
        private LegeDialogManager legedialog = new LegeDialogManager();
        private const int pageSize = 15;
        private int nextPageToLoad = 0;

        public bool HasMoreData { get; private set; }

        private IChatRouter Router { get => router as IChatRouter; }    

		public LegedialogListPresenter(IChatRouter legedialogRouter)
        {
			this.router = legedialogRouter;
        }

        public async Task<List<MessageThread>> GetThreads()
        {
            nextPageToLoad = 0;
            return await LoadMore();
        }

        public async Task<List<MessageThread>> LoadMore()
        {
            var response = await legedialog.GetAllThreads(nextPageToLoad++, pageSize);
            if (response.IsSuccess && response.Result.Content != null)
            {
                HasMoreData = nextPageToLoad < response.Result.TotalPages;
                var dataList = response.Result.Content;
                dataList.Sort((x, y) => y.LastUpdated.CompareTo(x.LastUpdated));
                return dataList;
            }
            else
            {
                RaiseError(response.ResponseInfo);
                return new List<MessageThread>();
            }
        }

        public void DidSelectThread(MessageThread messageThread)
        {
            Router.NavigateToChat(messageThread);
        }

        public async Task<Response> DidTapOnNewDialog()
        {
            if (ApplicationCore.Instance.CurrentUser.HasDoctor())
            {
                var response = await ApplicationCore.Instance.CurrentUser.GetDoctor();
                if (response.IsSuccess)
                {
                    Router.NavigateToNewDialogView();
                }

                return response;
            }
            else
            {
                throw new NotImplementedException();
            }
        }
    }
}
