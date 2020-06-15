using ApnaCHS.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApnaCHS.Web.Models
{
    public class SocietyViewModel
    {
        public int id { get; set; }
        public string name { get; set; }
        public string registrationNo { get; set; }
        public DateTime? dateOfIncorporation { get; set; }
        public DateTime? dateOfRegistration { get; set; }

        public string email { get; set; }
        public string phoneNo { get; set; }
        public string contactPerson { get; set; }
        public int billingCycle { get; set; }
        public int dueDays { get; set; }
        public long? second2Wheeler { get; set; }
        public long? second4Wheeler { get; set; }
        public decimal? interestPercent { get; set; }
        public int approvalsCount { get; set; }
        public bool openingInterest { get; set; }
        public ComplexTrimViewModel complex { get; set; }

        public SocietyAssetTrimViewModel[] societyAssets { get; set; }
    }

    public class SocietyTrimViewModel
    {
        public int id { get; set; }
        public string name { get; set; }
        public string registrationNo { get; set; }
        public DateTime? dateOfIncorporation { get; set; }
        public DateTime? dateOfRegistration { get; set; }

        public string email { get; set; }
        public string phoneNo { get; set; }
        public string contactPerson { get; set; }
        public DateTime billingCycle { get; set; }
        public int dueDays { get; set; }
        public long? second2Wheeler { get; set; }
        public long? second4Wheeler { get; set; }
        public decimal? interestPercent { get; set; }
        public int approvalsCount { get; set; }
        public bool openingInterest { get; set; }
    }

    public class SocietyImportResultViewModel
    {
        public long id { get; set; }
        public string result { get; set; }
        public bool isSuccess { get; set; }
    }
}