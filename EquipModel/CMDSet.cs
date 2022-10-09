using System;
using System.Collections.Generic;
using System.Text;
using Tools;

namespace EquipModel
{
    public class CMDSet
    {
        public int ID { get; set; } 
        public string CMD { get; set; }
        public string Param { get; set; }
        public string ParamValue { get; set; }
        public string TimeOut { get; set; }
        public string Level { get; set; }
        public string IsCurve { get; set; }
        public string Notes { get; set; }
        public DataStatus State { get; set; } = DataStatus.NONE;
    }
}
