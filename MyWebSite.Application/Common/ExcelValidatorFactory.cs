using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace MyWebSite.Application.Common
{
    public class ExcelValidatorFactory
    {
        private string configPath = AppDomain.CurrentDomain.BaseDirectory;
        private string configXMLFile = "ExcelValidatorCfg.xml";
        private static ExcelValidatorFactory instance = null;
        private static XmlDocument xmlDoc = new XmlDocument();

        public ExcelValidatorFactory()
        {
            try
            {
                xmlDoc.Load(configPath + configXMLFile);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

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

                XmlNodeList colList = excel.GetElementsByTagName("Col");

                foreach (XmlElement colNode in colList)
                {
                    string colNo = colNode.GetAttribute("ColNo");
                    string id = colNode.GetAttribute("ID");                   
                    string desc = colNode.GetAttribute("Desc");
                    string type = colNode.GetAttribute("Type");
                    string necessary = colNode.GetAttribute("Necessary");
                    string regex = colNode.GetAttribute("Regex");
                    string regexMessage = colNode.GetAttribute("RegexMessage");
                    int? length = null;
                    if (!string.IsNullOrEmpty(colNode.GetAttribute("Length")))
                        length = Convert.ToInt32(colNode.GetAttribute("Length"));
                    int? minValue = string.IsNullOrEmpty(colNode.GetAttribute("MinValue")) ? int.MinValue : Convert.ToInt32(colNode.GetAttribute("MinValue"));
                    container.ColsName.Add(Convert.ToInt32(colNo),id);
                    container.ColsDesc.Add(Convert.ToInt32(colNo),desc);
                    container.ColsType.Add(Convert.ToInt32(colNo),type.ToUpper());

                    IValidators validator = null;
                    switch (type.ToUpper())
                    {
                        case "STRING":
                            validator = new StringValidator(Convert.ToBoolean(necessary), Convert.ToInt32(length), regex, regexMessage);
                            container.FormatValidators.Add(Convert.ToInt32(colNo), validator);
                            break;
                        default:
                            throw new Exception();
                    }
                }

                XmlNodeList extList = excel.GetElementsByTagName("ExtValidator");
                foreach (XmlElement extNode in extList)
                {
                    InvokerInfo info = new InvokerInfo();
                    info.Assembly = extNode.GetAttribute("Assembly");

                    foreach (XmlElement paraNode in extNode.ChildNodes)
                    {
                        info.ParamsColNo.Add(Convert.ToInt32(paraNode.GetAttribute("ValueColNo")));
                        //info.ParamsType.Add(Convert)
                        container.ExtValidators.Add(info);
                    }                  
                }
                return container;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static ExcelValidatorContainer GetValidator(string excelID)
        {
            if (instance == null)
            {
                instance = new ExcelValidatorFactory();
            }
            return instance.CreateValidator(excelID);
           
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
