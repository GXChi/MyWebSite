using MyWebSite.Domain.Common.GridPager;

namespace MyWebSit.Domain.Common.GridPager
{
    public class GridPagerObject
    {
        /// <summary>
        /// 页数
        /// </summary>
        public int CurrentPage { get; set;}

        /// <summary>
        /// 每页行数
        /// </summary>
        public int RowsPerPage { get; set; }

        /// <summary>
        /// 排序字段名称
        /// </summary>
        public string SortCloumnName { get; set; }

        /// <summary>
        /// 排序方式，如排序（asc），倒叙（desc）
        /// </summary>
        public string SortOrder { get; set; }

        /// <summary>
        /// 查询条件集合
        /// </summary>
        public QueryModel QueryModel { get; set; }

        /// <summary>
        /// 排序集合
        /// </summary>
        public OrderModel OrderModel { get; set; }
    }
}
