using Microsoft.AspNetCore.Hosting;
using MyWebSite.Core.Common;
using Npoi.Core.HSSF.UserModel;
using Npoi.Core.SS.UserModel;
using Npoi.Core.XSSF.UserModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Reflection;
using System.Text;

namespace MyWebSit.Core.Helpers
{
    public class ImportExcelHelper
    {
       
        string dataSheetName = "Sheet1";
        IWorkbook workbook;
        ExcelValidatorContainer container;
        Hashtable typeHashtable = new Hashtable();
        Hashtable methodHashtable = new Hashtable();
  
        public ImportExcelHelper(string excelID, string importFileName)
        {
           
            FileStream file = new FileStream(importFileName, FileMode.Open, FileAccess.Read); 
            
            string fileType = importFileName.Substring(importFileName.LastIndexOf("."));
            if (fileType == ".xls")
            {
                workbook = new HSSFWorkbook(file);
            }
            else if (fileType == ".xlsx")
            {
                workbook = new XSSFWorkbook(file);
            }
            container = ExcelValidatorFactory.GetValidator(excelID);
           
        }

        public Boolean Validate(int allowErrorNum, out List<string> errMsgList)
        {
            Boolean result = true;
            errMsgList = new List<string>();

            //创建扩展验证器
            foreach (InvokerInfo info in container.ExtValidators)
            {
                try
                {
                    Assembly ass = Assembly.Load(info.Assembly);
                    Type t = ass.GetType(info.ClassName, true);
                    typeHashtable.Add(info.ClassName, Activator.CreateInstance(t));

                    List<Type> typeList = new List<Type>();
                    for (int i = 0; i < info.ParamsType.Count; i++)
                    {
                        switch (info.ParamsType[i].ToUpper())
                        {
                            case "STRING":
                                typeList.Add(typeof(string));
                                break;
                            default:
                                throw new Exception();
                        }
                    }
                    MethodInfo methodInfo = t.GetMethod(info.MethodName, typeList.ToArray());
                    methodHashtable.Add(info.MethodName, methodInfo);
                }
                catch (Exception ex)
                {
                    result = false;
                    throw ex;
                }
            }

            //验证工作表是否存在
            ISheet st = null;
            st = workbook.GetSheet(dataSheetName);
            bool tr = true;

            if (st == null)
            {
                errMsgList.Add($"文件中必须存在名为{dataSheetName}的工作表");
                return result;
            }

           
            IEnumerator it = st.GetRowEnumerator();
            //验证表头是否正确
            try
            {
                // 判断是否添加动态列方法
                if (container.DynamicTable)
                    AddDynamicCols(st);

                var headRowNo = container.HeadRowNo;
                foreach (KeyValuePair<int,string> name in container.ColsDesc)
                {
                    if (!name.Value.Trim().Equals(st.GetRow(headRowNo).GetCell(name.Key).StringCellValue.Trim()))
                    {
                        result = false;
                        errMsgList.Add($"表格式不正确。第{name.Key}列应该是{name.Value}");
                        return result;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            //验证列表中的数据是否正确
            for (int i = container.DataStartRowNo; i < st.LastRowNum; i++)
            {
                //格式验证
                try
                {
                    foreach (KeyValuePair<int,IValidators> v in container.FormatValidators)
                    {
                        IValidators validator = v.Value;
                        if (!validator.Validate(st.GetRow(i).GetCell(v.Key)))
                        {
                            result = false;
                            errMsgList.Add($"第{i}行/第{v.Key}列格式验证不通过，原因：{validator.ErrorMessage}");
                            if (errMsgList.Count >= allowErrorNum)
                                return result;
                        }
                    }
                }
                catch
                {
                    result = false;
                    errMsgList.Add("数据格式不正确，数据格式验证出现异常");
                    return result;
                }

                //扩展验证（存在性验证）
                foreach (InvokerInfo info in container.ExtValidators)
                {
                    try
                    {
                        List<object> objectList = new List<object>();

                        for (int r = 0; r < info.ParamsType.Count; r++)
                        {
                            switch (info.ParamsType[r].ToUpper())
                            {
                                case "STRING":
                                    objectList.Add(Convert.ToString(st.GetRow(r).GetCell(info.ParamsColNo[r])));
                                    break;
                                default:
                                    throw new Exception();
                            }
                        }
                        MethodInfo methodInfo = (MethodInfo)methodHashtable[info.MethodName];
                        FastInvoke.FastInvokeHandler fastInvoker = FastInvoke.GetMethodInvoker(methodInfo);
                        string extMsg = (string)fastInvoker(typeHashtable[info.ClassName], objectList.ToArray());
                        if (!string.IsNullOrEmpty(extMsg))
                        {
                            result = false;
                            errMsgList.Add($"第{i}行验证通不过 原因：{extMsg}");
                            if (errMsgList.Count >= allowErrorNum)
                                return result;
                        }
                    }
                    
                    catch (Exception ex)
                    {
                        result = false;
                        throw ex;
                    }
                }
            }

            return result;
        }

        public DataTable GetDataTable()
        {
            DataTable dt = new DataTable();
            foreach (KeyValuePair<int,string> col in container.ColsName)
            {
                dt.Columns.Add(col.Value);
            }

            ISheet st = workbook.GetSheet(dataSheetName);
            IEnumerator it = st.GetRowEnumerator();

            for (int i = container.DataStartRowNo ; i <= st.LastRowNum ; i++)
            {
                int j = 0;
                DataRow dr = dt.NewRow();

                foreach (KeyValuePair<int, string> col in container.ColsName)
                {
                    string type = container.ColsType[col.Key];
                    if (st.GetRow(i).GetCell(col.Key) != null && "DATEIME".Equals(type.ToUpper()) && st.GetRow(i).GetCell(col.Key).CellType == CellType.Numeric)
                    {
                        dr[j] = st.GetRow(i).GetCell(col.Key).DateCellValue;
                    }
                    else
                    {
                        dr[i] = st.GetRow(i).GetCell(col.Key);
                    }
                    j++;
                }
                dt.Rows.Add(dr);
            }
            return dt;
        }

        public void AddDynamicCols(ISheet st)
        {
            int count = st.GetRow(container.HeadRowNo).Cells.Count;
        }
    }
}
