using System;
using System.Collections.Generic;
using System.Text;

namespace MyWebSite.Domain.Entities
{
    public class Article : Entity
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public string Url { get; set; }

        
    }
}
