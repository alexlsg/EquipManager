using System;
using System.Collections.Generic;
using System.Text;
using Tools;

namespace SystemSet
{
    /// <summary>
    /// 设备测点配置
    /// </summary>
    public class EquipSpotSet : BaseNotifyPropertyChanged
    {
        public EquipSpotSet()
        { 
            EquipTypes = Pub.EquipTypes;
            Type = 4;
        }
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }
        int _EquipId;
        /// <summary>
        /// 设备Id
        /// </summary>
        public int EquipId { get { return _EquipId; } set { _EquipId = value; OnPropertyChanged("EquipId"); } }
        int? _EquipType;
        /// <summary>
        /// 设备类型
        /// </summary>
        public int? EquipType { get { return _EquipType; } set { _EquipType = value; OnPropertyChanged("EquipType"); } }
        string _DataType;
        /// <summary>
        /// 数据类型,与测点编号在通道中
        /// </summary>
        public string DataType { get { return _DataType; } set { _DataType = value; OnPropertyChanged("DataType"); } }
        string _SpotNO;
        /// <summary>
        /// 测点编号
        /// </summary>
        public string SpotNO { get { return _SpotNO; } set { _SpotNO = value; OnPropertyChanged("SpotNO"); } }
        string _SpotNm;
        /// <summary>
        /// 名称
        /// </summary>
        public string SpotNm { get { return _SpotNm; } set { _SpotNm = value; OnPropertyChanged("SpotNm"); } }
        int _SaveType;
        /// <summary>
        /// 保存类型,0不保存、1外接设备类型、2状态、3实时值
        /// </summary>
        public int SaveType { get { return _SaveType; } set { _SaveType = value; OnPropertyChanged("SaveType"); } }
        /// <summary>
        /// 设备类型列表
        /// </summary>
        public List<EquipType> EquipTypes { get; set; }
        public int Type { get; set; }
    }
}
