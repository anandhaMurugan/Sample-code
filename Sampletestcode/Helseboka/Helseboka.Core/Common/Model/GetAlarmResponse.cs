using System;
using System.Collections.Generic;
using Helseboka.Core.MedicineModule.Model;

namespace Helseboka.Core.Common.Model
{
    public class GetAlarmResponse
    {
        public MedicineReminder Context { get; set; }
        public List<AlarmInfo> Alarms { get; set; }
    }
}
