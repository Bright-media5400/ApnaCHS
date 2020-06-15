using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApnaCHS.Entities
{
    public class MasterValue : EntityBase<int>
    {
        public MasterValue()
        {
        }

        public string Text { get; set; }

        public string Description { get; set; }

        public byte Type { get; set; }

        public string CustomFields { get; set; }

   }
}
