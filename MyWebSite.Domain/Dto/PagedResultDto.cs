using System;
using System.Collections.Generic;
using System.Text;

namespace MyWebSite.Domain.Dto
{
    /// <summary>
    /// 用于承载集合分页数据
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PagedResultDto<T> : Pagination
    {
        public IList<T> Items { get; set; }
        public object DynamicItems { get; set; }

        /// <summary>
        /// 实例化一个<see cref="PagedResultDto{T}"/> 对象
        /// </summary>
        /// <param name="items">集合数据</param>
        /// <param name="pageIndex">单前页码</param>
        /// <param name="totalCount">数据总数</param>
        /// <param name="pageSize">分页大小</param>
        public PagedResultDto(IList<T> items, int pageIndex, int totalCount,int pageSize) : base(pageIndex, totalCount, pageSize)
        {
            Items = items;
            DynamicItems = items;

            TotalCount = totalCount;
        }


        public PagedResultDto(IList<T> items, int pageIndex, int pageSize, int totalCount, int totalPage)
        {
            Items = items;
            DynamicItems = items;

            CurrentPage = pageIndex;
            CurrentSize = pageSize;
            TotalCount = totalCount;
            TotalPage = totalPage;
        }
       
    }
}
