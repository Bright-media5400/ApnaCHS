using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApnaCHS.Web.Models
{
    public class PendingBillViewModel
    {
        public long id { get; set; }

        public int receiptNo { get; set; }

        public string name { get; set; }

        public DateTime date { get; set; }

        public decimal amount { get; set; }

        public decimal pending { get; set; }

        public int month { get; set; }

        public int year { get; set; }
    }
}