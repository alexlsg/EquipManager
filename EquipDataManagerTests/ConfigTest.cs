using EquipDataManager.Dal;
using EquipDataManager.Relealise;
using EquipModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using ThirdParty.Json.LitJson;
using Tools;

namespace EquipDataManagerTests
{
    [TestClass]
    public class ConfigTest
    {

        [TestMethod]
        public void DataPickerRealiseTest()
        {
            string _data = File.ReadAllText("122.xml",Encoding.UTF8);
            DataPickerRealise _dpr = new DataPickerRealise();
            List<EquipData> _datas = _dpr.AnalyzData(_data,1);
            Assert.AreEqual(_datas.Count > 0, true);
        }
    }
}
