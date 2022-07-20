using EquipModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Tools;

namespace EquipDataManager.Dal
{
    public class EquipDataDal
    {

        internal static void SaveEvent(EquipData data, Equip equip)
        {
            StringBuilder _sql = new StringBuilder();
            _sql.AppendLine("insert into equipevent(groupid,typeid,equipname,devid,starttime,spotname,spotid,dvalue)");
            _sql.AppendLine($"values('{equip.GroupID}','{equip.EquipType}','{equip.Name}','{equip.ID}',now() ,'{data.SpotName}','{data.SpotNO}','{data.DataType + data.Data}')");
            DBHelper.ExecuteCommand(_sql.ToString());
        }

        public static object GetEvent(string groupid, string equiptypeid, DateTime ksrq, DateTime jsrq, string datatype, string eventkey)
        {
            StringBuilder _sql = new StringBuilder();
            _sql.AppendLine("select a.*,c.EquipTypeName,d.EquipGroupName from equipevent a,Equip b,equiptype c,equipgroup d");
            _sql.AppendLine(@"where a.devID = b.Id and a.GroupID = d.EquipGroupId
and a.TypeID = c.EquipTypeId");
            if (!string.IsNullOrWhiteSpace(groupid))
            {
                _sql.AppendLine($"and b.ProductionLineGroupBinding = '{groupid}'");
            }
            if (!string.IsNullOrWhiteSpace(equiptypeid))
            {
                _sql.AppendLine($" and b.TypeBinding = {equiptypeid}");
            }
            if (!string.IsNullOrWhiteSpace(datatype))
            {
                _sql.AppendLine($" and a.SpotID like '{datatype}%'");
            }
            if (!string.IsNullOrWhiteSpace(eventkey))
            {
                _sql.AppendLine($" and a.event like '%{eventkey}%'");
            }
            _sql.AppendLine($"and date_format(StartTime,\"%Y%m%d\") >= '{ksrq.ToString("yyyyMMdd")}' and date_format(StartTime,\"%Y%m%d\") <= '{jsrq.ToString("yyyyMMdd")}'");
            DataTable _dt = DBHelper.GetDataTable(_sql.ToString());
            var _temp = from a in _dt.AsEnumerable()
                        select new
                        {
                            GroupName = a.Field<string>("EquipGroupName"),
                            TypeName = a.Field<string>("EquipTypeName"),
                            EquipName = a.Field<string>("EquipName"),
                            DevID = a.Field<string>("devID"),
                            SpotName = a.Field<string>("SpotName"),
                            SpotID = a.Field<string>("SpotID"),
                            Event = a.Field<string>("event"),
                            Proc = a.Field<string>("Proc"),
                            StartTime = a.Field<DateTime?>("StartTime"),
                            EndTime = a.Field<DateTime?>("EndTime"),
                            DValue = a.Field<string>("DValue"),
                            Level = a.Field<string>("Level"),
                            IsConfirm = a.Field<string>("isconfirm"),
                            Confirmor = a.Field<string>("Confirmor"),
                            Confirm_time = a.Field<string>("Confirm_time"),
                            Confirm_opinion = a.Field<string>("Confirm_opinion")
                        };
            return _temp.ToList();
        }

        static List<string> tables = new List<string>();
        internal static List<EquipData_Ls> GetLssj(string spotno, DateTime ksrq, DateTime jsrq)
        {
            string _tablename = "ed_" + spotno;
            DateTime _ksrq = new DateTime(ksrq.Year, ksrq.Month, ksrq.Day);
            DateTime _jsrq1 = jsrq.AddDays(1);
            DateTime _jsrq = new DateTime(_jsrq1.Year, _jsrq1.Month, _jsrq1.Day);
            DataTable _dt = DBHelper.GetDataTable($"select data,sj from {_tablename} where sj >= '{_ksrq.ToString("yyyy-MM-dd HH:mm:ss")}' and sj < '{_jsrq.ToString("yyyy-MM-dd HH:mm:ss")}'");
            var _temp = from a in _dt.AsEnumerable()
                        select new EquipData_Ls
                        {
                            Rq = a.Field<DateTime>("sj"),
                            Data = double.Parse(a.Field<string>("data"))
                        };
            return _temp.ToList();
        }
        internal static void SaveMx(EquipData data)
        {
            string _tablename = "ed_" + data.EquipID + data.DataType + data.SpotNO;
            if (!tables.Contains(_tablename))
            {
                try
                {
                    StringBuilder _csql = new StringBuilder();
                    _csql.AppendLine($"create table `{_tablename}`(");
                    //_csql.AppendLine("`equipno` varchar(50) NULL DEFAULT NULL COLLATE 'utf8mb4_unicode_ci',");
                    //_csql.AppendLine("`datatype` varchar(50) NULL DEFAULT NULL COLLATE 'utf8mb4_unicode_ci',");
                    _csql.AppendLine("`event` varchar(50) NULL DEFAULT NULL COLLATE 'utf8mb4_unicode_ci',");
                    _csql.AppendLine("`data` varchar(50) NULL DEFAULT NULL COLLATE 'utf8mb4_unicode_ci',");
                    _csql.AppendLine("`sj` datetime not null)COLLATE='utf8mb4_unicode_ci' ENGINE=InnoDB;");
                    DBHelper.ExecuteCommand(_csql.ToString());
                }
                catch (Exception)
                {

                }

                tables.Add(_tablename);
            }
            StringBuilder _sql = new StringBuilder();
            _sql.AppendLine($"insert into {_tablename}(data,sj,event)");
            _sql.AppendLine($"values('{data.Data}',now(),'{data.Event}')");
            DBHelper.ExecuteCommand(_sql.ToString());
        }
    }
}
