using Microsoft.VisualStudio.TestTools.UnitTesting;
using EquipDataManager.Dal;
using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using ThirdParty.Json.LitJson;
using Tools;

namespace EquipDataManager.Dal.Tests
{
    [TestClass()]
    public class EquipDalTests
    {
        string TokenUrl = "http://sgfdhafgdfhg.gnway.cc:53454/cgi-bin/login1.cgi?user=admin&password=888";
        string DataUrl = "http://sgfdhafgdfhg.gnway.cc:53454/cgi-bin/api3.cgi?json={\"type\":\"EquipAllSpotValueState\",\"data\":[{\"equipNo\":\"1\"}]}";
        [TestMethod()]
        public void GetAllEquipsTest()
        {
            string _tokenresult = new HttpHelper().GetHtml(TokenUrl);
            JsonData jsonData = JsonMapper.ToObject(_tokenresult);
            bool _true = !string.IsNullOrEmpty(jsonData[0]["token"].ToString());
            Assert.IsTrue(_true);

            string _data = new HttpHelper().GetHtml(DataUrl,new List<KeyValue>() { new KeyValue() { Key = "token" ,Value = jsonData[0]["token"].ToString() } });
            JsonData jsonData1 = JsonMapper.ToObject(_data);
            bool _true1 = !string.IsNullOrEmpty(jsonData1[0]["token"].ToString());
            Assert.IsTrue(_true1);
        }
    }
}