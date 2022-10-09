using EquipDataManager.Bll;
using EquipModel;
using EquipModel.Interfaces;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace EquipDataManagerTests
{
    [TestClass]
    public class FjfxTest
    {
        [TestMethod]
        public void 有报警()
        {
            using (ShimsContext.Create())
            {
                EquipDataManager.Relealise.Fakes.ShimDataPickerRealise.AllInstances.GetDataFromEquipEquip = (a, b) =>
                {
                    return Resource.data1;
                };
                DataPicker.Instance.Init();
                DataPicker.Instance.datapicker = new EquipDataManager.Relealise.DataPickerRealise();
                Equip _e = DataPicker.Instance.equips.Find(n => n.NO == "12");
                DataPicker.Instance.ReadData(_e);
            }
        }

        [TestMethod]
        public void 设备离线()
        {
            using (ShimsContext.Create())
            {
                EquipDataManager.Relealise.Fakes.ShimDataPickerRealise.AllInstances.GetDataFromEquipEquip = (a, b) =>
                {
                    return Resource.data2;
                };
                DataPicker.Instance.Init();
                DataPicker.Instance.datapicker = new EquipDataManager.Relealise.DataPickerRealise();
                Equip _e = DataPicker.Instance.equips.Find(n => n.NO == "12");
                DataPicker.Instance.ReadData(_e);
                EquipDataManager.Relealise.Fakes.ShimDataPickerRealise.AllInstances.GetDataFromEquipEquip = (a, b) =>
                {
                    return Resource.data1;
                };
                DataPicker.Instance.ReadData(_e);
            }
        }
    }
}
