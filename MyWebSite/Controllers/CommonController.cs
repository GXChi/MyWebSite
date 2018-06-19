using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MyWebSite.Controllers
{
    public class CommonController : Controller
    {
        public IHostingEnvironment _hostingEnvironment;

        public CommonController(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 上传Excel文件到服务器
        /// </summary>
        /// <param name="files">excel文件</param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult UploadExcelFiles()
        {
            var files = Request.Form.Files; //从前端接受数据
            Guid guid = Guid.NewGuid();
            string filePath = string.Empty;
            foreach (var file in files)
            {
                string fileExt = Path.GetExtension(file.FileName); //获得文件扩展名
                if (fileExt == ".xls" || fileExt == ".xlsx")
                {
                    //string fileExt = formFile.FileName.Substring(formFile.FileName.LastIndexOf("."));//获取文件扩展名

                    //上传文件保存路径,如果不存在,则新增文件夹
                    filePath = _hostingEnvironment.ContentRootPath + "\\UpLoadFiles\\";                     
                    if (!Directory.Exists(filePath))
                    {
                        Directory.CreateDirectory(filePath);
                    }

                    filePath += guid + fileExt; //新文件路径+新名称
                    if (file.Length > 0)
                    {
                        //using (FileStream fileStream = System.IO.File.Create(filePath))
                        //{
                        //    file.CopyTo(fileStream);
                        //    fileStream.Flush();
                        //}

                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            file.CopyTo(fileStream);
                        }
                    }
                }

            }
            return Content(filePath);
        }   
               
    }
}
