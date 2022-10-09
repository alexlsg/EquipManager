using EquipDataManager.Dal;
using EquipModel;
using EquipModel.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using ThirdParty.Json.LitJson;
using Tools;

namespace EquipDataManager.Bll
{
    /// <summary>
    /// 数据采集
    /// </summary>
    public partial class DataPicker
    {
        /// <summary>
        /// 数据缓存
        /// </summary>
        public List<EquipData> dataCache = new List<EquipData>();
        public static readonly object dataCachelockobj = new object();
        /// <summary>
        /// 设备测点对应的上次数据表
        /// </summary>
        Dictionary<string, EquipData> datas = new Dictionary<string, EquipData>();
        public Dictionary<string, EquipData> lsdatas = new Dictionary<string, EquipData>();
        Dictionary<string, EquipData> eventdatas = new Dictionary<string, EquipData>();
        public Dictionary<string, DateTime> dataSaveTime = new Dictionary<string, DateTime>();
        Dictionary<string, int> eventtypes = new Dictionary<string, int>();
        //设备配置
        public List<Equip> equips;
        //测点配置
        public List<EquipSpotSet> equipsSpotSets;
        List<EquipTjSet> equipTjSets;
        List<EquipGroup> equipsGroupSets;
        public List<EventSet> eventsets = new List<EventSet>();
        List<EventYzSet> eventYzSets = new List<EventYzSet>();
        List<CMDSet> cmdsets = new List<CMDSet>();
        public IDataPicker datapicker;
        private DataPicker()
        {
        }
        public static DataPicker Instance = new DataPicker();
        /// <summary>
        /// 运行标志
        /// </summary>
        bool running = false;
        /// <summary>
        /// 运行状态
        /// </summary>
        bool status = false;
        /// <summary>
        /// 保存时间差,单位分钟
        /// </summary>
        int savetimespan = 0;
        Thread _th;
        /// <summary>
        /// 开始采集
        /// </summary>
        public void Start(IDataPicker idp)
        {
            while (true)
            {
                try
                {
                    DBHelper.ExecuteCommand("select * from Equip limit 1");
                    break;
                }
                catch (Exception ex)
                {
                    Log.Add("检查数据库连接失败！10秒后重试");
                    Thread.Sleep(10 * 1000);
                }
            }
            datapicker = idp;
            //CheckAuthor.Check((string msg) => { Log.Add(msg); });
            Stop();
            running = true;
            _th = new Thread(DataPickerRun);
            _th.IsBackground = true;
            _th.Start();
            InitCmd();
        }

        public void RefreshEventSet()
        {
            lock (eventsets)
            {
                eventsets = EventSetDal.GetList();
            }
            lock (cmdsets)
            {
                cmdsets = CMDSetDal.GetList();
            }
            lock (eventYzSets)
            {
                eventYzSets = EventYzSetDal.GetList();
            }
        }

        Dictionary<int, int> 设备通讯错误次数 = new Dictionary<int, int>();
        Dictionary<int, bool> 设备通讯故障中 = new Dictionary<int, bool>();
        /// <summary>
        /// 数据采集执行方法
        /// </summary>
        /// <param name="obj"></param>
        private void DataPickerRun()
        {
            status = true;
            try
            {
                Log.Add("启动设备管理数据轮询服务!");
                Init();//初始化配置
                Log.Add("服务启动成功!");
                //LoadFile();
                while (running)
                {
                    foreach (var item in equips)//循环读取设备数据
                    {
                        ReadData(item);

                    }
                    Thread.Sleep(30000);
                }

                Log.Add("设备管理数据轮询服务已结束!");
            }
            catch (Exception ex)
            {
                Log.Add(ex.Message);
                Log.Add(ex);
            }
            status = false;
        }

        public void ReadData(Equip item)
        {
            try
            {
                Pub.AddTestLog($"开始读取设备{item.NO}数据.");
                string _data;
                try
                {
                    _data = datapicker.GetDataFromEquip(item);//从设备读取数据
                }
                catch (Exception ex)
                {
                    Log.Add($"读取数据失败:设备ID:{item.ID},COM:{item.IP},PORT:{item.PORT}");
                    Log.Add(ex);
                    if (!设备通讯错误次数.ContainsKey(item.ID))
                    {
                        设备通讯错误次数[item.ID] = 0;
                    }
                    设备通讯错误次数[item.ID]++;
                    if (设备通讯错误次数[item.ID] > item.Txcs)
                    {
                        保存通讯故障事件(item);
                    }
                    throw;
                }
                Fxclsj(equipsSpotSets, item, _data);
                Thread.Sleep(10);
            }
            catch (Exception ex)
            {
                Log.Add("读取数据失败:" + ex.Message);
                Log.Add(ex);
            }
        }

        private void 保存通讯故障事件(Equip item)
        {
            if (!设备通讯故障中.ContainsKey(item.ID))
            {
                设备通讯故障中[item.ID] = false;
            }
            if (!设备通讯故障中[item.ID])
            {
                设备通讯故障中[item.ID] = true;
                EquipDataDal.SaveEvent(item.GroupName, item.GroupID, item.EquipTypeName, item.EquipType, item.Name, item.ID, "2", item.TxgzDesc, item.EventLevel, "00000002");
            }
        }
        private void 保存通讯正常事件(Equip item)
        {
            if (!设备通讯故障中.ContainsKey(item.ID))
            {
                设备通讯故障中[item.ID] = false;
            }
            if (设备通讯故障中[item.ID])
            {
                设备通讯故障中[item.ID] = false;
                EquipDataDal.SaveTxzcEvent(item.GroupName, item.GroupID, item.EquipTypeName, item.EquipType, item.Name, item.ID,"0", item.TxzcDesc, 0, "00000001");
            }
        }

