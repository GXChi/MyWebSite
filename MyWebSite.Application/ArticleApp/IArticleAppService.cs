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

        List<ArticleDto> GetAllList();

        ArticleDto Get(Guid id);

        void Delete(Guid id);

        ArticleDto Update(Article article);


    }
}
