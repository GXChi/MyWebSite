using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyWebSit.Core;
using MyWebSite.Application.UserApp;

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
        public async Task<IActionResult> Index(string sortOrder,string searchString)
        {
            ViewData["NameSortParm"] = string.IsNullOrEmpty(sortOrder) ? "name_desc": "";
            ViewData["DateSortParm"] = sortOrder == "Date" ? "data_desc" : "Date";

            var users = from u in _dbContext.Users
                        select u;
            if (!string.IsNullOrEmpty(searchString))
            {
                users = users.Where(u => u.UserName.Contains(searchString) || u.Name.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    users = users.OrderBy(s => s.UserName);
                    break;
                case "user_name":
                    users = users.OrderBy(s => s.UserName);
                    break;
                default:
                    users = users.OrderByDescending(s => s.UserName);
                    break;
            }

            return View(await users.AsNoTracking().ToListAsync());
        }
    }
}