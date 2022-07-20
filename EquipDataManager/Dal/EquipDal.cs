using EquipModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Tools;

namespace EquipDataManager.Dal
{
    public class EquipDal
    {
        public static List<Equip> GetAllEquips()
        {
            DataTable _dt = DBHelper.GetDataTable("select a.id,a.EquipNO NO,a.EquipName Name,b.IP,a.TypeBinding EquipType,b.Port,c.EquipTypeName EquipTypeName,d.equipgroupname GroupID,d.equipgroupid  " +
                "from Equip a ,gateway b,equiptype c,equipgroup d" +
                " where a.gatewayid = b.id and a.TypeBinding = c.EquipTypeId" +
                " and a.ProductionLineGroupBinding = d.equipgroupid");
            var _temp = from a in _dt.AsEnumerable()
                        select new Equip
                        {
                            ID = a.Field<int>("ID"),
                            NO = a.Field<string>("NO"),
                            Name = a.Field<string>("Name"),
                            IP = a.Field<string>("IP"),
                            PORT = a.Field<string>("PORT"),
                            EquipType = a.Field<int>("EquipType").ToString(),
                            EquipTypeName = a.Field<string>("EquipTypeName"),
                            GroupName = a.Field<string>("GroupID"),
                            GroupID = a.Field<int>("equipgroupid").ToString()
                        };
            return new List<Equip>(_temp);
        }

        internal static List<EquipGroup> GetGroupSets()
        {
            DataTable _dt = DBHelper.GetDataTable("select * from EquipGroup");
            var _temp = from a in _dt.AsEnumerable()
                        select new EquipGroup
                        {
                            Code = a.Field<int>("EquipGroupID").ToString(),
                            Name = a.Field<string>("EquipGroupName")
                        };
            return _temp.ToList();
        }
    }
}
