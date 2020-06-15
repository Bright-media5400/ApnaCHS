using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApnaCHS.Web.Models
{
    public class UploadReplyViewModel
    {
        public long id { get; set; }

        public string message { get; set; }

        public bool isSuccess { get; set; }
    }
}