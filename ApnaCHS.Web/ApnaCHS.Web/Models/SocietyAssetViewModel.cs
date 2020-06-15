using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApnaCHS.Web.Models
{
    public class SocietyAssetViewModel
    {
        public int id { get; set; }
        public string name { get; set; }

        public bool isUsable { get; set; }

        public bool isOperational { get; set; }
        public int quantity { get; set; }
        public string companyName { get; set; }
        public string brand { get; set; }
        public DateTime? purchaseDate { get; set; }
        public string modelNo { get; set; }
        public string srNo { get; set; }
        public string contactPerson { get; set; }
        public string customerCareNo { get; set; }
        public long? floorId { get; set; }
        
        public FloorTrimViewModel floor { get; set; }

        public FacilityTrimViewModel facility { get; set; }

        public SocietyTrimViewModel society { get; set; }

        public ComplexTrimViewModel complex { get; set; }
    }

    public class SocietyAssetTrimViewModel
    {
        public int id { get; set; }
        public string name { get; set; }

        public bool isUsable { get; set; }

        public bool isOperational { get; set; }
        public int quantity { get; set; }
        public string companyName { get; set; }
        public string brand { get; set; }
        public DateTime? purchaseDate { get; set; }
        public string modelNo { get; set; }
        public string srNo { get; set; }
        public string contactPerson { get; set; }
        public string customerCareNo { get; set; }
        public long? floorId { get; set; }
    }

}
   
