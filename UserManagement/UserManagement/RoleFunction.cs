using System;
using System.Collections.Generic;
using System.Text;

namespace UserManagement
{
    /// <summary>
    /// 角色功能类
    /// </summary>
    public class RoleFunction
    {
        public int Id { get; set; }
        /// <summary>
        /// 角色ID
        /// </summary>
        public int RoleId { get; set; }
        /// <summary>
        /// 角色名称
        /// </summary>
        public string RoleName { get; set; }
        /// <summary>
        /// 功能ID
        /// </summary>
        public int FunctionId { get; set; }
        /// <summary>
        /// 功能名称
        /// </summary>
        public string FunctionName { get; set; }
    }
}
