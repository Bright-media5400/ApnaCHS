using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApnaCHS.Web.Models
{
    public class ApprovalReplyViewModel
    {
        public long Id { get; set; }

        public string Message { get; set; }

        public bool IsSucces { get; set; }
    }
}