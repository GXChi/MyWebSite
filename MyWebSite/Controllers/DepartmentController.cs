using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MyWebSit.Core.Helpers;
using MyWebSite.Application.DepartmentApp;
using MyWebSite.Application.DepartmentApp.Dtos;
using MyWebSite.Application.Dto;
using MyWebSite.Domain.Dto;

namespace MyWebSite.Controllers
{
    public class DepartmentController : Controller
    {
        public IDepartmentAppService<DepartmentDto> _departmentAppService;

        public DepartmentController(IDepartmentAppService<DepartmentDto> departmentAppService)
        {
            _departmentAppService = departmentAppService;
        }
        public IActionResult Index(int pageIndex = 1, int pageSize = 5)
        {
            var department = _departmentAppService.GetAll();
            var totalPage = PagingHelper.GetTotalPage(department.Count, ref pageIndex, ref pageSize);
            var dto = new PagedResultDto<DepartmentDto>(department, pageIndex, pageSize, department.Count, totalPage);
            return View(dto);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(DepartmentDto department)
        {
            _departmentAppService.Insert(department);
            return RedirectToAction("Index", "Department");
        }

        [HttpPost]
        public IActionResult GetById(Guid id)
        {
            var department = _departmentAppService.GetById(id);
            return Json(department);
        }
        
        [HttpPost]
        public IActionResult Update(DepartmentDto department)
        {
            _departmentAppService.Update(department);
            return RedirectToAction("Index", "Department");
        }

        public IActionResult Detail(Guid id)
        {
            var department = _departmentAppService.GetById(id);
            return View(department);
        }

        public IActionResult Delete(Guid id)
        {
            _departmentAppService.Delete(id);
            return RedirectToAction("Index", "Department");
        }

    }
}