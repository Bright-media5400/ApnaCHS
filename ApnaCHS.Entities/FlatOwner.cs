using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApnaCHS.Entities
{
    public class FlatOwner : EntityBase<long>
    {
        public FlatOwner()
        {
            Approvals = new HashSet<DataApproval>();
            Comments = new HashSet<CommentFlatOwner>();
        }
        public string Name { get; set; }
        public string MobileNo { get; set; }
        public string EmailId { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string AadhaarCardNo { get; set; }

        public int GenderId { get; set; }
        [ForeignKey("GenderId")]
        public virtual MasterValue Gender { get; set; }

        public virtual ICollection<MapFlatToFlatOwner> Flats { get; set; }
        public virtual ICollection<FlatOwnerFamily> FlatOwnerFamilies { get; set; }
        public virtual ICollection<Vehicle> Vehicles { get; set; }
        public virtual ICollection<DataApproval> Approvals { get; set; }
        public virtual ICollection<CommentFlatOwner> Comments { get; set; }

        public long? UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}
