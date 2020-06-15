using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApnaCHS.Web.Models
{
    public class UploadFlatViewModel
    {
        public int srno { get; set; }

        public string registrationNo { get; set; }

        public string society { get; set; }

        public string building { get; set; }

        public string wing { get; set; }

        public string floor { get; set; }

        public string floorType { get; set; }

        public string flatType { get; set; }

        public string name { get; set; }

        public int? totalArea { get; set; }

        public int? carpetArea { get; set; }

        public int? buildUpArea { get; set; }

        public bool isCommercialSpace { get; set; }

        public bool haveParking { get; set; }

        public bool isSuccess { get; set; }

        public string message { get; set; }
    }
}