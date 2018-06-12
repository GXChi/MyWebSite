using System;
using System.Collections.Generic;
using System.Text;

namespace MyWebSite.Domain.Entities
{
    public class Role : Entity
    {
        /// <summary>
        /// 角色编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 角色名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 角色备注
        /// </summary>
        public string Remarks { get; set; }

        /// <summary>
        /// 角色菜单集合
        /// </summary>
        public virtual ICollection<RoleMenu> RoleMenus { get; set; }
    }
}
