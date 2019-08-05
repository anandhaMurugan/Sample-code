using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Helseboka.Core.Common.CommonImpl;
using Helseboka.Core.Common.Constant;
using Helseboka.Core.Common.DataAccess;
using Helseboka.Core.Common.Interfaces;
using Helseboka.Core.Common.Model;
using Helseboka.Core.Legedialog.Interface;
using Helseboka.Core.Legedialog.Model;

namespace Helseboka.Core.Legedialog.DataAccess
{
	public class MessagingDataHandler : BaseDataHandler, IMessagingAPI
	{
        public async Task<Response<PaginationResponse<MessageThread>>> GetAllThreads(int pageNum, int pageSize)
        {
            var url = String.Format(APIEndPoints.GetAllThreads, pageNum, pageSize);
            var apiHandler = GetAPIhandlerForGet<PaginationResponse<MessageThread>>(url);
            return await apiHandler.Execute();
        }

        public async Task<Response<PaginationResponse<ChatMessage>>> GetAllThreadMessages(int threadId, int pageNum, int pageSize)
        {
            var url = String.Format(APIEndPoints.GetMessagesForAThread,threadId, pageNum, pageSize);
            var apiHandler = GetAPIhandlerForGet<PaginationResponse<ChatMessage>>(url);
            return await apiHandler.Execute();
        }

        public async Task<Response> SendMessage(int threadId, SendMessageRequest request)
        {
            var url = String.Format(APIEndPoints.SendMessageToAThread, threadId);
            var apiHandler = GetAPIhandlerForPost<SendMessageRequest, Empty>(url, request);
            return await apiHandler.Execute();
        }

        public async Task<Response> CreateThread(CreateThreadRequest request)
        {
            var apiHandler = GetAPIhandlerForPost<CreateThreadRequest, Empty>(APIEndPoints.CreateNewThread, request);
            return await apiHandler.Execute();
        }
    }
}
