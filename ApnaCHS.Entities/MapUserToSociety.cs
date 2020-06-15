using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApnaCHS.Entities
{
    public class MapUserToSociety
    {
        [Key]
        [Column(Order = 0)]
        public long UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; }

        [Key]
        [Column(Order = 1)]
        public long SocietyId { get; set; }
        [ForeignKey("SocietyId")]
        public virtual Society Society { get; set; }

        [Key]
        [Column(Order = 2)]
        public long RoleId { get; set; }
        [ForeignKey("RoleId")]
        public virtual ApplicationRole Role { get; set; }

        public bool IsBlocked { get; set; }
    }
}
