using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApnaCHS.Entities
{
    public class Bill : EntityBase<long>
    {
        public Bill()
        {
            BillingLines = new HashSet<BillingLine>();
        }

        public string Name { get; set; }
        public int ReceiptNo { get; set; }
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }

        public decimal MonthlyAmount { get; set; }
        public decimal Pending { get; set; }
        public byte BillType { get; set; }
        public string Note { get; set; }
        public long FlatId { get; set; }
        [ForeignKey("FlatId")]
        public virtual Flat Flat { get; set; }

        public long SocietyId { get; set; }
        [ForeignKey("SocietyId")]
        public virtual Society Society { get; set; }

        public virtual ICollection<BillingLine> BillingLines { get; set; }
    }
}
