using System;
using System.Collections.Generic;

namespace Helseboka.Core.MedicineModule.Model
{
    public class RenewPrescriptionRequest
    {
        public List<int> MedicineIds { get; set; }
        public int DoctorId { get; set; }
        public String Notes { get; set; }
    }
}
