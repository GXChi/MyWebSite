using System;
using System.Collections.Generic;
using System.Text;

namespace MyWebSite.Domain.Dto
{
    class PagedResultDto<T> 
    {
        public IList<T> Items { get; set; }
        public object DynamicItems { get; set; }

        public PagedResultDto(object items, int pageIndex, int pageSize, int totalCount, int totalPage)
        {

        }

        public PagedResultDto(IList<T> Items, int pageIndex, int pageSize, int totalCount)
        {

        }
    }
}
