using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApnaCHS.Entities
{
    public class DataApproval
    {
        [Key]
        [Column(Order = 0)]
        public long Id { get; set; }

        public long? FlatId { get; set; }
        [ForeignKey("FlatId")]
        public virtual Flat Flat { get; set; }

        public long? FlatOwnerId { get; set; }
        [ForeignKey("FlatOwnerId")]
        public virtual FlatOwner FlatOwner { get; set; }

        public long? MaintenanceCostId { get; set; }
        [ForeignKey("MaintenanceCostId")]
        public virtual MaintenanceCost MaintenanceCost { get; set; }

        public long? FlatOwnerFamilyId { get; set; }
        [ForeignKey("FlatOwnerFamilyId")]
        public virtual FlatOwnerFamily FlatOwnerFamily { get; set; }

        public long? VehicleId { get; set; }
        [ForeignKey("VehicleId")]
        public virtual Vehicle Vehicle { get; set; }

        public DateTime? ApprovedDate { get; set; }

        public string ApprovedBy { get; set; }

        public string ApprovedName { get; set; }

        public string Note { get; set; }

        public string ApprovedData { get; set; }
    }
}
