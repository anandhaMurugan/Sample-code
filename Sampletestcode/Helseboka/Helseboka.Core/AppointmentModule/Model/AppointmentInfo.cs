using System;
using System.Collections.Generic;
using Helseboka.Core.Common.EnumDefinitions;
using Helseboka.Core.Common.Model;

namespace Helseboka.Core.AppointmentModule.Model
{
    public class AppointmentInfo
    {
        public int DoctorId { get; set; }
        public List<DayOfWeek> RequestedDays { get; set; }
        public List<TimeOfDay> RequestedTimes { get; set; }
        public List<String> Topics { get; set; }
        public String Notes;

        public Doctor Doctor;
    }
}
