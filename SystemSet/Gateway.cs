using System;
using System.Collections.Generic;
using System.Text;
using Tools;

namespace SystemSet
{
    /// <summary>
    /// 网关设置
    /// </summary>
    public class Gateway : BaseNotifyPropertyChanged
    {
        public Gateway()
        {
            PORT = "80";
            Type = 2;
        }
        public int Id { get; set; }
        string _Name;
        /// <summary>
        /// 网关名称
        /// </summary>
        public string Name { get { return _Name; } set { _Name = value; OnPropertyChanged("Name"); } }
        string _IP;
        /// <summary>
        /// ip
        /// </summary>
        public string IP { get { return _IP; } set { _IP = value; OnPropertyChanged("IP"); } }
        string _PORT;
        /// <summary>
        /// 端口
        /// </summary>
        public string PORT { get { return _PORT; } set { _PORT = value; OnPropertyChanged("PORT"); } }

        /// <summary>
        /// 设备列表
        /// </summary>
        public List<Equip> Equips { get; set; }
        public int Type { get; set; }
    }
}
