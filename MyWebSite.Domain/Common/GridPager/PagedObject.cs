using System;
using System.Collections.Generic;
using System.Text;

namespace MyWebSit.Domain.Common.GridPager
{
    public class PagedObject<T>
    {
        public IList<T> ObjectList { get; set; }

        public int TotalCount { get; set; }

        public GridPagerObject GridPager { get; set; }
    }
}
