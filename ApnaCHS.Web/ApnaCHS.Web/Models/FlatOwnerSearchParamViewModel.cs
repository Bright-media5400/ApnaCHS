using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApnaCHS.Web.Models
{
    public class FlatOwnerSearchParamViewModel
    {
        public bool? isApproved { get; set; }

        public bool? isRejected { get; set; }

        public byte? flatOwnerType { get; set; }

        public long societyId { get; set; }
    }
}