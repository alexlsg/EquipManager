using EquipDataManager.Bll;
using EquipModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Tools;

namespace EquipDataManager.Dal
{
    public class CMDSetDal
    {
        public static void SaveCMDSet(CMDSet set)
        {
            if (set.ID > 0)
            {
                if (set.State == DataStatus.DEL)
                {
                    Del(set.ID);
                }
                else
                    ModCMDSet(set);
            }
            else
                AddCMDSet(set);
            DataPicker.Instance.RefreshEventSet();
        }
        private static void AddCMDSet(CMDSet set)
        {
            StringBuilder _sql = new StringBuilder();
            _sql.AppendLine("insert into Equip_linkage(CMD,Param,ParamValue,TimeOut,Level,IsCurve,Notes)");
            _sql.AppendLine($"values('{set.CMD}','{set.Param}','{set.ParamValue}','{set.TimeOut}','{set.Level}','{set.IsCurve}','{set.Notes}')");
            DBHelper.ExecuteCommand(_sql.ToString());
        }
        private static void ModCMDSet(CMDSet set)
        {
            StringBuilder _sql = new StringBuilder();
            _sql.AppendLine($"update Equip_linkage set CMD='{set.CMD}',Param='{set.Param}',ParamValue='{set.ParamValue}',TimeOut='{set.TimeOut}',Level='{set.Level}',IsCurve='{set.IsCurve}',Notes='{set.Notes}' where id = {set.ID}");
            DBHelper.ExecuteCommand(_sql.ToString());
        }

        private static void Del(int id)
        {
            DBHelper.ExecuteCommand($"delete from Equip_linkage where id = {id}");
        }

        public static List<CMDSet> GetList()
        {
            List<CMDSet> _result = new List<CMDSet>();
            DataTable _dt = DBHelper.GetDataTable("select * from Equip_linkage");
            foreach (DataRow dr in _dt.Rows)
            {
                CMDSet _es = new CMDSet()
                {
                    CMD = dr.Field<string>("CMD"),
                    Param = dr.Field<string>("Param"),
                    ParamValue = dr.Field<string>("ParamValue"),
                    TimeOut = dr.Field<string>("TimeOut"),
                    ID = dr.Field<int>("ID"),
                    Level = dr.Field<string>("Level"),
                    IsCurve = dr.Field<string>("IsCurve"),
                    Notes = dr.Field<string>("Notes")
                };
                _result.Add(_es);
            }
            return _result;
        }
    }
}
