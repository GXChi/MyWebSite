using System;
using System.Collections.Generic;
using System.Text;

namespace MyWebSit.Core.Helpers
{
    public static class PagingHelper
    {
        /// <summary>
        /// 获取总页数
        /// </summary>
        /// <param name="totalCount">数据总数</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">每页数据量</param>
        /// <returns></returns>
        public static int GetTotalPage(int totalCount, ref int pageIndex, ref int pageSize)
        {
            int totalPage;
            if (pageIndex == 0)
            {
                pageIndex = 1;
                pageSize = totalCount;
                totalPage = 1;
            }
            else
            {
                totalPage =(int)Math.Ceiling(totalCount / (double)pageSize);
                if (totalPage < pageIndex)
                {
                    pageIndex = totalPage;
                }                
            }
            return totalPage; 
        }
    }
}
