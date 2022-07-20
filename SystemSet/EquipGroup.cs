using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Tools;

namespace SystemSet
{
    /// <summary>
    /// 设备组
    /// </summary>
    public class EquipGroup : BaseNotifyPropertyChanged
    {
        public EquipGroup()
        {
            Type = 1;
        }
        /// <summary>
        /// 设备组Id
        /// </summary>
        public int EquipGroupId { get; set; }
        string _EquipGroupName;
        /// <summary>
        /// 设备组名称
        /// </summary>
        public string EquipGroupName { get { return _EquipGroupName; } set { _EquipGroupName = value; OnPropertyChanged("EquipGroupName"); } }
        /// <summary>
        /// 网关列表
        /// </summary>
        public ObservableCollection<Equip> Equips { get; set; } = new ObservableCollection<Equip>();
        public int Type { get; set; }
    }
}
