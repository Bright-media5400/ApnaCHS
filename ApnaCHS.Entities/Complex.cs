using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApnaCHS.Entities
{
    public class Complex : EntityBase<long>
    {
        public byte Type { get; set; }

        public string Name { get; set; }
        public string Address { get; set; }
        public string Area { get; set; }

        public int CityId { get; set; }
        [ForeignKey("CityId")]
        public virtual MasterValue City { get; set; }

        public int StateId { get; set; }
        [ForeignKey("StateId")]
        public virtual MasterValue State { get; set; }

        public string RegistrationNo { get; set; }
        
        public DateTime? DateOfIncorporation { get; set; }
        
        public DateTime? DateOfRegistration { get; set; }

        public int NoOfBuilding { get; set; }

        public int NoOfGate { get; set; }

        public int NoOfSocieties { get; set; }

        public int NoOfAmenities { get; set; }

        public string Email { get; set; }

        public string PhoneNo { get; set; }

        public string ContactPerson { get; set; }

        public int Pincode { get; set; }

        public virtual ICollection<Society> Societies { get; set; }

        public virtual ICollection<Facility> Facilities { get; set; }

        public virtual ICollection<SocietyAsset> SocietyAssets { get; set; }


        public virtual ICollection<MapUserToComplex> User { get; set; }

    }
}
        