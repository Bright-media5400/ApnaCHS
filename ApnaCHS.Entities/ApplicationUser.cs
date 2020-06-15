using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Principal;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Threading.Tasks;
using System.Security.Claims;
using System.Data.Entity;

namespace ApnaCHS.Entities
{
    public class ApplicationUser : IdentityUser<long, ApplicationUserLogin, ApplicationUserRole, ApplicationUserClaim>, IUser<long>, IAuditProperties
    {
        public ApplicationUser()
        {
            CreatedDate = DateTime.Now;
            ModifiedDate = DateTime.Now;
        }

        public DateTime CreatedDate { get; set; }

        public DateTime ModifiedDate { get; set; }

        public string CreatedBy { get; set; }

        public string ModifiedBy { get; set; }

        public bool Deleted { get; set; }

        public string Name { get; set; }

        public int MaxAttempts { get; set; }

        public bool bBlocked { get; set; }

        public bool bChangePass { get; set; }

        public bool IsBack { get; set; }

        public bool IsDefault { get; set; }

        public virtual ICollection<MapUserToComplex> Complexes { get; set; }

        public virtual ICollection<MapUserToSociety> Societies { get; set; }
    }
}