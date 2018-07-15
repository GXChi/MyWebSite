using System;
using System.Collections.Generic;
using System.Text;

namespace MyWebSit.Domain.Common.GridPager
{
    public class OrderByItem
    {
        /// <summary>
        /// 排序字段
        /// </summary>
        public string SortCloumnName { get; set; }

        /// <summary>
        /// 排序顺序
        /// </summary>
        public string SortOrder { get; set; }
        public OrderByItem() { }

        public OrderByItem(string sortCloumnName, string sortOrder)
        {
            SortCloumnName = sortCloumnName;
            SortOrder = sortOrder;
        }
    }
}
