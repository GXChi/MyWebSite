using MyWebSite.Domain.Common.Query;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyWebSite.Domain.Common.GridPager
{
    /// <summary>
    /// 用户自动收集搜索条件的Model
    /// </summary>
    public class QueryModel
    {
        public QueryModel()
        {
            Items = new List<ConditionItem>();
        }

        /// <summary>
        /// 查询条件
        /// </summary>
        public List<ConditionItem> Items { get; set; }
    }
}
