using AutoMapper;
using MyWebSite.Application.DepartmentApp.Dtos;
using MyWebSite.Domain.Entities;
using MyWebSite.Domain.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace MyWebSite.Application.DepartmentApp
{
    public class DepartmentAppService : IDepartmentAppService<DepartmentDto>
    {
        private readonly IDepartmentRepository _departmentRepository;

        public DepartmentAppService(IDepartmentRepository departmentRepository)
        {
            _departmentRepository = departmentRepository;
        }

        public void Delete(Guid id)
        {
            _departmentRepository.Delete(id);
        }


        public List<DepartmentDto> GetAll()
        {
           return Mapper.Map<List<DepartmentDto>>(_departmentRepository.GetAll());
        }

        public DepartmentDto GetById(Guid id)
        {
           return Mapper.Map<DepartmentDto>(_departmentRepository.GetById(id));
        }

        public List<DepartmentDto> GetList(Expression<Func<Department, bool>> where)
        {
            return Mapper.Map<List<DepartmentDto>>(_departmentRepository.GetAllList(where));
        }

        public void Insert(DepartmentDto department)
        {
            _departmentRepository.Insert(Mapper.Map<Department>(department));
        }

        public void Update(DepartmentDto department)
        {
            _departmentRepository.Update(Mapper.Map<Department>(department));
        }
    }
}
