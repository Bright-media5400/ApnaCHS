using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApnaCHS.Web.Models
{
    public class FloorViewModel
    {
        public int id { get; set; }
        public string name { get; set; }
        public FacilityTrimViewModel facility { get; set; }
        public byte type { get; set; }
        public int floorNumber { get; set; }
        public FlatTrimViewModel[] flats { get; set; }
        public FlatParkingTrimViewModel[] flatParkings { get; set; } 
    }

    public class FloorTrimViewModel
    {
        public int id { get; set; }        
        public string name { get; set; }
        public long facilityId { get; set; }
        public byte type { get; set; }
        public int floorNumber { get; set; }
    }

    public class FloorForFlatsViewModel
    {
        public int id { get; set; }
        public string name { get; set; }
        public int totalNoFlats { get; set; }
        public bool isParkingLot { get; set; }
        public bool sharedResource { get; set; }
        public byte type { get; set; }
        public int floorNumber { get; set; }
        public FlatTrimViewModel[] flats { get; set; }
    }

    public class FloorForFlatParkingsViewModel
    {
        public int id { get; set; }
        public string name { get; set; }
        public int totalNoFlats { get; set; }
        public bool isParkingLot { get; set; }
        public bool sharedResource { get; set; }
        public byte type { get; set; }
        public int floorNumber { get; set; }
        public FlatParkingTrimViewModel[] flatParkings { get; set; }
    }
}