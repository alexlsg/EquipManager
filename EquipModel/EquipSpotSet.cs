using System;
using System.Collections.Generic;
using System.Text;

namespace EquipModel
{
    /// <summary>
    /// 设备测点保存配置
    /// </summary>
    public class EquipSpotSet
    {
        public int ID { get; set; }
        /// <summary>
        /// 设备类型
        /// </summary>
        public string EquipTypeName { get; set; }
        /// <summary>
        /// 数据类型,与测点编号在通道中
        /// </summary>
        public string DataType { get; set; }
        /// <summary>
        /// 测点编号
        /// </summary>
        public string SpotNO { get; set; }
        /// <summary>
        /// 测点名称
        /// </summary>
        public string SpotName { get; set; }
        /// <summary>
        /// 保存类型,0不保存、1外接设备类型、2状态、3实时值
        /// </summary>
        public int SaveType { get; set; }
    }
}
