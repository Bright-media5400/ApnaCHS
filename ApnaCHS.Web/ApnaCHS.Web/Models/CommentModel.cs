using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApnaCHS.Web.Models
{
    public class CommentFlatViewModel
    {
        public long id { get; set; }

        public long flatId { get; set; }

        public string text { get; set; }

        public DateTime createdDate { get; set; }

        public string createdBy { get; set; }

        public string commentBy { get; set; }
    }

    public class CommentFlatOwnerViewModel
    {
        public long id { get; set; }

        public long flatOwnerId { get; set; }

        public string text { get; set; }

        public DateTime createdDate { get; set; }

        public string createdBy { get; set; }

        public string commentBy { get; set; }
    }

    public class CommentMCViewModel
    {
        public long id { get; set; }

        public long maintenanceCostId { get; set; }

        public string text { get; set; }

        public DateTime createdDate { get; set; }

        public string createdBy { get; set; }

        public string commentBy { get; set; }
    }

    public class CommentFlatOwnerFamilyViewModel
    {
        public long id { get; set; }

        public long flatOwnerFamilyId { get; set; }

        public string text { get; set; }

        public DateTime createdDate { get; set; }

        public string createdBy { get; set; }

        public string commentBy { get; set; }
    }

    public class CommentVehicleViewModel
    {
        public long id { get; set; }

        public long vehicleId { get; set; }

        public string text { get; set; }

        public DateTime createdDate { get; set; }

        public string createdBy { get; set; }

        public string commentBy { get; set; }
    }

}