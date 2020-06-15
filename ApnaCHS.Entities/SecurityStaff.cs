using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApnaCHS.Entities
{
    public class SecurityStaff : EntityBase<long>
    {
        public string Name { get; set; }
        public string PhoneNo { get; set; }
        public string AadharCardNo { get; set; }
        public string Photo { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Address { get; set; }
        public string NativeAddress { get; set; }
        public DateTime JoiningDate { get; set; }
        public DateTime? LastWorkingDay { get; set; }
        public long SocietyId { get; set; }

        [ForeignKey("SocietyId")]
        public virtual Society Society { get; set; }

        public int ShiftTypeId { get; set; }

        [ForeignKey("ShiftTypeId")]
        public virtual MasterValue ShiftType { get; set; }

        public virtual ICollection<Attendance> AttendanceList { get; set; }
        
    }
}
