using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApnaCHS.Web.Models
{
    public class FlatViewModel
    {
        public long id { get; set; }
        public string name { get; set; }
        public int totalArea { get; set; }
        public int carpetArea { get; set; }
        public int? buildUpArea { get; set; }
        public bool haveParking { get; set; }
        public bool isRented { get; set; }
        public bool isCommercialSpace { get; set; }
        public bool isApproved { get; set; }
        public DateTime? approvedDate { get; set; }
        public string approvedBy { get; set; }
        public bool deleted { get; set; }
        public bool isRejected { get; set; }
        public FloorTrimViewModel floor { get; set; }
        public MasterValueViewModel flatType { get; set; }

        public FlatParkingTrimViewModel[] flatParkings { get; set; }

        public MapFlatToFlatOwnerViewModel[] flatOwners { get; set; }

        public CommentFlatViewModel[] comments { get; set; }

        public DataApprovalViewModel[] approvals { get; set; }
    }

    public class FlatTrimViewModel
    {
        public long id { get; set; }
        public string name { get; set; }
        public string totalArea { get; set; }
        public string carpetArea { get; set; }
        public int? buildUpArea { get; set; }
        public int? bathroom { get; set; }
        public int? balcony { get; set; }
        public bool haveParking { get; set; }
        public bool isRented { get; set; }
        public bool isCommercialSpace { get; set; }
        public long floorId { get; set; }
        public int flatTypeId { get; set; }

    }

}