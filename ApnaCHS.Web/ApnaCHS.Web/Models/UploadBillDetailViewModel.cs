using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApnaCHS.Web.Models
{
    public class UploadBillDetailViewModel
    {
        public int srNo { get; set; }

        public string flatName { get; set; }

        public decimal amount { get; set; }

        public string note { get; set; }
    }
}