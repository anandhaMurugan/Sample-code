using System;
using System.Threading.Tasks;
using Helseboka.Core.Common.CommonImpl;
using Helseboka.Core.Common.EnumDefinitions;
using Helseboka.Core.Common.Model;
using Helseboka.Core.MedicineModule.Interface;
using Helseboka.Core.MedicineModule.Model;

namespace Helseboka.Core.MedicineModule.Model
{
    public class AlarmDetails : AlarmInfo
    {
        public MedicineInfo Medicine { get; protected set; }
        public ReminderInfo Reminder { get; protected set; }
        public bool IsNextAlarm { get; internal set; } = false;

        public AlarmDetails(AlarmInfo alarm, MedicineReminder medicineReminder)
        {
            this.Id = alarm.Id;
            this.Status = alarm.Status;
            this.Time = alarm.Time;
            this.Medicine = medicineReminder.Medicine;
            this.Reminder = medicineReminder.Reminder;
        }

        public async Task<Response> MarkAlarmAsComplete()
        {
            if (this.Status == AlarmStatus.Pending)
            {
                var dataHandler = ApplicationCore.Container.Resolve<IMedicineAPI>();
                var response = await dataHandler.MarkAlarmAsComplete(Id);
                if (response.IsSuccess)
                {
                    this.Status = AlarmStatus.Completed;
                }

                return response;
            }
            else
            {
                return Response.GetSuccessResponse();
            }
        }
    }
}
