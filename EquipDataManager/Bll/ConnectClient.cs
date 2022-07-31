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
        LogDelegate log;
        Thread _t;
        public ConnectClient(LogDelegate log)
        {
            this.log = log;
        }
        public void StartReciev()
        {
            _t = new Thread(OnMessage);
            _t.IsBackground = true;
            _t.Start(client);
        }

        private void OnMessage(object clientSocket)
        {
            Socket _client = (Socket)clientSocket;
            string _ip = ((IPEndPoint)_client.RemoteEndPoint).Address.ToString();
            log?.Invoke($"{_ip}已连接!");
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
            log?.Invoke($"{_ip}已断开!");
            _client.Shutdown(SocketShutdown.Both);
            _client.Close();
        }

        private void DealMsg(byte[] result)
        {
            try
            {
                string _value = Encoding.UTF8.GetString(result);
                _value = _value.Replace("＝", "=");
                log?.Invoke(_value);
                string[] _ss = _value.Split('{')[1].Split('}')[0].Split(',');
                Dictionary<string, string> _values = new Dictionary<string, string>();
                foreach (var item in _ss)
                {
                    string[] _ss1 = item.Split('=');
                    _values[_ss1[0]] = _ss1[1];
                }
                DBHelper.ExecuteCommand($"insert into equipevent(groupid,devid,event,starttime,dvalue)values('{_values["line"]}','{_values["id"]}','{_values["qrcode"]}',now(),'{_values["adc0"]}')");
                client.Send(Encoding.UTF8.GetBytes("ok" + _value));
            }
            catch (Exception ex)
            {
                log?.Invoke(ex.Message);
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
