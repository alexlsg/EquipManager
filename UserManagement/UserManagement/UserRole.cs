using System;
using System.Collections.Generic;
using System.Text;

namespace UserManagement
{
    public class UserRole
    {
        public string User { get; set; }
        public Tools.Enum_Role Role { get; set; }
        public string GroupQx { get; set; }
        public string EquipQx { get; set; }
        public string PageQx { get; set; }
    }
}
