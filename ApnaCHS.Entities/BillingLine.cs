using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApnaCHS.Entities
{
    public class BillingLine : EntityBase<long>
    {
        public long BillId { get; set; }
        [ForeignKey("BillId")]
        public virtual Bill Bill { get; set; }

        public string Definition { get; set; }

        public decimal Amount { get; set; }

        public decimal BaseAmount { get; set; }

        public decimal? OtherAmount { get; set; }

        public int? OnArea { get; set; }
    }
}
