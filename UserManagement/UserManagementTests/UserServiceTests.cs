using Microsoft.VisualStudio.TestTools.UnitTesting;
using UserManagement;
using System;
using System.Collections.Generic;
using System.Text;
using Tools;
using System.Data;

namespace UserManagement.Tests
{
    [TestClass()]
    public class UserServiceTests
    {
        [TestMethod()]
        public void AddUserTest()
        {
            Tools.DBHelper.ExecuteCommand("truncate table USER");
            User user = new User();
            user.ZoneBinding = "ZoneBinding";
            user.Remarks = "Remarks";
            user.UserName = "UserName";
            user.ThemeColor = "ThemeColor";
            user.LastLoginTime = DateTime.Now;
            user.PassWord = "PassWord";
            user.RoleGroup = "RoleGroup";
            UserService service = new UserService();
            HttpResult httpResult = service.AddUser(user);
            Assert.AreEqual(httpResult.Status,true);
            Assert.AreEqual(httpResult.Message, "添加用户成功");
            DataTable dataTable = Tools.DBHelper.GetDataTable("select * from User");
            Assert.AreEqual(dataTable.Rows.Count, 1);
            Assert.AreEqual(dataTable.Rows[0]["ZoneBinding"], "ZoneBinding");
            Assert.AreEqual(dataTable.Rows[0]["Remarks"], "Remarks");
            Assert.AreEqual(dataTable.Rows[0]["UserName"], "UserName");
            Assert.AreEqual(dataTable.Rows[0]["ThemeColor"], "ThemeColor");
            Assert.AreEqual(((DateTime)dataTable.Rows[0]["LastLoginTime"]).Date, DateTime.Now.Date);
            Assert.AreEqual(dataTable.Rows[0]["PassWord"], "nd9uI9o/m5DVyw2k9+TVlw==");
            Assert.AreEqual(dataTable.Rows[0]["RoleGroup"], "RoleGroup");
        }

