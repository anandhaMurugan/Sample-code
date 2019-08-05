using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Helseboka.Core.MedicineModule.Model;

namespace Helseboka.Core.Common.Interfaces
{
    public interface INotificationService
    {
        Task CompletePendingTask();

        Task ScheduleLocalNotification(DayOfWeek day, String time, MedicineInfo medicine, DateTime startDate, DateTime? endDate = null);

        Task DeleteScheduledNotification(MedicineInfo medicine);

        Task ScheduleNotification(List<MedicineReminder> reminderList);
    }
}