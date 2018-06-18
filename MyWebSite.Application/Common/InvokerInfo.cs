using System;
using System.Collections.Generic;
using System.Text;

namespace MyWebSite.Application.Common
{
    public class InvokerInfo
    {
        /// <summary>
        /// 组件名
        /// </summary>
        public string Assembly { get; set; }

        /// <summary>
        /// 类名
        /// </summary>
        public string ClassName { get; set; }

        /// <summary>
        /// 方法名
        /// </summary>
        public string MethodName { get; set; }

        /// <summary>
        /// 参数类型
        /// </summary>
        public List<string> ParamsType = new List<string>();


        /// <summary>
        /// 参数列
        /// </summary>
        public List<int> ParamsColNo = new List<int>();

    }
}
