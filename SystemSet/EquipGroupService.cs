using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Text;
using Tools;

namespace SystemSet
{
    public class EquipGroupService
    {
        /// <summary>
        /// 添加设备组
        /// </summary>
        /// <param name="EquipGroup">设备组对象</param>
        /// <returns></returns>
        public HttpResult AddEquipGroup(EquipGroup EquipGroup)
        {
            HttpResult httpResult = new HttpResult();
            try
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append(@"
INSERT INTO EquipGroup(EquipGroupName)
VALUES(@EquipGroupName)
");
                MySqlParameter[] mySqlParameters = new MySqlParameter[1];
                mySqlParameters[0] = new MySqlParameter("EquipGroupName", EquipGroup.EquipGroupName);
                int _res = Tools.DBHelper.ExecuteCommand(stringBuilder.ToString(), mySqlParameters);
                httpResult = HttpResult.GetJsonResult(_res == 1, "添加设备组成功", "添加设备组失败");
            }
            catch (Exception ex)
            {
                httpResult = HttpResult.GetJsonResult(false, string.Empty, ex.Message);
                Tools.Log.Add("添加设备组异常：" + ex.Message);
            }
            return httpResult;
        }

        /// <summary>
        /// 修改设备组
        /// </summary>
        /// <param name="EquipGroup">设备组对象</param>
        /// <returns></returns>
        public HttpResult ModEquipGroup(EquipGroup EquipGroup)
        {
            HttpResult httpResult = new HttpResult();
            try
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append(@"
UPDATE EquipGroup SET EquipGroupName=@EquipGroupName
WHERE EquipGroupId=@EquipGroupId
");
                MySqlParameter[] mySqlParameters = new MySqlParameter[2];
                mySqlParameters[0] = new MySqlParameter("EquipGroupName", EquipGroup.EquipGroupName);
                mySqlParameters[1] = new MySqlParameter("EquipGroupId", EquipGroup.EquipGroupId);
                int _res = Tools.DBHelper.ExecuteCommand(stringBuilder.ToString(), mySqlParameters);
                httpResult = HttpResult.GetJsonResult(_res == 1, "修改设备组成功", "修改设备组失败");
            }
            catch (Exception ex)
            {
                httpResult = HttpResult.GetJsonResult(false, string.Empty, ex.Message);
                Tools.Log.Add("修改设备组异常：" + ex.Message);
            }
            return httpResult;
        }

        /// <summary>
        /// 删除设备组
        /// </summary>
        /// <param name="EquipGroup">设备组对象</param>
        /// <returns></returns>
        public HttpResult DelEquipGroup(EquipGroup EquipGroup)
        {
            HttpResult httpResult = new HttpResult();
            try
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append(@"
DELETE From EquipGroup WHERE EquipGroupId=@EquipGroupId;
DELETE  a,b From Equip a INNER JOIN Gateway b on a.GatewayId=b.Id WHERE b.EquipGroupId=@EquipGroupId
");
                MySqlParameter[] mySqlParameters = new MySqlParameter[1];
                mySqlParameters[0] = new MySqlParameter("EquipGroupId", EquipGroup.EquipGroupId);
                Tools.DBHelper.ExecuteCommand(stringBuilder.ToString(), mySqlParameters);
                httpResult = HttpResult.GetJsonResult(true, "删除设备组成功", "删除设备组失败");
            }
            catch (Exception ex)
            {
                httpResult = HttpResult.GetJsonResult(false, string.Empty, ex.Message);
                Tools.Log.Add("删除设备组异常：" + ex.Message);
            }
            return httpResult;
        }
        /// <summary>
        /// 查询设备组列表
        /// </summary>
        /// <returns></returns>
        public HttpResult GetEquipGroupList()
        {
            HttpResult httpResult = new HttpResult();
            try
            {
                List<EquipGroup> EquipGroups = new List<EquipGroup>();
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append(@"SELECT * FROM EquipGroup");
                DataTable _ds = Tools.DBHelper.GetDataTable(stringBuilder.ToString());
                for (int i = 0; i < _ds.Rows.Count; i++)
                {
                    EquipGroup EquipGroup = new EquipGroup();
                    EquipGroup.EquipGroupName = _ds.Rows[i]["EquipGroupName"].ToString();
                    EquipGroup.EquipGroupId = (int)_ds.Rows[i]["EquipGroupId"];
                    EquipGroup.DataStatus = DataStatus.NONE;
                    EquipGroups.Add(EquipGroup);
                }
                httpResult = HttpResult.GetJsonResult(true, "查询设备组列表成功", string.Empty, EquipGroups);
            }
            catch (Exception ex)
            {
                Tools.Log.Add("查询设备组列表异常：" + ex.Message);
                httpResult = HttpResult.GetJsonResult(false, string.Empty, ex.Message);
            }
            return httpResult;
        }

        /// <summary>
        /// 查询设备组列表
        /// </summary>
        /// <returns></returns>
        public HttpResult GetProductionLineGroupList()
        {
            HttpResult httpResult = new HttpResult();
            try
            {
                List<Equip> Equips = new List<Equip>();
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append(@"SELECT * FROM Equip");
                DataTable _ds = Tools.DBHelper.GetDataTable(stringBuilder.ToString());
                for (int i = 0; i < _ds.Rows.Count; i++)
                {
                    Equip Equip = new Equip();
                    Equip.Id = (int)_ds.Rows[i]["Id"];
                    Equip.EquipName = _ds.Rows[i]["EquipName"].ToString();
                    Equip.EquipNO = _ds.Rows[i]["EquipNO"].ToString();
                    Equip.GatewayId = (int)_ds.Rows[i]["GatewayId"];
                    Equip.TypeBinding = (int)_ds.Rows[i]["TypeBinding"];
                    Equip.ProductionLineGroupBinding = _ds.Rows[i]["ProductionLineGroupBinding"].ToString();
                    Equip.DataStatus = DataStatus.NONE;
                    Equips.Add(Equip);
                }
                List<EquipGroup> EquipGroups = new List<EquipGroup>();
                StringBuilder stringBuilder1 = new StringBuilder();
                stringBuilder1.Append(@"SELECT * FROM EquipGroup");
                DataTable _ds1 = Tools.DBHelper.GetDataTable(stringBuilder1.ToString());
                for (int i = 0; i < _ds1.Rows.Count; i++)
                {
                    EquipGroup EquipGroup = new EquipGroup();
                    EquipGroup.EquipGroupName = _ds1.Rows[i]["EquipGroupName"].ToString();
                    EquipGroup.EquipGroupId = (int)_ds1.Rows[i]["EquipGroupId"];
                    EquipGroup.DataStatus = DataStatus.NONE;
                    EquipGroup.Equips = new ObservableCollection<Equip>(Equips.FindAll(n => n.ProductionLineGroupBinding == EquipGroup.EquipGroupId.ToString()));
                    EquipGroups.Add(EquipGroup);
                }

                httpResult = HttpResult.GetJsonResult(true, "查询产线组列表成功", string.Empty, EquipGroups);
            }
            catch (Exception ex)
            {
                Tools.Log.Add("查询产线组列表异常：" + ex.Message);
                httpResult = HttpResult.GetJsonResult(false, string.Empty, ex.Message);
            }
            return httpResult;
        }
    }
}
