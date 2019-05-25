using System;
using System.Collections.Generic;
using System.Text;
using SupportApp.Entities.AuditableEntity;

namespace SupportApp.Entities
{
    public class Customer : IAuditableEntity
    {
        public int Id { get; set; }

        public string Number { get; set; }

        public string Name { get; set; }

        public string Family { get; set; }

        public string FullName => Name + " " + Family;

        public virtual SoftwareVersion SoftwareVersion { get; set; }
        public int SoftwareVersionId { get; set; }

        public string LockNumber { get; set; }

        public string LockVersion { get; set; }

        public int AccountCount { get; set; }

        public int CompanyCount { get; set; }

        public string Address { get; set; }

        public string Tell { get; set; }

        public DateTimeOffset SupportEndDate { get; set; }
    }
}
