using AutoMapper;
using MyWebSit.Core.Repositories;
using MyWebSite.Application.ArticleApp.Dtos;
using MyWebSite.Domain.IRepositories;
using MyWebSite.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq.Expressions;
using System.Linq;

namespace MyWebSite.Application.ArticleApp
{
    public class ArticleAppService : IArticleAppService
    {
        private readonly IArticleRepository _articleRepository;
        public ArticleAppService(IArticleRepository articleRepository)
        {
            _articleRepository = articleRepository;
        }

        public ArticleDto Insert(ArticleDto dto)
        {
           var article = _articleRepository.Insert(Mapper.Map<Article>(dto));
           return Mapper.Map<ArticleDto>(article);
        }

        public List<ArticleDto> GetAll()
        {
            return Mapper.Map<List<ArticleDto>>(_articleRepository.GetAll());
        }

        public ArticleDto Get(Guid id)
        {
            return Mapper.Map<ArticleDto>(_articleRepository.Get(id));
        }

        //public List<ArticleDto> GetAllList()
        //{
        //    return Mapper.Map<List<ArticleDto>>(_articleRepository.GetAllList(it=>it.Id!=Guid.Empty).OrderBy(it => it.Content));
        //}

        public List<ArticleDto> GetAllList(Expression<Func<Article, bool>> where)
        {
            return Mapper.Map<List<ArticleDto>>(_articleRepository.GetAllList(where));
        }
        public void Delete(Guid id)
        {
            _articleRepository.Delete(id);
        }

        public ArticleDto Update(Article article)
        {
            return Mapper.Map<ArticleDto>(_articleRepository.Update(article));
        }

        public List<ArticleDto> GetPage(int startPage,int pageSize,out int rowCount,Expression<Func<Article,bool>> where, Expression<Func<Article, object>> order)
        {           
            return Mapper.Map<List<ArticleDto>>(_articleRepository.LoadPageList(startPage, pageSize, out rowCount, where, order));
        }

    }
}
