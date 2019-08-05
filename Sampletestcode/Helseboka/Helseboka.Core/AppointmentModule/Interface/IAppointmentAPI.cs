using System;
using System.Threading.Tasks;
using Helseboka.Core.AppointmentModule.Model;
using Helseboka.Core.Common.Model;

namespace Helseboka.Core.AppointmentModule.Interface
{
    public interface IAppointmentAPI
    {
        Task<Response<PaginationResponse<AppointmentDetails>>> GetAllAppointments(int pageNum, int pageSize);

        Task<Response> AddAppointment(AppointmentInfo appointment);

        Task<Response> CancelAppointment(String appointmentId);

        Task<Response<AppointmentDetails>> GetNextAppointment();
    }
}
