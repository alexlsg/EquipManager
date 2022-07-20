using System;
using System.Collections.Generic;
using System.Text;
using Tools;

namespace SystemSet
{
    /// <summary>
    /// 统计配置
    /// </summary>
    public class EquipTjSet : BaseNotifyPropertyChanged
    {
        public EquipTjSet()
        {
            EquipTypes = Pub.EquipTypes;
        }
        public int Id { get; set; }
        int _EquipType;
        /// <summary>
        /// 设备类型
        /// </summary>
        public int EquipType { get { return _EquipType; } set { _EquipType = value; OnPropertyChanged("EquipType"); } }
        string _DataType;
        /// <summary>
        /// 数据类型
        /// </summary>
        public string DataType{ get { return _DataType; } set { _DataType = value; OnPropertyChanged("DataType"); } }
        string _SpotNO;
        /// <summary>
        /// 测点编号
        /// </summary>
        public string SpotNO{ get { return _SpotNO; } set { _SpotNO = value; OnPropertyChanged("SpotNO"); } }
        string _Tjlx;
        /// <summary>
        /// 统计类型
        /// </summary>
        public string Tjlx{ get { return _Tjlx; } set { _Tjlx = value; OnPropertyChanged("Tjlx"); } }
        string _Tjzt;
        /// <summary>
        /// 统计状态
        /// </summary>
        public string Tjzt{ get { return _Tjzt; } set { _Tjzt = value; OnPropertyChanged("Tjzt"); } }
        string _Data;
        /// <summary>
        /// 对应值
        /// </summary>
        public string Data{ get { return _Data; } set { _Data = value; OnPropertyChanged("Data"); } }
        string _Tjtj;
        /// <summary>
        /// 统计条件
        /// </summary>
        public string Tjtj{ get { return _Tjtj; } set { _Tjtj = value; OnPropertyChanged("Tjtj"); } }
        bool _Cyhj;
        /// <summary>
        /// 参与合计
        /// </summary>
        public bool Cyhj{ get { return _Cyhj; } set { _Cyhj = value; OnPropertyChanged("Cyhj"); } }
        bool _Cyzshj;
        /// <summary>
        /// 参与总数合计
        /// </summary>
        public bool Cyzshj{ get { return _Cyzshj; } set { _Cyzshj = value; OnPropertyChanged("Cyzshj"); } }
        /// <summary>
        /// 设备类型列表
        /// </summary>
        public List<EquipType> EquipTypes { get; set; }
    }
}
