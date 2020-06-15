using ApnaCHS.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApnaCHS.Web.Models
{
    public class SecurityStaffViewModel
    {
        public int id { get; set; }
        public string name { get; set; }
        public string phoneNo { get; set; }
        public string aadharCardNo { get; set; }
        public string photo { get; set; }
        public DateTime? dateOfBirth { get; set; }
        public string address { get; set; }
        public string nativeAddress { get; set; }
        public DateTime joiningDate { get; set; }
        public DateTime? lastWorkingDay { get; set; }
        public SocietyTrimViewModel society { get; set; }
        public MasterValueViewModel shiftType { get; set; }
    }
}