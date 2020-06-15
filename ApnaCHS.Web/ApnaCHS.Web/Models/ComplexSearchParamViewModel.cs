using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApnaCHS.Web.Models
{
    public class ComplexSearchParamViewModel
    {
        public int? city { get; set; }
        
        public int? state { get; set; }
        
        public string complexname { get; set; }
        
        public string societyname { get; set; }
        
        public byte? amenitytype { get; set; }
    }
}