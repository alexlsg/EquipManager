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
    public class EquipTypeServiceTests
    {
        [TestMethod()]
        public void AddEquipTypeTest()
        {
            Tools.DBHelper.ExecuteCommand("truncate table EquipType");
            EquipType EquipType = new EquipType();
            EquipType.EquipTypeId = 1;
            EquipType.EquipTypeName = "EquipTypeName";
            EquipTypeService service = new EquipTypeService();
            HttpResult httpResult = service.AddEquipType(EquipType);
            Assert.AreEqual(httpResult.Status, true);
            Assert.AreEqual(httpResult.Message, "添加设备类型成功");
            DataTable dataTable = Tools.DBHelper.GetDataTable("select * from EquipType");
            Assert.AreEqual(dataTable.Rows.Count, 1);
            Assert.AreEqual((int)dataTable.Rows[0]["EquipTypeId"], 1);
            Assert.AreEqual(dataTable.Rows[0]["EquipTypeName"], "EquipTypeName");
        }

        [TestMethod()]
        public void ModEquipTypeTest()
        {
            Tools.DBHelper.ExecuteCommand(@"
truncate table EquipType
");
            Tools.DBHelper.ExecuteCommand(@"
insert into EquipType(EquipTypeName)
values('1')
");
            EquipType EquipType = new EquipType();
            EquipType.EquipTypeName = "EquipTypeName";
            EquipType.EquipTypeId = 1;
            EquipTypeService service = new EquipTypeService();
            HttpResult httpResult = service.ModEquipType(EquipType);
            Assert.AreEqual(httpResult.Status, true);
            Assert.AreEqual(httpResult.Message, "修改设备类型成功");
            DataTable dataTable = Tools.DBHelper.GetDataTable("select * from EquipType");
            Assert.AreEqual(dataTable.Rows.Count, 1);
            Assert.AreEqual(dataTable.Rows[0]["EquipTypeName"], "EquipTypeName");
        }

        [TestMethod()]
        public void DelEquipTypeTest()
        {
            Tools.DBHelper.ExecuteCommand(@"
truncate table EquipType
");
            Tools.DBHelper.ExecuteCommand(@"
insert into EquipType(EquipTypeName)
values('1')
");

            EquipType EquipType = new EquipType();
            EquipType.EquipTypeName = "ZoneBinding";
            EquipType.EquipTypeId = 1;
            EquipTypeService service = new EquipTypeService();
            HttpResult httpResult = service.DelEquipType(EquipType);
            Assert.AreEqual(httpResult.Status, true);
            Assert.AreEqual(httpResult.Message, "删除设备类型成功");
            DataTable dataTable = Tools.DBHelper.GetDataTable("select * from EquipType");
            Assert.AreEqual(dataTable.Rows.Count, 0);
        }

        [TestMethod()]
        public void GetEquipTypeList()
        {
            Tools.DBHelper.ExecuteCommand(@"
truncate table EquipType
");
            Tools.DBHelper.ExecuteCommand(@"
insert into EquipType(EquipTypeName)
values('EquipTypeName')
");
            EquipTypeService service = new EquipTypeService();
            HttpResult httpResult = service.GetEquipTypeList();
            Assert.AreEqual(httpResult.Status, true);
            Assert.AreEqual(httpResult.Message, "查询设备类型列表成功");
            List<EquipType> EquipType = httpResult.Data as List<EquipType>;
            Assert.AreEqual(EquipType[0].EquipTypeName, "EquipTypeName");
            Assert.AreEqual((int)EquipType[0].EquipTypeId, 1);
        }
    }
}