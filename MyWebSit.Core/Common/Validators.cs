using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace MyWebSite.Core.Common
{
    /// <summary>
    /// 验证器接口
    /// </summary>
    public interface IValidators
    {
        Boolean Validate(Object obj);

        string ErrorMessage
        {
            get;
        }
    }

    /// <summary>
    /// 字符串验证器
    /// </summary>
    public class StringValidator : IValidators
    {
        bool isNecessary;   //是否必需
        int? length;        //字符串长度       
        string regexString; //正则表达式
        string regexMessage;//正则表达式错误信息提示       

        /// <summary>
        /// 错误验证信息
        /// </summary>
        public string ErrorMessage { get; private set; }

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
                    ErrorMessage = "值不能为空";
                }
            }
            else if (length != null && tmp.Length > length)
            {
                result = false;
                ErrorMessage = String.Format("值不能超过({0})", length);
            }
            else if (!string.IsNullOrEmpty(regexString))
            {
                result = Regex.IsMatch(tmp, regexString);
                if (!result)
                {
                    result = false;
                    if (string.IsNullOrEmpty(regexMessage))
                        regexMessage = "";
                    ErrorMessage = $"{tmp}{regexMessage}";
                }
            }
            return result;
        }
    }

    /// <summary>
    /// 整数型验证器
    /// </summary>
    public class IntegerValidator : IValidators
    {
        Boolean isNecessary; //是否必须
        int? length;
        int? minValue;
        int? maxValue;

        /// <summary>
        /// 错误验证信息
        /// </summary>
        public string ErrorMessage { get; private set; }

        public IntegerValidator(Boolean isNecessary, int? minValue, int? maxValue)
        {
            this.minValue = minValue;
            this.maxValue = maxValue;
            this.isNecessary = isNecessary;
        }

        //验证
        public bool Validate(object obj)
        {
            Boolean result = true;
            string tmp = Convert.ToString(obj);
            int itmp = Convert.ToInt32(tmp);
            if (string.IsNullOrEmpty(tmp))
            {
                if (isNecessary)
                {
                    result = false;
                    ErrorMessage = "值不能为空";
                }
            }
            else if (length != null && itmp > length)
            {
                result = false;
                ErrorMessage = String.Format("值不能超过({0})", length);
            }
            else
            {                
                try
                {
                    if (maxValue != null && itmp > maxValue)
                    {
                        result = false;
                        ErrorMessage = $"值大于最大值{maxValue}";
                    }
                    if (minValue != null && itmp < minValue)
                    {
                        result = false;
                        ErrorMessage = $"值小于最小值{minValue}";
                    }

                }
                catch
                {
                    result = false;
                    ErrorMessage = "值不是整形";
                }
            }
            
            return result;
        }
    }

    /// <summary>
    /// 日期类型验证器
    /// </summary>
    public class DataTimeValidator : IValidators
    {
        bool isNecessary;    //是否必须
        string regexString;  //正则表达式
        string regexMessage; //正则表达式错误信息提示

        /// <summary>
        /// 错误验证信息
        /// </summary>
        public string ErrorMessage { get; private set; }

        public DataTimeValidator(Boolean isNecessary, string regexString, string regexMessage)
        {
            this.regexMessage = regexMessage;
            this.regexString = regexString;
            this.isNecessary = isNecessary;
        }

        /// <summary>
        /// 验证
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool Validate(object obj)
        {
            Boolean result = true;
            string tmp = Convert.ToString(obj);
            if (String.IsNullOrEmpty(tmp))
            {
                if (isNecessary)
                {
                    result = false;
                    ErrorMessage = "值不能为空";
                }
            }
            else if (!string.IsNullOrEmpty(regexString) && !string.IsNullOrEmpty(tmp))
            {
                result = Regex.IsMatch(tmp, regexString);
                if (!result)
                {
                    result = false;
                    if (string.IsNullOrEmpty(regexMessage))
                        regexMessage = "";
                    ErrorMessage = $"{regexMessage},{tmp}";
                }
            }
            else
            {
                try
                {
                    result = Npoi.Core.SS.UserModel.DateUtil.IsCellDateFormatted((Npoi.Core.SS.UserModel.ICell)obj);
                    if (!result)
                    {
                        ErrorMessage = "值不是日期类型";
                    }

                    //DateTime.TryParseExact(tmp, "yyyy-dd-mm HH:mm:ss");
                    //Convert.ToDateTime(tmp);
                }
                catch
                {
                    result = false;
                    ErrorMessage = "值不是日期类型";
                }
            }
            return result;
        }
    }

    /// <summary>
    /// 数字验证器
    /// </summary>
    public class DecimalValidator : IValidators
    {
        Boolean isNecessary;
        int? decimals;       //字符串长度

        /// <summary>
        /// 错误验证信息
        /// </summary>
        public string ErrorMessage { get; private set; }

        public DecimalValidator(bool isNecessary, int? decimals)
        {
            this.isNecessary = isNecessary;
            this.decimals = decimals;
        }


        /// <summary>
        /// 验证
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool Validate(object obj)
        {
            Boolean result = true;
            if (obj != null)
            {
                string tmp = obj.ToString();
                if (string.IsNullOrEmpty(tmp))
                {
                    if (isNecessary)
                    {
                        result = false;
                        ErrorMessage = "值不能为空";
                    }
                }
                else
                {
                    if (tmp.IndexOf(".") > 0)
                    {
                        string mTmp = tmp.Substring(tmp.IndexOf(".") + 1);
                        if (!string.IsNullOrEmpty(mTmp) && mTmp.Length > decimals)
                        {
                            result = false;
                            ErrorMessage = $"小数位不能超过{decimals.Value}";
                        }
                        if (result)
                        {
                            try
                            {
                                decimal data = Convert.ToDecimal(tmp);
                            }
                            catch
                            {
                                result = false;
                                ErrorMessage = "值不是实数类型";
                            }
                        }
                    }
                }
            }
            else
            {
                result = false;
            }
            return result;

        }
    }

}
