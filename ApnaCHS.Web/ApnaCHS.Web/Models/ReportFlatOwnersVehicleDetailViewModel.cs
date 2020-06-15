using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApnaCHS.Web.Models
{
    public class ReportFlatOwnersVehicleDetailViewModel
    {
        public int srno { get; set; }
        public long? societyId { get; set; }
        public string society { get; set; }
        public string building { get; set; }
        public string wing { get; set; }
        public long? buildingId { get; set; }
        public string floor { get; set; }
        public long? floorId { get; set; }
        public string flat { get; set; }
        public long? flatId { get; set; }
        public string flatOwner { get; set; }
        public string flatOwnerType { get; set; }
        public string name { get; set; }
        public string make { get; set; }
        public string number { get; set; }
        public byte? type { get; set; }
        public bool isSuccess { get; set; }
        public string message { get; set; }
    }

    public class ReportFlatOwnersVehicleDetailSearchParamsViewModel
    {
        public long? SocietyId { get; set; }
    }

}