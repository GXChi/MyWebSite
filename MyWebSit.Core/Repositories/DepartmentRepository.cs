using MyWebSite.Domain.Entities;
using MyWebSite.Domain.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace MyWebSit.Core.Repositories
{
    public class DepartmentRepository : RepositoryBase<Department>,IDepartmentRepository
    {       
        public DepartmentRepository(MyWebSiteDbContext dbContext) : base(dbContext)
        {

        }

    
    }
}
