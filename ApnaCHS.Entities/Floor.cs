using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApnaCHS.Entities
{
    public class Floor : EntityBase<long>
    {
        public Floor()
        {
            Flats = new HashSet<Flat>();
        }

        public string Name { get; set; }

        public long FacilityId { get; set; }
        [ForeignKey("FacilityId")]
        public virtual Facility Facility { get; set; }

        public byte Type { get; set; }

        public int FloorNumber { get; set; }

        public virtual ICollection<Flat> Flats { get; set; }

        public virtual ICollection<FlatParking> FlatParkings { get; set; }

        public virtual ICollection<SocietyAsset> SocietyAssets { get; set; }

    }
}
