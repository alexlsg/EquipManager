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
        public int ID { get; set; }
        public string Name { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int RoleType { get; set; }
        public string Groups { get; set; }
        public string Equips { get; set; }
        public string Pages { get; set; }
    }
}
