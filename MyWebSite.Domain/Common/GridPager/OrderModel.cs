using System;
using System.Collections.Generic;
using System.Text;

namespace MyWebSit.Domain.Common.GridPager
{
    /// <summary>
    /// 用户自动收集排序条件的Model
    /// </summary>
    public class OrderModel
    {
        /// <summary>
        /// 排序条件
        /// </summary>
        public List<OrderByItem> Items { get; set; }

        public OrderModel()
        {
            Items = new List<OrderByItem>();
        }
    }
}
