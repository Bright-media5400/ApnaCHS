using ApnaCHS.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApnaCHS.Web.Models
{
    public class VehicleViewModel
    {
        public int id { get; set; }
        public string name { get; set; }
        public string make { get; set; }
        public string number { get; set; }
        public byte? type { get; set; }
        public FlatOwnerTrimViewModel flatOwner { get; set; }
        public FlatTrimViewModel flat { get; set; }

        public DateTime? approvedDate { get; set; }
        public string approvedBy { get; set; }
        public bool deleted { get; set; }
        public bool isRejected { get; set; }
        public bool isApproved { get; set; }

        public CommentVehicleViewModel[] comments { get; set; }

        public DataApprovalViewModel[] approvals { get; set; }
    }

    public class VehicleTrimViewModel
    {
        public int id { get; set; }
        public string name { get; set; }
        public string make { get; set; }
        public string number { get; set; }
        public byte? type { get; set; }
    }
}