using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApnaCHS.Entities
{
    public class FlatParking : EntityBase<long>
    {
        public string Name { get; set; }
        public int ParkingNo { get; set; }
        public int SqftArea { get; set; }
        public byte? Type { get; set; }        
        public long? FacilityId { get; set; }

        [ForeignKey("FacilityId")]
        public virtual Facility Facility { get; set; }

        public long? FloorId { get; set; }

        [ForeignKey("FloorId")]
        public virtual Floor Floor { get; set; }

        public long? FlatId { get; set; }

        [ForeignKey("FlatId")]
        public virtual Flat Flat { get; set; }

    }
}
