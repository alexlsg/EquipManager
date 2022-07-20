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
    public class SystemSettingServiceTests
    {
        [TestMethod()]
        public void AddSystemSettingTest()
        {
            Tools.DBHelper.ExecuteCommand("truncate table SystemSetting");
            SystemSetting SystemSetting = new SystemSetting();
            SystemSetting.Title = "Title";
            SystemSettingService service = new SystemSettingService();
            HttpResult httpResult = service.AddSystemSetting(SystemSetting);
            Assert.AreEqual(httpResult.Status, true);
            Assert.AreEqual(httpResult.Message, "添加系统设置成功");
            DataTable dataTable = Tools.DBHelper.GetDataTable("select * from SystemSetting");
            Assert.AreEqual(dataTable.Rows.Count, 1);
            Assert.AreEqual(dataTable.Rows[0]["Title"], "Title");
        }

        [TestMethod()]
        public void ModSystemSettingTest()
        {
            Tools.DBHelper.ExecuteCommand(@"
truncate table SystemSetting
");
            Tools.DBHelper.ExecuteCommand(@"
insert into SystemSetting(Title)
values('1')
");
            SystemSetting SystemSetting = new SystemSetting();
            SystemSetting.Title = "Title";
            SystemSetting.Id = 1;
            SystemSettingService service = new SystemSettingService();
            HttpResult httpResult = service.ModSystemSetting(SystemSetting);
            Assert.AreEqual(httpResult.Status, true);
            Assert.AreEqual(httpResult.Message, "修改系统设置成功");
            DataTable dataTable = Tools.DBHelper.GetDataTable("select * from SystemSetting");
            Assert.AreEqual(dataTable.Rows.Count, 1);
            Assert.AreEqual(dataTable.Rows[0]["Title"], "Title");
        }

        [TestMethod()]
        public void DelSystemSettingTest()
        {
            Tools.DBHelper.ExecuteCommand(@"
truncate table SystemSetting
");
            Tools.DBHelper.ExecuteCommand(@"
insert into SystemSetting(Title)
values('1')
");

            SystemSetting SystemSetting = new SystemSetting();
            SystemSetting.Title = "Title";
            SystemSetting.Id = 1;
            SystemSettingService service = new SystemSettingService();
            HttpResult httpResult = service.DelSystemSetting(SystemSetting);
            Assert.AreEqual(httpResult.Status, true);
            Assert.AreEqual(httpResult.Message, "删除系统设置成功");
            DataTable dataTable = Tools.DBHelper.GetDataTable("select * from SystemSetting");
            Assert.AreEqual(dataTable.Rows.Count, 0);
        }

        [TestMethod()]
        public void GetSystemSettingList()
        {
            Tools.DBHelper.ExecuteCommand(@"
truncate table SystemSetting
");
            Tools.DBHelper.ExecuteCommand(@"
insert into SystemSetting(Title)
values('Title')
");
            SystemSettingService service = new SystemSettingService();
            HttpResult httpResult = service.GetSystemSettingList();
            Assert.AreEqual(httpResult.Status, true);
            Assert.AreEqual(httpResult.Message, "查询系统设置成功");
            List<SystemSetting> SystemSetting = httpResult.Data as List<SystemSetting>;
            Assert.AreEqual(SystemSetting[0].Title, "Title");
        }
    }
}