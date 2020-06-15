using ApnaCHS.AppCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApnaCHS.DataAccess
{
    public class BTAuthorizationAction
    {
        public BTAuthorizationAction(EnActionList action, EnModuleList module)
        {
            this.action = action;
            this.module = module;
        }

        public EnActionList action { get; set; }
        public EnModuleList module { get; set; }
    }
}
