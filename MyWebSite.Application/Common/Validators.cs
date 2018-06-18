using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace MyWebSite.Application.Common
{
    /// <summary>
    /// 验证器基类
    /// </summary>
    public interface IValidators
    {
        Boolean Validate(Object obj);

        string Errormessage
        {
            get;
        }
    }

    public class StringValidator : IValidators
    {
        bool isNecessary;   //是否必需
        int? length;        //字符串长度       
        string regexString; //正则表达式
        string regexMessage;//正则表达式错误信息提示       
        string errorMessage;//错误信息

        /// <summary>
        /// 错误验证信息
        /// </summary>
        public string Errormessage { get { return errorMessage; } }

        public StringValidator(bool isNecessary, int? length, string regexString, string regexMessage)
        {
            this.isNecessary = isNecessary;
            this.length = length;
            this.regexMessage = regexMessage;
            this.regexString = regexString;
        }
        
        /// <summary>
        /// 验证
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool Validate(object obj)
        {
            bool result = true;
            string tmp = Convert.ToString(obj).Trim();


            if (string.IsNullOrEmpty(tmp))
            {
                if (isNecessary)
                {
                    result = false;
                    errorMessage = "值不能为空";
                }
            }
            else if (length != null && tmp.Length > length)
            {
                result = false;
                errorMessage = String.Format("值超长({0})", length);
            }
            else if (!string.IsNullOrEmpty(regexString))
            {
                result = Regex.IsMatch(tmp, regexString);
                if (!result)
                {
                    result = false;
                    if (string.IsNullOrEmpty(regexMessage))
                        regexMessage = "";
                    errorMessage = $"{tmp}{regexMessage}";
                }
            }
            return result;
        }
    }

}
