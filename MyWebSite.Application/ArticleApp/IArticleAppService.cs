using MyWebSite.Application.ArticleApp.Dtos;
using MyWebSite.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace MyWebSite.Application.ArticleApp
{
    public interface IArticleAppService
    {
        ArticleDto Insert(ArticleDto dto);

        List<ArticleDto> GetAll();

        List<ArticleDto> GetAllList(Expression<Func<Article, bool>> where);

        ArticleDto Get(Guid id);

        void Delete(Guid id);

        ArticleDto Update(Article article);

        List<ArticleDto> GetPage(int startPage, int pageSize, out int rowCount, Expression<Func<Article, bool>> where, Expression<Func<Article, object>> order);
       

    }
}
