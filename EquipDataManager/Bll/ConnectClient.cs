using EquipModel;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Tools;

namespace EquipDataManager.Bll
{

    public class ConnectClient
    {
        internal Socket client;
        Thread _t;
        public ConnectClient()
        {
        }
        public void StartReciev()
        {
            _t = new Thread(OnMessage);
            _t.IsBackground = true;
            _t.Start(client);
        }
        //        device_min 测量最小值    双精度浮点型

        //device_max  测量最大值 双精度浮点型

        //actual_min 实际最小值    双精度浮点型

        //actual_max  实际最大值 双精度浮点型
        double _device_min = -9999999;
        public double device_min
        {
            get
            {
                if (_device_min == -9999999)
                {
                    string _v = ConfigHelper.GetConfigString("settings:device_min");
                    double.TryParse(_v, out _device_min);
                }
                return _device_min;
            }
        }
        double _device_max = -9999999;
        public double device_max
        {
            get
            {
                if (_device_max == -9999999)
                {
                    string _v = ConfigHelper.GetConfigString("settings:device_max");
                    double.TryParse(_v, out _device_max);
                }
                return _device_max;
            }
        }

        double _actual_min = -9999999;
        public double actual_min
        {
            get
            {
                if (_actual_min == -9999999)
                {
                    string _v = ConfigHelper.GetConfigString("settings:actual_min");
                    double.TryParse(_v, out _actual_min);
                }
                return _actual_min;
            }
        }

        double _actual_max = -9999999;
        public double actual_max
        {
            get
            {
                if (_actual_max == -9999999)
                {
                    string _v = ConfigHelper.GetConfigString("settings:actual_max");
                    double.TryParse(_v, out _actual_max);
                }
                return _actual_max;
            }
        }

        string _qrlevel;
        public string qrlevel
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_qrlevel))
                {
                    _qrlevel = ConfigHelper.GetConfigString("save:qrlevel");
                    if (string.IsNullOrWhiteSpace(_qrlevel))
                    {
                        _qrlevel = "3";
                    }
                }
                return _qrlevel;
            }
        }
        private void OnMessage(object clientSocket)
        {
            Socket _client = (Socket)clientSocket;
            string _ip = ((IPEndPoint)_client.RemoteEndPoint).Address.ToString();
            Log.Add($"{_ip}已连接!");
            while (_client.Connected)
            {
                try
                {
                    byte[] _result = new byte[1000];
                    //通过clientSocket接收数据  
                    int _len = _client.Receive(_result);
                    if (_len == 0)
                    {
                        continue;
                    }
                    byte[] _buff = new byte[_len];
                    Array.Copy(_result, _buff, _len);
                    DealMsg(_buff);
                }
                catch (Exception ex)
                {
                    break;
                }
            }
            Log.Add($"{_ip}已断开!");
            _client.Shutdown(SocketShutdown.Both);
            _client.Close();
        }
        Dictionary<string, string> values = new Dictionary<string, string>();
        private void DealMsg(byte[] result)
        {
            try
            {
                string _value = Encoding.UTF8.GetString(result);
                _value = _value.Replace("＝", "=");
                string[] _ss = _value.Split('{')[1].Split('}')[0].Split(',');
                Dictionary<string, string> _values = new Dictionary<string, string>();
                foreach (var item in _ss)
                {
                    string[] _ss1 = item.Split('=');
                    _values[_ss1[0]] = _ss1[1];
                }
                string _cxid = _values["line"];
                int _i = 0;
                if (int.TryParse(_cxid, out _i))
                    _cxid = _i.ToString();
                string _key = _cxid + _values["ID"];
                string _v1 = _values["qrcode"];
                if (!values.ContainsKey(_key))
                {
                    values[_key] = "";
                }
                if (values[_key] != _v1)
                {
                    values[_key] = _v1;
                    double _d1 = double.Parse(_values["adc0"]);
                    double _dvalue = Math.Round((_d1 - device_min) * (actual_max - actual_min) / (device_max - device_min) + actual_min, 3);
                    string _level = "0";
                    if (string.IsNullOrWhiteSpace(_v1) || _v1.ToLower() == "noread")
                        _level = qrlevel;
                    DBHelper.ExecuteCommand($"insert into equipevent(groupid,typeid,devid,event,starttime,dvalue,etype,level)values('{_cxid}','10001','{_values["ID"]}','{_values["qrcode"]}',now(),'{_dvalue}','0x90001001',{_level})");
                }
                client.Send(Encoding.UTF8.GetBytes("ok" + _value));
            }
            catch (Exception ex)
            {
                Log.Add(ex.Message);
            }
        }

        //internal void CheckActive()
        //{
        //    DataHelper _data = new DataHelper(0, CMD.心跳, 1);
        //    try
        //    {
        //        client.Send(_data.Buffer);
        //    }
        //    catch (Exception ex)
        //    {
        //        Connected = false;
        //        _t.Abort();
        //    }
        //}

        public bool Connected = true;
    }
}
