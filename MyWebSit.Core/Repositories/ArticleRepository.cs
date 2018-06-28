using MyWebSite.Domain.Entities;
using MyWebSite.Domain.IRepositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyWebSit.Core.Repositories
{
    class ArticleRepository : RepositoryBase<Article>,IArticleReposirtory
    {     
        public ArticleRepository(MyWebSiteDbContext dbContext) : base(dbContext)
        {

        }
    }
}
