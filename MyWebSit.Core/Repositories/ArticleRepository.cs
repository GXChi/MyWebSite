using MyWebSite.Domain.Entities;
using MyWebSite.Domain.IRepositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyWebSit.Core.Repositories
{
    public class ArticleRepository : RepositoryBase<Article>,IArticleRepository
    {     
        public ArticleRepository(MyWebSiteDbContext dbContext) : base(dbContext)
        {

        }
    }
}
