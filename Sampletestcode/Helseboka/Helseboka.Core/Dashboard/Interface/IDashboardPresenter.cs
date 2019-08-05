using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Helseboka.Core.AppointmentModule.Model;
using Helseboka.Core.Common.Interfaces;
using Helseboka.Core.Common.Model;
using Helseboka.Core.MedicineModule.Model;

namespace Helseboka.Core.Dashboard.Interface
{
    public interface IDashboardPresenter : IBasePresenter
    {
        Task<AppointmentDetails> GetNextAppointmentDetails();

        Task<List<AlarmDetails>> GetAlarms(DateTime date);

        Task<Response> MarkAlarmAsComplete(AlarmDetails alarm);
    }
}
