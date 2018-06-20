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
        string errorMessage;//错误信息

        /// <summary>
        /// 错误验证信息
        /// </summary>
        public string ErrorMessage { get { return errorMessage; } }

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

    /// <summary>
    /// 整数型验证器
    /// </summary>
    public class IntegerValidator : IValidators
    {
        Boolean isNecessary; //是否必须
        int? length;
        int? minValue;
        int? maxValue;
        string errormessage;

        public string ErrorMessage
        {
            get { return errormessage; }
        }

        public IntegerValidaotr(Boolean isNecessary, int? minValue, int? maxValue)
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
            if (string.IsNullOrEmpty(tmp))
            {
                if (isNecessary)
                {
                    result = false;
                    errormessage = "值不能为空";
                }
            }
            else
            {
                int itmp;
                try
                {
                    itmp = Convert.ToInt32(tmp);
                    if (maxValue != null && itmp > maxValue)
                    {
                        result = false;
                        errormessage = $"值大于最大值{maxValue}";
                    }
                    if (minValue != null && itmp < minValue)
                    {
                        result = false;
                        errormessage = $"值小于最小值{minValue}";
                    }

                }
                catch (Exception ex)
                {
                    result = false;
                    errormessage = "值不是整形";
                }
            }
            return result;
        }
    }


    public class DataTimeValidator : IValidators
    {
        Boolean isNecessary; //是否必须
        string regexString;  //正则表达式
        string regexMessage; //正则表达式错误信息提示
        string errorMessage;

        public string ErrorMessage { get { return errorMessage; } }

        public DataTimeValidator(Boolean isNecessary, string regexString, string regexMessage)
        {
            this.regexMessage = regexMessage;
            this.regexString = regexString;
            this.isNecessary = isNecessary;
        }

        /// <summary>
        /// 日期类型验证器
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
                    errorMessage = "值不能为空";
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
                    errorMessage = $"{regexMessage},{tmp}";
                }
            }
            else
            {
                try
                {
                    result = NPOI.SS.UserModel.DateUtil.IsCellDateFormatted((NPOI.SS.UserModel.ICell)obj);
                    if (!result)
                    {
                        errorMessage = "值不是日期类型";
                    }
                }
                catch
                {
                    result = false;
                    errorMessage = "值不是日期类型";
                }
            }
            return result;
        }


        public class DecimalValidator : IValidators
        {
            Boolean isNecessary;
            int? decimals;       //字符串长度
            string errorMessage;

            public string ErrorMessage { get { return errorMessage; } }

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
                            errorMessage = "值不能为空";
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
                                errorMessage = $"小数位不能超过{decimals.Value}";
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
                                    errorMessage = "值不是实数类型";
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
}
