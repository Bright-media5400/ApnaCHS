using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApnaCHS.Entities
{
    public class Society : EntityBase<long>
    {
        public Society()
        {
            //default value
            ApprovalsCount = 1;
        }

        public string Name { get; set; }
        public string RegistrationNo { get; set; }
        public DateTime? DateOfIncorporation { get; set; }
        public DateTime? DateOfRegistration { get; set; }

        public string Email { get; set; }
        public string PhoneNo { get; set; }
        public string ContactPerson { get; set; }
        public int BillingCycle { get; set; }
        public int DueDays { get; set; }
        public long? Second2Wheeler { get; set; }
        public long? Second4Wheeler { get; set; }
        public decimal? InterestPercent { get; set; }
        public int ApprovalsCount { get; set; }
        public bool OpeningInterest { get; set; }
        public long ComplexId { get; set; }
        [ForeignKey("ComplexId")]
        public virtual Complex Complex { get; set; }

        public virtual ICollection<MapSocietiesToFacilities> Facilities { get; set; }

        public virtual ICollection<SocietyStaff> SocietyStaffList { get; set; }

        public virtual ICollection<SecurityStaff> SecurityStaffList { get; set; }
        
        public virtual ICollection<SocietyDocument> SocietyDocuments { get; set; }

        public virtual ICollection<MapUserToSociety> User { get; set; }

        public virtual ICollection<SocietyAsset> SocietyAssets { get; set; }
    }

    public class SocietyImportResult
    {
        public long Id { get; set; }
        public string Result { get; set; }

        public bool IsSuccess { get; set; }
    }
}
