using System;
using Helseboka.Core.Common.Constant;
using Helseboka.Core.Common.EnumDefinitions;

namespace Helseboka.Core.Legedialog.Model
{
    public class ChatMessage
    {
        public int Id { get; set; }
        public String Direction { get; set; }
        public String Text { get; set; }
        public DateTime Created { get; set; }
        public int ThreadId { get; set; }
        public String Status { get; set; }

        public ChatStatus StatusOfChat
        {
            get
            {
                if (Status == APIConstant.ChatStatusSent) return ChatStatus.Sent;
                else if (Status == APIConstant.ChatStatusDelivered) return ChatStatus.Delivered;
                else if (Status == APIConstant.ChatStatusReceived) return ChatStatus.Received;
                else return ChatStatus.Error;
            }
        }

        public MessageDirection MessageDirection
        {
            get
            {
                if (Direction == APIConstant.SentMessageDirection)
                {
                    return MessageDirection.Sent;
                }
                else if(Direction == APIConstant.ReceivedMessageDirection)
                {
                    return MessageDirection.Received;
                }
                else
                {
                    return MessageDirection.Others;
                }
            }
        }

    }
}
