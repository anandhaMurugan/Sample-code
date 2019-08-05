using System;
using System.Collections.Generic;
using System.Text;
using Helseboka.Core.Common.Extension;

namespace Helseboka.Core.Common.Model
{
    public class Doctor
    {
        public String Id { get; set; }
        //public String DisplayName { get; set; }
        public String FirstName { get; set; }
        public String MiddleName { get; set; }
        public String LastName { get; set; }
        public String PhoneNumber { get; set; }
        public String EmergencyNumber { get; set; }
        public String Gender { get; set; }
        public String OfficeId { get; set; }
        public String OfficeName { get; set; }
        public String OfficeStreet { get; set; }
        public String OfficeZip { get; set; }
        public String OfficeCity { get; set; }
        public Boolean Enabled { get; set; }
        public String Remarks { get; set; }
        public String VideoUrl { get; set; }

        public String FullName
        {
            get
            {
                List<String> nameParts = new List<String>();

                if(!String.IsNullOrEmpty(FirstName))
                {
                    nameParts.Add(FirstName.ToNameCase());
                }

                if (!String.IsNullOrEmpty(MiddleName))
                {
                    nameParts.Add(MiddleName.ToNameCase());
                }

                if (!String.IsNullOrEmpty(LastName))
                {
                    nameParts.Add(LastName.ToNameCase());
                }

                return String.Join(" ", nameParts).Trim();
            }
        }
    }
}
