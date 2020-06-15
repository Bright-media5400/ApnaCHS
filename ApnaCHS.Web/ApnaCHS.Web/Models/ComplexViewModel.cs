using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApnaCHS.Web.Models
{
    public class ComplexViewModel
    {
        public string id { get; set; }
        public byte type { get; set; }
        public string name { get; set; }
        public string address { get; set; }
        public string area { get; set; }
        public string registrationNo { get; set; }
        public DateTime? dateOfIncorporation { get; set; }
        public DateTime? dateOfRegistration { get; set; }
        public int noOfSocieties { get; set; }
        public int noOfBuilding { get; set; }
        public int noOfGate { get; set; }

        public int noOfAmenities { get; set; }
        public string email { get; set; }
        public string phoneNo { get; set; }
        public string contactPerson { get; set; }

        public int pincode { get; set; }

        public MasterValueViewModel city { get; set; }
        public MasterValueViewModel state { get; set; }

        public FacilityTrimViewModel[] facilities { get; set; }
        public SocietyTrimViewModel[] societies { get; set; }
        public SocietyAssetViewModel[] societyAssets { get; set; }
    }

    public class ComplexTrimViewModel
    {
        public string id { get; set; }
        public byte type { get; set; }
        public string name { get; set; }
        public string address { get; set; }
        public string area { get; set; }
        public string registrationNo { get; set; }
        public DateTime? dateOfIncorporation { get; set; }
        public DateTime? dateOfRegistration { get; set; }
        public int noOfSocieties { get; set; }
        public int noOfBuilding { get; set; }
        public int noOfGate { get; set; }

        public int noOfAmenities { get; set; }
        public string email { get; set; }
        public string phoneNo { get; set; }
        public string contactPerson { get; set; }
        public int pincode { get; set; }
        public MasterValueViewModel city { get; set; }
        public MasterValueViewModel state { get; set; }
    }
}