using EquipModel;
using EquipModel.Interfaces;
using System;
using System.Collections.Generic;
using ThirdParty.Json.LitJson;
using Tools;

namespace EquipDataManager.Relealise
{
    /// <summary>
    /// 网关获取数据
    /// </summary>
    public class DataPickerRealise : IDataPicker
    {
        string tokenurl = "http://{0}:{1}/cgi-bin/login1.cgi?user={2}&password={3}";
        string GetValue(JsonData json, string listname, int row, string column)
        {
            try
            {
                return json[listname][row][column].ToString();
            }
            catch (Exception ex)
            {
                Log.Add($"读取失败:{listname}[{row}][{column}]");
                throw;
            }
        }
        public List<EquipData> AnalyzData(string data, int equipid)
        {
            JsonData jsonData = JsonMapper.ToObject(data);
            List<EquipData> dataList = new List<EquipData>();
            if (jsonData["msg"] != null)
            {
                for (int i = 0; i < jsonData["msg"].Count; i++)
                {
                    EquipData _ed = new EquipData();
                    _ed.EquipID = equipid;
                    _ed.EquipNO = GetValue(jsonData, "msg", i, "EquipNo"); 
                    _ed.DataType = GetValue(jsonData, "msg", i, "Type");
                    _ed.State = GetValue(jsonData, "msg", i, "State");
                    _ed.Jlsj = DateTime.Now;
                    if (_ed.DataType == "E")
                    {
                        _ed.SpotNO = "0";
                        _ed.Data = _ed.State;
                        _ed.EquipName = GetValue(jsonData, "msg", i, "EquipNm"); 
                    }
                    else
                    {
                        _ed.SpotNO = GetValue(jsonData, "msg", i, "SpotNo"); 
                        _ed.SpotName = GetValue(jsonData, "msg", i, "SpotNm"); 
                        _ed.Data = GetValue(jsonData, "msg", i, "Value"); 
                        _ed.NoState = GetValue(jsonData, "msg", i, "NoState"); 
                        if (_ed.DataType == "A")
                            _ed.Unit = GetValue(jsonData, "msg", i, "Unit");
                    }
                    dataList.Add(_ed);
                }
            }
            else
            {
                Log.Add("数据错误,未包含msg节点.");
            }
            return dataList;
        }
        Dictionary<string, string> tokens = new Dictionary<string, string>();
        Dictionary<string, DateTime> tokensx = new Dictionary<string, DateTime>();
        string GetToken(Equip equip)
        {
            string _url = equip.IP + ":" + equip.PORT;
            if (tokensx.ContainsKey(_url) && (DateTime.Now - tokensx[_url]).TotalHours > 4)
            {
                tokens.Remove(_url);
            }
            if (!tokens.ContainsKey(_url))
            {
                string _user = ConfigHelper.GetConfigString("token:user");
                string _password = ConfigHelper.GetConfigString("token:password");
                try
                {
                    HttpHelper _http = new HttpHelper();
                    string _tokenresult = _http.GetHtml(string.Format(tokenurl, equip.IP, equip.PORT, _user, _password));
                    JsonData jsonData = JsonMapper.ToObject(_tokenresult);
                    tokens[_url] = jsonData[0]["token"].ToString();
                    tokensx[_url] = DateTime.Now;
                }
                catch (Exception ex)
                {
                    Log.Add($"获取token失败url:{string.Format(tokenurl, equip.IP, equip.PORT, _user, _password)}");
                }

            }
            return tokens[_url];
        }
        public string GetDataFromEquip(Equip equip)
        {
            string _token = GetToken(equip);
            HttpHelper http = new HttpHelper();
            List<KeyValue> _kv = new List<KeyValue>();
            _kv.Add(new KeyValue() { Key = "token", Value = _token });
            string _url = $"http://{equip.IP}:{equip.PORT}/cgi-bin/api3.cgi?json={{\"type\":\"EquipAllSpotValueState\",\"data\":[{{\"equipNo\":\"{equip.NO}\"}}]}}";
            try
            {
                return http.GetHtml(_url, _kv);
            }
            catch (Exception)
            {
                Log.Add($"获取数据失败url:{_url}");
                throw;
            }
        }
    }
}
