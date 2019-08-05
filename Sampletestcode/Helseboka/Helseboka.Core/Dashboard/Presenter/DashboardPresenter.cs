using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Helseboka.Core.AppointmentModule.Model;
using Helseboka.Core.Common.Model;
using Helseboka.Core.Common.Presenter;
using Helseboka.Core.Dashboard.Interface;
using Helseboka.Core.MedicineModule.Model;

namespace Helseboka.Core.Dashboard.Presenter
{
    public class DashboardPresenter : BasePresenter, IDashboardPresenter
    {
        public async Task<AppointmentDetails> GetNextAppointmentDetails()
        {
            if (ApplicationCore.Instance.CurrentUser != null)
            {
                var response = await ApplicationCore.Instance.CurrentUser.GetNextAppointment();
                if (response.IsSuccess)
                {
                    return response.Result;
                }
                else
                {
                    RaiseError(response.ResponseInfo);
                }
            }

            return null;
        }

        public async Task<List<AlarmDetails>> GetAlarms(DateTime date)
        {
            if (ApplicationCore.Instance.CurrentUser != null)
            {
                var response = await ApplicationCore.Instance.CurrentUser.GetAlarms(date);
                if (response.IsSuccess)
                {
                    return response.Result;
                }
                else
                {
                    RaiseError(response.ResponseInfo);
                }
            }

            return new List<AlarmDetails>();
        }

        public async Task<Response> MarkAlarmAsComplete(AlarmDetails alarm)
        {
            if (alarm != null)
            {
                Loading();
                var response = await alarm.MarkAlarmAsComplete();
                HideLoading();
                if (!response.IsSuccess)
                {
                    RaiseError(response.ResponseInfo);
                }
                return response;
            }
            else
            {
                return Response.GetGenericClientErrorResponse();
            }
        }
    }
}
