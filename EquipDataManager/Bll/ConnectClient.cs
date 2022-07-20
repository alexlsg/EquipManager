using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

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
                    //DataHelper _dh = new DataHelper(_result);
                    //int _datalen = _dh.DataLen;
                    //byte[] _data = new byte[_datalen];
                    //Array.Copy(_result, _data, Consts.HeaderLen);
                    //while (_len < _datalen)
                    //{
                    //    int size = 500 * 1024;
                    //    if (size > _datalen - _len)
                    //    {
                    //        size = _datalen - _len;
                    //    }
                    //    int _ii = _client.Receive(_data, _len, size, SocketFlags.None);
                    //    _len += _ii;
                    //}
                    //dealDataEvent?.Invoke(_data, _client);
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
