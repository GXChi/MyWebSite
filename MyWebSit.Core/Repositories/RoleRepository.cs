using MyWebSite.Domain.Entities;
using MyWebSite.Domain.IRepositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyWebSit.Core.Repositories
{
    class RoleRepository : RepositoryBase<Role>,IRoleRepository
    {
        public RoleRepository(MyWebSiteDbContext dbContext) : base(dbContext) { }
    }
}
