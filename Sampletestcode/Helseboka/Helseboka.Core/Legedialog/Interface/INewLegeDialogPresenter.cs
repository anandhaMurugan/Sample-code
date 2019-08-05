using System;
using System.Threading.Tasks;
using Helseboka.Core.Common.Interfaces;
using Helseboka.Core.Common.Model;

namespace Helseboka.Core.Legedialog.Interface
{
    public interface INewLegeDialogPresenter : IBasePresenter
    {
        void GoBack();

        Task<Response<Doctor>> GetDoctor();

        Task<Response> CreateThread(String subject, String body, bool isDoctorSelected);
    }
}
