using AutoMapper;
using MyWebSit.Core.Repositories;
using MyWebSite.Application.ArticleApp.Dtos;
using MyWebSite.Domain.IRepositories;
using MyWebSite.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq.Expressions;

namespace MyWebSite.Application.ArticleApp
{
    public class ArticleAppService : IArticleAppService
    {
        private readonly IArticleReposirtory _articleRepository;
        public ArticleAppService(IArticleReposirtory articleRepository)
        {
            _articleRepository = articleRepository;
        }

        public ArticleDto Insert(ArticleDto dto)
        {
           var article = _articleRepository.Insert(Mapper.Map<Article>(dto));
           return Mapper.Map<ArticleDto>(article);
        }

        public List<Article> GetAll()
        {
            return _articleRepository.GetAll();
        }

        public Article Get(Guid id)
        {
            return _articleRepository.Get(id);
        }

        public List<Article> GetAllList(Expression<Func<Article,bool>> where)
        {
            return _articleRepository.GetAllList(where);
        }

        public void Delete(Guid id)
        {
            _articleRepository.Delete(id);
        }

        public Article Update(Article article)
        {
            return _articleRepository.Update(article);
        }
        
    }
}
