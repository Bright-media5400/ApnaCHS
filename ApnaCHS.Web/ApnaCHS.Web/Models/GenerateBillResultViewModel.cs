using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApnaCHS.Web.Models
{
    public class GenerateBillResultViewModel
    {
        public string name { get; set; }

        public string email { get; set; }

        public decimal amount { get; set; }

        public int month { get; set; }

        public int year { get; set; }
    }
}