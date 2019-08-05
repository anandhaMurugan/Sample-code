using System;
using System.Threading.Tasks;
using Helseboka.Core.AppointmentModule.Interface;
using Helseboka.Core.AppointmentModule.Model;
using Helseboka.Core.Common.Constant;
using Helseboka.Core.Common.DataAccess;
using Helseboka.Core.Common.Model;

namespace Helseboka.Core.AppointmentModule.DataAccess
{
    public class AppointmentDataHandler : BaseDataHandler, IAppointmentAPI
    {
        public async Task<Response> AddAppointment(AppointmentInfo request)
        {
            var apiHandler = GetAPIhandlerForPost<AppointmentInfo, Empty>(APIEndPoints.AddAppointment, request);
            return await apiHandler.Execute();
        }

        public async Task<Response<PaginationResponse<AppointmentDetails>>> GetAllAppointments(int pageNum, int pageSize)
        {
            var url = String.Format(APIEndPoints.GetAllAppointments, pageNum, pageSize);
            var apiHandler = GetAPIhandlerForGet<PaginationResponse<AppointmentDetails>>(url);
            return await apiHandler.Execute();
        }

        public async Task<Response> CancelAppointment(String appointmentId)
        {
            var url = String.Format(APIEndPoints.CancelAppointment, appointmentId);
            var apiHandler = GetAPIhandlerForGet<Empty>(url);
            return await apiHandler.Execute();
        }

        public async Task<Response<AppointmentDetails>> GetNextAppointment()
        {
            var apiHandler = GetAPIhandlerForGet<AppointmentDetails>(APIEndPoints.NextAppointment);
            return await apiHandler.Execute();
        }
    }
}
