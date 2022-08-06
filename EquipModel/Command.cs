using System;
using System.Collections.Generic;
using System.Text;

namespace EquipModel
{
    public class WebCommand
    {
        public string User { get; set; }
        public List<Command> Commands { get; set; }
    }
    public class Command
    {
        public int EquipID { get; set; }
        public string CMD { get; set; }
        public string Param { get; set; }
        public string ParamValue { get; set; }
        public string TimeOut { get; set; }
        public string Level { get; set; }
        public string IsCurve { get; set; }
        public string CMDNotes { get; set; }
        public string User { get; set; }
        public string IP { get; set; }
        public string Result { get; set; }

    }
}
