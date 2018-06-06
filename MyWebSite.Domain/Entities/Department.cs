using System;
using System.Collections.Generic;
using System.Text;

namespace MyWebSite.Domain.Entities
{
    public class Department : Entity
    {
        public string Name { get; set; }

        public Guid ParentId { get; set; }
    }
}