        public void Fxclsj(List<EquipSpotSet> _equipsSpotSets, Equip item, string _data)
        {
            Pub.AddTestLog($"开始解析数据!设备ID:{item.ID}");
            List<EquipData> _edataes = datapicker.AnalyzData(_data, item);//解析数据
            bool _gz = false;
            Pub.AddTestLog($"数据解析完成，开始保存数据!设备ID:{item.ID},数据条数:{_edataes.Count}");
            if (_edataes.Exists(n => n.DataType == "E" && n.State == "2"))
            {
                Pub.AddTestLog($"设备E状态值等于2，触发故障事件!设备ID:{item.ID}");
                _gz = true;
                保存通讯故障事件(item);
            }
            foreach (var data in _edataes)
            {
                EquipTjSet _tjset = equipTjSets.Find(n => n.EquipTypeName == item.EquipType && n.SpotNO.Contains($"{data.DataType}-{data.SpotNO}"));
                if (_tjset != null)
                {
                    data.Event = _tjset.Tjlx + _tjset.Tjzt;
                }
                List<EquipSpotSet> _set = _equipsSpotSets.FindAll(n => n.EquipTypeName == item.EquipTypeName && n.DataType == data.DataType && (n.SpotNO == data.SpotNO || (string.IsNullOrEmpty(n.SpotNO) && data.SpotNO == "0")));
                if (_set != null)
                {
                    foreach (var _item in _set.Select(n => n.SaveType).Distinct())
                    {
                        // 保存类型,0不保存、1外接设备类型、2状态、3实时值
                        switch (_item)
                        {
                            case 3:
                            case 4:
                            case 5:
                                SaveMx(data, _set.Find(n => n.SaveType == _item));
                                break;
                            default:
                                break;
                        }
                    }
                }

                if (!item.Pb && !_gz)
                {
                    保存通讯正常事件(item);
                    SaveEvent(item, data);
                }
                else
                {
                    Pub.AddTestLog($"设备事件未进行保存!设备ID:{item.ID}，屏蔽设置:{item.Pb},故障标志:{_gz}");
                }
                SaveCache(data);
            }
            //SaveFile();
        }

        private void SaveFile()
        {
            XmlHelper.SaveListToXml(Path.Combine(XmlHelper.CurrentDir, "Data.dat"), dataCache);
        }
        public void LoadFile()
        {
            dataCache = XmlHelper.LoadListFromXml<EquipData>(Path.Combine(XmlHelper.CurrentDir, "Data.dat"));
            if (dataCache == null)
            {
                dataCache = new List<EquipData>();
            }
            else
                dataCache.ForEach(n => n.EquipID = 34);
        }

        /// <summary>
        /// 处理缓存
        /// </summary>
        /// <param name="data"></param>
        /// <exception cref="NotImplementedException"></exception>
        private void SaveCache(EquipData data)
        {
            lock (dataCachelockobj)
            {
                dataCache.RemoveAll(n => n.EquipID == data.EquipID && n.EquipNO == data.EquipNO && n.DataType == data.DataType && n.SpotNO == data.SpotNO);
                dataCache.Add(data);
            }
            datas[GetKey(data)] = data;
        }
        public string GetKey(EquipData data)
        {
            return data.EquipID + data.EquipNO + data.DataType + data.SpotNO;
        }


        /// <summary>
        /// 初始信息
        /// </summary>
        public void Init()
        {
            Log.Add("初始化设备信息!");
            equipsGroupSets = EquipDal.GetGroupSets();
            //初始化设备信息
            equips = EquipDal.GetAllEquips();
            Log.Add("初始化测点配置信息!");
            //初始化测点保存类型信息
            equipsSpotSets = EquipSpotSetDal.GetAllSpotSets();
            //初始化统计配置
            Log.Add("初始化统计配置信息!");
            equipTjSets = EquipSpotSetDal.GetAllTjSets();
            Log.Add("初始化事件规则配置信息!");
            RefreshEventSet();
            Log.Add($"设备信息{equips?.Count}条,设备测点保存设置{equipsSpotSets?.Count}条,统计配置{equipTjSets?.Count}条");
        }

        public void Stop()
        {
            running = false;
            while (status)
            {
                Thread.Sleep(100);
            }
            _th = null;
        }


        private string GetSbzt(string data)
        {
            switch (data)
            {
                case "10": return "../../images/line/1.png";
                case "9": return "../../images/line/2.png";
                case "7": return "../../images/line/3.png";
                case "6": return "../../images/line/4.png";
                case "4": return "../../images/line/5.png";
                case "13": return "../../images/line/6.png";
                case "3": return "../../images/line/7.png";
                case "1": return "../../images/line/33.png";
            }
            return "";
        }
        List<Command> Commands;
        List<Command> NeedreturnCommands;
        List<Command> ReturnCommands;
        void InitCmd()
        {
            Commands = new List<Command>();
            NeedreturnCommands = new List<Command>();
            ReturnCommands = new List<Command>();
            Thread _thread = new Thread(CmdZF);
            _thread.Start();
        }



    }
}
