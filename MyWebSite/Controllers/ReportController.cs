
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace MyWebSite.Controllers
{
    public class ReportController : Controller
    {
        public IActionResult Index(ref int pageIndex,ref int pageSize, out int totalCount, out int totalPage)
        {
            pageIndex = 1;
            pageSize = 2;
            totalCount = 1;
            totalPage = 2;
            return View();
        }
    }
}