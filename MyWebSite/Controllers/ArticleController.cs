using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MyWebSite.Application.ArticleApp;
using MyWebSite.Application.ArticleApp.Dtos;

namespace MyWebSite.Controllers
{
    public class ArticleController : Controller
    {
        private IArticleAppService _articleAppSerivce;

        public ArticleController(IArticleAppService articleAppService)
        {
            _articleAppSerivce = articleAppService;
        }
        public IActionResult Index()
        {
            var articles = _articleAppSerivce.GetAll();
            return View(articles);
        }

        public IActionResult Create(ArticleDto article)
        {
            _articleAppSerivce.Insert(article);
            return View();
        }
    }
}