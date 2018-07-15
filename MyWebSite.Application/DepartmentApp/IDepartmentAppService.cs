using MyWebSite.Application.DepartmentApp.Dtos;
using MyWebSite.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace MyWebSite.Application.DepartmentApp
{
    public interface IDepartmentAppService<TEntity>
    {
        List<TEntity> GetAll();

        TEntity GetById(Guid id);

        List<TEntity> GetList(Expression<Func<Department, bool>> where);

        void Insert(DepartmentDto department);

        void Update(DepartmentDto department);

        void Delete(Guid id);
    }
}
