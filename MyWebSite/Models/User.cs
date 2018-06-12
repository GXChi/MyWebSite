using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyWebSite.Models
{
    public class User
    {
        public string UserName { get; set; }

        public string PassWrod { get; set; }

        public string Name { get; set; }

        public string EMail { get; set; }

        public int IsDeleted { get; set; }

        //public virtual Department Department { get; set; }

        //public Guid DepartmentId { get; set; }
    }
}
