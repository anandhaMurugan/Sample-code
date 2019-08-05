using System;
using System.Collections.Generic;

namespace Helseboka.Core.Common.Model
{
    public class MedicalCenter
    {
        public String Id { get; set; }
        public String Name { get; set; }
        public String Street { get; set; }
        public String Zip { get; set; }
        public String City { get; set; }
        public List<Doctor> Doctors { get; set; }
    }
}
