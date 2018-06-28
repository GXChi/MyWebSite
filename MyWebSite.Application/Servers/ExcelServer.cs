using MyWebSit.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;

namespace MyWebSite.Application.Servers
{
    public static class ExcelServer
    {
        public static DataTable GetDataTableByExcel(string saveExcelFilePath, out string errorMsg)
        {
            errorMsg = string.Empty;

            if(string.IsNullOrEmpty(saveExcelFilePath))
            {
                errorMsg = "未找到导入的Excel文件，请重新选择要导入Excel文件";
            }

            string excelID = "Import_Excel";
            ImportExcelHelper improtExcelHelper = new ImportExcelHelper(excelID, saveExcelFilePath);

            List<string> msgList = new List<string>();
            improtExcelHelper.Validate(20, out msgList);

            if (msgList != null && msgList.Any())
            {
                //验证报错
                errorMsg = string.Join("<br />", msgList);
            }

            DataTable dataTable = improtExcelHelper.GetDataTable();
            return dataTable;

        }

        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <returns></returns>
        public static void DeleteFile(string path)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }
    }
}
