using EquipModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Tools;

namespace EquipDataManager.Dal
{
    internal class EquipDataDal
    {

        internal static void SaveEvent(EquipData data, Equip equip)
        {
            StringBuilder _sql = new StringBuilder();
            _sql.AppendLine("insert into equipevent(groupid,typeid,equipname,devid,starttime,spotname,spotid,dvalue)");
            _sql.AppendLine($"values('{equip.GroupName}','{equip.EquipTypeName}','{equip.Name}','{equip.NO}',now() ,'{data.SpotName}','{data.SpotNO}','{data.Data}')");
            DBHelper.ExecuteCommand(_sql.ToString());
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
