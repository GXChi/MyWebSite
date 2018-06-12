using MyWebSite.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyWebSite.Domain.IRepositories
{
    public interface IUserRepository : IRepository<User>
    {
        User CheckUser(string userName, string password);

        User GetWithRoles(Guid id);
       
    }
}
