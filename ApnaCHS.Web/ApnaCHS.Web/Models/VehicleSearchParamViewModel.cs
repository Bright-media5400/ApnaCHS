using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApnaCHS.Web.Models
{
    public class VehicleSearchParamViewModel
    {
        public long? flatOwnerId { get; set; }
        
        public long? flatId { get; set; }

        public long? societyId { get; set; }
    }
}