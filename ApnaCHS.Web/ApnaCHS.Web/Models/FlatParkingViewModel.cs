using ApnaCHS.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApnaCHS.Web.Models
{
    public class FlatParkingViewModel
    {
        public int id { get; set; }
        public string name { get; set; }
        public int sqftArea { get; set; }
        public int parkingNo { get; set; }
        public byte? type { get; set; }
        public FacilityTrimViewModel facility { get; set; }
        public FloorTrimViewModel floor { get; set; }
    }

    public class FlatParkingTrimViewModel
    {
        public int id { get; set; }
        public string name { get; set; }
        public int sqftArea { get; set; }
        public int parkingNo { get; set; }
        public byte? type { get; set; }
        public long? flatId { get; set; }
    }
}