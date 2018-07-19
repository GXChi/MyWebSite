using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace MyWebSite.Domain.Dto
{
    /// <summary>
    /// 分页
    /// </summary>
    public class Pagination : IPagination
    {
        /// <summary>
        /// 默认每页显示选项
        /// </summary>
        public static int[] DefaultPageSizeOption = new int[] { 10, 20, 30, 50 };

        /// <summary>
        /// 每页默认显示
        /// </summary>
        public static int DefaultPageSize = 20;

        /// <summary>
        /// 初始化
        /// </summary>
        public Pagination()
        {

        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="page">当前页码</param>
        /// <param name="count">总数</param>
        /// <param name="size">每页项目数</param>
        public Pagination(int page, int count, int size) 
            : this()
        {
            CurrentPage = page;
            CurrentSize = size;
            TotalPage = (int)Math.Ceiling(count / (double)size);
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="page">当前页码</param>
        /// <param name="count">总数</param>
        public Pagination(int page, int count) 
            : this(page, count, DefaultPageSize) { }

        /// <summary>
        /// 获取或设置页面大小选项。
        /// </summary>
        public int[] PageSizeOption { get; set; }

        /// <summary>
        /// 获取或设置总页数。
        /// </summary>
        public int TotalPage { get; set; }

        /// <summary>
        /// 当前页码
        /// </summary>
        public int CurrentPage { get; set; }

        /// <summary>
        /// 获取或设置每页的当前项目数
        /// </summary>
        public int CurrentSize { get; set; }

        /// <summary>
        /// 获取或设置总记录数
        /// </summary>
        public int TotalCount { get; set; }
    }
}
