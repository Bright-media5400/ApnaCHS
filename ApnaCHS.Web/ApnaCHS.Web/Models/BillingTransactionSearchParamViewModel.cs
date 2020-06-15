using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApnaCHS.Web.Models
{
    public class BillingTransactionSearchParamViewModel
    {
        public long? societyId { get; set; }

        public int? year { get; set; }

        public int? month { get; set; }

        public string username { get; set; }
    }
}
