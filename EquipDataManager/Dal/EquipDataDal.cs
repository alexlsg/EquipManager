using EquipDataManager.Bll;
using EquipModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Tools;
using UserManagement;

namespace EquipDataManager.Dal
{
    public class EquipDataDal
    {
        internal static void SaveEvent(string groupname, string groupid, string typename, string equiptype, string equipname, int sbid, string sj, string eventmsg, int? level, string etype)
        {
            Pub.AddTestLog($"开始写入事务设备:{sbid},事务:{eventmsg}");
            EventData _ed = new EventData()
            {
                DevID = sbid.ToString(),
                DValue = sj,
                GroupName = groupname,
                GroupID = groupid,
                EquipType = equiptype,
                TypeName = typename,
                EquipName = equipname,
                StartTime = DateTime.Now,
                Event = eventmsg,
                Level = level,
                Type = etype
            };
            DataPicker.Instance.DealCurrentEvent(_ed);
            StringBuilder _sql = new StringBuilder();
            _sql.AppendLine("insert into equipevent(groupid,typeid,equipname,devid,starttime,dvalue,event,level,etype)");
            _sql.AppendLine($"values('{groupid}','{equiptype}','{equipname}','{sbid}',now() ,'{sj}','{eventmsg}','{level}','{etype}')");
            DBHelper.ExecuteCommand($"update equipevent set endTime = now() where devid = {sbid} and endtime is null");
            DBHelper.ExecuteCommand(_sql.ToString());
            Pub.AddTestLog($"写入事务结束设备:{sbid},事务:{eventmsg}");
        }

        internal static void SaveTxzcEvent(string groupname, string groupid, string typename, string equiptype, string equipname, int sbid, string sj, string eventmsg, int? level, string etype)
        {
            Pub.AddTestLog($"开始写入事务设备:{sbid},事务:{eventmsg}");
            EventData _ed = new EventData()
            {
                DevID = sbid.ToString(),
                DValue = sj,
                GroupName = groupname,
                GroupID = groupid,
                EquipType = equiptype,
                TypeName = typename,
                EquipName = equipname,
                StartTime = DateTime.Now,
                Event = eventmsg,
                Level = level,
                Type = etype
            };
            DataPicker.Instance.DealCurrentEvent(_ed);
            StringBuilder _sql = new StringBuilder();
            _sql.AppendLine("insert into equipevent(groupid,typeid,equipname,devid,starttime,spotname,spotid,dvalue,event,level,etype,endTime)");
            _sql.AppendLine($"values('{groupid}','{equiptype}','{equipname}','{sbid}',now() ,'{sj}','{eventmsg}','{level}','{etype}',now())");
            DBHelper.ExecuteCommand($"update equipevent set endTime = now() where devid = {sbid} and endtime is null");
            DBHelper.ExecuteCommand(_sql.ToString());
            Pub.AddTestLog($"写入事务结束设备:{sbid},事务:{eventmsg}");
        }
        internal static void SaveEvent(EquipData data, Equip equip)
        {
            Pub.AddTestLog($"写入事务设备:{equip.ID},事务:{data.eventmessage}");
            int? _level = data.eventset == null ? data.eventyset.EventLevel : data.eventset.EventLevel;
            string _etype = null;
            switch (data.eventtype)
            {
                case 0: _etype = "0x00001001"; _level = 0; break;
                case 1: _etype = "0x00001004"; break;
                case 2: _etype = "0x00001002"; break;
                case 3:
                    if (data.eventset.EventLevel == 0)
                    {
                        _etype = "0x00002001";
                    }
                    else
                    {
                        _etype = "0x00002002";
                    }
                    break;
                default: break;
            }
            if (string.IsNullOrEmpty(_etype) && data.eventset != null)
            {
                _etype = data.eventset.EType;
            }

            EventData _ed = new EventData()
            {
                DevID = equip.ID.ToString(),
                DValue = data.Data,
                GroupName = equip.GroupName,
                GroupID = equip.GroupID,
                EquipType = equip.EquipType,
                TypeName = equip.EquipTypeName,
                EquipName = equip.Name,
                SpotName = data.SpotName,
                SpotID = data.SpotNO,
                StartTime = DateTime.Now,
                Event = data.eventmessage,
                Level = _level,
                Type = _etype
            };
            DataPicker.Instance.DealCurrentEvent(_ed);
            StringBuilder _sql = new StringBuilder();
            _sql.AppendLine("insert into equipevent(groupid,typeid,equipname,devid,starttime,spotname,spotid,dvalue,event,level,etype)");
            _sql.AppendLine($"values('{equip.GroupID}','{equip.EquipType}','{equip.Name}','{equip.ID}',now() ,'{data.SpotName}','{data.SpotNO}','{data.Data}','{data.eventmessage}','{_level}','{_etype}')");
            DBHelper.ExecuteCommand($"update equipevent set endTime = now() where devid = {equip.ID} and spotid = '{data.SpotNO}' and endtime is null");
            DBHelper.ExecuteCommand(_sql.ToString());
            if (data.eventtype == 0)
            {
                DBHelper.ExecuteCommand($"update equipevent set endTime = starttime where devid = {equip.ID} and spotid = '{data.SpotNO}' and endtime is null");
            }

            Pub.AddTestLog($"写入事务结束设备:{equip.ID},事务:{data.eventmessage}");
        }

