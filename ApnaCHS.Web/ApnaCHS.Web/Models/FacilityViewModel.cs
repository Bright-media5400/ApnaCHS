using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApnaCHS.Web.Models
{
    public class FacilityViewModel
    {
        public long id { get; set; }        
        public string name { get; set; }
        public string wing { get; set; }
        public int noOfFloors { get; set; }
        public int noOfFlats { get; set; }
        public int noOfLifts { get; set; }
        public int noOfParkinglots { get; set; }        
        public bool isParkingLot { get; set; }        
        public byte type { get; set; }        
        public ComplexTrimViewModel complex { get; set; }
        public MapSocietiesToFacilitiesViewModel[] societies { get; set; }
        public FloorTrimViewModel[] floors { get; set; }
        public FlatParkingTrimViewModel[] flatParkings { get; set; }
        public SocietyAssetViewModel[] societyAssets { get; set; }
    }

    public class FacilityTrimViewModel
    {
        public long id { get; set; }
        public string name { get; set; }
        public string wing { get; set; }
        public int noOfFloors { get; set; }
        public int noOfFlats { get; set; }
        public int noOfLifts { get; set; }
        public int noOfParkinglots { get; set; }
        public bool isParkingLot { get; set; }
        public long societyId { get; set; }        
        public byte type { get; set; }
    }


    public class FacilityForFlatViewModel
    {
        public long id { get; set; }
        public string name { get; set; }
        public string wing { get; set; }
        public int noOfFloors { get; set; }
        public int noOfFlats { get; set; }
        public int noOfLifts { get; set; }
        public int noOfParkinglots { get; set; }
        public bool isParkingLot { get; set; }
        public FloorForFlatsViewModel[] floors { get; set; }
    }

    public class FacilityForFlatParkingsViewModel
    {
        public long id { get; set; }
        public string name { get; set; }
        public string wing { get; set; }
        public int noOfFloors { get; set; }
        public int noOfFlats { get; set; }
        public int noOfLifts { get; set; }
        public int noOfParkinglots { get; set; }
        public bool isParkingLot { get; set; }
        public byte type { get; set; }
        public FloorForFlatParkingsViewModel[] floors { get; set; }
        public FlatParkingTrimViewModel[] flatParkings { get; set; }
    }

}