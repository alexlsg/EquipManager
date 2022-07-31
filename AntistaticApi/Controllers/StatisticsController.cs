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
    [TokenActionFilter1]
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
        [HttpPost]
        public IActionResult GetLsqx(Params par)
        {
            try
            {
                IEnumerable<object> _cds = DataPicker.Instance.GetLssj(par.spotno, par.ksrq, par.jsrq);
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
