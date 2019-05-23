using System;
using System.Collections.Generic;
using System.Text;
using SupportApp.Entities.AuditableEntity;

namespace SupportApp.Entities
{
    public class RequestType : IAuditableEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
    }
}
