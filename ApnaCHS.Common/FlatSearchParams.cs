using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApnaCHS.Common
{
    public class FlatSearchParams
    {
        public long? FloorId { get; set; }

        public long? SocietyId { get; set; }

        public long? FacilityId { get; set; }

        public string FlatName { get; set; }

        public string Owner { get; set; }

        public string Tenant { get; set; }

        public string Username { get; set; }

        public bool? IsApproved { get; set; }

        public bool? IsRejected { get; set; }
    }


    public class FlatReportResult
    {
        public long SocietyId { get; set; }

        public string Society { get; set; }

        public string Building { get; set; }

        public string Wing { get; set; }

        public long BuildingId { get; set; }

        public string Floor { get; set; }
        
        public long FloorId { get; set; }

        public string Flat { get; set; }
        
        public long FlatId { get; set; }

        public string CurrentOwner { get; set; }
        
        public long? CurrentOwnerId { get; set; }

        public string Tenants { get; set; }

        public List<TenantResult> TenantList { get; set; }

        public bool IsCommercialSpace { get; set; }

        public string TypeName { get; set; }

        public int TotalArea { get; set; }

        public byte? CurrentOwnerType { get; set; }

        public string RegistrationNo { get; set; }
    }

    public class TenantResult
    {
        public long Id { get; set; }

        public string Name { get; set; }

    }
}
