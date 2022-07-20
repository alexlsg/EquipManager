using System;
using System.Collections.Generic;
using System.Text;

namespace EquipDataManager
{
    internal static class Config
    {
        /// <summary>
        /// 静电设备类型
        /// </summary>
        internal const string Jdsblx = "5";
        /// <summary>
        /// 温湿度类型
        /// </summary>
        internal const string Wsdlx = "6";
        /// <summary>
        /// 离子风机类型
        /// </summary>
        internal const string Fjlx = "7";
        /// <summary>
        /// 静电设备取值测点
        /// </summary>
        internal static string[] Jdsbspots = new string[] { "11","13","15","17","13"};
        /// <summary>
        /// 静电设备报警值
        /// </summary>
        internal static string[] Jdsbbjz = new string[] { "4", "10", "7", "1" };
        /// <summary>
        /// 静电设备检测值
        /// </summary>
        internal static string[] Jdsbjcz = new string[] { "1", "3", "4", "6", "7", "9", "10", "13" };
    }
}
