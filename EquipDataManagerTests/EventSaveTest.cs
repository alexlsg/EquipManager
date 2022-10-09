using EquipDataManager.Bll;
using EquipDataManager.Dal;
using EquipDataManager.Relealise;
using EquipModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Tools;

namespace EquipDataManagerTests
{
    [TestClass]
    public class EventSaveTest
    {
        [TestMethod]
        public void Test1()
        {
            DataPicker _dp = DataPicker.Instance;
            _dp.Init();
            _dp.datapicker = new DataPickerRealise();
            _dp.Fxclsj(_dp.equipsSpotSets, _dp.equips.Find(n => n.ID == 13), Resource.data);
            DataTable _dt = DBHelper.GetDataTable("select * from equipevent");
            Assert.AreEqual(1, _dt.Rows.Count);
        }

        [TestMethod]
        public void Test3()
        {
            EquipDal.GetAllEquips();
        }

        [TestMethod]
        public void Test2()
        {
            DataPicker _dp = DataPicker.Instance;
            _dp.Init();
            _dp.datapicker = new DataPickerRealise();
            EquipData _ed = new EquipData()
            {
                EquipID = 1,
                EquipName = "A",
                EquipNO = "1",
                Data = "100",
                DataType = "A",
                SpotNO = "1",
            };
            _dp.lsdatas.Add(_dp.GetKey(_ed), _ed);
            _dp.dataSaveTime[_dp.GetKey(_ed)] = DateTime.Now;
            bool _b = _dp.CheckSave_Bhl(
                new EquipData()
                {
                    EquipID = 1,
                    EquipName = "A",
                    EquipNO = "1",
                    Data = "94",
                    DataType = "A",
                    SpotNO = "1",
                }, new EquipSpotSet()
                {
                    Bhl = 5,
                    DataType = "A",
                    SaveType = 3,
                    SpotNO = "1",

                });
            Assert.IsTrue(_b);
        }
        [TestMethod]
        public void TestMd5()
        {
          string _res =  Tools.DES.MD5Encrypt("123");
        }
    }
}
