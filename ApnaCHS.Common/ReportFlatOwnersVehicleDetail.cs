using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApnaCHS.Common
{
    public class ReportFlatOwnersVehicleDetail
    {
        public int Srno { get; set; }
        public long? SocietyId { get; set; }
        public string Society { get; set; }
        public string Building { get; set; }
        public string Wing { get; set; }
        public long? BuildingId { get; set; }
        public string Floor { get; set; }
        public long? FloorId { get; set; }
        public string Flat { get; set; }
        public long? FlatId { get; set; }
        public string FlatOwner { get; set; }
        public string FlatOwnerType { get; set; }
        public string Name { get; set; }
        public string Make { get; set; }
        public string Number { get; set; }
        public byte? Type { get; set; }
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
    }

    public class ReportFlatOwnersVehicleDetailSearchParams
    {
        public long? SocietyId { get; set; }
    }

}
