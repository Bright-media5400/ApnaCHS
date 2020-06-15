using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApnaCHS.Entities
{
    public class CommentMC : EntityBase<int>
    {
        public string Text { get; set; }

        public string CommentBy { get; set; }

        [ForeignKey("MaintenanceCost")]
        public long MaintenanceCostId { get; set; }
        public virtual MaintenanceCost MaintenanceCost { get; set; }
    }
}
