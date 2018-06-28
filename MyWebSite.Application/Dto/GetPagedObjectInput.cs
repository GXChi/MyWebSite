using System;
using System.Collections.Generic;
using System.Text;

namespace MyWebSite.Application.Dto
{
    public class GetPagedObjectInput : PagedInputDto
    {
        public string Filter { get; set; }

        public DateTime? StartTime { get; set; }

        public DateTime? EndTime { get; set; }

        public bool? IsFirstSearch { get; set; }

        public GetPagedObjectInput()
        {
            PageIndex = 1;
            PageSize = 10; 
        }

    }
}
