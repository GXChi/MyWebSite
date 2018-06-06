using System;
using System.Text;
using System.Collections.Generic;
using MyWebSite.Domain.Entities;

namespace MyWebSite.Domain.Entities
{
    public class User : Entity
    {
        public string UserName { get; set; }

        public string PassWrod { get; set; }

        public string Name { get; set; }

        public string EMail { get; set; }

        public int IsDeleted { get; set; }

        public virtual Department Department { get; set; }

        public Guid DepartmentId { get; set; }


    }
}
