using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApnaCHS.Web.Models
{
    public class FlatOwnerFamilyViewModel
    {
        public int id { get; set; }
        public string name { get; set; }
        public string mobileNo { get; set; }
        public string aadhaarCardNo { get; set; }
        public DateTime? dateOfBirth { get; set; }
        public bool adminMember { get; set; }
        public bool approverMember { get; set; }
        public FlatOwnerTrimViewModel flatOwner { get; set; }
        public MasterValueViewModel gender { get; set; }
        public MasterValueViewModel relationship { get; set; }
        public long flatOwnerId { get; set; }
        public DateTime? approvedDate { get; set; }
        public string approvedBy { get; set; }
        public bool deleted { get; set; }
        public bool isRejected { get; set; }
        public bool isApproved { get; set; }

        public CommentFlatOwnerFamilyViewModel[] comments { get; set; }

        public DataApprovalViewModel[] approvals { get; set; }
    }

    public class FlatOwnerFamilyTrimViewModel
    {
        public int id { get; set; }
        public string name { get; set; }
        public string mobileNo { get; set; }
        public string aadhaarCardNo { get; set; }
        public DateTime? dateOfBirth { get; set; }
        public bool adminMember { get; set; }
        public bool approverMember { get; set; }
        public MasterValueViewModel gender { get; set; }
        public MasterValueViewModel relationship { get; set; }
        public long flatOwnerId { get; set; }
    }
}