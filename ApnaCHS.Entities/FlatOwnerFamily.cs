using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApnaCHS.Entities
{
    public class FlatOwnerFamily : EntityBase<long>
    {
        public FlatOwnerFamily()
        {
            Approvals = new HashSet<DataApproval>();
            Comments = new HashSet<CommentFlatOwnerFamily>();
        }
        public string Name { get; set; }
        public string MobileNo { get; set; }
        public string AadhaarCardNo { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public bool AdminMember { get; set; }
        public bool ApproverMember { get; set; }
        
        public int GenderId { get; set; }
        [ForeignKey("GenderId")]
        public virtual MasterValue Gender { get; set; }

        public int RelationshipId { get; set; }
        [ForeignKey("RelationshipId")]  
        public virtual MasterValue Relationship { get; set; }
        
        public long FlatOwnerId { get; set; }
        [ForeignKey("FlatOwnerId")]
        public virtual FlatOwner FlatOwner { get; set; }

        public virtual ICollection<DataApproval> Approvals { get; set; }
        public virtual ICollection<CommentFlatOwnerFamily> Comments { get; set; }

        public long? UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual ApplicationUser ApplicationUser { get; set; }

    }
}
