using System;
using System.Collections.Generic;
using System.Text;
using Tools;

namespace SystemSet
{
    /// <summary>
    /// 产线组类
    /// </summary>
    public class ProductionLineGroup : BaseNotifyPropertyChanged
    {
        /// <summary>
        /// 产线组ID
        /// </summary>
        public int ProductionLineGroupId { get; set; }
        int _ProductionLineId;
        /// <summary>
        /// 产线ID
        /// </summary>
        public int ProductionLineId { get { return _ProductionLineId; } set { _ProductionLineId = value; OnPropertyChanged("ProductionLineId"); } }
        string _EquipNO;
        /// <summary>
        /// 设备编号
        /// </summary>
        public string EquipNO { get { return _EquipNO; } set { _EquipNO = value; OnPropertyChanged("EquipNO"); } }
        /// <summary>
        /// 设备类型
        /// </summary>
        public EquipType EquipType { get; set; }
    }
}
