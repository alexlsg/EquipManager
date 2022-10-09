using EquipDataManager.Bll;
using EquipModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Tools;

namespace EquipDataManager.Dal
{
    public class EventYzSetDal
    {
        public static void SaveEventYzSet(EventYzSet set)
        {
            if (set.ID > 0)
            {
                if (set.State == DataStatus.DEL)
                {
                    Del(set.ID);
                }
                else
                    ModEventYzSet(set);
            }
            else
                AddEventYzSet(set);
            DataPicker.Instance.RefreshEventSet();
        }
        private static void AddEventYzSet(EventYzSet set)
        {
            StringBuilder _sql = new StringBuilder();
            _sql.AppendLine("insert into EventYzSet(EquipID,DataType,SpotNO,LowerValue,UperValue,LowerHF,UperHF,MsHf,MsUper,MsLower,EventLevel,CMDID,etype)");
            _sql.AppendLine($"values({set.EquipID},'{set.DataType}','{set.SpotNO}',{set.LowerValue},{set.UperValue},{set.LowerHF},{set.UperHF},'{set.MsHf}','{set.MsUper}','{set.MsLower}',{Pub.IsDBNull(set.EventLevel)},{Pub.IsDBNull(set.CMDID)},'{set.EType}')");
            DBHelper.ExecuteCommand(_sql.ToString());
        }
        private static void ModEventYzSet(EventYzSet set)
        {
            StringBuilder _sql = new StringBuilder();
            _sql.AppendLine($"update EventYzSet set EquipID={set.EquipID},DataType='{set.DataType}',SpotNO='{set.SpotNO}',LowerValue={set.LowerValue},UperValue={set.UperValue},LowerHF={set.LowerHF},UperHF={set.UperHF},MsHf='{set.MsHf}',MsUper='{set.MsUper}',MsLower='{set.MsLower}',EventLevel={set.EventLevel},CMDID={Pub.IsDBNull(set.CMDID)},etype = '{set.EType}' where id = {set.ID}");
            DBHelper.ExecuteCommand(_sql.ToString());
        }

        private static void Del(int id)
        {
            DBHelper.ExecuteCommand($"delete from EventYzSet where id = {id}");
        }

        public static List<EventYzSet> GetList()
        {
            List<EventYzSet> _result = new List<EventYzSet>();
            DataTable _dt = DBHelper.GetDataTable("select * from EventYzSet");
            foreach (DataRow dr in _dt.Rows)
            {
                EventYzSet _es = new EventYzSet()
                {
                    DataType = dr.Field<string>("DataType"),
                    MsHf = dr.Field<string>("MsHf"),
                    MsLower = dr.Field<string>("MsLower"),
                    MsUper = dr.Field<string>("MsUper"),
                    EquipID = dr.Field<int>("EquipID"),
                    ID = dr.Field<int>("ID"),
                    SpotNO = dr.Field<string>("SpotNO"),
                    LowerHF = dr.Field<int>("LowerHF"),
                    LowerValue = dr.Field<int>("LowerValue"),
                    UperHF = dr.Field<int>("UperHF"),
                    UperValue = dr.Field<int>("UperValue"),
                    EventLevel = dr.Field<int>("EventLevel"),
                    CMDID = dr.Field<int?>("CMDID"),
                    EType = dr.Field<string>("etype")
                };
                _result.Add(_es);
            }
            return _result;
        }
    }
}
