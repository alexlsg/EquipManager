using EquipModel;
using System;
using System.Collections.Generic;
using System.Text;
using Tools;

namespace EquipDataManager.Dal
{
    public class LogEventDal
    {
        public static void AddLog(LogEvent log)
        {
            string _sql = $@"insert into LogEvent(Type,GroupID,Event,User,StartTime,Ipaddr)
values({log.Type},{log.GroupID},'{log.Event}','{log.User}',now(),'{log.Ipaddr}')
";
            DBHelper.ExecuteCommand(_sql);
        }

        internal static void AddLog(Equip eq, Command cmd)
        {
            LogEvent _log = new LogEvent()
            {
                Type = 8,
                GroupID = int.Parse(eq.GroupID),
                Event = $"{eq.Name};{cmd.CMDNotes};{cmd.CMD};{cmd.Param};{cmd.ParamValue}",
                User = cmd.User,
                Ipaddr = cmd.IP
            };
        }
    }
}
