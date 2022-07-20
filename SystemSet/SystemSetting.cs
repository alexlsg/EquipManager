using System;
using System.Collections.Generic;
using System.Text;

namespace SystemSet
{
    /// <summary>
    /// 系统设置类
    /// </summary>
    public class SystemSetting
    {
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// logo图标
        /// </summary>
        public byte[] Logo { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }
    }
}
