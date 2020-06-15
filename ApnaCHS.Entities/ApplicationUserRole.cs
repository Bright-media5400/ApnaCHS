using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApnaCHS.Entities
{
    public class ApplicationUserRole : IdentityUserRole<long> { }
}