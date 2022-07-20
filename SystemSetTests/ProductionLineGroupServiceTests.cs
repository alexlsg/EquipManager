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
    public class ProductionLineGroupServiceTests
    {
        [TestMethod()]
        public void AddProductionLineGroupTest()
        {
            Tools.DBHelper.ExecuteCommand("truncate table ProductionLineGroup");
            ProductionLineGroup ProductionLineGroup = new ProductionLineGroup();
            ProductionLineGroup.ProductionLineId = 1;
            ProductionLineGroup.EquipNO = "EquipNO";
            ProductionLineGroupService service = new ProductionLineGroupService();
            HttpResult httpResult = service.AddProductionLineGroup(ProductionLineGroup);
            Assert.AreEqual(httpResult.Status, true);
            Assert.AreEqual(httpResult.Message, "添加产线组成功");
            DataTable dataTable = Tools.DBHelper.GetDataTable("select * from ProductionLineGroup");
            Assert.AreEqual(dataTable.Rows.Count, 1);
            Assert.AreEqual((int)dataTable.Rows[0]["ProductionLineId"], 1);
            Assert.AreEqual(dataTable.Rows[0]["EquipNO"], "EquipNO");
        }

        [TestMethod()]
        public void ModProductionLineGroupTest()
        {
            Tools.DBHelper.ExecuteCommand(@"
truncate table ProductionLineGroup
");
            Tools.DBHelper.ExecuteCommand(@"
insert into ProductionLineGroup(ProductionLineId,EquipNO)
values(1,'2')
");
            ProductionLineGroup ProductionLineGroup = new ProductionLineGroup();
            ProductionLineGroup.EquipNO = "EquipNO";
            ProductionLineGroup.ProductionLineId = 2;
            ProductionLineGroup.ProductionLineGroupId = 1;
            ProductionLineGroupService service = new ProductionLineGroupService();
            HttpResult httpResult = service.ModProductionLineGroup(ProductionLineGroup);
            Assert.AreEqual(httpResult.Status, true);
            Assert.AreEqual(httpResult.Message, "修改产线组成功");
            DataTable dataTable = Tools.DBHelper.GetDataTable("select * from ProductionLineGroup");
            Assert.AreEqual(dataTable.Rows.Count, 1);
            Assert.AreEqual(dataTable.Rows[0]["EquipNO"], "EquipNO");
            Assert.AreEqual((int)dataTable.Rows[0]["ProductionLineId"],2);
        }

        [TestMethod()]
        public void DelProductionLineGroupTest()
        {
            Tools.DBHelper.ExecuteCommand(@"
truncate table ProductionLineGroup
");
            Tools.DBHelper.ExecuteCommand(@"
insert into ProductionLineGroup(ProductionLineId,EquipNO)
values(1,'2')
");

            ProductionLineGroup ProductionLineGroup = new ProductionLineGroup();
            ProductionLineGroup.EquipNO = "EquipNO";
            ProductionLineGroup.ProductionLineGroupId = 1;
            ProductionLineGroupService service = new ProductionLineGroupService();
            HttpResult httpResult = service.DelProductionLineGroup(ProductionLineGroup);
            Assert.AreEqual(httpResult.Status, true);
            Assert.AreEqual(httpResult.Message, "删除产线组成功");
            DataTable dataTable = Tools.DBHelper.GetDataTable("select * from ProductionLineGroup");
            Assert.AreEqual(dataTable.Rows.Count, 0);
        }

        [TestMethod()]
        public void GetProductionLineGroupList()
        {
            Tools.DBHelper.ExecuteCommand(@"
truncate table ProductionLineGroup
");
            Tools.DBHelper.ExecuteCommand(@"
insert into ProductionLineGroup(ProductionLineId,EquipNO)
values(1,'EquipNO')
");
            ProductionLineGroupService service = new ProductionLineGroupService();
            HttpResult httpResult = service.GetProductionLineGroupList();
            Assert.AreEqual(httpResult.Status, true);
            Assert.AreEqual(httpResult.Message, "查询产线组列表成功");
            List<ProductionLineGroup> ProductionLineGroup = httpResult.Data as List<ProductionLineGroup>;
            Assert.AreEqual(ProductionLineGroup[0].EquipNO, "EquipNO");
            Assert.AreEqual((int)ProductionLineGroup[0].ProductionLineId, 1);
        }
    }
}