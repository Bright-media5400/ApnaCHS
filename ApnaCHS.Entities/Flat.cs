using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApnaCHS.Entities
{
    public class Flat : EntityBase<long>
    {
        public Flat()
        {
            Approvals = new HashSet<DataApproval>();
            Comments = new HashSet<CommentFlat>();
        }

        public string Name { get; set; }
        public int TotalArea { get; set; }
        public int CarpetArea { get; set; }
        public int? BuildUpArea { get; set; }
        public bool HaveParking { get; set; }
        public bool IsRented { get; set; }
        public bool IsCommercialSpace { get; set; }
        public long FloorId { get; set; }
        [ForeignKey("FloorId")]
        public virtual Floor Floor { get; set; }

        public int? FlatTypeId { get; set; }
        [ForeignKey("FlatTypeId")]
        public virtual MasterValue FlatType { get; set; }

        public virtual ICollection<MaintenanceCost> MaintenanceCosts { get; set; }
        public virtual ICollection<MapFlatToFlatOwner> FlatOwners { get; set; }
        public virtual ICollection<FlatParking> FlatParkings { get; set; }
        public virtual ICollection<Vehicle> Vehicles { get; set; }
        public virtual ICollection<DataApproval> Approvals { get; set; }
        public virtual ICollection<CommentFlat> Comments { get; set; }

    }
}
