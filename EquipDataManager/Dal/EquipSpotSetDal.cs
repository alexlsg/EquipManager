using EquipModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Tools;

namespace EquipDataManager.Dal
{
    public class EquipSpotSetDal
    {
        public static List<EquipSpotSet> GetAllSpotSets()
        {
            DataTable _dt = DBHelper.GetDataTable("select a.*,b.equiptypename tname from equipspotset a,equiptype b where a.EquipType = b.EquipTypeId");
            var _temp = from a in _dt.AsEnumerable()
                        select new EquipSpotSet
                        {
                            ID = a.Field<int>("ID"),
                            DataType = a.Field<string>("Datatype"),
                            EquipTypeName = a.Field<string>("tname"),
                            SaveType = a.Field<int>("SaveType"),
                            SpotNO = a.Field<string>("SpotNO"),
                            SpotName = a.Field<string>("SpotNm"),
                            Bhl = a.Field<double?>("Bhl"),
                            Mrjg = a.Field<int?>("Mrjg")
                        };
            return new List<EquipSpotSet>(_temp);
        }

        internal static List<EquipTjSet> GetAllTjSets()
        {
            DataTable _dt = DBHelper.GetDataTable("select a.*,b.equiptypename tname,b.equiptypeid typeid from EquipTjSet a,equiptype b where a.EquipType = b.EquipTypeId");
            var _temp = from a in _dt.AsEnumerable()
                        select new EquipTjSet
                        {
                            ID = a.Field<int>("ID"),
                            Cyhj = Convert.ToBoolean(a["Cyhj"]),
                            Cyzshj = Convert.ToBoolean(a["Cyzshj"]),
                            Data = a.Field<string>("Data"),
                            DataType = a.Field<string>("DataType"),
                            EquipTypeName = a.Field<string>("tname"),
                            SpotNO = a.Field<string>("SpotNO"),
                            Tjlx = a.Field<string>("Tjlx"),
                            Tjtj = a.Field<string>("Tjtj"),
                            Tjzt = a.Field<string>("Tjzt"),
                            EquipType = a.Field<int>("typeid").ToString(),

                        };
            if (_temp.Count() == 0)
            {
                return MoniData();
            }
            return _temp.ToList();
        }

        /// <summary>
        /// 模拟数据
        /// </summary>
        /// <returns></returns>
        private static List<EquipTjSet> MoniData()
        {
            List<EquipTjSet> _sets = new List<EquipTjSet>();
            _sets.Add(new EquipTjSet(0, "静电监测设备", "A-11,A-13,A-15,A-17", "腕带", "正常", "3", "Data"));
            _sets.Add(new EquipTjSet(0, "静电监测设备", "A-11,A-13,A-15,A-17", "腕带", "未插", "13", "Data"));
            _sets.Add(new EquipTjSet(0, "静电监测设备", "A-11,A-13,A-15,A-17", "腕带", "异常", "4", "Data"));
            _sets.Add(new EquipTjSet(0, "静电监测设备", "A-11,A-13,A-15,A-17", "设备", "正常", "9", "Data"));
            _sets.Add(new EquipTjSet(0, "静电监测设备", "A-11,A-13,A-15,A-17", "设备", "异常", "10", "Data"));
            _sets.Add(new EquipTjSet(0, "静电监测设备", "A-11,A-13,A-15,A-17", "台垫", "正常", "6", "Data"));
            _sets.Add(new EquipTjSet(0, "静电监测设备", "A-11,A-13,A-15,A-17", "台垫", "异常", "7", "Data"));
            _sets.Add(new EquipTjSet(0, "静电监测设备", "A-2", "接地", "正常", "0", "Data"));
            _sets.Add(new EquipTjSet(0, "静电监测设备", "A-2", "接地", "异常", "1", "Data"));
            _sets.Add(new EquipTjSet(0, "静电监测设备", "E-0", "设备", "通讯正常", "<>2", "State", true, true));
            _sets.Add(new EquipTjSet(0, "静电监测设备", "E-0", "设备", "通讯故障", "2", "State", true, true));
            _sets.Add(new EquipTjSet(0, "静电监测设备", "E-0", "设备", "报警", "1", "State", false));

            _sets.Add(new EquipTjSet(0, "离子风机设备", "D-11", "风机", "运行", "1", "Data"));
            _sets.Add(new EquipTjSet(0, "离子风机设备", "D-11", "风机", "停止", "0", "Data"));
            _sets.Add(new EquipTjSet(0, "离子风机设备", "E-0", "设备", "通讯正常", "<>2", "State", true, true));
            _sets.Add(new EquipTjSet(0, "离子风机设备", "E-0", "设备", "通讯故障", "2", "State", true, true));
            _sets.Add(new EquipTjSet(0, "离子风机设备", "E-0", "设备", "报警", "1", "State", false));

            _sets.Add(new EquipTjSet(0, "粒子计数器", "E-0", "设备", "通讯正常", "<>2", "State", true, true));
            _sets.Add(new EquipTjSet(0, "粒子计数器", "E-0", "设备", "通讯故障", "2", "State", true, true));
            _sets.Add(new EquipTjSet(0, "粒子计数器", "E-0", "设备", "报警", "1", "State", false));

            _sets.Add(new EquipTjSet(0, "温湿度", "E-0", "设备", "通讯正常", "<>2", "State", true, true));
            _sets.Add(new EquipTjSet(0, "温湿度", "E-0", "设备", "通讯故障", "2", "State", true, true));
            _sets.Add(new EquipTjSet(0, "温湿度", "E-0", "设备", "报警", "1", "State", false));
            return _sets;
        }
    }
}
