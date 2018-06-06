using System;
using System.Collections.Generic;
using System.Text;

namespace MyWebSite.Domain
{
    /// <summary>
    /// 泛型实体基类
    /// </summary>
    /// <typeparam name="TPrimaryKey">主键类型</typeparam>
    public abstract class Entity<TPrimaryKey>
    {
        /// <summary>
        /// 泛型实体基类
        /// </summary>
        public virtual TPrimaryKey Id { get; set; }

    }

    /// <summary>
    /// 定义默认主键类型为Guid的实体基类
    /// </summary>
    public abstract class Entity : Entity<Guid>
    {
        public Guid CreateUserId { get; set; }

        public DateTime? CreateTime { get; set; }

    }
}
