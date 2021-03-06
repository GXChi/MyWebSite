﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MyWebSit.Core;
using MyWebSite.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
                    return;   // 已经初始化过数据，直接返回
                }

                context.Articles.AddRange(
                    new Article
                    {
                        Title = "头条",
                        Content = "地方骄傲老规矩多",
                        Url = "dddd"
                    },
                    new Article
                    {
                        Title = "头条",
                        Content = "地方骄傲老规矩",
                        Url = "dddd"
                    },
                     new Article
                     {
                         Title = "头条",
                         Content = "地方骄傲老规矩多",
                         Url = "dddd"
                     },
                      new Article
                      {
                          Title = "头条",
                          Content = "地方骄",
                          Url = "dddd"
                      },
                      new Article
                      {
                          Title = "头条",
                          Content = "地方骄傲老",
                          Url = "dddd"
                      }
                );

                
                Guid departmentId = Guid.NewGuid();
                //增加一个部门
                context.Departments.Add(
                   new Department
                   {
                       Id = departmentId,
                       Name = "Fonour集团总部",
                       ParentId = Guid.Empty
                   }
                );
                //增加一个超级管理员用户
                context.Users.AddRange(
                     new User
                     {
                         UserName = "admin",
                         PassWrod = "123456", //暂不进行加密
                         Name = "超级管理员",
                         DepartmentId = departmentId
                     },
                     new User
                     {
                         UserName = "admin1",
                         PassWrod = "123456", //暂不进行加密
                         Name = "超级管理员1",
                         DepartmentId = departmentId
                     },
                      new User
                      {
                          UserName = "admin2",
                          PassWrod = "123456", //暂不进行加密
                          Name = "超级管理员2",
                          DepartmentId = departmentId
                      },
                      new User
                      {
                          UserName = "admin3",
                          PassWrod = "123456", //暂不进行加密
                          Name = "超级管理员3",
                          DepartmentId = departmentId
                      },
                       new User
                       {
                           UserName = "wangwu",
                           PassWrod = "123456", //暂不进行加密
                           Name = "王五",
                           DepartmentId = departmentId
                       },
                        new User
                        {
                            UserName = "lisi",
                            PassWrod = "123456", //暂不进行加密
                            Name = "李四",
                            DepartmentId = departmentId
                        },
                        new User
                        {
                            UserName = "zhangsang",
                            PassWrod = "123456", //暂不进行加密
                            Name = "张三",
                            DepartmentId = departmentId
                        }
                );
                //增加四个基本功能菜单
                context.Menus.AddRange(
                   new Menu
                   {
                       Name = "组织机构管理",
                       Code = "Department",
                       SerialNumber = 0,
                       ParentId = Guid.Empty,
                       Icon = "fa fa-link"
                   },
                   new Menu
                   {
                       Name = "角色管理",
                       Code = "Role",
                       SerialNumber = 1,
                       ParentId = Guid.Empty,
                       Icon = "fa fa-link"
                   },
                   new Menu
                   {
                       Name = "用户管理",
                       Code = "User",
                       SerialNumber = 2,
                       ParentId = Guid.Empty,
                       Icon = "fa fa-link"
                   },
                   new Menu
                   {
                       Name = "功能管理",
                       Code = "Department",
                       SerialNumber = 3,
                       ParentId = Guid.Empty,
                       Icon = "fa fa-link"
                   }
                );

                context.SaveChanges();
            }
        }
    }
}
