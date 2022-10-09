using EquipDataManager.Bll;
using EquipModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Tools;

namespace EquipDataManager.Dal
{
    public class EventSetDal
    {
        public static void SaveEventSet(EventSet set)
        {
            if (set.ID > 0)
            {
                if (set.State == DataStatus.DEL)
                {
                    Del(set.ID);
                }
                else
                    ModEventSet(set);
            }
            else
                AddEventSet(set);
            DataPicker.Instance.RefreshEventSet();
        }
        private static void AddEventSet(EventSet set)
        {
            StringBuilder _sql = new StringBuilder();
            _sql.AppendLine("insert into eventset(EquipType,DataType,SpotNO,Zhi,Ms,EventLevel,CMDID,etype)");
            _sql.AppendLine($"values({set.EquipType},'{set.DataType}','{set.SpotNO}','{set.Value}','{set.Desc}',{Pub.IsDBNull(set.EventLevel)},{Pub.IsDBNull(set.CMDID)},'{set.EType}')");
            DBHelper.ExecuteCommand(_sql.ToString());
        }
        private static void ModEventSet(EventSet set)
        {
            StringBuilder _sql = new StringBuilder();
            _sql.AppendLine($"update eventset set EquipType={set.EquipType},DataType='{set.DataType}',SpotNO='{set.SpotNO}',Zhi='{set.Value}',Ms='{set.Desc}',EventLevel={set.EventLevel},CMDID={Pub.IsDBNull(set.CMDID)},etype='{set.EType}' where id = {set.ID}");
            DBHelper.ExecuteCommand(_sql.ToString());
        }

        private static void Del(int id)
        {
            DBHelper.ExecuteCommand($"delete from eventset where id = {id}");
        }

        public static List<EventSet> GetList()
        {
            List<EventSet> _result = new List<EventSet>();
            DataTable _dt = DBHelper.GetDataTable("select * from eventset");
            foreach (DataRow dr in _dt.Rows)
            {
                EventSet _es = new EventSet()
                {
                    DataType = dr.Field<string>("DataType"),
                    Desc = dr.Field<string>("Ms"),
                    EquipType = dr.Field<int>("EquipType"),
                    ID = dr.Field<int>("ID"),
                    SpotNO = dr.Field<string>("SpotNO"),
                    Value = dr.Field<string>("Zhi"),
                    EventLevel = dr.Field<int?>("EventLevel"),
                    CMDID = dr.Field<int?>("CMDID"),
                     EType = dr.Field<string>("etype")
                };
                _result.Add(_es);
            }
            return _result;
        }
    }
}
