using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApnaCHS.Web.Models
{
    public class BillingSearchParamViewModel
    {
        public long societyId { get; set; }

        public int? year { get; set; }

        public int? month { get; set; }

        public long? facility { get; set; }

        public long? floor { get; set; }
    }
}