        [TestMethod()]
        public void ModUserTest()
        {
            Tools.DBHelper.ExecuteCommand(@"
truncate table USER
insert into USER(ZoneBinding,Remarks,UserName,ThemeColor,LastLoginTime,PassWord,RoleGroup)
values('1','2','3','4','2020-10-10','5','6')
");
            User user = new User();
            user.ZoneBinding = "ZoneBinding";
            user.Remarks = "Remarks";
            user.UserName = "UserName";
            user.ThemeColor = "ThemeColor";
            user.LastLoginTime = DateTime.Now;
            user.PassWord = "PassWord";
            user.RoleGroup = "RoleGroup";
            user.Id = 1;
            UserService service = new UserService();
            HttpResult httpResult = service.ModUser(user);
            Assert.AreEqual(httpResult.Status, true);
            Assert.AreEqual(httpResult.Message, "修改用户成功");
            DataTable dataTable = Tools.DBHelper.GetDataTable("select * from User");
            Assert.AreEqual(dataTable.Rows.Count, 1);
            Assert.AreEqual(dataTable.Rows[0]["ZoneBinding"], "ZoneBinding");
            Assert.AreEqual(dataTable.Rows[0]["Remarks"], "Remarks");
            Assert.AreEqual(dataTable.Rows[0]["UserName"], "UserName");
            Assert.AreEqual(dataTable.Rows[0]["ThemeColor"], "ThemeColor");
            Assert.AreEqual(((DateTime)dataTable.Rows[0]["LastLoginTime"]).Date, DateTime.Now.Date);
            Assert.AreEqual(dataTable.Rows[0]["PassWord"], "nd9uI9o/m5DVyw2k9+TVlw==");
            Assert.AreEqual(dataTable.Rows[0]["RoleGroup"], "RoleGroup");
        }

        [TestMethod()]
        public void DelUserTest()
        {
            Tools.DBHelper.ExecuteCommand(@"
truncate table USER
insert into USER(ZoneBinding,Remarks,UserName,ThemeColor,LastLoginTime,PassWord,RoleGroup)
values('1','2','3','4','2020-10-10','5','6')
");
            User user = new User();
            user.ZoneBinding = "ZoneBinding";
            user.Remarks = "Remarks";
            user.UserName = "UserName";
            user.ThemeColor = "ThemeColor";
            user.LastLoginTime = DateTime.Now;
            user.PassWord = "PassWord";
            user.RoleGroup = "RoleGroup";
            user.Id = 1;
            UserService service = new UserService();
            HttpResult httpResult = service.DelUser(user);
            Assert.AreEqual(httpResult.Status, true);
            Assert.AreEqual(httpResult.Message, "删除用户成功");
            DataTable dataTable = Tools.DBHelper.GetDataTable("select * from User");
            Assert.AreEqual(dataTable.Rows.Count, 0);
        }

        [TestMethod()]
        public void GetUserList()
        {
            Tools.DBHelper.ExecuteCommand(@"
truncate table USER
insert into USER(ZoneBinding,Remarks,UserName,ThemeColor,LastLoginTime,PassWord,RoleGroup)
values('ZoneBinding','Remarks','UserName','ThemeColor','2020-10-10','PassWord','RoleGroup')
");
            UserService service = new UserService();
            HttpResult httpResult = service.GetUserList();
            Assert.AreEqual(httpResult.Status, true);
            Assert.AreEqual(httpResult.Message, "查询用户列表成功");
            List<User> user = httpResult.Data as List<User>;
            Assert.AreEqual(user[0].ZoneBinding, "ZoneBinding");
            Assert.AreEqual(user[0].Remarks, "Remarks");
            Assert.AreEqual(user[0].UserName, "UserName");
            Assert.AreEqual(user[0].ThemeColor, "ThemeColor");
            Assert.AreEqual(user[0].LastLoginTime.Value.Date.ToString("yyyyMMdd"), "20201010");
            Assert.AreEqual(user[0].PassWord, "nd9uI9o/m5DVyw2k9+TVlw==");
            Assert.AreEqual(user[0].RoleGroup, "RoleGroup");
        }

        [TestMethod()]
        public void GetUser()
        {
            Tools.DBHelper.ExecuteCommand(@"
truncate table USER
insert into USER(ZoneBinding,Remarks,UserName,ThemeColor,LastLoginTime,PassWord,RoleGroup)
values('ZoneBinding','Remarks','UserName','ThemeColor','2020-10-10','PassWord','RoleGroup')
");
            UserService service = new UserService();
            HttpResult httpResult = service.GetUser("UserName");
            Assert.AreEqual(httpResult.Status, true);
            Assert.AreEqual(httpResult.Message, "查询用户列表成功");
            User user = httpResult.Data as User;
            Assert.AreEqual(user.ZoneBinding, "ZoneBinding");
            Assert.AreEqual(user.Remarks, "Remarks");
            Assert.AreEqual(user.UserName, "UserName");
            Assert.AreEqual(user.ThemeColor, "ThemeColor");
            Assert.AreEqual(user.LastLoginTime.Value.Date.ToString("yyyyMMdd"), "20201010");
            Assert.AreEqual(user.PassWord, "nd9uI9o/m5DVyw2k9+TVlw==");
            Assert.AreEqual(user.RoleGroup, "RoleGroup");
        }

        [TestMethod()]
        public void Login()
        {
            Tools.DBHelper.ExecuteCommand(@"
truncate table USER
insert into USER(ZoneBinding,Remarks,UserName,ThemeColor,LastLoginTime,PassWord,RoleGroup)
values('ZoneBinding','Remarks','UserName','ThemeColor','2020-10-10','PassWord','RoleGroup')
");
            User user = new User();
            user.ZoneBinding = "ZoneBinding";
            user.Remarks = "Remarks";
            user.UserName = "UserName";
            user.ThemeColor = "ThemeColor";
            user.LastLoginTime = DateTime.Now;
            user.PassWord = "PassWord";
            user.RoleGroup = "RoleGroup";
            user.Id = 1;
            UserService service = new UserService();
            HttpResult httpResult = service.Login(user);
            Assert.AreEqual(httpResult.Status, true);
            Assert.AreEqual(httpResult.Message, "登录成功");
            User user1 = httpResult.Data as User;
            Assert.AreEqual(user1.ZoneBinding, "ZoneBinding");
            Assert.AreEqual(user1.Remarks, "Remarks");
            Assert.AreEqual(user1.UserName, "UserName");
            Assert.AreEqual(user1.ThemeColor, "ThemeColor");
            Assert.AreEqual(user1.LastLoginTime.Value.Date.ToString("yyyyMMdd"), "20201010");
            Assert.AreEqual(user1.PassWord, "nd9uI9o/m5DVyw2k9+TVlw==");
            Assert.AreEqual(user1.RoleGroup, "RoleGroup");
        }
    }
}