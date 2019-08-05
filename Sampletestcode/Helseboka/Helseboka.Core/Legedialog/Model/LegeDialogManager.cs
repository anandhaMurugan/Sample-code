using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Helseboka.Core.Common.CommonImpl;
using Helseboka.Core.Common.Constant;
using Helseboka.Core.Common.Model;
using Helseboka.Core.Legedialog.Interface;

namespace Helseboka.Core.Legedialog.Model
{
    public class LegeDialogManager
    {
        private IMessagingAPI DataHandler 
        {
            get => ApplicationCore.Container.Resolve<IMessagingAPI>();
        }

        public async Task<Response<PaginationResponse<MessageThread>>> GetAllThreads(int pageNum, int pageSize)
        {
            return await DataHandler.GetAllThreads(pageNum, pageSize);
        }

        public async Task<Response> CreateThread(String subject, String body, bool isDoctorSelected)
        {
            var doctor = ApplicationCore.Instance.CurrentUser.AssignedDoctor;
            var request = new CreateThreadRequest();
            request.title = subject;
            request.text = body;
            request.doctorId = doctor.Id;
            if (doctor != null)
            {
                if (isDoctorSelected)
                {
                    request.chatType = APIConstant.ChatTypeDoctor;
                }
                else
                {
                    request.chatType = APIConstant.ChatTypeOffice;
                    request.officeId = doctor.OfficeId;
                }
            }

            return await DataHandler.CreateThread(request);
        }
    }
}
