using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApnaCHS.Common
{
    public class UploadFlatOwner
    {
        public int SrNo { get; set; }
        public string RegistrationNo { get; set; }
        public string Society { get; set; }
        public string Building { get; set; }
        public string Wing { get; set; }
        public string Floor { get; set; }
        public string Flat { get; set; }
        public string Name { get; set; }
        public string MobileNo { get; set; }
        public string EmailId { get; set; }
        public string Gender { get; set; }
        public string AadhaarCardNo { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public DateTime? MemberSinceDate { get; set; }
        public DateTime? MemberTillDate { get; set; }
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
    }
}
