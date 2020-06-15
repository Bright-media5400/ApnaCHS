using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApnaCHS.Entities
{
    public class Attendance : EntityBase<long>
    {
        public DateTime Day { get; set; }
        public DateTime InTime { get; set; }
        public DateTime? OutTime { get; set; }

        public long? SecurityStaffId { get; set; }

        [ForeignKey("SecurityStaffId")]
        public virtual SecurityStaff SecurityStaff { get; set; }

        public long? SocietyStaffId { get; set; }

        [ForeignKey("SocietyStaffId")]
        public virtual SocietyStaff SocietyStaff { get; set; }

        public int ShiftTypeId { get; set; }

        [ForeignKey("ShiftTypeId")]
        public virtual MasterValue ShiftType { get; set; }
    }
}
