using System;
using System.Collections.Generic;
using System.Text;
using Tools;

namespace EquipModel
{
    public class EventSet
    {
        public int ID { get; set; }
        public int EquipType { get; set; }    
        public string DataType { get; set; }
        public string SpotNO { get; set; }
        public string Value { get; set; }
        public string Desc { get; set; }
        public int? EventLevel { get; set; } = 0;
        public int? CMDID { get; set; }
        public string EType { get; set; }
        public DataStatus State { get; set; } = DataStatus.NONE;
    }
}
