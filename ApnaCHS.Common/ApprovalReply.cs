using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApnaCHS.Common
{
    public class ApprovalReply
    {
        public long Id { get; set; }

        public string Message { get; set; }

        public bool IsSucces { get; set; }

        public object Obj { get; set; }
    }
}
