using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApnaCHS.Entities
{
    public class CommentFlatOwner : EntityBase<int>
    {
        public string Text { get; set; }

        public string CommentBy { get; set; }

        [ForeignKey("FlatOwner")]
        public long FlatOwnerId { get; set; }
        public virtual FlatOwner FlatOwner { get; set; }
    }
}
