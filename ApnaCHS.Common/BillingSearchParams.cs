using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApnaCHS.Common
{
    public class BillingSearchParams
    {
        public long SocietyId { get; set; }

        public int? Year { get; set; }

        public int? Month { get; set; }

        public long? Facility { get; set; }

        public long? Floor { get; set; }
    }
}
