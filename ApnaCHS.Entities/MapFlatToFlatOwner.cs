using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApnaCHS.Entities
{
    public class MapFlatToFlatOwner     
    {
        [Key]
        [Column(Order = 0)]
        public long FlatId { get; set; }
        [ForeignKey("FlatId")]
        public virtual Flat Flat { get; set; }

        [Key]
        [Column(Order = 1)]
        public long FlatOwnerId { get; set; }
        [ForeignKey("FlatOwnerId")]
        public virtual FlatOwner FlatOwner { get; set; }

        public DateTime? MemberSinceDate { get; set; }

        public DateTime? MemberTillDate { get; set; }

        public byte FlatOwnerType { get; set; }
    }
}
