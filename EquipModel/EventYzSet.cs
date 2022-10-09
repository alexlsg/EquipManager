using System;
using System.Collections.Generic;
using System.Text;
using Tools;

namespace EquipModel
{
    public class EventYzSet
    {
        public int ID { get; set; }
        public int EquipID { get; set; }
        public string DataType { get; set; }
        public string SpotNO { get; set; }
        public int LowerValue { get; set; }
        public int UperValue { get; set; }
        public int LowerHF { get; set; }
        public int UperHF { get; set; }
        public string MsHf { get; set; }
        public string MsUper { get; set; }
        public string MsLower { get; set; }
        public int EventLevel { get; set; } = 0;
        public int? CMDID { get; set; }
        public string EType { get; set; }
        public DataStatus State { get; set; } = DataStatus.NONE;
    }
}
