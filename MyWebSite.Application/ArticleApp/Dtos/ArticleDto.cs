using System;
using System.Collections.Generic;
using System.Text;


namespace MyWebSite.Application.ArticleApp.Dtos
{
    public class ArticleDto
    {
        public Guid id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Url { get; set; }
    }
}
