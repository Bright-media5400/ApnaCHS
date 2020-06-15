using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApnaCHS.Entities
{
    public class Facility : EntityBase<long>
    {
        public string Name { get; set; }

        public string Wing { get; set; }

        public int NoOfFloors { get; set; }

        public int NoOfFlats { get; set; }
        
        public int NoOfLifts { get; set; }

        public int NoOfParkinglots { get; set; }
        
        public bool IsParkingLot { get; set; }

        public long ComplexId { get; set; }
        [ForeignKey("ComplexId")]
        public virtual Complex Complex { get; set; }

        public byte Type { get; set; }

        public virtual ICollection<MapSocietiesToFacilities> Societies { get; set; }

        public virtual ICollection<Floor> Floors { get; set; }

        public virtual ICollection<FlatParking> FlatParkings { get; set; }

        public virtual ICollection<SocietyAsset> SocietyAssets { get; set; }
    }
}
