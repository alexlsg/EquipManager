using System;
using System.Collections.Generic;
using System.Text;
using Tools;

namespace SystemSet
{
    /// <summary>
    /// 设备信息
    /// </summary>
    public class Equip : BaseNotifyPropertyChanged
    {
        public Equip()
        {
            EquipTypes = Pub.EquipTypes;
            Type = 3;
        }
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }
        string _EquipNO;
        /// <summary>
        /// 设备编号
        /// </summary>
        public string EquipNO { get { return _EquipNO; } set { _EquipNO = value; OnPropertyChanged("EquipNO"); } }
        string _EquipName;
        /// <summary>
        /// 设备名称
        /// </summary>
        public string EquipName { get { return _EquipName; } set { _EquipName = value; OnPropertyChanged("EquipName"); } }
        int _TypeBinding;
        /// <summary>
        /// 类型绑定
        /// </summary>
        public int TypeBinding { get { return _TypeBinding; } set { _TypeBinding = value; OnPropertyChanged("TypeBinding"); } }
        string _ProductionLineGroupBinding;
        /// <summary>
        /// 产线组绑定
        /// </summary>
        public string ProductionLineGroupBinding { get { return _ProductionLineGroupBinding; } set { _ProductionLineGroupBinding = value; OnPropertyChanged("ProductionLineGroupBinding"); } }
        int _GatewayId;
        /// <summary>
        /// 网关ID
        /// </summary>
        public int GatewayId { get { return _GatewayId; } set { _GatewayId = value; OnPropertyChanged("GatewayId"); } }

        /// <summary>
        /// 设备类型列表
        /// </summary>
        public List<EquipType> EquipTypes { get; set; }


        public List<EquipSpotSet> EquipSpotSets { get; set; }
        bool selected;
        public bool Selected { get { return selected; } set { selected = value; OnPropertyChanged("Selected"); } }
        public int Type { get; set; }

    }
}
