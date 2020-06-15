using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApnaCHS.Web.Models
{
    public class MaintenanceCostSearchParamViewModel
    {
        public long societyId { get; set; }

        public bool? isApproved { get; set; }

        public bool? isDeleted { get; set; }

        public bool? isActive { get; set; }

        public bool? isRejected { get; set; }
    }
}