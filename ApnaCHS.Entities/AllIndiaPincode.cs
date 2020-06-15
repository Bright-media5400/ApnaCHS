using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApnaCHS.Entities
{
    public class AllIndiaPincode
    {
        [Key]
        public int Id { get; set; }

        public string OfficeName { get; set; }
        public int Pincode { get; set; }
        public string OfficeType { get; set; }
        public string DeliveryStatus { get; set; }
        public string DivisionName { get; set; }
        public string RegionName { get; set; }
        public string CircleName { get; set; }
        public string Taluk { get; set; }
        public string DistrictName { get; set; }
        public string StateName { get; set; }
        public string Telephone { get; set; }
        public string RelatedSuboffice { get; set; }
        public string RelatedHeadoffice { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }
    }
}
