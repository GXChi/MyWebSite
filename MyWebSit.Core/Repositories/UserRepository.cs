using MyWebSite.Domain.Entities;
using MyWebSite.Domain.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyWebSit.Core.Repositories
{
    public class UserRepository : RepositoryBase<User>, IUserRepository
    {
        public UserRepository(MyWebSiteDbContext dbContext) : base(dbContext)
        { }
        public User CheckUser(string userName, string password)
        {
            return _dbContext.Set<User>().FirstOrDefault(x => x.UserName == userName && x.PassWrod == password);
        }

        public User GetWithRoles(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
