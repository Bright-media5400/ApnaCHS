using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApnaCHS.Application.Models
{
    public class EmailSettingViewModel
    {
        public int id { get; set; }

        public string emailAddress { get; set; }

        public string password { get; set; }

        public string outgoingMailServer { get; set; }

        public int smtpPort { get; set; }
    }
}