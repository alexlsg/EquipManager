using System;

namespace AntistaticApi.Model
{
    /// <summary>
    /// 参数类
    /// </summary>
    public class Params
    {
        internal string user;
        internal string token;

        public int id { get; set; }
        /// <summary>
        /// 检索文本
        /// </summary>
        public string text { get; set; }
        /// <summary>
        /// 设备ID
        /// </summary>
        public string equipid { get; set; }
        /// <summary>
        /// 分组ID
        /// </summary>
        public string groupid { get; set; }
        /// <summary>
        /// 角色ID
        /// </summary>

        public string roleid { get; set; }

        public string equiptypeid { get; set; }

        public string datatype { get; set; }
        public string eventkey { get; set; }
        public DateTime ksrq { get; set; }
        public DateTime jsrq { get; set; }
        public string spotno { get; set; }

        public string typeid { get; set; }
        public string eventlevel { get; set; }
        public string eventtype { get; set; }
    }
}
