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
    public class GatewayServiceTests
    {
        [TestMethod()]
        public void AddGatewayTest()
        {
            Tools.DBHelper.ExecuteCommand("truncate table Gateway");
            Gateway Gateway = new Gateway();
            Gateway.IP = "IP";
            Gateway.PORT = "PORT";
            Gateway.Name = "Name";
            GatewayService service = new GatewayService();
            HttpResult httpResult = service.AddGateway(Gateway);
            Assert.AreEqual(httpResult.Status, true);
            Assert.AreEqual(httpResult.Message, "添加网关功能成功");
            DataTable dataTable = Tools.DBHelper.GetDataTable("select * from Gateway");
            Assert.AreEqual(dataTable.Rows.Count, 1);
            Assert.AreEqual((int)dataTable.Rows[0]["Id"], 1);
            Assert.AreEqual(dataTable.Rows[0]["IP"], "IP");
            Assert.AreEqual(dataTable.Rows[0]["PORT"], "PORT");
            Assert.AreEqual(dataTable.Rows[0]["Name"], "Name");
        }

        [TestMethod()]
        public void ModGatewayTest()
        {
            Tools.DBHelper.ExecuteCommand(@"
truncate table Gateway
");
            Tools.DBHelper.ExecuteCommand(@"
insert into Gateway(IP,PORT,Name)
values('1','2','3')
");
            Gateway Gateway = new Gateway();
            Gateway.IP = "IP";
            Gateway.PORT = "PORT";
            Gateway.Name = "Name";
            Gateway.Id = 1;
            GatewayService service = new GatewayService();
            HttpResult httpResult = service.ModGateway(Gateway);
            Assert.AreEqual(httpResult.Status, true);
            Assert.AreEqual(httpResult.Message, "修改网关功能成功");
            DataTable dataTable = Tools.DBHelper.GetDataTable("select * from Gateway");
            Assert.AreEqual(dataTable.Rows.Count, 1);
            Assert.AreEqual(dataTable.Rows[0]["IP"], "IP");
            Assert.AreEqual(dataTable.Rows[0]["PORT"], "PORT");
            Assert.AreEqual(dataTable.Rows[0]["Name"], "Name");
        }

        [TestMethod()]
        public void DelGatewayTest()
        {
            Tools.DBHelper.ExecuteCommand(@"
truncate table Gateway
");
            Tools.DBHelper.ExecuteCommand(@"
insert into Gateway(Name)
values('1')
");

            Gateway Gateway = new Gateway();
            Gateway.Name = "Name";
            Gateway.Id = 1;
            GatewayService service = new GatewayService();
            HttpResult httpResult = service.DelGateway(Gateway);
            Assert.AreEqual(httpResult.Status, true);
            Assert.AreEqual(httpResult.Message, "删除网关功能成功");
            DataTable dataTable = Tools.DBHelper.GetDataTable("select * from Gateway");
            Assert.AreEqual(dataTable.Rows.Count, 0);
        }

        [TestMethod()]
        public void GetGatewayList()
        {
            Tools.DBHelper.ExecuteCommand(@"
truncate table Gateway
");
            Tools.DBHelper.ExecuteCommand(@"
insert into Gateway(IP,PORT,Name)
values('IP','PORT','Name')
");
            GatewayService service = new GatewayService();
            HttpResult httpResult = service.GetGatewayList();
            Assert.AreEqual(httpResult.Status, true);
            Assert.AreEqual(httpResult.Message, "查询网关列表成功");
            List<Gateway> Gateway = httpResult.Data as List<Gateway>;
            Assert.AreEqual(Gateway[0].IP, "IP");
            Assert.AreEqual(Gateway[0].PORT, "PORT");
            Assert.AreEqual(Gateway[0].Name, "Name");
        }
    }
}