        public static object GetEvent(string user, string groupid, string equiptypeid, DateTime ksrq, DateTime jsrq, string eventkey, string sbid, string cdbh, string level, string eventtype)
        {
            StringBuilder _sql = new StringBuilder();
            _sql.AppendLine("select a.*,c.EquipTypeName,d.EquipGroupName from equipevent a,Equip b,equiptype c,equipgroup d");
            _sql.AppendLine(@"where a.devID = b.Id and a.GroupID = d.EquipGroupId
and a.TypeID = c.EquipTypeId");
            _sql.AppendLine($"and date_format(StartTime,\"%Y%m%d\") >= '{ksrq.ToString("yyyyMMdd")}' and date_format(StartTime,\"%Y%m%d\") <= '{jsrq.ToString("yyyyMMdd")}'");
            _sql.AppendLine("union");
            _sql.AppendLine("select a.*,c.EquipTypeName,d.EquipGroupName from equipevent a, equiptype c,equipgroup d");
            _sql.AppendLine($"where date_format(StartTime,\"%Y%m%d\") >= '{ksrq.ToString("yyyyMMdd")}' and date_format(StartTime,\"%Y%m%d\") <= '{jsrq.ToString("yyyyMMdd")}'");
            _sql.AppendLine("and a.GroupID = d.EquipGroupId");
            _sql.AppendLine("and a.TypeID = c.EquipTypeId");
            DataTable _dt = DBHelper.GetDataTable(_sql.ToString());
            var _temp = from a in _dt.AsEnumerable()
                        where (string.IsNullOrWhiteSpace(groupid) || groupid.Split(',').Contains(a.Field<string>("GroupID")))
                        && (string.IsNullOrWhiteSpace(equiptypeid) || equiptypeid.Split(',').Contains(a.Field<string>("TypeID")))
                        && (string.IsNullOrWhiteSpace(sbid) || sbid.Split(',').Contains(a.Field<string>("devID")))
                        && (string.IsNullOrWhiteSpace(cdbh) || cdbh.Split(',').Contains(a.Field<string>("SpotID")))
                        && (string.IsNullOrWhiteSpace(level) || level.Split(',').Contains(a.Field<string>("Level")))
                        && (string.IsNullOrWhiteSpace(eventtype) || a.Field<string>("etype")?.Contains(eventtype) == true)
                        && (string.IsNullOrWhiteSpace(eventkey) || a.Field<string>("EquipName")?.Contains(eventkey) == true || a.Field<string>("SpotName")?.Contains(eventkey) == true || a.Field<string>("event")?.Contains(eventkey) == true)
                        && Pub.CheckQx(user, a.Field<string>("devID"), a.Field<string>("GroupID"))
                        select new EventData
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
                            Level = GetValue(a.Field<string>("Level")),
                            IsConfirm = a.Field<string>("isconfirm"),
                            Confirmor = a.Field<string>("Confirmor"),
                            Confirm_time = a.Field<string>("Confirm_time"),
                            Confirm_opinion = a.Field<string>("Confirm_opinion"),
                            Type = a.Field<string>("etype")
                        };
            var _result = new
            {
                AllCount = _dt.Rows.Count,
                ResultCount = _temp.Count(),
                Data = _temp.ToList().OrderByDescending(n => n.StartTime)
            };
            return _result;
        }



        static int? GetValue(string s)
        {
            if (s == null)
            {
                return null;
            }
            else
                return int.Parse(s);
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
