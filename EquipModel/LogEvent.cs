using System;
using System.Collections.Generic;
using System.Text;

namespace EquipModel
{
    public class LogEvent
    {
        public int Type { get; set; }
        public int GroupID { get; set; }
        public string Event { get; set; }
        public string User { get; set; }
        public DateTime StartTime { get; set; }
        public string Ipaddr { get; set; }
    }
}
