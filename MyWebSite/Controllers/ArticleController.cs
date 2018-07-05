using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MyWebSite.Application.ArticleApp;
using MyWebSite.Application.ArticleApp.Dtos;
using System.Linq;

namespace MyWebSite.Controllers
{
    public class ArticleController : Controller
    {
        private IArticleAppService _articleAppSerivce;
 

        public ArticleController(IArticleAppService articleAppService)
        {
            _articleAppSerivce = articleAppService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var articles = _articleAppSerivce.GetAll();

            return View(articles);
        }
            

        [HttpPost]
        public IActionResult Index(string searchString, out int rowCount)
        { 
            List<ArticleDto> articles;
            articles = _articleAppSerivce.GetPage(1, 2, out rowCount, it => it.Title.Contains(searchString), it => it.Title);
            if (!string.IsNullOrEmpty(searchString))
            {
                articles = _articleAppSerivce.GetAllList(it => it.Title.Contains(searchString));

            }
            else
            {
                articles = _articleAppSerivce.GetAll();
            }
            return View(articles);

        } 

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(ArticleDto article)
        {
            _articleAppSerivce.Insert(article);
            return View();
        }

        
        public IActionResult Delete(Guid id)
        {
            _articleAppSerivce.Delete(id);
            return RedirectToAction("Index");
        }


        public IActionResult Detail(Guid id)
        {
            var article = _articleAppSerivce.Get(id);
            return View(article);
        }
    }
}