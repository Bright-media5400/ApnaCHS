using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApnaCHS.Entities
{
    public class ReportFamilyResult
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string MobileNo { get; set; }
        public string AadhaarCardNo { get; set; }
        public long FlatId { get; set; }
        public string FlatName { get; set; }
        public string GenderText { get; set; }
        public string RelationshipText { get; set; }
        public string FlatOwnerName { get; set; }
        public bool Deleted { get; set; }
        public bool IsRejected { get; set; }
        public bool IsApproved { get; set; }

        public virtual ICollection<DataApproval> Approvals { get; set; }
        public virtual ICollection<CommentFlatOwnerFamily> Comments { get; set; }
    }
}
