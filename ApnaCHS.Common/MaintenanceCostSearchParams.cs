using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApnaCHS.Common
{
   public class MaintenanceCostSearchParams
    {
       public long SocietyId { get; set; }

       public bool? IsApproved { get; set; }

       public bool? IsDeleted { get; set; }

       public bool? IsActive { get; set; }

       public bool? IsRejected { get; set; }
    }
}
