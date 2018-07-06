using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyWebSit.Core;
using MyWebSite.Application.Dto;
using MyWebSite.Application.UserApp;
using MyWebSite.Application.UserApp.Dtos;
using MyWebSite.Domain.Entities;

namespace MyWebSite.Controllers
{
    public class UserController : Controller
    {
        private IUserAppService _userAppService;
        private MyWebSiteDbContext _dbContext;

        public UserController(IUserAppService userAppService, MyWebSiteDbContext dbContext)
        {
            _userAppService = userAppService;
            _dbContext = dbContext;
        }
        public async Task<IActionResult> Index(string sortOrder, string searchString, string currentFilter, int? page, int pageSize = 5)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParm"] = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["DateSortParm"] = sortOrder == "user" ? "user_desc" : "user";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;

            var users = from u in _dbContext.Users
                        select u;
            if (!string.IsNullOrEmpty(searchString))
            {
                users = users.Where(u => u.UserName.Contains(searchString) || u.Name.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    users = users.OrderByDescending(s => s.Name);
                    break;
                case "user":
                    users = users.OrderBy(s => s.UserName);
                    break;
                case "user_desc":
                    users = users.OrderByDescending(s => s.UserName);
                    break;
                default:
                    users = users.OrderBy(s => s.Name);
                    break;
            }

            return View(await PaginatedList<User>.CreateAsync(users.AsNoTracking(), page ?? 1, pageSize));
        }
    }
}