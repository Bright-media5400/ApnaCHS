using System.Runtime.CompilerServices;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System;

namespace ApnaCHS.Entities
{
    public class ApplicationRole : IdentityRole<long, ApplicationUserRole>, IAuditProperties
    {
        public ApplicationRole()
            : base()
        {

        }

        public ApplicationRole(string name,int instanceId)
            : base()
        {
            Name = name;
        }

        public string DisplayName { get; set; }
        
        public DateTime CreatedDate { get; set; }
        
        public DateTime ModifiedDate { get; set; }
        
        public string CreatedBy { get; set; }
        
        public string ModifiedBy { get; set; }
        
        public bool Deleted { get; set; }

        public bool? IsBack { get; set; }

        public bool IsDefault { get; set; }

        public long? SocietyId { get; set; }
        [ForeignKey("SocietyId")]
        public virtual Society Society { get; set; }

        public long? ComplexId { get; set; }
        [ForeignKey("ComplexId")]
        public virtual Complex Complex { get; set; }

        public bool CanChange { get; set; }
    }

}