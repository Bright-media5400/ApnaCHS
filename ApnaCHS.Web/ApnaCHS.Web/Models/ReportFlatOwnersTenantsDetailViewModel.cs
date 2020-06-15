using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApnaCHS.Web.Models
{
    public class ReportFlatOwnersTenantsDetailViewModel
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
        public string owner { get; set; }
        public string ownerType { get; set; }
        public string name { get; set; }
        public string gender { get; set; }
        public string relationship { get; set; }
        public DateTime? dateOfBirth { get; set; }
        public string mobileNo { get; set; }
        public string aadhaarCardNo { get; set; }
        public bool isSuccess { get; set; }
        public string message { get; set; }
    }

    public class ReportFlatOwnersTenantsDetailSearchParamsViewModel
    {
        public long? societyId { get; set; }
    }
}