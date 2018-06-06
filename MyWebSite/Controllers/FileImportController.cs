using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;

namespace MyWebSite.Controllers
{
    public class FileImportController : Controller
    {
        private IHostingEnvironment _hostingEnvironment;

        public FileImportController(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 导入Excel文件
        /// </summary>
        /// <param name="excelFile"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Import(IFormFile excelFile)
        {
            string sWebRootFolder = _hostingEnvironment.WebRootPath;
            string sFileName = $"{Guid.NewGuid()}.slsx";
            FileInfo file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
            try
            {
                using (FileStream fs = new FileStream(file.ToString(), FileMode.Create))
                {
                    excelFile.CopyTo(fs);
                    fs.Flush();
                }
                using (ExcelPackage package = new ExcelPackage(file))
                {
                    StringBuilder sb = new StringBuilder();
                    ExcelWorksheet workSheet = package.Workbook.Worksheets[1];
                    int rowCount = workSheet.Dimension.Rows;
                    int colCount = workSheet.Dimension.Columns;
                    bool bHeaderRow = true;

                    for (int row = 1; row < rowCount; row++)
                    {
                        for (int col = 1; col <= colCount; col++)
                        {
                            if (bHeaderRow)
                            {
                                sb.Append(workSheet.Cells[row, col].Value.ToString() + "\t");
                            }
                            else
                            {
                                sb.Append(workSheet.Cells[row, col].Value.ToString() + "\t");
                            }
                        }
                        sb.Append(Environment.NewLine);
                    }
                    return Content(sb.ToString());                   
                }
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }            
        }
    }
}