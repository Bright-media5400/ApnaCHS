using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApnaCHS.Common
{
    public class PendingBill
    {
        public long Id { get; set; }

        public int ReceiptNo { get; set; }

        public string Name { get; set; }

        public DateTime Date { get; set; }

        public decimal Amount { get; set; }

        public decimal Pending { get; set; }

        public int Month { get; set; }

        public int Year { get; set; }
    }
}
