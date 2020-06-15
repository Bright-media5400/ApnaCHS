using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApnaCHS.Entities
{
    public class MaintenanceCost : EntityBase<long>
    {
        public MaintenanceCost()
        {
            Flats = new HashSet<Flat>();
            Approvals = new HashSet<DataApproval>();
            Comments = new HashSet<CommentMC>();
        }

        public long DefinitionId { get; set; }
        [ForeignKey("DefinitionId")]
        public virtual MaintenanceCostDefinition Definition { get; set; }

        public int Amount { get; set; }
        public DateTime Date { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }

        public decimal? PerSqrArea { get; set; }
        public bool AllFlats { get; set; }

        public long SocietyId { get; set; }

        [ForeignKey("SocietyId")]
        public virtual Society Society { get; set; }

        public virtual ICollection<Flat> Flats { get; set; }

        public virtual ICollection<DataApproval> Approvals { get; set; }

        public virtual ICollection<CommentMC> Comments { get; set; }
    }
}
