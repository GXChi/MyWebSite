using System;
using System.Collections.Generic;
using System.Text;

namespace MyWebSite.Domain.Common.Query
{
    public enum QueryMethod
    {
        /// <summary>
        /// 等于
        /// </summary>
        Equal = 0,

        /// <summary>
        /// 不等于
        /// </summary>
        NotEqual = 1,

        /// <summary>
        /// 小于
        /// </summary>
        LessThan = 2,

        /// <summary>
        /// 小于等于
        /// </summary>
        LessThanOrEqual = 3,

        /// <summary>
        /// 大于
        /// </summary>
        GreaterThan = 4,

        /// <summary>
        /// 大于等于
        /// </summary>
        GreaterThanOrEqual = 5,

        /// <summary>
        /// 输入一个时间获取当天的时间块操作，ToSql未实现，仅实现了IQueryable
        /// </summary>
        Between = 6,

        /// <summary>
        /// 处理Like的问题
        /// </summary>
        Like = 7,

        /// <summary>
        /// 处理In的问题
        /// </summary>
        In = 8,
        StartsWith = 9,
        EndsWith = 10,

        /// <summary>
        /// Like
        /// </summary>
        ExLike = 11,

        /// <summary>
        /// In
        /// </summary>
        ExIn = 12
    }
}
