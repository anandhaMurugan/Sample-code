using System;
using System.Collections.Generic;
using System.Text;

namespace Helseboka.Core.Models
{
    public class MedicineList
    {
        public List<Medicine> Medicines { get; set; }
    }

    public class Medicine
    {
        public string  id { get; set; }
        public string name { get; set; }
        public string nameFormStrength { get; set; }
        public string atcName { get; set; }
        public string atcCode { get; set; }
        public string form { get; set; }
        public object dangerousDriving { get; set; }
    }
}
