using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApnaCHS.Web.Models
{
    public class MasterValueViewModel
    {
        public int id { get; set; }

        public string text { get; set; }

        public string description { get; set; }

        public byte type { get; set; }

        public bool deleted { get; set; }

        public string customFields { get; set; }
    }
}