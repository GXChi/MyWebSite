using MyWebSite.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyWebSite.Application.UserApp.Dto
{
    public class UserDto
    {        
        public string UserName { get; set; }

        public string PassWrod { get; set; }

        public string Name { get; set; }

        public string EMail { get; set; }

        //public int IsDeleted { get; set; }

        //public virtual Department Department { get; set; }

        //public Guid DepartmentId { get; set; }

    }
}
