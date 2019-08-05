using System;
using System.Threading.Tasks;
using Helseboka.Core.Common.CommonImpl;
using Helseboka.Core.Common.Constant;
using Helseboka.Core.Common.EnumDefinitions;
using Helseboka.Core.Common.Extension;
using Helseboka.Core.Common.Model;
using Helseboka.Core.Legedialog.Interface;

namespace Helseboka.Core.Legedialog.Model
{
    public class MessageThread
    {
        private IMessagingAPI DataHandler
        {
            get => ApplicationCore.Container.Resolve<IMessagingAPI>();
        }

        public int Id { get; set; }
        public String Title { get; set; }
        public DateTime Created { get; set; }
        public DateTime LastUpdated { get; set; }
        public String PartnerName { get; set; }
        public String PartnerType { get; set; }
        public int PartnerId { get; set; }
        public String Status { get; set; }

        public ChatStatus LastMessageStatus
        {
            get
            {
                if (Status == APIConstant.ChatStatusSent) return ChatStatus.Sent;
                else if (Status == APIConstant.ChatStatusDelivered) return ChatStatus.Delivered;
                else if (Status == APIConstant.ChatStatusReceived) return ChatStatus.Received;
                else return ChatStatus.Error;
            }
        }

        public Day GetDay()
        {
            return LastUpdated.GetDay();
        }

        public PartnerType TypeOfPartner
        {
            get
            {
                switch(PartnerType)
                {
                    case APIConstant.PartnerTypeDoctor: return Model.PartnerType.Doctor;
                    case APIConstant.PartnerTypeMedicalCenter: return Model.PartnerType.MedicalCenter;
                    default: return Model.PartnerType.Others;
                }
            }
        }

        public async Task<Response<PaginationResponse<ChatMessage>>> GetThreadMessages(int pageNum, int pageSize)
        {
            return await DataHandler.GetAllThreadMessages(Id, pageNum, pageSize);
        }

        public async Task<Response> SendMessage(String message)
        {
            var request = new SendMessageRequest() { text = message };
            return await DataHandler.SendMessage(Id, request);
        }

    }

    public enum PartnerType
    {
        Others,
        Doctor,
        MedicalCenter
    }
}
