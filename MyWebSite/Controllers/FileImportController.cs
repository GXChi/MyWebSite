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

        /// <summary>
        /// 导入Excel
        /// </summary>
        /// <param name="excelFile"></param>
        /// <returns></returns>
        [HttpPost]
        public ContentResult ImportDataExcelUploadify(IFormFile excelFile)
        {
            Guid guid = Guid.NewGuid();
            string filePath = "";
            string ext = Path.GetExtension(excelFile.FileName);
            if (ext == ".xls" || ext == ".xlsx")
            {
                string fileExt = excelFile.FileName.Substring(excelFile.FileName.LastIndexOf("."));
                filePath = _hostingEnvironment.ContentRootPath + "\\UpLoadFiles\\";

                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }
                filePath += guid + fileExt;
                if (excelFile.Length > 0)
                {
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        excelFile.CopyTo(stream);
                    }
                }
            }

            //long size = excelFile.Sum(f => f.Length);
            //Guid guid = Guid.NewGuid();
            //string filePath = "";
            //foreach (var formFile in excelFile)
            //{
            //    string ext = Path.GetExtension(formFile.FileName);
            //    if (ext == ".xls" || ext == ".xlsx")
            //    {
            //        string fileExt = formFile.FileName.Substring(formFile.FileName.LastIndexOf("."));
            //        filePath = _hostingEnvironment.ContentRootPath + "\\UpLoadFiles\\";

            //        if (!Directory.Exists(filePath))
            //        {
            //            Directory.CreateDirectory(filePath);
            //        }
            //        filePath += guid + fileExt;
            //        if (formFile.Length > 0)
            //        {
            //            using (var stream = new FileStream(filePath, FileMode.Create))
            //            {
            //                formFile.CopyTo(stream);
            //            }
            //        }
            //    }

        //}
            return Content(filePath);           
        }


        public IActionResult SaveImportFile(string excelFile)
        {
            try
            {
                string saveExcelFilePath = excelFile; //服务器保存Excel文件的路径
                string errorMsg = string.Empty;       //验证错误信息
              
                DataTable dataTable = GetDataTableByExcel(out saveExcelFilePath,out errorMsg);
                if (!string.IsNullOrEmpty(errorMsg))
                {
                    return Json(errorMsg);
                }
                //DeleteFile(excelFile);
                return View(excelFile);
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }

        /// <summary>
        /// 根据导入的Excel文件生成DataTable表
        /// </summary>
        /// <param name="excelFile"></param>
        /// <param name="saveExcelFilePath"></param>
        /// <param name="errorMsg"></param>
        /// <returns></returns>
        public DataTable GetDataTableByExcel(out string saveExcelFilePath, out string errorMsg)
        {
            saveExcelFilePath = string.Empty;
            errorMsg = string.Empty;
            string excelIDName = "Import_Excel";

            ImportExcelHelper helper = new ImportExcelHelper(excelIDName, saveExcelFilePath);
            List<string> msgList = new List<string>();
            helper.Validate(20, out msgList);
            DataTable dataTabel = helper.GetDataTable();
            return dataTabel;
        }

        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="path">文件路径</param>
        private static void DeleteFile(string path)
        {
            if(System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }
        }

    }
}