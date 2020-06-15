using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApnaCHS.Web.Models
{
    public class DataApprovalViewModel
    {
        public long id { get; set; }

        public DateTime? approvedDate { get; set; }

        public string approvedBy { get; set; }

        public string note { get; set; }

        public string approvedName { get; set; }
    }
}