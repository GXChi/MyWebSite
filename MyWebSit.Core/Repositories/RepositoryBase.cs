using MyWebSite.Domain;
using MyWebSite.Domain.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace MyWebSit.Core.Repositories
{

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <typeparam name="TPrimaryKey"></typeparam>
    public abstract class RepositoryBase<TEntity, TPrimaryKey> : IRepository<TEntity, TPrimaryKey> where TEntity: Entity<TPrimaryKey>
    {
        protected readonly MyWebSiteDbContext _dbContext;
        public RepositoryBase(MyWebSiteDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// 新增实体
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns></returns>
        public TEntity Insert(TEntity entity)
        {
            return _dbContext.Set<TEntity>().Add(entity).Entity;
        }

        /// <summary>
        /// 获取所有实体
        /// </summary>
        /// <returns></returns>
        public List<TEntity> GetAll()
        {
            return _dbContext.Set<TEntity>().ToList();
        }
      
        public TEntity Update(TEntity entity)
        {
            return _dbContext.Set<TEntity>().Update(entity).Entity;
        }

        public TEntity Get(TPrimaryKey id)
        {
            throw new NotImplementedException();
        }

        public List<TEntity> GetAll(Expression<Func<TEntity, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public TEntity FirstOrDefault(Expression<Func<TEntity, bool>> redicate)
        {
            throw new NotImplementedException();
        }

        public TEntity Insert(TEntity entity, bool autoSave = true)
        {
            throw new NotImplementedException();
        }

        public TEntity Update(TEntity entity, bool autoSave = true)
        {
            throw new NotImplementedException();
        }

        public TEntity InsertOrUpdate(TEntity entity, bool autoSave = true)
        {
            throw new NotImplementedException();
        }

        public void Delete(TEntity entity, bool autoSave = true)
        {
            throw new NotImplementedException();
        }

        public void Delete(TPrimaryKey id, bool autoSave = true)
        {
            throw new NotImplementedException();
        }

        public void Delete(Expression<Func<TEntity, bool>> predicate, bool autoSave = true)
        {
            throw new NotImplementedException();
        }

        public IQueryable<TEntity> LoadPageList(int startPage, int pageSize, out int rowCount, Expression<Func<TEntity, bool>> where, Expression<Func<TEntity, object>> order)
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
            _dbContext.SaveChanges();
        }
    }

    public abstract class RepositoryBase<TEntity> : RepositoryBase<TEntity, Guid> where TEntity : Entity
    {
        public RepositoryBase(MyWebSiteDbContext dbContext) : base(dbContext)
        {

        }
    }
}
