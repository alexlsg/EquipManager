using Microsoft.VisualStudio.TestTools.UnitTesting;
using SystemSet;
using System;
using System.Collections.Generic;
using System.Text;
using Tools;

namespace SystemSet.Tests
{
    [TestClass()]
    public class EquipSpotServiceTests
    {
        [TestMethod()]
        public void GetEquipListByEquipNO()
        {
            Tools.DBHelper.ExecuteCommand(@"
truncate table EquipSpot
");
            Tools.DBHelper.ExecuteCommand(@"
insert into EquipSpot(State,NoState,EquipNO,Type,SpotNo,Unit,Value,SpotNm)
values(1,2,'EquipNO','Type','SpotNo','Unit','Value','SpotNm')
");
            EquipSpotService service = new EquipSpotService();
            HttpResult httpResult = service.GetEquipListByEquipNO("EquipNO");
            Assert.AreEqual(httpResult.Status, true);
            Assert.AreEqual(httpResult.Message, "查询设备测点功能列表成功");
            List<EquipSpot> EquipSpot = httpResult.Data as List<EquipSpot>;
            Assert.AreEqual(EquipSpot[0].State, 1);
            Assert.AreEqual(EquipSpot[0].NoState, 2);
            Assert.AreEqual(EquipSpot[0].EquipNo, "EquipNO");
            Assert.AreEqual(EquipSpot[0].Type, "Type");
            Assert.AreEqual(EquipSpot[0].SpotNo, "SpotNo");
            Assert.AreEqual(EquipSpot[0].Unit, "Unit");
            Assert.AreEqual(EquipSpot[0].Value, "Value");
            Assert.AreEqual(EquipSpot[0].SpotNm, "SpotNm");
        }
    }
}