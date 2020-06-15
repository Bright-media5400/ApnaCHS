using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApnaCHS.Web.Models
{
    public class MaintenanceCostDefinitionViewModel
    {
        public long id { get; set; }
        public string name { get; set; }

        public bool calculationOnPerSftArea { get; set; }

        public bool for2Wheeler { get; set; }

        public bool for4Wheeler { get; set; }

        public byte? facilityType { get; set; }
    }
}