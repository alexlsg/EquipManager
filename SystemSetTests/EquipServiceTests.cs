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
    public class EquipServiceTests
    {
        [TestMethod()]
        public void AddEquipTest()
        {
            Tools.DBHelper.ExecuteCommand("truncate table Equip");
            Equip Equip = new Equip();
            Equip.EquipNO = "EquipNO";
            Equip.EquipName = "EquipName";
            Equip.GatewayId = 1;
            Equip.TypeBinding = "TypeBinding";
            Equip.ProductionLineGroupBinding = "ProductionLineGroupBinding";
            EquipService service = new EquipService();
            HttpResult httpResult = service.AddEquip(Equip);
            Assert.AreEqual(httpResult.Status, true);
            Assert.AreEqual(httpResult.Message, "添加设备功能成功");
            DataTable dataTable = Tools.DBHelper.GetDataTable("select * from Equip");
            Assert.AreEqual(dataTable.Rows.Count, 1);
            Assert.AreEqual(dataTable.Rows[0]["EquipName"].ToString(), "EquipName");
            Assert.AreEqual((int)dataTable.Rows[0]["GatewayId"], 1);
            Assert.AreEqual(dataTable.Rows[0]["EquipNO"].ToString(), "EquipNO");
            Assert.AreEqual(dataTable.Rows[0]["TypeBinding"].ToString(), "TypeBinding");
            Assert.AreEqual(dataTable.Rows[0]["ProductionLineGroupBinding"].ToString(), "ProductionLineGroupBinding");
        }

        [TestMethod()]
        public void ModEquipTest()
        {
            Tools.DBHelper.ExecuteCommand(@"
truncate table Equip
");
            Tools.DBHelper.ExecuteCommand(@"
insert into Equip(EquipNO,EquipName,GatewayId,TypeBinding,ProductionLineGroupBinding)
values('1','2',1,'3','4')
");
            Equip Equip = new Equip();
            Equip.EquipNO = "EquipNO";
            Equip.EquipName = "EquipName";
            Equip.GatewayId = 2;
            Equip.TypeBinding = "TypeBinding";
            Equip.ProductionLineGroupBinding = "ProductionLineGroupBinding";
            Equip.Id = 1;
            EquipService service = new EquipService();
            HttpResult httpResult = service.ModEquip(Equip);
            Assert.AreEqual(httpResult.Status, true);
            Assert.AreEqual(httpResult.Message, "修改设备功能成功");
            DataTable dataTable = Tools.DBHelper.GetDataTable("select * from Equip");
            Assert.AreEqual(dataTable.Rows.Count, 1);
            Assert.AreEqual(dataTable.Rows[0]["EquipName"].ToString(), "EquipName");
            Assert.AreEqual((int)dataTable.Rows[0]["GatewayId"], 2);
            Assert.AreEqual(dataTable.Rows[0]["EquipNO"].ToString(), "EquipNO");
            Assert.AreEqual(dataTable.Rows[0]["TypeBinding"].ToString(), "TypeBinding");
            Assert.AreEqual(dataTable.Rows[0]["ProductionLineGroupBinding"].ToString(), "ProductionLineGroupBinding");
        }

        [TestMethod()]
        public void DelEquipTest()
        {
            Tools.DBHelper.ExecuteCommand(@"
truncate table Equip
");
            Tools.DBHelper.ExecuteCommand(@"
insert into Equip(EquipNO,EquipName,GatewayId,TypeBinding,ProductionLineGroupBinding)
values('1','2',1,'3','4')
");
            Equip Equip = new Equip();
            Equip.Id = 1;
            EquipService service = new EquipService();
            HttpResult httpResult = service.DelEquip(Equip);
            Assert.AreEqual(httpResult.Status, true);
            Assert.AreEqual(httpResult.Message, "删除设备功能成功");
            DataTable dataTable = Tools.DBHelper.GetDataTable("select * from Equip");
            Assert.AreEqual(dataTable.Rows.Count, 0);
        }

        [TestMethod()]
        public void GetEquipList()
        {
            Tools.DBHelper.ExecuteCommand(@"
truncate table Equip
");
            Tools.DBHelper.ExecuteCommand(@"
insert into Equip(EquipNO,EquipName,GatewayId,TypeBinding,ProductionLineGroupBinding)
values('EquipNO','EquipName',1,'TypeBinding','ProductionLineGroupBinding')
");
            EquipService service = new EquipService();
            HttpResult httpResult = service.GetEquipList();
            Assert.AreEqual(httpResult.Status, true);
            Assert.AreEqual(httpResult.Message, "查询设备功能列表成功");
            List<Equip> Equip = httpResult.Data as List<Equip>;
            Assert.AreEqual(Equip[0].EquipNO, "EquipNO");
            Assert.AreEqual(Equip[0].GatewayId, 1);
            Assert.AreEqual(Equip[0].ProductionLineGroupBinding, "ProductionLineGroupBinding");
            Assert.AreEqual(Equip[0].EquipName, "EquipName");
        }
    }
}