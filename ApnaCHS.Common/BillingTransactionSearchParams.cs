using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApnaCHS.Common
{
    public class BillingTransactionSearchParams
    {
        public long? SocietyId { get; set; }

        public int? Year { get; set; }

        public int? Month { get; set; }

        public string Username { get; set; }
    }
}
