using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyWebSit.Core.Helpers;
using MyWebSite.Application.ArticleApp;
using MyWebSite.Application.UserApp;
using MyWebSite.Application.UserApp.Dtos;
using MyWebSite.Models;

namespace MyWebSite.Controllers
{
    public class HomeController : Controller
    {
        private IHostingEnvironment _hostingEnvironment;
        

        private readonly IUserAppService _userAppService;
        private IArticleAppService _articleAppService;

        public HomeController(IUserAppService userAppService, IHostingEnvironment hostingEnvironment, IArticleAppService articleAppService)
        {
            _userAppService = userAppService;
            _articleAppService = articleAppService;
            _hostingEnvironment = hostingEnvironment;
        }
        public IActionResult Index()
        {
            var model = _articleAppService.GetAll();
            return View(model);
        }

        public IActionResult Create(UserDto user)
        {
            _userAppService.Insert(user);
            return RedirectToAction("Index","Home");
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        [HttpGet]
        public IActionResult Import()
        {
            return View();
        }
        //[HttpPost("UploadFiles")]
        [HttpPost]
        public async Task<IActionResult> Import(List<IFormFile> files)
        {
            long size = files.Sum(f => f.Length);

            // full path to file in temp location
            var filePath = Path.GetTempFileName();

            foreach (var formFile in files)
            {
                if (formFile.Length > 0)
                {
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await formFile.CopyToAsync(stream);
                    }
                }
            }
            return Ok(new { count = files.Count, size, filePath });
        }

        [HttpPost]
        public IActionResult Import1(List<IFormFile> files)
        {
            long size = 0;
            var filePath = string.Empty;
            var fileName = string.Empty;
            var ffff = string.Empty;
            var guid = Guid.NewGuid();
            var files1 = Request.Form.Files;
            foreach (var file in files1)
            {
                fileName = file.FileName;                
                filePath = ContentDispositionHeaderValue
                                .Parse(file.ContentDisposition)
                                .FileName
                                .Trim('"');
                filePath = _hostingEnvironment.ContentRootPath + "\\UpLoadFiles\\";
              
                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }
                filePath += guid + ".xls";
                size += file.Length;
                using (FileStream fs = System.IO.File.Create(filePath))
                {
                    file.CopyTo(fs);

                    fs.Flush();
                }
                ffff = filePath;
                using (var stream = new FileStream(ffff, FileMode.Create))
                {
                    file.CopyTo(stream);
                }
            }
            ViewBag.Message = $"{files.Count}个文件 /{size}字节上传成功!";
            return Content(size+ fileName+"''''''''"+ffff);
        }

        public IActionResult Error() => View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
