using EquipModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EquipDataManager.Bll
{
    public partial class DataPicker
    {
        public List<EventData> CurrentEvent { get; set; } = new List<EventData>();

        public void DealCurrentEvent(EventData ed)
        {
            lock (CurrentEvent)
            {
                CurrentEvent.RemoveAll(n => n.DevID == ed.DevID && n.SpotID == ed.SpotID);
                if (ed.Level != 0)
                {
                    CurrentEvent.Add(ed);
                }
            }
        }


        public object GetCurrentEvent(string user,string groupid, string equiptypeid, DateTime ksrq, DateTime jsrq, string eventkey, string sbid, string cdbh, string level, string eventtype)
        {
            lock (CurrentEvent)
            {
                var _temp = from a in CurrentEvent
                            where (string.IsNullOrWhiteSpace(groupid) || groupid.Split(',').Contains(a.GroupID))
                            && (string.IsNullOrWhiteSpace(equiptypeid) || equiptypeid.Split(',').Contains(a.EquipType))
                            && (string.IsNullOrWhiteSpace(sbid) || sbid.Split(',').Contains(a.DevID))
                            && (string.IsNullOrWhiteSpace(cdbh) || cdbh.Split(',').Contains(a.SpotID))
                            && (string.IsNullOrWhiteSpace(level) || level.Split(',').Contains(a.Level?.ToString()))
                            && (string.IsNullOrWhiteSpace(eventtype) || a.Type?.Contains(eventtype) == true)
                            && (string.IsNullOrWhiteSpace(eventkey) || a.EquipName?.Contains(eventkey) == true || a.SpotName?.Contains(eventkey) == true || a.Event?.Contains(eventkey) == true)
                            && Pub.CheckQx(user,a.DevID,a.GroupID)
                            select a;
                var _result = new
                {
                    AllCount = CurrentEvent.Count,
                    ResultCount = _temp.Count(),
                    Data = _temp.ToList().OrderByDescending(n => n.StartTime)
                };
                return _result;
            }
        }
    }
}
