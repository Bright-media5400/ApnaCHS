using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApnaCHS.Entities
{
    public class UserRole
    {
        public int RoleId { get; set; }
        public int UserId { get; set; }
        public string RoleName { get; set; }
    }
}
