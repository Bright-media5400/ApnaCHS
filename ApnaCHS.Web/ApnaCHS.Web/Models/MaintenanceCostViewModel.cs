using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApnaCHS.Web.Models
{
    public class MaintenanceCostViewModel
    {
        public int id { get; set; }

        public MaintenanceCostDefinitionViewModel definition { get; set; }
        public int amount { get; set; }
        public DateTime date { get; set; }
        public DateTime? fromDate { get; set; }
        public DateTime? toDate { get; set; }
        public decimal? perSqrArea { get; set; }
        public bool allFlats { get; set; }
        public bool isApproved { get; set; }
        public DateTime? approvedDate { get; set; }
        public string approvedBy { get; set; }
        public bool deleted { get; set; }
        public bool isRejected { get; set; }
        public SocietyTrimViewModel society { get; set; }
        public FlatTrimViewModel[] flats { get; set; }
        public CommentMCViewModel[] comments { get; set; }
        public DataApprovalViewModel[] approvals { get; set; }
    }
}