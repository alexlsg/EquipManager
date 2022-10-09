using System;
using System.Collections.Generic;
using System.Text;

namespace EquipModel
{
    public class EventData
    {
        public string GroupID { get; set; }
        public string GroupName { get; set; }
        public string EquipType { get; set; }
        public string TypeName { get; set; }
        public string EquipName { get; set; }
        public string DevID { get; set; }
        public string SpotName { get; set; }
        public string SpotID { get; set; }
        public string Event { get; set; }
        public string Proc { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public string DValue { get; set; }
        public int? Level { get; set; }
        public string IsConfirm { get; set; }
        public string Confirmor { get; set; }
        public string Confirm_time { get; set; }
        public string Confirm_opinion { get; set; }
        public string Type { get; set; }
    }
}
