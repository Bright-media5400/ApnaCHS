using ApnaCHS.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApnaCHS.Web.Models
{
    public class FlatOwnerViewModel
    {
        public int id { get; set; }
        public string name { get; set; }
        public string mobileNo { get; set; }
        public string emailId { get; set; }
        public DateTime? dateOfBirth { get; set; }
        public string aadhaarCardNo { get; set; }

        public MasterValueViewModel gender { get; set; }
        public MapFlatToFlatOwnerViewModel[] flats { get; set; }
        public VehicleTrimViewModel[] vehicles { get; set; }
        public FlatOwnerFamilyTrimViewModel[] flatOwnerFamilies { get; set; }

        public CommentFlatOwnerViewModel[] comments { get; set; }

        public DataApprovalViewModel[] approvals { get; set; }

        public DateTime? approvedDate { get; set; }
        public string approvedBy { get; set; }
        public bool deleted { get; set; }
        public bool isRejected { get; set; }
        public bool isApproved { get; set; }
    }

    public class FlatOwnerTrimViewModel
    {
        public int id { get; set; }
        public string name { get; set; }
        public string mobileNo { get; set; }
        public string emailId { get; set; }
        public DateTime? dateOfBirth { get; set; }
        public string aadhaarCardNo { get; set; }
        public MasterValueViewModel gender { get; set; }
        public bool isApproved { get; set; }
    }

}