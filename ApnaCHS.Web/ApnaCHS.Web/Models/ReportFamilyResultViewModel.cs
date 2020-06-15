using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApnaCHS.Web.Models
{
    public class ReportFamilyResultViewModel
    {
        public long id { get; set; }
        public string name { get; set; }
        public string mobileNo { get; set; }
        public string aadhaarCardNo { get; set; }
        public long flatId { get; set; }
        public string flatName { get; set; }
        public string genderText { get; set; }
        public string relationshipText { get; set; }
        public string flatOwnerName { get; set; }
        public DateTime? approvedDate { get; set; }
        public string approvedBy { get; set; }
        public bool deleted { get; set; }
        public bool isRejected { get; set; }
        public bool isApproved { get; set; }

        public CommentFlatOwnerFamilyViewModel[] comments { get; set; }

        public DataApprovalViewModel[] approvals { get; set; }
    }
}