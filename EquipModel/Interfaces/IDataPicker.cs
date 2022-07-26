﻿using EquipModel;
using System.Collections.Generic;

namespace EquipModel.Interfaces
{
    public interface IDataPicker
    {
        string GetDataFromEquip(Equip equip);
        List<EquipData> AnalyzData(string data, Equip equip);
        string GetToken(Equip equip);
    }
}
