using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApnaCHS.Web.Models
{
    public class FlatSearchParamViewModel
    {
        public long? floorId { get; set; }

        public long? societyId { get; set; }

        public long? facilityId { get; set; }

        public string flatName { get; set; }

        public string owner { get; set; }

        public string tenant { get; set; }

        public string username { get; set; }

        public bool? isApproved { get; set; }

        public bool? isRejected { get; set; }
    }

    public class FlatReportResultViewModel
    {
        public long societyId { get; set; }

        public string society { get; set; }

        public string building { get; set; }

        public string wing { get; set; }

        public long buildingId { get; set; }

        public string floor { get; set; }

        public long floorId { get; set; }

        public string flat { get; set; }

        public long flatId { get; set; }

        public string currentOwner { get; set; }

        public long? currentOwnerId { get; set; }

        public List<TenantResultViewModel> tenantList { get; set; }        

        public bool isCommercialSpace { get; set; }

        public string typeName { get; set; }

        public int totalArea { get; set; }

        public byte? currentOwnerType { get; set; }

        public string registrationNo { get; set; }
    }

    public class TenantResultViewModel
    {
        public long id { get; set; }

        public string name { get; set; }

    }

}