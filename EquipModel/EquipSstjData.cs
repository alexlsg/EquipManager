using System;
using System.Collections.Generic;
using System.Text;

namespace EquipModel
{
    /// <summary>
    /// 实时统计数据
    /// </summary>
    public class EquipSstjData
    {
        public string TjType { get; set; }
        /// <summary>
        /// 设备类型名称
        /// </summary>
        public string TypeName { get; set; }
        /// <summary>
        /// 统计类型
        /// </summary>
        public string Tjlx { get; set; }
        /// <summary>
        /// 统计状态
        /// </summary>
        public string Tjzt { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        public int Count { get; set; }
        public int Px1 { get; set; }
        public int Px { get; set; }
        /// <summary>
        /// 参与合计
        /// </summary>
        public bool Cyhj { get; set; }
        /// <summary>
        /// 参与设备总数合计
        /// </summary>
        public bool Cyzshj { get; set; }
    }
}
