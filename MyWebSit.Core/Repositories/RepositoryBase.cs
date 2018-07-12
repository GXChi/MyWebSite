using Microsoft.EntityFrameworkCore;
using MyWebSit.Domain.Common.GridPager;
using MyWebSite.Domain;
using MyWebSite.Domain.Common.Extensions;
using MyWebSite.Domain.Common.Query;
using MyWebSite.Domain.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;


namespace MyWebSit.Core.Repositories
{

    /// <summary>
    /// 仓储基类
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <typeparam name="TPrimaryKey">主键类型</typeparam>
    public abstract class RepositoryBase<TEntity, TPrimaryKey> : IRepository<TEntity, TPrimaryKey> where TEntity : Entity<TPrimaryKey>
    {
        /// <summary>
        /// 定义数据访问上下文对象
        /// </summary>
        protected readonly MyWebSiteDbContext _dbContext;
        protected readonly DbSet<TEntity> _dbSet;

        /// <summary>
        /// 通过构造函数注入得到数据上下文对象实例
        /// </summary>
        /// <param name="dbContext"></param>
        public RepositoryBase(MyWebSiteDbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = dbContext.Set<TEntity>();
        }

        #region 获取实体

        /// <summary>
        /// 获取所有实体
        /// </summary>
        /// <returns></returns>
        public List<TEntity> GetAll()
        {
            return _dbSet.ToList();
            //return _dbContext.Set<TEntity>().ToList();
        }
    
        /// <summary>
        /// 根据主键id获取实体合计
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public TEntity GetById(TPrimaryKey id)
        {
            return _dbSet.FirstOrDefault(CreateEqualityExpressionForId(id));
            //return _dbSet.Find(id);
        }

        /// <summary>
        /// 更具lambda获取实体集合
        /// </summary>
        /// <param name="predicate">lambda表达式</param>
        /// <returns></returns>
        public List<TEntity> GetAllList(Expression<Func<TEntity, bool>> predicate)
        {
            if (predicate != null)
            {
                return _dbSet.Where(predicate).ToList();
            }
            return _dbSet.ToList();
            
        }                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                               

        /// <summary>
        /// 更具lambda获取单个实体
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate)
        {
            return _dbSet.FirstOrDefault(predicate);
        }

        /// <summary>
        /// 根据主键构建判断表达式
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns></returns>
        protected static Expression<Func<TEntity, bool>> CreateEqualityExpressionForId(TPrimaryKey id)
        {
            var lambdaParam = Expression.Parameter(typeof(TEntity));
            var lambdaBody = Expression.Equal(
                Expression.PropertyOrField(lambdaParam, "Id"),
                Expression.Constant(id, typeof(TPrimaryKey))
                );
            return Expression.Lambda<Func<TEntity, bool>>(lambdaBody, lambdaParam);
        }
        #endregion

        #region 新增修改实体

        /// <summary>
        /// 新增实体
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="autoSave">是否立即执行保存</param>
        /// <returns></returns>
        public TEntity Insert(TEntity entity, bool autoSave = true)
        {
            #region Argument Validation
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            #endregion

            _dbSet.Add(entity);
            if (autoSave)
                Save();
            return entity;
        }
     
        /// <summary>
        /// 更新实体
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="autoSave">是否自动保存</param>
        /// <returns></returns>
        public TEntity Update(TEntity entity, bool autoSave = true)
        {            
            #region Argument Validation
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            #endregion
            var obj = GetById(entity.Id);
            EntityToEntity(entity, obj);
            if (autoSave)
                Save();
            return entity;
        }

        /// <summary>
        /// 更新或新增实体
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="autoSave">是否立即执行保存</param>
        /// <returns></returns>
        public TEntity InsertOrUpdate(TEntity entity, bool autoSave = true)
        {
            #region Argument Validation
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            #endregion
            if (GetById(entity.Id) == null)
                return Insert(entity, autoSave);
            return Update(entity, autoSave);
        }

        public void EntityToEntity<T>(T pTargetObjSrc, T pTargetObjDest)
        {
            foreach (var item in typeof(T).GetProperties())
            {
                item.SetValue(pTargetObjDest, item.GetValue(pTargetObjSrc, new object[] { }));
            }
        }

        #endregion

        #region 删除实体
        /// <summary>
        /// 删除实体
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="autoSave">是否立即执行保存</param>
        public void Delete(TEntity entity, bool autoSave = true)
        {
            #region Argument Validation
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            #endregion
            _dbSet.Remove(entity);
            if (autoSave)
                Save();
        }

