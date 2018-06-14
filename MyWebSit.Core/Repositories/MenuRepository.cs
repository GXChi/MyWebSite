using MyWebSite.Domain.Entities;
using MyWebSite.Domain.IRepositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyWebSit.Core.Repositories
{
    class MenuRepository : RepositoryBase<Menu>, IMenuRepository
    {
        public MenuRepository(MyWebSiteDbContext dbContext) : base(dbContext)
        {}
    }
}
