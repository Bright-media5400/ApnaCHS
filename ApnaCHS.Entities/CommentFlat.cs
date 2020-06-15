using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApnaCHS.Entities
{
    public class CommentFlat : EntityBase<int>
    {
        public string Text { get; set; }

        public string CommentBy { get; set; }

        [ForeignKey("Flat")]
        public long FlatId { get; set; }
        public virtual Flat Flat { get; set; }
    }
}
