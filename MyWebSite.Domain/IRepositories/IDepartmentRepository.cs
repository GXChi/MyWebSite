﻿using MyWebSite.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyWebSite.Domain.IRepositories
{
    public interface IDepartmentRepository : IRepository<Department,Guid>
    {
              
    }
}
