using Microsoft.EntityFrameworkCore;
using MyWebSit.Core.Helpers;
using MyWebSite.Domain.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWebSite.Application.Dto
{
    public class PaginatedList<T> 
    {
        public IList<T> Items { get; set; }
        public int PageIndex { get; private set; }

        public int TotalPages { get; private set; }

        public int TotalCount { get; set; }

        public int CurrentPage { get; set; }

        public PaginatedList(List<T> items, int totalCount, int pageIndex, int pageSize)
        {
            Items = items;
            TotalPages = PagingHelper.GetTotalPage(totalCount, ref pageIndex, ref pageSize);
            TotalCount = totalCount;
            CurrentPage = items.Count();
            PageIndex = pageIndex;         

            //AddRange(items);
        }
        public PaginatedList(List<T> items, int totalCount, int pageIndex, int pageSize,int totalPages)
        {
            Items = items;
            TotalPages = totalPages;
            TotalCount = totalCount;
            CurrentPage = items.Count();
            PageIndex = pageIndex;

            //AddRange(items);
        }

        public bool HasPreviousPage
        {
            get
            {
                return (PageIndex > 1);
            }
        }

        public bool HasNexPage
        {
            get
            {
                return (PageIndex < TotalPages);
            }
        }

        public static async Task<PaginatedList<T>> CreateAsync(IQueryable<T> soure, int pageIndex, int pageSize)
        {
            if (pageSize <= 0)
            {
                pageSize = 2;
            }
            var count = await soure.CountAsync();
            var items = await soure.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
            return new PaginatedList<T>(items, count, pageIndex, pageSize);
        }
    }
}
