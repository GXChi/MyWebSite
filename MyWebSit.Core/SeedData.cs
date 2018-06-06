using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MyWebSite.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyWebSit.Core
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new MyWebSiteDbContext(serviceProvider.GetRequiredService<DbContextOptions<MyWebSiteDbContext>>()))
            {
                if (context.Users.Any())
                {
                    return;
                }

                Guid departmentId = Guid.NewGuid();

                context.Departments.Add(
                    new Department
                    {
                        Id = departmentId,
                        Name = "总部",
                        ParentId = Guid.Empty
                    });

                context.Users.Add(
                    new User
                    {
                        UserName = "admin",
                        PassWrod = "123456",
                        Name = "超级管理员",
                        DepartmentId = departmentId
                    });

                //context.Roles.Add(
                //    new Role
                //    {

                //    });
                context.SaveChanges();

            }
        }
    }
}
