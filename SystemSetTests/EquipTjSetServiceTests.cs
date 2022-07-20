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
    public class EquipTjSetServiceTests
    {
        [TestMethod()]
        public void AddEquipTjSetTest()
        {
            Tools.DBHelper.ExecuteCommand("truncate table EquipTjSet");
            EquipTjSet EquipTjSet = new EquipTjSet();
            EquipTjSet.EquipType = "EquipType";
            EquipTjSet.DataType = "DataType";
            EquipTjSet.SpotNO = "SpotNO";
            EquipTjSet.Tjlx = "Tjlx";
            EquipTjSet.Tjzt = "Tjzt";
            EquipTjSet.Data = "Data";
            EquipTjSet.Tjtj = "Tjtj";
            EquipTjSet.Cyhj = true;
            EquipTjSet.Cyzshj = true;
            EquipTjSetService service = new EquipTjSetService();
            HttpResult httpResult = service.AddEquipTjSet(EquipTjSet);
            Assert.AreEqual(httpResult.Status, true);
            Assert.AreEqual(httpResult.Message, "添加统计配置成功");
            DataTable dataTable = Tools.DBHelper.GetDataTable("select * from EquipTjSet");
            Assert.AreEqual(dataTable.Rows.Count, 1);
            Assert.AreEqual(dataTable.Rows[0]["EquipType"].ToString(), "EquipType");
            Assert.AreEqual(dataTable.Rows[0]["DataType"].ToString(), "DataType");
            Assert.AreEqual(dataTable.Rows[0]["SpotNO"].ToString(), "SpotNO");
            Assert.AreEqual(dataTable.Rows[0]["Tjlx"].ToString(), "Tjlx");
            Assert.AreEqual(dataTable.Rows[0]["Tjzt"].ToString(), "Tjzt");
            Assert.AreEqual(dataTable.Rows[0]["Data"].ToString(), "Data");
            Assert.AreEqual(dataTable.Rows[0]["Tjtj"].ToString(), "Tjtj");
            Assert.AreEqual(dataTable.Rows[0]["Cyhj"].ToString(), "1");
            Assert.AreEqual(dataTable.Rows[0]["Cyzshj"].ToString(), "1");
        }

        [TestMethod()]
        public void ModEquipTjSetTest()
        {
            Tools.DBHelper.ExecuteCommand(@"
truncate table EquipTjSet
");
            Tools.DBHelper.ExecuteCommand(@"
insert into EquipTjSet(EquipType,DataType,SpotNO,Tjlx,Tjzt,Data,Tjtj,Cyhj,Cyzshj)
values('1','2','3','4','5','6','7',false,false)
");
            EquipTjSet EquipTjSet = new EquipTjSet();
            EquipTjSet.EquipType = "EquipType";
            EquipTjSet.DataType = "DataType";
            EquipTjSet.SpotNO = "SpotNO";
            EquipTjSet.Tjlx = "Tjlx";
            EquipTjSet.Tjzt = "Tjzt";
            EquipTjSet.Data = "Data";
            EquipTjSet.Tjtj = "Tjtj";
            EquipTjSet.Cyhj = true;
            EquipTjSet.Cyzshj = true;
            EquipTjSet.Id = 1;
            EquipTjSetService service = new EquipTjSetService();
            HttpResult httpResult = service.ModEquipTjSet(EquipTjSet);
            Assert.AreEqual(httpResult.Status, true);
            Assert.AreEqual(httpResult.Message, "修改统计配置成功");
            DataTable dataTable = Tools.DBHelper.GetDataTable("select * from EquipTjSet");
            Assert.AreEqual(dataTable.Rows.Count, 1);
            Assert.AreEqual(dataTable.Rows[0]["EquipType"].ToString(), "EquipType");
            Assert.AreEqual(dataTable.Rows[0]["DataType"].ToString(), "DataType");
            Assert.AreEqual(dataTable.Rows[0]["SpotNO"].ToString(), "SpotNO");
            Assert.AreEqual(dataTable.Rows[0]["Tjlx"].ToString(), "Tjlx");
            Assert.AreEqual(dataTable.Rows[0]["Tjzt"].ToString(), "Tjzt");
            Assert.AreEqual(dataTable.Rows[0]["Data"].ToString(), "Data");
            Assert.AreEqual(dataTable.Rows[0]["Tjtj"].ToString(), "Tjtj");
            Assert.AreEqual(dataTable.Rows[0]["Cyhj"].ToString(), "1");
            Assert.AreEqual(dataTable.Rows[0]["Cyzshj"].ToString(), "1");
        }

        [TestMethod()]
        public void DelEquipTjSetTest()
        {
            Tools.DBHelper.ExecuteCommand(@"
truncate table EquipTjSet
");
            Tools.DBHelper.ExecuteCommand(@"
insert into EquipTjSet(EquipType,DataType,SpotNO,Tjlx,Tjzt,Data,Tjtj,Cyhj,Cyzshj)
values('1','2','3','4','5','6','7',false,false)
");
            EquipTjSet EquipTjSet = new EquipTjSet();
            EquipTjSet.Id = 1;
            EquipTjSetService service = new EquipTjSetService();
            HttpResult httpResult = service.DelEquipTjSet(EquipTjSet);
            Assert.AreEqual(httpResult.Status, true);
            Assert.AreEqual(httpResult.Message, "删除统计配置成功");
            DataTable dataTable = Tools.DBHelper.GetDataTable("select * from EquipTjSet");
            Assert.AreEqual(dataTable.Rows.Count, 0);
        }

        [TestMethod()]
        public void GetEquipTjSetList()
        {
            Tools.DBHelper.ExecuteCommand(@"
truncate table EquipTjSet
");
            Tools.DBHelper.ExecuteCommand(@"
insert into EquipTjSet(EquipType,DataType,SpotNO,Tjlx,Tjzt,Data,Tjtj,Cyhj,Cyzshj)
values('EquipType','DataType','SpotNO','Tjlx','Tjzt','Data','Tjtj',false,false)
");
            EquipTjSetService service = new EquipTjSetService();
            HttpResult httpResult = service.GetEquipTjSetList();
            Assert.AreEqual(httpResult.Status, true);
            Assert.AreEqual(httpResult.Message, "查询统计配置列表成功");
            List<EquipTjSet> EquipTjSet = httpResult.Data as List<EquipTjSet>;
            Assert.AreEqual(EquipTjSet[0].EquipType, "EquipType");
            Assert.AreEqual(EquipTjSet[0].DataType, "DataType");
            Assert.AreEqual(EquipTjSet[0].SpotNO, "SpotNO");
            Assert.AreEqual(EquipTjSet[0].Tjlx, "Tjlx");
            Assert.AreEqual(EquipTjSet[0].Tjzt, "Tjzt");
            Assert.AreEqual(EquipTjSet[0].Data, "Data");
            Assert.AreEqual(EquipTjSet[0].Tjtj, "Tjtj");
            Assert.AreEqual(EquipTjSet[0].Cyhj, false);
            Assert.AreEqual(EquipTjSet[0].Cyzshj, false);
        }
    }
}