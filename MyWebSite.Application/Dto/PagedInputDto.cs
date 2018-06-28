using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MyWebSite.Application.Dto
{
    public class PagedInputDto
    {
        [Range(0, int.MaxValue)]
        public int PageIndex { get; set; }

        [Range(1,int.MaxValue)]
        public int PageSize { get; set; }
    }
}
