using System;
using System.Collections.Generic;
using System.Text;

namespace EquipModel
{
    /// <summary>
    /// 设备信息
    /// </summary>
    public class Equip
    {
        public int ID { get; set; }
        public string GroupID { get; set; }
        /// <summary>
        /// 产线ID
        /// </summary>
        public string GroupName { get; set; }
        /// <summary>
        /// 设备类型
        /// </summary>
        public string EquipTypeName { get; set; }
        /// <summary>
        /// 编号
        /// </summary>
        public string NO { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 数据获取ip
        /// </summary>
        public string IP { get; set; }
        /// <summary>
        /// 数据获取端口
        /// </summary>
        public string PORT { get; set; }
        public string EquipType { get; set; }

        private string txgzDesc;
        public string TxgzDesc { get => txgzDesc; set { txgzDesc = value;  } }
        private string txzcdesc;
        public string TxzcDesc { get => txzcdesc; set { txzcdesc = value; } }
        private int? eventlevel;
        public int? EventLevel { get => eventlevel; set { eventlevel = value; } }
        bool pb = false;
        public bool Pb { get { return pb; } set { pb = value;  } }
        private int? txcs;
        public int? Txcs { get => txcs; set { txcs = value;  } }
        //public string EquipType { get; set; }
    }
}
