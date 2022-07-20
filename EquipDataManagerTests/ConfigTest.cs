using EquipDataManager.Dal;
using EquipDataManager.Relealise;
using EquipModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using Tools;

namespace EquipDataManagerTests
{
    [TestClass]
    public class ConfigTest
    {
        [TestMethod]
        public void TestConfig()
        {
            string _s = ConfigHelper.GetConfigString("Data:DefaultConnection:ConnectionString");
            Assert.IsNotNull(_s);
        }

        [TestMethod]
        public void AddEquip()
        {
            DBHelper.ExecuteCommand("truncate table Equip");
            DBHelper.ExecuteCommand("Insert into Equip(NO,NAME,ip,port,EquipType) values('1','设备1','sgfdhafgdfhg.gnway.cc','53454','静电监测设备')");
            DBHelper.ExecuteCommand("Insert into Equip(NO,NAME,ip,port,EquipType) values('2','设备2','sgfdhafgdfhg.gnway.cc','53454','静电监测设备')");
            DBHelper.ExecuteCommand("Insert into Equip(NO,NAME,ip,port,EquipType) values('3','设备3','sgfdhafgdfhg.gnway.cc','53454','静电监测设备')");
            DBHelper.ExecuteCommand("Insert into Equip(NO,NAME,ip,port,EquipType) values('4','设备4','sgfdhafgdfhg.gnway.cc','53454','静电监测设备')");
            DBHelper.ExecuteCommand("Insert into Equip(NO,NAME,ip,port,EquipType) values('5','设备5','sgfdhafgdfhg.gnway.cc','53454','温湿度')");
            DBHelper.ExecuteCommand("Insert into Equip(NO,NAME,ip,port,EquipType) values('6','设备6','sgfdhafgdfhg.gnway.cc','53454','静电监测设备')");
            DBHelper.ExecuteCommand("Insert into Equip(NO,NAME,ip,port,EquipType) values('7','设备7','sgfdhafgdfhg.gnway.cc','53454','静电监测设备')");
            DBHelper.ExecuteCommand("Insert into Equip(NO,NAME,ip,port,EquipType) values('8','设备8','sgfdhafgdfhg.gnway.cc','53454','静电监测设备')");
            DBHelper.ExecuteCommand("Insert into Equip(NO,NAME,ip,port,EquipType) values('9','设备9','sgfdhafgdfhg.gnway.cc','53454','静电监测设备')");
            DBHelper.ExecuteCommand("Insert into Equip(NO,NAME,ip,port,EquipType) values('10','设备10','sgfdhafgdfhg.gnway.cc','53454','静电监测设备')");
            DBHelper.ExecuteCommand("Insert into Equip(NO,NAME,ip,port,EquipType) values('11','设备11','sgfdhafgdfhg.gnway.cc','53454','静电监测设备')");
            DBHelper.ExecuteCommand("Insert into Equip(NO,NAME,ip,port,EquipType) values('12','设备12','sgfdhafgdfhg.gnway.cc','53454','静电监测设备')");
            DBHelper.ExecuteCommand("Insert into Equip(NO,NAME,ip,port,EquipType) values('13','设备13','sgfdhafgdfhg.gnway.cc','53454','静电监测设备')");
            DBHelper.ExecuteCommand("Insert into Equip(NO,NAME,ip,port,EquipType) values('14','设备14','sgfdhafgdfhg.gnway.cc','53454','温湿度')");
            DBHelper.ExecuteCommand("Insert into Equip(NO,NAME,ip,port,EquipType) values('15','设备15','sgfdhafgdfhg.gnway.cc','53454','温湿度')");
            DBHelper.ExecuteCommand("Insert into Equip(NO,NAME,ip,port,EquipType) values('201','设备15','sgfdhafgdfhg.gnway.cc','53454','离子风机设备')");
            DBHelper.ExecuteCommand("Insert into Equip(NO,NAME,ip,port,EquipType) values('202','设备15','sgfdhafgdfhg.gnway.cc','53454','离子风机设备')");
            DBHelper.ExecuteCommand("Insert into Equip(NO,NAME,ip,port,EquipType) values('203','设备15','sgfdhafgdfhg.gnway.cc','53454','离子风机设备')");
            DBHelper.ExecuteCommand("Insert into Equip(NO,NAME,ip,port,EquipType) values('204','设备15','sgfdhafgdfhg.gnway.cc','53454','离子风机设备')");
            DBHelper.ExecuteCommand("Insert into Equip(NO,NAME,ip,port,EquipType) values('205','设备15','sgfdhafgdfhg.gnway.cc','53454','离子风机设备')");

            List<Equip> equips = EquipDal.GetAllEquips();
            Assert.AreEqual(equips.Count, 20);
        }

        [TestMethod]
        public void AddSpotSet()
        {
            DBHelper.ExecuteCommand("truncate table equipspotset");
            DBHelper.ExecuteCommand("Insert into equipspotset(equiptype,datatype,spotno,savetype) values('001','A','1',1)");
            DBHelper.ExecuteCommand("Insert into equipspotset(equiptype,datatype,spotno,savetype) values('001','A','2',3)");
            DBHelper.ExecuteCommand("Insert into equipspotset(equiptype,datatype,spotno,savetype) values('001','E','',2)");

            List<EquipSpotSet> equipSpotSets = EquipSpotSetDal.GetAllSpotSets();
            Assert.AreEqual(equipSpotSets.Count, 3);
        }
        [TestMethod]
        public void DataPickerRealiseTest()
        {
            string _data = File.ReadAllText("jsdata.json",Encoding.UTF8);
            DataPickerRealise _dpr = new DataPickerRealise();
            List<EquipData> _datas = _dpr.AnalyzData(_data,1);
            Assert.AreEqual(_datas.Count > 0, true);
        }
    }
}
