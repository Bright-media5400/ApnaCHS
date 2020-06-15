using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApnaCHS.Entities
{
    public class BillingTransaction : EntityBase<long>
    {
        public string Name { get; set; }

        public int TransactionNo { get; set; }

        public decimal Amount { get; set; }

        public DateTime Date { get; set; }

        public string Reference { get; set; }

        public byte Mode { get; set; }

        public string AuthorizedBy { get; set; }

        public string Description { get; set; }

        public string ChequeNo { get; set; }

        public string Bank { get; set; }

        public string Branch { get; set; }

        public DateTime? ChequeDate { get; set; }
                
        public long? FlatId { get; set; }
        [ForeignKey("FlatId")]
        public virtual Flat Flat { get; set; }

        //public long? FlatOwnerId { get; set; }
        //[ForeignKey("FlatOwnerId")]
        //public virtual FlatOwner FlatOwner { get; set; }

        public long SocietyId { get; set; }
        [ForeignKey("SocietyId")]
        public virtual Society Society { get; set; }
    }
}
