using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ApnaCHS.Web.Models
{
    public class ApplicationRoleViewModel
    {
        public int id { get; set; }

        [Required]
        public string name { get; set; }

        public SocietyTrimViewModel society { get; set; }

        public ComplexTrimViewModel complex { get; set; }

        public bool canChange { get; set; }
    }

    public class ApplicationRoleTrimViewModel
    {
        public int id { get; set; }

        public string name { get; set; }
    }

}