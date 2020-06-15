using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApnaCHS.Common
{
    public class FlatOwnerSearchParams
    {
        public bool? IsApproved { get; set; }

        public bool? IsRejected { get; set; }

        public byte? FlatOwnerType { get; set; }

        public long SocietyId { get; set; }
    }
}
