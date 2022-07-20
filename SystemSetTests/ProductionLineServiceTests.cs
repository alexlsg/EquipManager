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
    public class ProductionLineServiceTests
    {
        [TestMethod()]
        public void AddProductionLineTest()
        {
            Tools.DBHelper.ExecuteCommand("truncate table ProductionLine");
            ProductionLine ProductionLine = new ProductionLine();
            ProductionLine.ProductionLineName = "ProductionLineName";
            ProductionLineService service = new ProductionLineService();
            HttpResult httpResult = service.AddProductionLine(ProductionLine);
            Assert.AreEqual(httpResult.Status, true);
            Assert.AreEqual(httpResult.Message, "添加产线成功");
            DataTable dataTable = Tools.DBHelper.GetDataTable("select * from ProductionLine");
            Assert.AreEqual(dataTable.Rows.Count, 1);
            Assert.AreEqual(dataTable.Rows[0]["ProductionLineName"], "ProductionLineName");
        }

        [TestMethod()]
        public void ModProductionLineTest()
        {
            Tools.DBHelper.ExecuteCommand(@"
truncate table ProductionLine
");
            Tools.DBHelper.ExecuteCommand(@"
insert into ProductionLine(ProductionLineName)
values('2')
");
            ProductionLine ProductionLine = new ProductionLine();
            ProductionLine.ProductionLineName = "ProductionLineName";
            ProductionLine.ProductionLineId = 1;
            ProductionLineService service = new ProductionLineService();
            HttpResult httpResult = service.ModProductionLine(ProductionLine);
            Assert.AreEqual(httpResult.Status, true);
            Assert.AreEqual(httpResult.Message, "修改产线成功");
            DataTable dataTable = Tools.DBHelper.GetDataTable("select * from ProductionLine");
            Assert.AreEqual(dataTable.Rows.Count, 1);
            Assert.AreEqual(dataTable.Rows[0]["ProductionLineName"], "ProductionLineName");
        }

        [TestMethod()]
        public void DelProductionLineTest()
        {
            Tools.DBHelper.ExecuteCommand(@"
truncate table ProductionLine
");
            Tools.DBHelper.ExecuteCommand(@"
insert into ProductionLine(ProductionLineName)
values('2')
");

            ProductionLine ProductionLine = new ProductionLine();
            ProductionLine.ProductionLineName = "ProductionLineName";
            ProductionLine.ProductionLineId = 1;
            ProductionLineService service = new ProductionLineService();
            HttpResult httpResult = service.DelProductionLine(ProductionLine);
            Assert.AreEqual(httpResult.Status, true);
            Assert.AreEqual(httpResult.Message, "删除产线成功");
            DataTable dataTable = Tools.DBHelper.GetDataTable("select * from ProductionLine");
            Assert.AreEqual(dataTable.Rows.Count, 0);
        }

        [TestMethod()]
        public void GetProductionLineList()
        {
            Tools.DBHelper.ExecuteCommand(@"
truncate table ProductionLine
");
            Tools.DBHelper.ExecuteCommand(@"
insert into ProductionLine(ProductionLineName)
values('ProductionLineName')
");
            ProductionLineService service = new ProductionLineService();
            HttpResult httpResult = service.GetProductionLineList();
            Assert.AreEqual(httpResult.Status, true);
            Assert.AreEqual(httpResult.Message, "查询产线列表成功");
            List<ProductionLine> ProductionLine = httpResult.Data as List<ProductionLine>;
            Assert.AreEqual(ProductionLine[0].ProductionLineName, "ProductionLineName");
        }
    }
}