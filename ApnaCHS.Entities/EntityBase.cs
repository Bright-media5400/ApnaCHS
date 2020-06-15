using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApnaCHS.Entities
{
    public class EntityBase<T> : IAuditProperties
    {
        public EntityBase()
        {
            CreatedDate = DateTime.Now;
            ModifiedDate = DateTime.Now;
            Deleted = false;
        }

        [Key] 
        [Column(Order = 0)]
        public T Id { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime ModifiedDate { get; set; }

        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }

        public bool Deleted { get; set; }

        public bool IsApproved { get; set; }

        public bool IsRejected { get; set; }

        public string UnApprovedData { get; set; }
    }

    public interface IAuditProperties
    {
        DateTime CreatedDate { get; set; }

        DateTime ModifiedDate { get; set; }

        string CreatedBy { get; set; }
        string ModifiedBy { get; set; }

        bool Deleted { get; set; }
    }
}
