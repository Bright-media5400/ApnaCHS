using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApnaCHS.Entities
{
    public class MaintenanceCostDefinition : EntityBase<long>
    {
        public string Name { get; set; }

        public bool CalculationOnPerSftArea { get; set; }

        public bool For2Wheeler { get; set; }

        public bool For4Wheeler { get; set; }

        public byte? FacilityType { get; set; }

        public virtual ICollection<MaintenanceCost> MaintenanceCosts { get; set; }
    }
}
