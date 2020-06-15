using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApnaCHS.Web.Models
{
    public class UploadFlatOwnerViewModel
    {
        public int srno { get; set; }
        public string registrationNo { get; set; }
        public string society { get; set; }
        public string building { get; set; }
        public string wing { get; set; }
        public string floor { get; set; }
        public string flat { get; set; }
        public string name { get; set; }
        public string mobileNo { get; set; }
        public string emailId { get; set; }
        public string gender { get; set; }
        public string aadhaarCardNo { get; set; }
        public DateTime? dateOfBirth { get; set; }
        public DateTime? memberSinceDate { get; set; }

        public DateTime? memberTillDate { get; set; }
        public bool isSuccess { get; set; }
        public string message { get; set; }

    }
}