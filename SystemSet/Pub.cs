using System;
using System.Collections.Generic;
using System.Text;
using SystemSet;
using Tools;

namespace SystemSet
{
    public class Pub
    {
        static List<EquipType> equipTypes;
        /// <summary>
        /// 设备类型列表
        /// </summary>
        public static List<EquipType> EquipTypes
        {
            get
            {
                if (equipTypes == null || equipTypes.Count == 0)
                {
                    EquipTypeService equipTypeService = new EquipTypeService();
                    HttpResult _httpResult = equipTypeService.GetEquipTypeList();
                    equipTypes = _httpResult.Data as List<EquipType>;
                }
                return equipTypes;
            }
        }

        static List<Equip> equips;
        /// <summary>
        /// 设备类型列表
        /// </summary>
        public static List<Equip> Equips
        {
            get
            {
                if (equips == null || equips.Count == 0)
                {
                    EquipService equipTypeService = new EquipService();
                    HttpResult _httpResult = equipTypeService.GetEquipList();
                    equips = _httpResult.Data as List<Equip>;
                }
                return equips;
            }
        }
    }
}
