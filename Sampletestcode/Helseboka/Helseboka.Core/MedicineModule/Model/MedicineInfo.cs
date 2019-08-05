using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Helseboka.Core.Common.CommonImpl;
using Helseboka.Core.Common.Model;
using Helseboka.Core.Common.Extension;
using Helseboka.Core.MedicineModule.Interface;
using Helseboka.Core.Common.Interfaces;

namespace Helseboka.Core.MedicineModule.Model
{
    public class MedicineInfo : IEquatable<MedicineInfo>, ICloneable<MedicineInfo>
    {
        public int Id { get; set; }
        public String Name { get; set; }
        public String NameFormStrength { get; set; }
        public String AtcName { get; set; }
        public String AtcCode { get; set; }
        public String Form { get; set; }
        public bool DangerousDriving { get; set; }

        public String Strength
        {
            get => NameFormStrength.Replace(Name, String.Empty).Trim();
        }

        private IMedicineAPI DataHandler
        {
            get => ApplicationCore.Container.Resolve<IMedicineAPI>();
        }

        public async Task<Response> AddToProfile()
        {
            var request = new AddMedicineRequest();
            request.MedicineList = new List<int>() { Id };

            return await DataHandler.AddMedicineOrReminder(request);
        }

        public async Task<Response> AddReminder(List<DayOfWeek> days, List<String> frequencies, DateTime startDate, DateTime? endDate = null, List<MedicineInfo> medicineList = null)
        {
            var request = new AddMedicineRequest();
            request.Reminder = new ReminderRequest();

            request.MedicineList = new List<int> { Id };
            if (medicineList != null)
            {
                request.MedicineList.AddRange(medicineList.Select(x => x.Id).ToList());
            }

            request.Reminder.StartDate = startDate;
            request.Reminder.EndDate = endDate;
            request.Reminder.FrequencyPerDay = frequencies;
            request.Reminder.Days = days;

            var response = await DataHandler.AddMedicineOrReminder(request);

            if (response.IsSuccess)
            {
                var notificationService = ApplicationCore.Container.Resolve<INotificationService>();
                foreach (var day in days)
                {
                    foreach (var time in frequencies)
                    {
                        notificationService.ScheduleLocalNotification(day, time, this, startDate, endDate).Forget();
                    }
                }
            }

            return response;
        }

        public bool Equals(MedicineInfo other)
        {
            if (other != null)
            {
                return Id == other.Id;
            }

            return false;
        }

        public MedicineInfo Clone()
        {
            var result = new MedicineInfo();

            result.Id = this.Id;
            result.Name = this.Name;
            result.NameFormStrength = this.NameFormStrength;
            result.AtcName = this.AtcName;
            result.AtcCode = this.AtcCode;
            result.Form = this.Form;
            result.DangerousDriving = this.DangerousDriving;

            return result;
        }
    }
}
