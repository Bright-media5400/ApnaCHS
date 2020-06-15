using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApnaCHS.Web.Models
{
    public class MapFlatToFlatOwnerViewModel
    {
        public FlatTrimViewModel flat { get; set; }

        public FlatOwnerTrimViewModel flatOwner { get; set; }

        public DateTime? memberSinceDate { get; set; }

        public DateTime? memberTillDate { get; set; }

        public byte flatOwnerType { get; set; }
    }
}