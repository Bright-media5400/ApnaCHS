using ApnaCHS.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApnaCHS.Web.Models
{
    public class SocietyDocumentViewModel
    {
        public int id { get; set; }
        public string name { get; set; }
        public string filePath { get; set; }
        public SocietyViewModel society { get; set; }
    }
}