using System;
using System.Collections.Generic;
using System.Text;
using Tools;

namespace SystemSet
{
    /// <summary>
    /// 产线类
    /// </summary>
    public class ProductionLine : BaseNotifyPropertyChanged
    {
        public ProductionLine()
        {
            Type = 1;
        }

        /// <summary>
        /// 产线Id
        /// </summary>
        public int ProductionLineId { get; set; }
        string _ProductionLineName;
        /// <summary>
        /// 产线名称
        /// </summary>
        public string ProductionLineName { get { return _ProductionLineName; } set { _ProductionLineName = value; OnPropertyChanged("ProductionLineName"); } }
        public List<EquipType> EquipTypes { get; set; }
        public List<Equip> Equips { get; set; }
        public int Type { get; set; }
    }
}
