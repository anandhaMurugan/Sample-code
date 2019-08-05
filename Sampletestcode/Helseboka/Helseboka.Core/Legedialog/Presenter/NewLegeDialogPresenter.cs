using System;
using System.Threading.Tasks;
using Helseboka.Core.Common.Model;
using Helseboka.Core.Common.Presenter;
using Helseboka.Core.Legedialog.Interface;
using Helseboka.Core.Legedialog.Model;

namespace Helseboka.Core.Legedialog.Presenter
{
    public class NewLegeDialogPresenter : BasePresenter, INewLegeDialogPresenter
    {
        private IChatRouter Router { get => router as IChatRouter; }

        public NewLegeDialogPresenter(IChatRouter legedialogRouter)
        {
            router = legedialogRouter;
        }

        public void GoBack()
        {
            Router.GoBackToHome();
        }

        public async Task<Response<Doctor>> GetDoctor()
        {
            Loading();
            var response = await ApplicationCore.Instance.CurrentUser.GetDoctor();
            HideLoading();
            if (!response.IsSuccess)
            {
                RaiseError(response.ResponseInfo);
            }

            return response;
        }

        public async Task<Response> CreateThread(String subject, String body, bool isDoctorSelected)
        {
            return await new LegeDialogManager().CreateThread(subject, body, isDoctorSelected);
        }
    }
}
