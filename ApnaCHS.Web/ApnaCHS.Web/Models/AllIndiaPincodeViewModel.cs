using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApnaCHS.Web.Models
{
    public class AllIndiaPincodeViewModel
    {
        public int id { get; set; }
        public string officeName { get; set; }
        public int pincode { get; set; }
        public string officeType { get; set; }
        public string deliveryStatus { get; set; }
        public string divisionName { get; set; }
        public string regionName { get; set; }
        public string circleName { get; set; }
        public string taluk { get; set; }
        public string districtName { get; set; }
        public string stateName { get; set; }
        public string telephone { get; set; }
        public string relatedSuboffice { get; set; }
        public string relatedHeadoffice { get; set; }
        public string longitude { get; set; }
        public string latitude { get; set; }
    }
}