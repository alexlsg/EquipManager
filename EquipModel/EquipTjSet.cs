using System;
using System.Collections.Generic;
using System.Text;

namespace EquipModel
{
    /// <summary>
    /// 统计配置
    /// </summary>
    public class EquipTjSet
    {
        public EquipTjSet()
        { }
        public EquipTjSet(int id, string equiptypename, string spotno, string tjlx, string tjzt, string data, string tjtj, bool cyhj = true, bool cyzshj = false)
        {
            ID = id;
            EquipTypeName = equiptypename;
            SpotNO = spotno;
            Tjlx = tjlx;
            Tjzt = tjzt;
            Data = data;
            Tjtj = tjtj;
            Cyhj = cyhj;
            Cyzshj = cyzshj;
        }
        public int ID { get; set; }
        /// <summary>
        /// 设备类型名称
        /// </summary>
        public string EquipTypeName { get; set; }
        /// <summary>
        /// 设备类型
        /// </summary>
        public string EquipType { get; set; }
        
        /// <summary>
        /// 数据类型
        /// </summary>
        public string DataType { get; set; }
        /// <summary>
        /// 测点编号
        /// </summary>
        public string SpotNO { get; set; }
        /// <summary>
        /// 统计类型
        /// </summary>
        public string Tjlx { get; set; }
        /// <summary>
        /// 统计状态
        /// </summary>
        public string Tjzt { get; set; }
        /// <summary>
        /// 对应值
        /// </summary>
        public string Data { get; set; }
        /// <summary>
        /// 统计条件
        /// </summary>
        public string Tjtj { get; set; }
        /// <summary>
        /// 参与合计
        /// </summary>
        public bool Cyhj { get; set; }
        /// <summary>
        /// 参与总数合计
        /// </summary>
        public bool Cyzshj { get; set; }
    }
}
