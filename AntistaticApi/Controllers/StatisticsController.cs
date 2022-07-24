using AntistaticApi.IdentityService;
using AntistaticApi.Model;
using EquipDataManager.Bll;
using EquipDataManager.Dal;
using EquipModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using Tools;

namespace AntistaticApi.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class StatisticsController : ControllerBase
    {
        [HttpGet]
        public IActionResult Test()
        {
            return new JsonResult("测试");
        }
        /// <summary>
        /// 根据设备类型统计实时情况
        /// </summary>
        /// <param name="Equip"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult GetEquipStatusByType(Params paras)
        {
            try
            {
                List<EquipSstjData> _datas = DataPicker.Instance.GetEquipSstjDataByType(paras.typeid, "");
                HttpResult _httpResult = new HttpResult();
                _httpResult.Data = _datas;
                _httpResult.Status = true;
                return new JsonResult(_httpResult);
            }
            catch (Exception ex)
            {
                Log.Add(ex);
                return new JsonResult(ex.Message);
            }
        }

        /// <summary>
        /// 根据设备分组统计实时情况
        /// </summary>
        /// <param name="Equip"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult GetEquipStatusByGroup(Params paras)
        {
            try
            {
                List<EquipSstjData> _datas = DataPicker.Instance.GetEquipSstjDataByType("", paras.groupid);
                HttpResult _httpResult = new HttpResult();
                _httpResult.Message = "parametter:" + paras.groupid;
                _httpResult.Data = _datas;
                _httpResult.Status = true;
                return new JsonResult(_httpResult);
            }
            catch (Exception ex)
            {
                Log.Add(ex);
                return new JsonResult(ex.Message);
            }
        }

        /// <summary>
        /// 获取历史测点列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult GetLscdlb(Params paras)
        {
            try
            {
                IEnumerable<object> _cds = DataPicker.Instance.GetLssjcdlb(paras.groupid);
                HttpResult _httpResult = new HttpResult();
                _httpResult.Data = _cds;
                _httpResult.Status = true;
                return new JsonResult(_httpResult);
            }
            catch (Exception ex)
            {
                Log.Add(ex);
                return new JsonResult(ex.Message);
            }
        }
        public static List<EquipData> _equiplist = new List<EquipData>();
        /// <summary>
        /// 历史数据查询左侧
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult GetLsqx(DateTime begintime, DateTime endtime, string spotno)
        {
            try
            {
                IEnumerable<object> _cds = DataPicker.Instance.GetLssj(spotno, begintime, endtime);
                HttpResult _httpResult = new HttpResult();
                _httpResult.Data = _cds;
                _httpResult.Status = true;
                return new JsonResult(_httpResult);
            }
            catch (Exception ex)
            {
                Log.Add(ex);
                return new JsonResult(ex.Message);
            }
        }
        /// <summary>
        /// 查询历史曲线
        /// </summary>
        /// <param name="Equip"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult GetLsjlBy(string euqid)
        {
            BightData _vbight = new BightData();
            if (!string.IsNullOrEmpty(euqid))
            {
                // 数据处理
                string _str = euqid.Substring(1, euqid.Length - 1);
                string[] _strarr = _str.Split(",");
                // 曲线title
                List<object> _title = new List<object>();
                List<DataSeries> _dlist = new List<DataSeries>();
                for (var i = 0; i < _strarr.Length; i++)
                {
                    List<object> _data = new List<object>();
                    string _name = _equiplist.Find(x => x.EquipID.ToString() == _strarr[i]).SpotName.ToString();
                    _title.Add(_name);
                    // 模拟数据
                    DataSeries _ser = new DataSeries();
                    for (DateTime time = Convert.ToDateTime("2022-07-05"); time <= DateTime.Now; time = time.AddDays(1))
                    {
                        Random _random = new Random();
                        int _int = _random.Next(1, 100);
                        _data.Add((i + _int) * 25);
                        _ser.Data = _data;
                    }
                    _ser.Type = "line";
                    _ser.Smooth = true;
                    _ser.Name = _name;
                    _dlist.Add(_ser);
                }
                // 模拟日期
                List<object> _date = new List<object>();
                // 模拟数据
                for (DateTime time = Convert.ToDateTime("2022-07-05"); time <= DateTime.Now; time = time.AddDays(1))
                {
                    _date.Add(time.ToString("MM-dd"));
                }
                _vbight.Title = _title;
                _vbight.DateCore = _date;
                _vbight.DataCore = _dlist;
            }
            HttpResult _httpResult = HttpResult.GetJsonResult(true, "线组列表查询成功", string.Empty, _vbight);
            return new JsonResult(_httpResult);
        }

        /// <summary>
        /// 获取历史数据
        /// </summary>
        /// <param name="equipid">设备id</param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult GetLsjlByEquipID(Params paras)
        {
            try
            {
                HttpResult _httpResult = new HttpResult();
                _httpResult.Data = DataPicker.Instance.dataCache.FindAll(n => n.EquipID.ToString() == paras.equipid);
                _httpResult.Status = true;
                return new JsonResult(_httpResult);
            }
            catch (Exception ex)
            {
                Log.Add(ex);
                return new JsonResult(ex.Message);
            }
        }

        /// <summary>
        /// 获取历史数据
        /// </summary>
        /// <param name="equipid">产线id</param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult GetLsjlByGroupID(Params paras)
        {
            try
            {
                HttpResult _httpResult = new HttpResult();
                _httpResult.Data = from a in DataPicker.Instance.dataCache
                                   join b in DataPicker.Instance.equips on a.EquipID equals b.ID
                                   where b.GroupID == paras.groupid
                                   select a;
                _httpResult.Status = true;
                return new JsonResult(_httpResult);
            }
            catch (Exception ex)
            {
                Log.Add(ex);
                return new JsonResult(ex.Message);
            }
        }

        /// <summary>
        /// 获取历史数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult GetAllLsjl()
        {
            try
            {
                HttpResult _httpResult = new HttpResult();
                _httpResult.Data = DataPicker.Instance.dataCache;
                _httpResult.Status = true;
                return new JsonResult(_httpResult);
            }
            catch (Exception ex)
            {
                Log.Add(ex);
                return new JsonResult(ex.Message);
            }
        }
        /// <summary>
        /// 查询历史曲线
        /// </summary>
        /// <param name="Equip"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult GetLsjl([FromBody] string spotno, DateTime ksrq, DateTime jsrq)
        {
            try
            {
                List<EquipData_Ls> _datas = DataPicker.Instance.GetLssj(spotno, ksrq, jsrq);
                HttpResult _httpResult = new HttpResult();
                _httpResult.Data = _datas;
                _httpResult.Status = true;
                return new JsonResult(_httpResult);
            }
            catch (Exception ex)
            {
                Log.Add(ex);
                return new JsonResult(ex.Message);
            }
        }
        [HttpPost]
        public IActionResult GetEvent(Params paras)
        {
            try
            {
                object data = EquipDataDal.GetEvent(paras.groupid, paras.equiptypeid, paras.ksrq, paras.jsrq, paras.datatype, paras.eventkey);
                HttpResult _httpResult = new HttpResult();
                _httpResult.Data = data;
                _httpResult.Status = true;
                return new JsonResult(_httpResult);
            }
            catch (Exception ex)
            {
                Log.Add(ex);
                return new JsonResult(ex.Message);
            }
        }
    }
    public class BightData
    {
        /// <summary>
        /// 曲线头
        /// </summary>
        public List<object> Title { set; get; }
        /// <summary>
        /// 日期范围
        /// </summary>
        public List<object> DateCore { set; get; }
        /// <summary>
        /// 数据范围
        /// </summary>
        public List<DataSeries> DataCore { set; get; }
    }
    public class DataSeries
    {
        public List<object> Data { set; get; }
        /// <summary>
        /// 类型
        /// </summary>
        public string Type { set; get; }
        /// <summary>
        /// 弯曲
        /// </summary>
        public bool Smooth { set; get; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { set; get; }
    }
    public class AllData
    {
        public AllData()
        {
            jdsbDataList = new List<JdsbData>();
            wsdDataList = new List<WsdData>();
            jddbDataList = new List<BtData>();
            fjztDataList = new List<BtData>();
        }
        public List<JdsbData> jdsbDataList { get; set; }
        public List<WsdData> wsdDataList { get; set; }
        public List<BtData> jddbDataList { get; set; }
        public List<BtData> fjztDataList { get; set; }
    }
    public class JdsbData
    {
        public string Name { set; get; }
        public string X1 { set; get; }
        public string X2 { set; get; }
        public string X3 { set; get; }
        public string X4 { set; get; }

    }
    public class WsdData
    {
        public string Name { set; get; }
        public string Wd { set; get; }
        public string Sd { set; get; }

    }

    public class BtData
    {
        public string Name { set; get; }
        public string Value { set; get; }

    }
}
