using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MyWebSite.Domain;
using MyWebSite.Domain.IRepositories;

namespace MyWebSite.Controllers
{
    public class TodoController : Controller
    {
        private readonly ITodoRepository _todoRepository;
        private readonly ILogger _logger;

        public TodoController(ITodoRepository todoRepository, ILogger<TodoController> logger)
        {
            _todoRepository = todoRepository;
            _logger = logger;
        }
       
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult GetById(string id)
        {
            _logger.LogInformation(LoggingEvents.GetItem,"日志{ID}", id);
            var item = _todoRepository.Find(id);
            if (item == null)
            {
                _logger.LogWarning(LoggingEvents.GetItemNotFound,"GetById({ID}) Not Found",id);
                return NotFound();
            }

            return new ObjectResult(item);
        }
    }
}