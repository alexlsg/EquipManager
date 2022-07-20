using System;
using System.Collections.Generic;
using System.Text;

namespace EquipModel
{

    /// <summary>
    /// 设备数据
    /// </summary>
    public class EquipData
    {
        /// <summary>
        /// 设备表ID
        /// </summary>
        public int EquipID { get; set; }
        /// <summary>
        /// 设备编号
        /// </summary>
        public string EquipNO { get; set; }
        /// <summary>
        /// 设备名称
        /// </summary>
        public string EquipName { get; set; }
        /// <summary>
        /// 数据类型
        /// </summary>
        public string DataType { get; set; }
        /// <summary>
        /// 测点
        /// </summary>
        public string SpotNO { get; set; }
        /// <summary>
        /// 测点名称
        /// </summary>
        public string SpotName { get; set; }
        /// <summary>
        /// 数据
        /// </summary>
        public string Data { get; set; }
        /// <summary>
        /// 事件
        /// </summary>
        public string Event { get; set; }
        /// <summary>
        /// 记录时间
        /// </summary>
        public DateTime Jlsj { get; set; }
        public string State { get; set; }
        public string NoState { get; set; }
        public string Unit { get; set; }
        public new string ToString()
        {
            return $"EquipName:{EquipName};EquipNO:{EquipNO};State:{State};type:{DataType};spot:{SpotNO};SpotName:{SpotName};Data:{Data}";
        }
    }
}
