using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApnaCHS.Web.Models
{
    public class BillingViewModel
    {
        public long id { get; set; }
        public string name { get; set; }
        public int receiptNo { get; set; }
        public DateTime date { get; set; }
        public decimal amount { get; set; }
        public int month { get; set; }
        public int year { get; set; }
        public decimal monthlyAmount { get; set; }
        public decimal pending { get; set; }
        public byte billType { get; set; }
        public string note { get; set; }

        public FlatTrimViewModel flat { get; set; }
        public SocietyTrimViewModel society { get; set; }
    }
}