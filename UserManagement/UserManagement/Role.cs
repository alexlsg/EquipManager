using System;
using System.Collections.Generic;
using System.Text;

namespace UserManagement
{
    /// <summary>
    /// 角色类
    /// </summary>
    public class Role
    {
        /// <summary>
        /// 角色ID
        /// </summary>
        public int RoleId { get; set; }
        /// <summary>
        /// 角色名称
        /// </summary>
        public string RoleName { get; set; }
        /// <summary>
        /// 角色功能列表
        /// </summary>

        public List<RoleFunction> RoleFunctions { get; set; }
    }
}
