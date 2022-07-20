using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Tools;

namespace EquipDataManager.Bll
{
    /// <summary>
    /// socket监听
    /// </summary>
    public class SocketListen
    {
        Socket server;
        LogDelegate logevent;
        int port;
        string szjd = "settings:listenport";
        private SocketListen()
        { }
        public static SocketListen Instance = new SocketListen();

        public void Start(LogDelegate log)
        {
            logevent = log;
            InitSet();
            if (port > 0)
            {
                server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IPEndPoint _ip = new IPEndPoint(IPAddress.Any, port);
                server.Bind(_ip);
                server.Listen(30);
                logevent?.Invoke($"端口{port}监听已打开!");
                Thread _t = new Thread(ConnectListen);
                _t.IsBackground = true;
                _t.Start();
            }
            else
            {
                logevent?.Invoke("未配置端口,监听已关闭!");
            }
        }

        private void ConnectListen()
        {
            try
            {
                while (true)
                {
                    Socket _client = server.Accept();
                    ConnectClient _cc = new ConnectClient(logevent);
                    _cc.client = _client;
                    _cc.StartReciev();
                }
            }
            catch (Exception ex)
            {
                logevent?.Invoke(ex.Message);
            }
        }
        private void InitSet()
        {
            string _port = ConfigHelper.GetConfigString(szjd);
            bool _b = int.TryParse(_port, out port);
            if (!_b)
            {
                Log.Add("获取监听端口失败,请在" + szjd + "节点配置监听端口.");
            }
        }
    }
}
