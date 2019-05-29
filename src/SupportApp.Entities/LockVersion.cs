using System;
using System.Collections.Generic;
using System.Text;
using SupportApp.Entities.AuditableEntity;

namespace SupportApp.Entities
{
    public class LockVersion : IAuditableEntity
    {
        public LockVersion()
        {
            Customers = new HashSet<Customer>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public virtual ICollection<Customer> Customers { get; set; }
    }
}
