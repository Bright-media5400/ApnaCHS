using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApnaCHS.Entities
{
    public class Vehicle : EntityBase<long>
    {
        public Vehicle()
        {
            Approvals = new HashSet<DataApproval>();
            Comments = new HashSet<CommentVehicle>();
        }
        public string Name { get; set; }
        public string Make { get; set; }
        public string Number { get; set; }
        public byte? Type { get; set; }

        public long FlatOwnerId { get; set; }
        [ForeignKey("FlatOwnerId")]
        public virtual FlatOwner FlatOwner { get; set; }

        public long FlatId { get; set; }
        [ForeignKey("FlatId")]
        public virtual Flat Flat { get; set; }

        public virtual ICollection<DataApproval> Approvals { get; set; }
        public virtual ICollection<CommentVehicle> Comments { get; set; }
    }
}
