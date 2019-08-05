using System;
using System.Collections.Generic;

namespace Helseboka.Core.MedicineModule.Model
{
    public class AddMedicineRequest
    {
        public List<int> MedicineList { get; set; }
        public ReminderRequest Reminder { get; set; }
    }

    public class ReminderRequest
    {
        //public int Id { get; set; }
        //public String AlarmSound { get; set; }
        //public int ServingsPerIntakeInterval { get; set; }
        //public String Status { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        //public bool ShowMedicineNameInNotification { get; set; }
        public List<String> FrequencyPerDay { get; set; }
        public List<DayOfWeek> Days { get; set; }
    }
}
