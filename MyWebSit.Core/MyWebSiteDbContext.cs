using Microsoft.EntityFrameworkCore;
using MyWebSite.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyWebSit.Core
{
    public class MyWebSiteDbContext : DbContext
    {
        public MyWebSiteDbContext(DbContextOptions<MyWebSiteDbContext> options) : base(options)
        { }

        public DbSet<User> Users { get; set; }

        public DbSet<Department> Departments { get; set; }

        public DbSet<Role>  Roles { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    
    }
}
