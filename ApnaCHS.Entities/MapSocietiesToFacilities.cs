using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApnaCHS.Entities
{
    public class MapSocietiesToFacilities 
    {
        [Key]
        [Column(Order = 0)]
        public long SocietyId { get; set; }
        [ForeignKey("SocietyId")]
        public virtual Society Society { get; set; }

        [Key]
        [Column(Order = 1)]
        public long FacilityId { get; set; }
        [ForeignKey("FacilityId")]
        public virtual Facility Facility { get; set; }

    }
}
