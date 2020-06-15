using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApnaCHS.Web.Models
{
    public class MapUserToSocietyViewModel
    {
        public int id { get; set; }
        public string name { get; set; }
        public string registrationNo { get; set; }
        public string city { get; set; }
        public string address { get; set; }
        public string group { get; set; }
    }
}