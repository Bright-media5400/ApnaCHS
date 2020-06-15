using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApnaCHS.Common
{
    public class UploadFlat
    {
        public int Srno { get; set; }
        
        public string RegistrationNo { get; set; }
        
        public string Society { get; set; }
        
        public string Building { get; set; }
        
        public string Wing { get; set; }

        public string Floor { get; set; }

        public string FloorType { get; set; }

        public string FlatType { get; set; }

        public string Name { get; set; }

        public int? TotalArea { get; set; }

        public int? CarpetArea { get; set; }

        public int? BuildUpArea { get; set; }

        public bool IsCommercialSpace { get; set; }

        public bool HaveParking { get; set; }

        public bool IsSuccess { get; set; }

        public string Message { get; set; }

    }
}
