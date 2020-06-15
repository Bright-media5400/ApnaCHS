using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApnaCHS.Common
{
    public class UploadReply
    {
        public long Id { get; set; }

        public string Message { get; set; }

        public bool IsSuccess { get; set; }
    }

    public class UploadBillDetail
    {
        public int SrNo { get; set; }

        public string FlatName { get; set; }

        public decimal Amount { get; set; }

        public string Note { get; set; }
    }
}
