using System;
using System.Text;
using System.Collections.Generic;
using MyWebSite.Domain.Entities;

namespace MyWebSite.Domain.Entities
{
    public class User : Entity 
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string PassWrod { get; set; }

        /// <summary>
        /// 用户姓名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 电子邮箱
        /// </summary>
        public string EMail { get; set; }

        /// <summary>
        /// 是否删除
        /// </summary>
        public int IsDeleted { get; set; }

        /// <summary>
        /// 上次登录时间
        /// </summary>
        public DateTime LastLoginTime { get; set; }

        /// <summary>
        /// 登录次数
        /// </summary>
        public int LoginTimes { get; set; }

        /// <summary>
        /// 所属部门
        /// </summary>
        public virtual Department Department { get; set; }

        /// <summary>
        /// 部门Id
        /// </summary>
        public Guid DepartmentId { get; set; }
      
        /// <summary>
        /// 角色集合
        /// </summary>
         public virtual ICollection<UserRole> UserRoles { get; set; }



    }
}
