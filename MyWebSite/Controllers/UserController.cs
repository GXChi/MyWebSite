using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MyWebSit.Core;
using MyWebSit.Core.Helpers;
using MyWebSite.Application.Dto;
using MyWebSite.Application.UserApp;
using MyWebSite.Application.UserApp.Dtos;
using MyWebSite.Domain.Entities;
using MyWebSite.Domain.IRepositories;

namespace MyWebSite.Controllers
{
    public class UserController : Controller
    {
        private IUserAppService _userAppService;
        private MyWebSiteDbContext _dbContext;
        //private readonly ILogger _logger;
        private readonly ITodoRepository _todoRepository;

        public UserController(IUserAppService userAppService, MyWebSiteDbContext dbContext, ITodoRepository todoRepository)
        {
            _userAppService = userAppService;
            _dbContext = dbContext;
            //_logger = logger;
            _todoRepository = todoRepository;
        }
        //public async Task<IActionResult> Index(string sortOrder, string searchString, string currentFilter, int? page, int pageSize = 5)
        //{
        //    ViewData["CurrentSort"] = sortOrder;
        //    ViewData["NameSortParm"] = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
        //    ViewData["DateSortParm"] = sortOrder == "user" ? "user_desc" : "user";

        //    if (searchString != null)
        //    {
        //        page = 1;
        //    }
        //    else
        //    {
        //        searchString = currentFilter;
        //    }

        //    ViewData["CurrentFilter"] = searchString;

        //    var users = from u in _dbContext.Users
        //                select u;
        //    if (!string.IsNullOrEmpty(searchString))
        //    {
        //        users = users.Where(u => u.UserName.Contains(searchString) || u.Name.Contains(searchString));
        //    }

        //    switch (sortOrder)
        //    {
        //        case "name_desc":
        //            users = users.OrderByDescending(s => s.Name);
        //            break;
        //        case "user":
        //            users = users.OrderBy(s => s.UserName);
        //            break;
        //        case "user_desc":
        //            users = users.OrderByDescending(s => s.UserName);
        //            break;
        //        default:
        //            users = users.OrderBy(s => s.Name);
        //            break;
        //    }

        //    return View(await PaginatedList<User>.CreateAsync(users.AsNoTracking(), page ?? 1, pageSize));
        //}

        //public IActionResult GetById(string id)
        //{
        //    _logger.LogInformation(LoggingEvents.GetItem, "Getting item {ID}", id);
        //    var item = _todoRepository.Find(id);
        //    if (item == null)
        //    {
        //        _logger.LogWarning(LoggingEvents.GetItemNotFound, "GetById({ID}) NOT FOUND", id);
        //        return NotFound();
        //    }
        //    return new ObjectResult(item);
        //}

        public async Task<IActionResult> Index(int? page, int pageSize = 5)
        {
            //var users = from u in _dbContext.Users
            //            select u;
            var users =  _userAppService.GetAll();
            int pageIndex = page ?? 1;
            var totalCount =  users.Count();
            var items = users.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            //var dto = new PaginatedList<User>(items, totalCount, pageIndex, pageSize);
            var totalPages = PagingHelper.GetTotalPage(totalCount,ref pageIndex, ref pageSize);
            var dto = new PaginatedList<UserDto>(items, totalCount, pageIndex, pageSize, totalPages);
            return View(dto);
        }
    }
    public class LoggingEvents
    {
        public const int GenerateItems = 1000;
        public const int ListItems = 1001;
        public const int GetItem = 1002;
        public const int InsertItem = 1003;
        public const int UpdateItem = 1004;
        public const int DeleteItem = 1005;

        public const int GetItemNotFound = 4000;
        public const int UpdateItemNotFound = 4001;
    }
}