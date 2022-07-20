using EquipDataManager.Dal;
using EquipModel;
using EquipModel.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using Tools;

namespace EquipDataManager.Bll
{
    public delegate void LogDelegate(string message);
    /// <summary>
    /// 数据采集
    /// </summary>
    public class DataPicker
    {
        LogDelegate logevent;
        /// <summary>
        /// 数据缓存
        /// </summary>
        public List<EquipData> dataCache = new List<EquipData>();
        /// <summary>
        /// 设备测点对应的上次数据表
        /// </summary>
        Dictionary<string, EquipData> datas = new Dictionary<string, EquipData>();
        Dictionary<string, DateTime> dataSaveTime = new Dictionary<string, DateTime>();
        //设备配置
        List<Equip> equips;
        //测点配置
        List<EquipSpotSet> equipsSpotSets;
        List<EquipTjSet> equipTjSets;
        List<EquipGroup> equipsGroupSets;
        IDataPicker datapicker;
        string logmodal = "error";
        void AddTestLog(string msg)
        {
            if (logmodal == "all")
            {
                logevent?.Invoke(msg);
            }
        }
        private DataPicker()
        {
            logmodal = ConfigHelper.GetConfigString("log:modal");
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
        public void Start(IDataPicker idp, LogDelegate loge)
        {
            while (true)
            {
                try
                {
                    DBHelper.ExecuteCommand("select * from Equip");
                    break;
                }
                catch (Exception ex)
                {
                    loge?.Invoke("检查数据库连接失败！10秒后重试");
                    Thread.Sleep(10 * 1000);
                }
            }
            logevent = loge;
            datapicker = idp;
            CheckAuthor.Check((string msg) => { logevent?.Invoke(msg); });
            Stop();
            running = true;
            _th = new Thread(DataPickerRun);
            _th.IsBackground = true;
            _th.Start();
        }


        /// <summary>
        /// 数据采集执行方法
        /// </summary>
        /// <param name="obj"></param>
        private void DataPickerRun()
        {
            status = true;
            try
            {
                logevent?.Invoke("启动设备管理数据轮询服务!");
                Init();//初始化配置
                logevent?.Invoke("服务启动成功!");
                //LoadFile();
                while (running)
                {
                    foreach (var item in equips)//循环读取设备数据
                    {
                        try
                        {
                            AddTestLog($"开始读取设备{item.NO}数据.");
                            string _data = datapicker.GetDataFromEquip(item);//从设备读取数据
                            Fxclsj(equipsSpotSets, item, _data);
                            Thread.Sleep(10);
                        }
                        catch (Exception ex)
                        {
                            logevent?.Invoke("读取数据失败:" + ex.Message);
                        }

                    }
                    Thread.Sleep(30000);
                }

                logevent?.Invoke("设备管理数据轮询服务已结束!");
            }
            catch (Exception ex)
            {
                logevent?.Invoke(ex.Message);
            }
            status = false;
        }

        private void Fxclsj(List<EquipSpotSet> _equipsSpotSets, Equip item, string _data)
        {
            AddTestLog("开始解析数据!");
            List<EquipData> _edataes = datapicker.AnalyzData(_data, item.ID);//解析数据
            AddTestLog("数据解析完成，开始保存数据!");
            foreach (var data in _edataes)
            {
                EquipTjSet _tjset = equipTjSets.Find(n => n.EquipType == item.EquipType && n.SpotNO.Contains($"{data.DataType}-{data.SpotNO}"));
                if (_tjset != null)
                {
                    data.Event = _tjset.Tjlx + _tjset.Tjzt;
                }
                EquipSpotSet _set = _equipsSpotSets.Find(n => n.EquipTypeName == item.EquipTypeName && n.DataType == data.DataType && (n.SpotNO == data.SpotNO || (string.IsNullOrEmpty(n.SpotNO) && data.SpotNO == "0")));
                if (_set != null)
                {
                    // 保存类型,0不保存、1外接设备类型、2状态、3实时值
                    switch (_set.SaveType)
                    {
                        case 2: SaveEvent(item, data); break;
                        case 3: SaveMx(data); break;
                        default:
                            break;
                    }
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
            dataCache.RemoveAll(n => n.EquipID == data.EquipID && n.EquipNO == data.EquipNO && n.DataType == data.DataType && n.SpotNO == data.SpotNO);
            dataCache.Add(data);
            datas[GetKey(data)] = data;
        }
        string GetKey(EquipData data)
        {
            return data.EquipID + data.EquipNO + data.DataType + data.SpotNO;
        }
        /// <summary>
        /// 保存明细
        /// </summary>
        /// <param name="data"></param>
        /// <exception cref="NotImplementedException"></exception>
        private void SaveMx(EquipData data)
        {
            if (data.Data == "--")
            {
                return;
            }
            InitSaveTimeSpan();
            string _key = GetKey(data);
            //无缓存，或者缓存时间超过间隔
            if (!dataSaveTime.ContainsKey(_key) || (data.Jlsj - dataSaveTime[_key]).TotalSeconds > savetimespan * 60)
            {
                dataSaveTime[_key] = data.Jlsj;
                EquipDataDal.SaveMx(data);
            }
        }
        /// <summary>
        /// 初始化保存时间间隔
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        private void InitSaveTimeSpan()
        {
            try
            {
                if (savetimespan == 0)
                {
                    string _s = ConfigHelper.GetConfigString("save:timespan");
                    savetimespan = int.Parse(_s);
                }
            }
            catch (Exception ex)
            {
                logevent?.Invoke("获取保存时间间隔配置失败!" + ex.Message);
            }
        }

        /// <summary>
        /// 保存事件
        /// </summary>
        /// <param name="item"></param>
        /// <param name="data"></param>
        /// <exception cref="NotImplementedException"></exception>
        private void SaveEvent(Equip item, EquipData data)
        {
            if (data.Data == "--")
            {
                return;
            }
            string _key = GetKey(data);
            if (!datas.ContainsKey(_key) || data.State != datas[_key].State || data.Data != datas[_key].Data)
            {
                EquipDataDal.SaveEvent(data, item);
            }
        }
        /// <summary>
        /// 初始信息
        /// </summary>
        public void Init()
        {
            logevent?.Invoke("初始化设备信息!");
            equipsGroupSets = EquipDal.GetGroupSets();
            //初始化设备信息
            equips = EquipDal.GetAllEquips();
            logevent?.Invoke("初始化测点配置信息!");
            //初始化测点保存类型信息
            equipsSpotSets = EquipSpotSetDal.GetAllSpotSets();
            //初始化统计配置
            logevent?.Invoke("初始化统计配置信息!");
            equipTjSets = EquipSpotSetDal.GetAllTjSets();
            logevent?.Invoke($"设备信息{equips?.Count}条,设备测点保存设置{equipsSpotSets?.Count}条,统计配置{equipTjSets?.Count}条");
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
        /// <summary>
        /// 获取历史数据左边列表
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Lscdlb> GetLssjcdlb()
        {
            var _temp = from a in equips
                        from b in equipsSpotSets
                        where a.EquipTypeName == b.EquipTypeName && b.SaveType == 3
                        select new Lscdlb
                        {
                            Name = a.GroupName + a.Name + b.SpotName,
                            IsSelected = false,
                            SpotNO = a.ID + b.DataType + b.SpotNO
                        };
            return _temp;
        }
        /// <summary>
        /// 查询历史数据
        /// </summary>
        /// <param name="spotno"></param>
        /// <param name="ksrq"></param>
        /// <param name="jsrq"></param>
        /// <returns></returns>
        public List<EquipData_Ls> GetLssj(string spotno, DateTime ksrq, DateTime jsrq)
        {
            return EquipDataDal.GetLssj(spotno, ksrq, jsrq);
        }
        /// <summary>
        /// 获取设备实时统计数据,按类型
        /// </summary>
        /// <returns></returns>
        public List<EquipSstjData> GetEquipSstjDataByType()
        {
            var _temp = from a in dataCache
                        from b in equipTjSets
                        from c in equips
                        where a.EquipID == c.ID && c.EquipTypeName == b.EquipType && GetSjdb(a, b)
                        group a by new { b.EquipType, b.Tjlx, b.Tjzt, b.Cyhj, b.Cyzshj } into g
                        select new EquipSstjData
                        {
                            TypeName = g.Key.EquipType,
                            Tjlx = g.Key.Tjlx,
                            Tjzt = g.Key.Tjzt,
                            Count = g.Count(),
                            Cyhj = g.Key.Cyhj,
                            Cyzshj = g.Key.Cyzshj,
                            TjType = "明细",
                            Px = 0
                        };
            var _temp2 = from a in equipTjSets
                         where !_temp.ToList().Exists(n => n.TypeName == a.EquipType && n.Tjlx == a.Tjlx && n.Tjzt == a.Tjzt)
                         select new EquipSstjData
                         {
                             TypeName = a.EquipType,
                             Tjlx = a.Tjlx,
                             Tjzt = a.Tjzt,
                             Count = 0,
                             Cyhj = a.Cyhj,
                             Cyzshj = a.Cyzshj,
                             TjType = "明细",
                             Px = 0
                         };
            List<EquipSstjData> _temp3 = new List<EquipSstjData>(_temp);
            _temp3.AddRange(_temp2);
            var _temp1 = from a in _temp3
                         where a.Cyhj
                         group a by new { a.TypeName, a.Tjlx, a.Cyzshj } into g
                         select new EquipSstjData
                         {
                             TjType = g.Key.Cyzshj ? "总计" : "合计",
                             TypeName = g.Key.TypeName,
                             Tjlx = g.Key.Tjlx,
                             Tjzt = "总数",
                             Count = g.Sum(n => n.Count),
                             Cyzshj = g.Key.Cyzshj,
                             Px = 99
                         };
            List<EquipSstjData> _result = new List<EquipSstjData>();
            _result.AddRange(_temp3);
            _result.AddRange(_temp1);
            //_result.AddRange(_temp2);
            return _result.OrderBy(n => n.TypeName + n.Cyzshj.ToString() + n.Tjlx + n.Px.ToString() + n.Tjzt).ToList();
        }

        /// <summary>
        /// 获取设备实时统计数据,按产线
        /// </summary>
        /// <returns></returns>
        public List<EquipSstjData> GetEquipSstjDataByGroup()
        {
            var _temp = from a in dataCache
                        from b in equipTjSets
                        from c in equips
                        where a.EquipID == c.ID && c.EquipTypeName == b.EquipType && GetSjdb(a, b)
                        group a by new { c.GroupName, b.Tjlx, b.Tjzt, b.Cyhj, b.Cyzshj } into g
                        select new EquipSstjData
                        {
                            TjType = "明细",
                            TypeName = g.Key.GroupName,
                            Tjlx = g.Key.Tjlx,
                            Tjzt = g.Key.Tjzt,
                            Count = g.Count(),
                            Cyhj = g.Key.Cyhj,
                            Cyzshj = g.Key.Cyzshj,
                            Px = 0
                        };
            var _temp2 = from a in equipTjSets
                         join b in equips on a.EquipType equals b.EquipTypeName
                         where !_temp.ToList().Exists(n => n.TypeName == b.GroupName && n.Tjlx == a.Tjlx && n.Tjzt == a.Tjzt)
                         group new { a, b } by new { b.GroupName, a.Tjlx, a.Tjzt, a.Cyhj, a.Cyzshj } into g
                         select new EquipSstjData
                         {
                             TjType = "明细",
                             TypeName = g.Key.GroupName,
                             Tjlx = g.Key.Tjlx,
                             Tjzt = g.Key.Tjzt,
                             Count = 0,
                             Cyhj = g.Key.Cyhj,
                             Cyzshj = g.Key.Cyzshj,
                             Px = 0
                         };
            List<EquipSstjData> _temp3 = new List<EquipSstjData>(_temp);
            _temp3.AddRange(_temp2);
            var _temp1 = from a in _temp3
                         where a.Cyhj
                         group a by new { a.TypeName, a.Tjlx, a.Cyzshj } into g
                         select new EquipSstjData
                         {
                             TjType = g.Key.Cyzshj ? "总计" : "合计",
                             TypeName = g.Key.TypeName,
                             Tjlx = g.Key.Tjlx,
                             Tjzt = "总数",
                             Count = g.Sum(n => n.Count),
                             Cyzshj = g.Key.Cyzshj,
                             Px = 99
                         };
            List<EquipSstjData> _result = new List<EquipSstjData>();
            _result.AddRange(_temp3);
            _result.AddRange(_temp1);
            //_result.AddRange(_temp2);
            return _result.OrderBy(n => n.TypeName + n.Cyzshj.ToString() + n.Tjlx + n.Px.ToString() + n.Tjzt).ToList();
        }


        //public object GetDataByGroup(string groupid)
        //{
        //    var _temp = from a in dataCache
        //                from b in equips
        //                where a.EquipNO == b.NO && b.GroupID == groupid
        //                select new
        //                {
        //                    TypeName = b.EquipTypeName,
        //                    Name = b.Name,
        //                    Data = a.Data,
        //                    DataType = "1"
        //                };
        //    return _temp;
        //}

        public object GetDataByGroup(string groupid)
        {
            var _jdsh = from a in dataCache
                        from b in equips
                        where
                        a.EquipID == b.ID
                        && b.EquipType == Config.Jdsblx
                        && b.GroupID == groupid
                        && a.DataType == "A"
                        && Config.Jdsbspots.Contains(a.SpotNO)
                        && Config.Jdsbjcz.Contains(a.Data)
                        group a by a.EquipName into g
                        select new
                        {
                            Name = g.Key,
                            Images = g.Select(n => GetSbzt(n.Data))
                        };
            var _wsd = from a in dataCache
                       from b in equips
                       where
                        a.EquipID == b.ID
                       && b.EquipType == Config.Wsdlx
                       && b.GroupID == groupid
                       && a.DataType == "A"
                       && Config.Jdsbspots.Contains(a.SpotNO)
                       group a by a.EquipName into g
                       select new
                       {
                           Name = g.Key,
                           Wd = g.Where(n => n.SpotNO == "51").First().Data,
                           Sd = g.Where(n => n.SpotNO == "52").First().Data,
                       };

            var _jdsbhz = from a in dataCache
                          from b in equips
                          from c in equipTjSets
                          where
                        a.EquipID == b.ID
                     && b.EquipType == Config.Jdsblx
                     && b.GroupID == groupid
                     && a.DataType == "A"
                     && Config.Jdsbspots.Contains(a.SpotNO)
                     && c.EquipType == Config.Jdsblx
                     && GetSjdb(a, c)
                          group new { a, c } by c.Tjlx into g
                          select new
                          {
                              Name = g.Key,
                              Zs = g.Count(),
                              Bjs = g.Where(n => Config.Jdsbbjz.Contains(n.a.Data)).Count()
                          };
            var _lzfjtj = from a in dataCache
                          from b in equipTjSets
                          from c in equips
                          where
                          a.EquipID == c.ID
                          && c.EquipType == Config.Fjlx
                          && c.EquipTypeName == b.EquipType
                          && GetSjdb(a, b)
                          group a by new { b.Tjzt } into g
                          select new
                          {
                              Name = g.Key,
                              Count = g.Count()
                          };
            var _result = new
            {
                Jdsb = _jdsh,
                Wsd = _wsd,
                Jdsbhz = _jdsbhz,
                Lzfjhz = _lzfjtj
            };
            return _result;
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

        private bool GetSjdb(EquipData a, EquipTjSet b)
        {
            if (b.SpotNO.Split(',').Contains(a.DataType + "-" + a.SpotNO))
            {
                if (b.Tjtj == "State")
                {
                    if (b.Data.StartsWith("<>"))
                    {
                        string _v = b.Data.Replace("<>", "");
                        return a.State != _v;
                    }
                    else
                        return a.State == b.Data;
                }
                else if (b.Tjtj == "NoState")
                {
                    if (b.Data.StartsWith("<>"))
                    {
                        string _v = b.Data.Replace("<>", "");
                        return a.NoState != _v;
                    }
                    else
                        return a.NoState == b.Data;
                }
                else if (b.Tjtj == "Data")
                {
                    double _d1;
                    double _d2;
                    if (double.TryParse(a.Data, out _d1) && double.TryParse(b.Data, out _d2))
                        return _d1 == _d2;
                    return false;
                }
            }
            return false;
        }
    }
}
