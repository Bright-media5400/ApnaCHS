using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApnaCHS.Common
{
    public class ReportFlatOwnersTenantsDetail
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
        public string Owner { get; set; }
        public string OwnerType { get; set; }
        public string Name { get; set; }
        public string Gender { get; set; }
        public string Relationship { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string MobileNo { get; set; }
        public string AadhaarCardNo { get; set; }
        public bool IsSuccess  { get; set; }
        public string Message { get; set; }
    }

    public class ReportFlatOwnersTenantsDetailSearchParams
    {
        public long? SocietyId { get; set; }
    }

}
