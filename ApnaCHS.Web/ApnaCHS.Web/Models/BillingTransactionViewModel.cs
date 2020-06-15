using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApnaCHS.Web.Models
{
    public class BillingTransactionViewModel
    {
        public long id { get; set; }

        public string name { get; set; }

        public int transactionNo { get; set; }

        public int receiptNo { get; set; }

        public decimal amount { get; set; }

        public DateTime date { get; set; }

        public string reference { get; set; }

        public byte mode { get; set; }

        public string authorizedBy { get; set; }

        public string description { get; set; }

        public string chequeNo { get; set; }

        public string bank { get; set; }

        public string branch { get; set; }

        public DateTime? chequeDate { get; set; }

        public FlatTrimViewModel flat { get; set; }

        public SocietyTrimViewModel society { get; set; }
    }
}