        /// <summary>
        /// 根据主键删除实体
        /// </summary>
        /// <param name="id">实体主键</param>
        /// <param name="autoSave">是否立即执行保存</param>
        public void Delete(TPrimaryKey id, bool autoSave = true)
        {
            #region Argument Validation
            if (id == null)
            {
                throw new ArgumentNullException("id");
            }
            #endregion

            _dbSet.Remove(GetById(id));
            if(autoSave)
                Save();
        }

        /// <summary>
        /// 根据条件删除实体
        /// </summary>
        /// <param name="where">lambda表达式</param>
        /// <param name="autoSave"></param>
        public void Delete(Expression<Func<TEntity, bool>> where, bool autoSave = true)
        {
            _dbSet.Where(where).ToList().ForEach(it => _dbContext.Set<TEntity>().Remove(it));
            if (autoSave)
                Save();
        }
        #endregion

        /// <summary>
        /// 事务性保存
        /// </summary>
        public void Save()
        {
            _dbContext.SaveChanges();
        }


        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="startPage">页码</param>
        /// <param name="pageSize">单页数据数</param>
        /// <param name="rowCount">行数</param> 
        /// <param name="where">条件</param>
        /// <param name="order">排序</param>
        /// <returns></returns>
        public IQueryable<TEntity> LoadPageList(int startPage, int pageSize, out int rowCount, Expression<Func<TEntity, bool>> where, Expression<Func<TEntity, object>> order)
        {
            var result = from p in _dbContext.Set<TEntity>()
                         select p;
            if (where != null)
                result = result.Where(where);
            if (order != null)
                result = result.OrderBy(order);
            else
                result = result.OrderBy(m => m.Id);
            rowCount = result.Count();
            return result.Skip((startPage - 1) * pageSize).Take(pageSize);
        }

        protected virtual PagedObject<TEntity> GetPageList(GridPagerObject filter, IQueryable<TEntity> source)
        {
            var result = source;

            //去掉值为空的查询条
            if (filter.QueryModel != null && filter.QueryModel.Items.Count > 0)
            {
                filter.QueryModel.Items.RemoveAll(x => x.Value == null || string.IsNullOrEmpty(x.Value.ToString()));
            }

            if (filter.QueryModel != null && filter.QueryModel.Items.Count > 0)
            {
                result = result.Where(filter.QueryModel);
            }
            if (!string.IsNullOrWhiteSpace(filter.SortCloumnName))
            {
                result = result.OrderBy(filter.SortCloumnName, (string.IsNullOrEmpty(filter.SortOrder) ? true : filter.SortOrder.ToLower() == "asc"));
            }
            else if (string.IsNullOrWhiteSpace(filter.SortCloumnName) && filter.OrderModel != null && filter.OrderModel.Items.Count > 0)
            {
                var orderModel = filter.OrderModel.Items.FirstOrDefault();
                result = result.OrderBy(orderModel.SortCloumnName, (string.IsNullOrEmpty(orderModel.SortOrder) ? true : orderModel.SortOrder.ToLower() == "asc"));
                filter.OrderModel.Items.RemoveAt(0);
            }
            //判断是多列排序条件
            if (filter.OrderModel != null && filter.OrderModel.Items.Count > 0)
            {
                foreach (var item in filter.OrderModel.Items)
                {
                    result = result.ThenBy(item.SortCloumnName, (string.IsNullOrEmpty(item.SortOrder) ? true : item.SortOrder.ToLower() == "asc"));
                }
            }

            var totalCount = result.Count();
            result = result.Skip((filter.CurrentPage - 1) * filter.RowsPerPage).Take(filter.RowsPerPage);

            return new PagedObject<TEntity>()
            {
                ObjectList = result.ToList(),
                TotalCount = totalCount,
                GridPager = filter
            };

        }
       
        public PagedObject<TEntity> GetPageList(GridPagerObject filter)
        {
             throw new NotImplementedException();
        }

        public PagedObject<TEntity> GetPageList(IQueryable<TEntity> source, GridPagerObject filter)
        {
            throw new NotImplementedException();
        }

        public PagedObject<TEntity> GetPageList(GridPagerObject filter, string includes)
        {
            throw new NotImplementedException();
        }

        public PagedObject<TEntity> GetPageList(GridPagerObject filter, Expression<Func<TEntity, bool>> expression)
        {
            throw new NotImplementedException();
        }

        public PagedObject<TEntity> GetPageList(GridPagerObject filter, IQueryable<TEntity> source, Expression<Func<TEntity, bool>> expression)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 主键为Guid类型的仓储基类
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public abstract class RepositoryBase<TEntity> : RepositoryBase<TEntity, Guid> where TEntity : Entity
    {
        public RepositoryBase(MyWebSiteDbContext dbContext) : base(dbContext)
        {

        }
    }
}
