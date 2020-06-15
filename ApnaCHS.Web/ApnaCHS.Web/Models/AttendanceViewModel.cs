using ApnaCHS.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApnaCHS.Web.Models
{
    public class AttendanceViewModel
    {
        public int id { get; set; }
        public DateTime day { get; set; }
        public DateTime inTime { get; set; }
        public DateTime? outTime { get; set; }

        public SecurityStaffViewModel securityStaff { get; set; }
        public SocietyStaffViewModel societyStaff { get; set; }
        public MasterValueViewModel shiftType { get; set; }
    }
}