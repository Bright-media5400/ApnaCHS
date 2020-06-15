using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApnaCHS.Entities
{
   public class SocietyDocument : EntityBase<long>
    {
       public string Name { get; set; }
       public string FilePath { get; set; }

       public long SocietyId { get; set; }
       [ForeignKey("SocietyId")]
       public virtual Society Society { get; set; }
    }
}
