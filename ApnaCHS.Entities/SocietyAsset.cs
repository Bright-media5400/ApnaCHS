using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApnaCHS.Entities
{
    public class SocietyAsset : EntityBase<long>
    {
        public string Name { get; set; }
        public bool IsUsable { get; set; }
        public bool IsOperational { get; set; }
        public int Quantity { get; set; }
        public string CompanyName { get; set; }
        public string Brand { get; set; }
        public DateTime? PurchaseDate { get; set; }
        public string ModelNo { get; set; }
        public string SrNo { get; set; }
        public string ContactPerson { get; set; }
        public string CustomerCareNo { get; set; }
        public long? FloorId { get; set; }
        [ForeignKey("FloorId")]
        public virtual Floor Floor { get; set; }

        public long? FacilityId { get; set; }
        [ForeignKey("FacilityId")]
        public virtual Facility Facility { get; set; }

        public long? SocietyId { get; set; }
        [ForeignKey("SocietyId")]
        public virtual Society Society { get; set; }

        public long ComplexId { get; set; }
        [ForeignKey("ComplexId")]
        public virtual Complex Complex { get; set; }
    }
}
