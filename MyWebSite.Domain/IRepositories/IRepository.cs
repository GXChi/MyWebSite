using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace MyWebSite.Domain.IRepositories
{
    public interface IRepository
    {

    }
    public interface IRepository<TEntity, TPrimaryKey> : IRepository where TEntity : Entity<TPrimaryKey>
    {
        List<TEntity> GetAll();
        TEntity Get(TPrimaryKey id);
        List<TEntity> GetAll(Expression<Func<TEntity, bool>> predicate);
        TEntity FirstOrDefault(Expression<Func<TEntity, bool>> redicate);

        TEntity Insert(TEntity entity, bool autoSave = true);
        TEntity Update(TEntity entity, bool autoSave = true);
        TEntity InsertOrUpdate(TEntity entity, bool autoSave = true);

        void Delete(TEntity entity, bool autoSave = true);
        void Delete(TPrimaryKey id, bool autoSave = true);
        void Delete(Expression<Func<TEntity, bool>> predicate, bool autoSave = true);

        IQueryable<TEntity> LoadPageList(int startPage, int pageSize, out int rowCount, Expression<Func<TEntity, bool>> where, Expression<Func<TEntity, object>> order);

        void Save();
    }

    public interface IRepository<TEntity> : IRepository<TEntity, Guid> where TEntity : Entity
    {

    }
}
