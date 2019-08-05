using System;
using Helseboka.Core.Common.EnumDefinitions;

namespace Helseboka.Core.MedicineModule.Model
{
    public class AlarmInfo
    {
        public int Id { get; set; }
        public AlarmStatus Status { get; set; }
        public DateTime Time { get; set; }
    }
}
