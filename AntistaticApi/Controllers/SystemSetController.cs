using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using SystemSet;
using Tools;

namespace AntistaticApi.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    //[Authorize]
    public class SystemSetController : ControllerBase
    {
        /// <summary>
        /// 新增设备
        /// </summary>
        /// <param name="Equip"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult AddEquip([FromBody] Equip Equip)
        {
            EquipService EquipService = new EquipService();
            HttpResult httpResult = EquipService.AddEquip(Equip);
            return new JsonResult(httpResult);
        }
        /// <summary>
        /// 修改设备
        /// </summary>
        /// <param name="Equip"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult ModEquip([FromBody] Equip Equip)
        {
            EquipService EquipService = new EquipService();
            HttpResult httpResult = EquipService.AddEquip(Equip);
            return new JsonResult(httpResult);
        }
        /// <summary>
        /// 删除设备
        /// </summary>
        /// <param name="Equip"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult DelEquip([FromBody] Equip Equip)
        {
            EquipService EquipService = new EquipService();
            HttpResult httpResult = EquipService.DelEquip(Equip);
            return new JsonResult(httpResult);
        }
        /// <summary>
        /// 查询设备列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult GetEquipList()
        {
            EquipService EquipService = new EquipService();
            HttpResult httpResult = EquipService.GetEquipList();
            return new JsonResult(httpResult);
        }

        // <summary>
        /// 根据产线ID和设备类型ID查询设备功能列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult GetEquipListByProductionLineIdANDEquipTypeId([FromBody] string productionLineId,string equipTypeId)
        {
            EquipService EquipService = new EquipService();
            HttpResult httpResult = EquipService.GetEquipListByProductionLineIdANDEquipTypeId(productionLineId, equipTypeId);
            return new JsonResult(httpResult);
        }

        // <summary>
        /// 根据设备编号查询设备测点功能列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult GetEquipListByEquipNO([FromBody] string equipNO)
        {
            EquipSpotService EquipSpotService = new EquipSpotService();
            HttpResult httpResult = EquipSpotService.GetEquipListByEquipNO(equipNO);
            return new JsonResult(httpResult);
        }

        /// <summary>
        /// 新增设备测点
        /// </summary>
        /// <param name="EquipSpotSet"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult AddEquipSpotSet([FromBody] EquipSpotSet EquipSpotSet)
        {
            EquipSpotSetService EquipSpotSetService = new EquipSpotSetService();
            HttpResult httpResult = EquipSpotSetService.AddEquipSpotSet(EquipSpotSet);
            return new JsonResult(httpResult);
        }
        /// <summary>
        /// 修改设备测点
        /// </summary>
        /// <param name="EquipSpotSet"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult ModEquipSpotSet([FromBody] EquipSpotSet EquipSpotSet)
        {
            EquipSpotSetService EquipSpotSetService = new EquipSpotSetService();
            HttpResult httpResult = EquipSpotSetService.AddEquipSpotSet(EquipSpotSet);
            return new JsonResult(httpResult);
        }
        /// <summary>
        /// 删除设备测点
        /// </summary>
        /// <param name="EquipSpotSet"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult DelEquipSpotSet([FromBody] EquipSpotSet EquipSpotSet)
        {
            EquipSpotSetService EquipSpotSetService = new EquipSpotSetService();
            HttpResult httpResult = EquipSpotSetService.DelEquipSpotSet(EquipSpotSet);
            return new JsonResult(httpResult);
        }
        /// <summary>
        /// 查询设备测点列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult GetEquipSpotSetList()
        {
            EquipSpotSetService EquipSpotSetService = new EquipSpotSetService();
            HttpResult httpResult = EquipSpotSetService.GetEquipSpotSetList();
            return new JsonResult(httpResult);
        }

        /// <summary>
        /// 新增设备类型
        /// </summary>
        /// <param name="EquipType"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult AddEquipType([FromBody] EquipType EquipType)
        {
            EquipTypeService EquipTypeService = new EquipTypeService();
            HttpResult httpResult = EquipTypeService.AddEquipType(EquipType);
            return new JsonResult(httpResult);
        }
        /// <summary>
        /// 修改设备类型
        /// </summary>
        /// <param name="EquipType"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult ModEquipType([FromBody] EquipType EquipType)
        {
            EquipTypeService EquipTypeService = new EquipTypeService();
            HttpResult httpResult = EquipTypeService.AddEquipType(EquipType);
            return new JsonResult(httpResult);
        }
        /// <summary>
        /// 删除设备类型
        /// </summary>
        /// <param name="EquipType"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult DelEquipType([FromBody] EquipType EquipType)
        {
            EquipTypeService EquipTypeService = new EquipTypeService();
            HttpResult httpResult = EquipTypeService.DelEquipType(EquipType);
            return new JsonResult(httpResult);
        }
        /// <summary>
        /// 查询设备类型列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult GetEquipTypeList()
        {
            EquipTypeService EquipTypeService = new EquipTypeService();
            HttpResult httpResult = EquipTypeService.GetEquipTypeList();
            return new JsonResult(httpResult);
        }

        /// <summary>
        /// 新增网关
        /// </summary>
        /// <param name="Gateway"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult AddGateway(Gateway Gateway)
        {
            GatewayService GatewayService = new GatewayService();
            HttpResult httpResult = GatewayService.AddGateway(Gateway);
            return new JsonResult(httpResult);
        }
        /// <summary>
        /// 修改网关
        /// </summary>
        /// <param name="Gateway"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult ModGateway(Gateway Gateway)
        {
            GatewayService GatewayService = new GatewayService();
            HttpResult httpResult = GatewayService.AddGateway(Gateway);
            return new JsonResult(httpResult);
        }
        /// <summary>
        /// 删除网关
        /// </summary>
        /// <param name="Gateway"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult DelGateway(Gateway Gateway)
        {
            GatewayService GatewayService = new GatewayService();
            HttpResult httpResult = GatewayService.DelGateway(Gateway);
            return new JsonResult(httpResult);
        }
        /// <summary>
        /// 查询网关列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult GetGatewayList()
        {
            GatewayService GatewayService = new GatewayService();
            HttpResult httpResult = GatewayService.GetGatewayList();
            return new JsonResult(httpResult);
        }

        /// <summary>
        /// 新增产线
        /// </summary>
        /// <param name="ProductionLine"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult AddProductionLine(ProductionLine ProductionLine)
        {
            ProductionLineService ProductionLineService = new ProductionLineService();
            HttpResult httpResult = ProductionLineService.AddProductionLine(ProductionLine);
            return new JsonResult(httpResult);
        }
        /// <summary>
        /// 修改产线
        /// </summary>
        /// <param name="ProductionLine"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult ModProductionLine(ProductionLine ProductionLine)
        {
            ProductionLineService ProductionLineService = new ProductionLineService();
            HttpResult httpResult = ProductionLineService.AddProductionLine(ProductionLine);
            return new JsonResult(httpResult);
        }
        /// <summary>
        /// 删除产线
        /// </summary>
        /// <param name="ProductionLine"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult DelProductionLine(ProductionLine ProductionLine)
        {
            ProductionLineService ProductionLineService = new ProductionLineService();
            HttpResult httpResult = ProductionLineService.DelProductionLine(ProductionLine);
            return new JsonResult(httpResult);
        }
        /// <summary>
        /// 查询产线列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult GetProductionLineList()
        {
            ProductionLineService ProductionLineService = new ProductionLineService();
            HttpResult httpResult = ProductionLineService.GetProductionLineList();
            return new JsonResult(httpResult);
        }

        /// <summary>
        /// 新增产线组
        /// </summary>
        /// <param name="ProductionLineGroup"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult AddProductionLineGroup(ProductionLineGroup ProductionLineGroup)
        {
            ProductionLineGroupService ProductionLineGroupService = new ProductionLineGroupService();
            HttpResult httpResult = ProductionLineGroupService.AddProductionLineGroup(ProductionLineGroup);
            return new JsonResult(httpResult);
        }
        /// <summary>
        /// 修改产线组
        /// </summary>
        /// <param name="ProductionLineGroup"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult ModProductionLineGroup(ProductionLineGroup ProductionLineGroup)
        {
            ProductionLineGroupService ProductionLineGroupService = new ProductionLineGroupService();
            HttpResult httpResult = ProductionLineGroupService.AddProductionLineGroup(ProductionLineGroup);
            return new JsonResult(httpResult);
        }
        /// <summary>
        /// 删除产线组
        /// </summary>
        /// <param name="ProductionLineGroup"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult DelProductionLineGroup(ProductionLineGroup ProductionLineGroup)
        {
            ProductionLineGroupService ProductionLineGroupService = new ProductionLineGroupService();
            HttpResult httpResult = ProductionLineGroupService.DelProductionLineGroup(ProductionLineGroup.ProductionLineId);
            return new JsonResult(httpResult);
        }
        /// <summary>
        /// 查询产线组列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult GetProductionLineGroupList()
        {
            EquipGroupService ProductionLineGroupService = new EquipGroupService();
            HttpResult httpResult = ProductionLineGroupService.GetProductionLineGroupList();
            return new JsonResult(httpResult);
        }
        /// <summary>
        /// 查询产线组列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult GetProductionLineGroupEquipTypeListByProductionLineId(string productionLineId)
        {
            ProductionLineGroupService ProductionLineGroupService = new ProductionLineGroupService();
            HttpResult httpResult = ProductionLineGroupService.GetProductionLineGroupEquipTypeListByProductionLineId(productionLineId);
            return new JsonResult(httpResult);
        }

        /// <summary>
        /// 新增系统设置
        /// </summary>
        /// <param name="SystemSetting"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult AddSystemSetting(SystemSetting SystemSetting)
        {
            SystemSettingService SystemSettingService = new SystemSettingService();
            HttpResult httpResult = SystemSettingService.AddSystemSetting(SystemSetting);
            return new JsonResult(httpResult);
        }
        /// <summary>
        /// 修改系统设置
        /// </summary>
        /// <param name="SystemSetting"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult ModSystemSetting(SystemSetting SystemSetting)
        {
            SystemSettingService SystemSettingService = new SystemSettingService();
            HttpResult httpResult = SystemSettingService.AddSystemSetting(SystemSetting);
            return new JsonResult(httpResult);
        }
        /// <summary>
        /// 删除系统设置
        /// </summary>
        /// <param name="SystemSetting"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult DelSystemSetting(SystemSetting SystemSetting)
        {
            SystemSettingService SystemSettingService = new SystemSettingService();
            HttpResult httpResult = SystemSettingService.DelSystemSetting(SystemSetting);
            return new JsonResult(httpResult);
        }
        /// <summary>
        /// 查询系统设置列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult GetSystemSettingList()
        {
            SystemSettingService SystemSettingService = new SystemSettingService();
            HttpResult httpResult = SystemSettingService.GetSystemSettingList();
            return new JsonResult(httpResult);
        }

        /// <summary>
        /// 新增统计配置
        /// </summary>
        /// <param name="EquipTjSet"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult AddEquipTjSet(EquipTjSet EquipTjSet)
        {
            EquipTjSetService EquipTjSetService = new EquipTjSetService();
            HttpResult httpResult = EquipTjSetService.AddEquipTjSet(EquipTjSet);
            return new JsonResult(httpResult);
        }
        /// <summary>
        /// 修改统计配置
        /// </summary>
        /// <param name="EquipTjSet"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult ModEquipTjSet(EquipTjSet EquipTjSet)
        {
            EquipTjSetService EquipTjSetService = new EquipTjSetService();
            HttpResult httpResult = EquipTjSetService.AddEquipTjSet(EquipTjSet);
            return new JsonResult(httpResult);
        }
        /// <summary>
        /// 删除统计配置
        /// </summary>
        /// <param name="EquipTjSet"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult DelEquipTjSet(EquipTjSet EquipTjSet)
        {
            EquipTjSetService EquipTjSetService = new EquipTjSetService();
            HttpResult httpResult = EquipTjSetService.DelEquipTjSet(EquipTjSet);
            return new JsonResult(httpResult);
        }
        /// <summary>
        /// 查询统计配置列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult GetEquipTjSetList()
        {
            EquipTjSetService EquipTjSetService = new EquipTjSetService();
            HttpResult httpResult = EquipTjSetService.GetEquipTjSetList();
            return new JsonResult(httpResult);
        }
    }
}
