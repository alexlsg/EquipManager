using Microsoft.VisualStudio.TestTools.UnitTesting;
using SystemSet;
using System;
using System.Collections.Generic;
using System.Text;
using Tools;
using System.Data;

namespace SystemSet.Tests
{
    [TestClass()]
    public class EquipSpotSetServiceTests
    {
        [TestMethod()]
        public void AddEquipSpotSetTest()
        {
            Tools.DBHelper.ExecuteCommand("truncate table EquipSpotSet");
            EquipSpotSet EquipSpotSet = new EquipSpotSet();
            EquipSpotSet.EquipType = "EquipType";
            EquipSpotSet.DataType = "DataType";
            EquipSpotSet.SpotNO = "SpotNO";
            EquipSpotSet.SaveType = 1;
            EquipSpotSetService service = new EquipSpotSetService();
            HttpResult httpResult = service.AddEquipSpotSet(EquipSpotSet);
            Assert.AreEqual(httpResult.Status, true);
            Assert.AreEqual(httpResult.Message, "添加设备测点功能成功");
            DataTable dataTable = Tools.DBHelper.GetDataTable("select * from EquipSpotSet");
            Assert.AreEqual(dataTable.Rows.Count, 1);
            Assert.AreEqual(dataTable.Rows[0]["EquipType"].ToString(), "EquipType");
            Assert.AreEqual(dataTable.Rows[0]["SaveType"].ToString(), "1");
            Assert.AreEqual(dataTable.Rows[0]["DataType"].ToString(), "DataType");
            Assert.AreEqual(dataTable.Rows[0]["SpotNO"].ToString(), "SpotNO");
        }

        [TestMethod()]
        public void ModEquipSpotSetTest()
        {
            Tools.DBHelper.ExecuteCommand(@"
truncate table EquipSpotSet
");
            Tools.DBHelper.ExecuteCommand(@"
insert into EquipSpotSet(EquipType,DataType,SpotNO,SaveType)
values('1','2','3',4)
");
            EquipSpotSet EquipSpotSet = new EquipSpotSet();
            EquipSpotSet.EquipType = "EquipType";
            EquipSpotSet.DataType = "DataType";
            EquipSpotSet.SaveType = 2;
            EquipSpotSet.SpotNO = "SpotNO";
            EquipSpotSet.Id = 1;
            EquipSpotSetService service = new EquipSpotSetService();
            HttpResult httpResult = service.ModEquipSpotSet(EquipSpotSet);
            Assert.AreEqual(httpResult.Status, true);
            Assert.AreEqual(httpResult.Message, "修改设备测点功能成功");
            DataTable dataTable = Tools.DBHelper.GetDataTable("select * from EquipSpotSet");
            Assert.AreEqual(dataTable.Rows.Count, 1);
            Assert.AreEqual(dataTable.Rows[0]["EquipType"].ToString(), "EquipType");
            Assert.AreEqual(dataTable.Rows[0]["SaveType"].ToString(), "2");
            Assert.AreEqual(dataTable.Rows[0]["DataType"].ToString(), "DataType");
            Assert.AreEqual(dataTable.Rows[0]["SpotNO"].ToString(), "SpotNO");
        }

        [TestMethod()]
        public void DelEquipSpotSetTest()
        {
            Tools.DBHelper.ExecuteCommand(@"
truncate table EquipSpotSet
");
            Tools.DBHelper.ExecuteCommand(@"
insert into EquipSpotSet(EquipType,DataType,SpotNO,SaveType)
values('1','2','3',4)
");
            EquipSpotSet EquipSpotSet = new EquipSpotSet();
            EquipSpotSet.Id = 1;
            EquipSpotSetService service = new EquipSpotSetService();
            HttpResult httpResult = service.DelEquipSpotSet(EquipSpotSet);
            Assert.AreEqual(httpResult.Status, true);
            Assert.AreEqual(httpResult.Message, "删除设备测点功能成功");
            DataTable dataTable = Tools.DBHelper.GetDataTable("select * from EquipSpotSet");
            Assert.AreEqual(dataTable.Rows.Count, 0);
        }

        [TestMethod()]
        public void GetEquipSpotSetList()
        {
            Tools.DBHelper.ExecuteCommand(@"
truncate table EquipSpotSet
");
            Tools.DBHelper.ExecuteCommand(@"
insert into EquipSpotSet(EquipType,DataType,SpotNO,SaveType)
values('EquipType','DataType','SpotNO',4)
");
            EquipSpotSetService service = new EquipSpotSetService();
            HttpResult httpResult = service.GetEquipSpotSetList();
            Assert.AreEqual(httpResult.Status, true);
            Assert.AreEqual(httpResult.Message, "查询设备测点功能列表成功");
            List<EquipSpotSet> EquipSpotSet = httpResult.Data as List<EquipSpotSet>;
            Assert.AreEqual(EquipSpotSet[0].EquipType, "EquipType");
            Assert.AreEqual(EquipSpotSet[0].SaveType, 4);
            Assert.AreEqual(EquipSpotSet[0].DataType, "DataType");
            Assert.AreEqual(EquipSpotSet[0].SpotNO, "SpotNO");
        }
    }
}