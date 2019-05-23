using System;
using System.Collections.Generic;
using System.Text;
using SupportApp.Entities.AuditableEntity;

namespace SupportApp.Entities
{
    public class SoftwareVersion : IAuditableEntity
    {
        public SoftwareVersion()
        {
            Customers = new HashSet<Customer>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public string ReleaseNote { get; set; }

        public string Description { get; set; }

        public virtual ICollection<Customer> Customers { get; set; }
    }
}
