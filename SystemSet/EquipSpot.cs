using System;
using System.Collections.Generic;
using System.Text;
using Tools;

namespace SystemSet
{
    /// <summary>
    /// 设置测点类
    /// </summary>
    public class EquipSpot : BaseNotifyPropertyChanged
    {
        /// <summary>
        ///  ID
        /// </summary>
        public int Id { get; set; }
        string _EquipNo;
        /// <summary>
        ///  设备编号（同网关下，设备号唯一，范围0-9999）
        /// </summary>
        public string EquipNo { get { return _EquipNo; } set { _EquipNo = value; OnPropertyChanged("EquipNo"); } }
        string _Type;
        /// <summary>
        /// 测点类型（E或A 或D测点类型）
        /// </summary>
        public string Type { get { return _Type; } set { _Type = value; OnPropertyChanged("Type"); } }
        string _SpotNo;
        /// <summary>
        /// 测点编号（同设备同类型下，测点号唯一，范围0-9999）
        /// </summary>
        public string SpotNo { get { return _SpotNo; } set { _SpotNo = value; OnPropertyChanged("SpotNo"); } }
        string _Unit;
        /// <summary>
        /// 单位
        /// </summary>
        public string Unit { get { return _Unit; } set { _Unit = value; OnPropertyChanged("Unit"); } }
        string _Value;
        /// <summary>
        /// 当前实时值
        /// </summary>
        public string Value { get { return _Value; } set { _Value = value; OnPropertyChanged("_alue"); } }
        string _SpotNm;
        /// <summary>
        /// 名称
        /// </summary>
        public string SpotNm { get { return _SpotNm; } set { _SpotNm = value; OnPropertyChanged("SpotNm"); } }
        int _State;
        /// <summary>
        /// 处理后的当前状态（0正常；1报警；屏蔽后返回正常）
        /// </summary>
        public int State { get { return _State; } set { _State = value; OnPropertyChanged("State"); } }
        int _NoState;
        /// <summary>
        /// 当前状态（0正常；1报警）
        /// </summary>
        public int NoState { get { return _NoState; } set { _NoState = value; OnPropertyChanged("NoState"); } }

    }
}
