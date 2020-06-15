using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApnaCHS.Entities
{
    public class CommentFlatOwnerFamily : EntityBase<int>
    {
        public string Text { get; set; }

        public string CommentBy { get; set; }

        [ForeignKey("FlatOwnerFamily")]
        public long FlatOwnerFamilyId { get; set; }
        public virtual FlatOwnerFamily FlatOwnerFamily { get; set; }
    }
}
