using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Net.Http.Headers;
using MyWebSit.Core.Helpers;
using MyWebSite.Application.Servers;
using MyWebSite.Attributes;
using MyWebSite.Models;
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

        public IActionResult SaveImportFile(string excelPath)
        {
            try
            {
                string saveExcelFilePath = excelPath; //服务器保存Excel文件的路径
                string errorMsg = string.Empty;       //验证错误信息
              
                DataTable dataTable = ExcelServer.GetDataTableByExcel(saveExcelFilePath,out errorMsg);
                if (!string.IsNullOrEmpty(errorMsg))
                {
                    return Json(errorMsg);
                }
                //dataTable保存到数据库，或下载


                //ExcelServer.DeleteFile(excelPath);
                return View(excelPath);
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }


    }
}