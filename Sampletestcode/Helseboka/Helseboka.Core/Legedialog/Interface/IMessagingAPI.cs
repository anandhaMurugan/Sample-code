using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Helseboka.Core.Common.Model;
using Helseboka.Core.Legedialog.Model;

namespace Helseboka.Core.Legedialog.Interface
{
    public interface IMessagingAPI
    {
        Task<Response<PaginationResponse<MessageThread>>> GetAllThreads(int pageNum, int pageSize);

        Task<Response<PaginationResponse<ChatMessage>>> GetAllThreadMessages(int threadId, int pageNum, int pageSize);

        Task<Response> SendMessage(int threadId, SendMessageRequest request);

        Task<Response> CreateThread(CreateThreadRequest request);
    }
}
