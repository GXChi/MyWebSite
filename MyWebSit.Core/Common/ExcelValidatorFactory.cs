using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

namespace MyWebSite.Core.Common
{
    /// <summary>
    /// Excel验证工厂
    /// </summary>
    public class ExcelValidatorFactory
    {
        private string configPath = Directory.GetCurrentDirectory();
        private string configXMLFile = "ExcelValidatorCfg.xml";
        private static ExcelValidatorFactory instance = null;
        private static XmlDocument xmlDoc = new XmlDocument();

        public ExcelValidatorFactory()
        {
            try
            {
                //xml文件路径
                xmlDoc.Load(configPath + "\\" + configXMLFile);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 实例化ExcelValidatorFactory,获取验证器
        /// </summary>
        /// <param name="excelID">xml节点Id</param>
        /// <returns></returns>
        public static ExcelValidatorContainer GetValidator(string excelID)
        {
            if (instance == null)
            {
                instance = new ExcelValidatorFactory();
            }
            return instance.CreateValidator(excelID);

        }

        /// <summary>
        /// 创建验证器
        /// </summary>
        /// <param name="excelID">xml节点Id</param>
        /// <returns></returns>
        public ExcelValidatorContainer CreateValidator(string excelID)
        {
            try
            {
                XmlElement excel = GetXmlNodeById("Excel", "ID", excelID);
                if (excel == null)
                {
                    throw new Exception($"找不到{excelID}的配置信息");
                }

                ExcelValidatorContainer container = new ExcelValidatorContainer();
                container.HeadRowNo = Convert.ToInt32(excel.GetAttribute("HeadRowNo"));
                container.DataStartRowNo = Convert.ToInt32(excel.GetAttribute("DataStartRowNo"));
                container.DynamicTable = Convert.ToBoolean(excel.GetAttribute("DynamicTable"));
                container.DynamicStartColNo = Convert.ToInt32(excel.GetAttribute("DynamicStartColNo")); ;

                XmlNodeList colList = excel.GetElementsByTagName("Col");

                foreach (XmlElement colNode in colList)
                {
                    string id = colNode.GetAttribute("ID");
                    string colNo = colNode.GetAttribute("ColNo");
                    string desc = colNode.GetAttribute("Desc");
                    string type = colNode.GetAttribute("Type");
                    string necessary = colNode.GetAttribute("Necessary");
                    int? length = null;
                    if (!string.IsNullOrEmpty(colNode.GetAttribute("Length")))
                        length = Convert.ToInt32(colNode.GetAttribute("Length"));

                    string regex = colNode.GetAttribute("Regex");
                    string regexMessage = colNode.GetAttribute("RegexMessage");
                    int? minValue = string.IsNullOrEmpty(colNode.GetAttribute("MinValue")) ? int.MinValue : Convert.ToInt32(colNode.GetAttribute("MinValue"));
                    int? maxValue = string.IsNullOrEmpty(colNode.GetAttribute("MaxValue")) ? int.MaxValue : Convert.ToInt32(colNode.GetAttribute("MaxValue"));
                    int decimals = 0;
                    int.TryParse(colNode.GetAttribute("decimals"), out decimals);

                    container.ColsName.Add(Convert.ToInt32(colNo), id);
                    container.ColsDesc.Add(Convert.ToInt32(colNo), desc);
                    container.ColsType.Add(Convert.ToInt32(colNo), type.ToUpper());

                    IValidators validator = null;
                    switch (type.ToLower())
                    {
                        case "string":
                            validator = new StringValidator(Convert.ToBoolean(necessary), Convert.ToInt32(length), regex, regexMessage);
                            container.FormatValidators.Add(Convert.ToInt32(colNo), validator);
                            break;
                        case "int":
                            validator = new IntegerValidator(Convert.ToBoolean(necessary), minValue, maxValue);
                            container.FormatValidators.Add(Convert.ToInt32(colNo), validator);
                            break;
                        case "datetime":
                            validator = new DataTimeValidator(Convert.ToBoolean(necessary), regex, regexMessage);
                            container.FormatValidators.Add(Convert.ToInt32(colNo), validator);
                            break;
                        case "decimal":
                            validator = new DecimalValidator(Convert.ToBoolean(necessary), decimals);
                            container.FormatValidators.Add(Convert.ToInt32(colNo), validator);
                            break;
                        default:
                            throw new Exception();
                    }
                }

                #region 加载验证方法 没用到
                //加载验证方法
                XmlNodeList extList = excel.GetElementsByTagName("ExtValidator");
                foreach (XmlElement extNode in extList)
                {
                    InvokerInfo info = new InvokerInfo();
                    info.Assembly = extNode.GetAttribute("Assembly");
                    info.ClassName = extNode.GetAttribute("Class");
                    info.MethodName = extNode.GetAttribute("Method");

                    foreach (XmlElement paraNode in extNode.ChildNodes)
                    {
                        info.ParamsColNo.Add(Convert.ToInt32(paraNode.GetAttribute("ValueColNo")));
                        info.ParamsType.Add(paraNode.GetAttribute("Type"));

                    }
                    container.ExtValidators.Add(info);
                }
                #endregion

                return container;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static XmlElement GetXmlNodeById(string tagName, string attribute, string value)
        {
            foreach (XmlElement node in xmlDoc.GetElementsByTagName(tagName))
            {
                if (value.Equals(node.GetAttribute(attribute)))
                {
                    return node;
                }
            }
            return null;
        }

    }
}
