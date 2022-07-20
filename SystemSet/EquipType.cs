using System;
using System.Collections.Generic;
using System.Text;
using Tools;

namespace SystemSet
{
    /// <summary>
    /// 设备类型类
    /// </summary>
    public class EquipType : BaseNotifyPropertyChanged
    {
        public EquipType()
        {
            Type = 2;
        }
        /// <summary>
        /// 设备类型Id
        /// </summary>
        public int EquipTypeId { get; set; }
        string _EquipTypeName;
        /// <summary>
        /// 设备类型名称
        /// </summary>
        public string EquipTypeName { get { return _EquipTypeName; } set { _EquipTypeName = value; OnPropertyChanged("EquipTypeName"); } }

        public List<Equip> Equips { get; set; }
        public int Type { get; set; }
    }
}
