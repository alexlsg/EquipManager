
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Tools;

namespace SystemSet
{
    /// <summary>
    /// 网关业务类
    /// </summary>
    public class GatewayService
    {
        /// <summary>
        /// 添加网关功能
        /// </summary>
        /// <param name="Gateway">网关功能对象</param>
        /// <returns></returns>
        public HttpResult AddGateway(Gateway Gateway)
        {
            HttpResult httpResult = new HttpResult();
            try
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append(@"
INSERT INTO Gateway(Name,IP,PORT)
VALUES(@Name,@IP,@PORT)
");
                MySqlParameter[] mySqlParameters = new MySqlParameter[3];
                mySqlParameters[0] = new MySqlParameter("Name", Gateway.Name);
                mySqlParameters[1] = new MySqlParameter("IP", Gateway.IP);
                mySqlParameters[2] = new MySqlParameter("PORT", Gateway.PORT);
                int _res = Tools.DBHelper.ExecuteCommand(stringBuilder.ToString(), mySqlParameters);
                httpResult = HttpResult.GetJsonResult(_res == 1, "添加网关功能成功", "添加网关功能失败");
            }
            catch (Exception ex)
            {
                httpResult = HttpResult.GetJsonResult(false, string.Empty, ex.Message);
                Tools.Log.Add("添加网关功能异常：" + ex.Message);
            }
            return httpResult;
        }

        /// <summary>
        /// 修改网关功能
        /// </summary>

        /// <param name="Gateway">网关功能对象</param>
        /// <returns></returns>
        public HttpResult ModGateway(Gateway Gateway)
        {
            HttpResult httpResult = new HttpResult();
            try
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append(@"
UPDATE Gateway SET Name=@Name,IP=@IP,PORT=@PORT
WHERE Id=@Id
");
                MySqlParameter[] mySqlParameters = new MySqlParameter[4];
                mySqlParameters[0] = new MySqlParameter("Name", Gateway.Name);
                mySqlParameters[1] = new MySqlParameter("IP", Gateway.IP);
                mySqlParameters[2] = new MySqlParameter("PORT", Gateway.PORT);
                mySqlParameters[3] = new MySqlParameter("Id", Gateway.Id);
                int _res = Tools.DBHelper.ExecuteCommand(stringBuilder.ToString(), mySqlParameters);
                httpResult = HttpResult.GetJsonResult(_res == 1, "修改网关功能成功", "修改网关功能失败");
            }
            catch (Exception ex)
            {
                httpResult = HttpResult.GetJsonResult(false, string.Empty, ex.Message);
                Tools.Log.Add("修改网关功能异常：" + ex.Message);
            }
            return httpResult;
        }

        /// <summary>
        /// 删除网关功能
        /// </summary>

        /// <param name="Gateway">网关功能对象</param>
        /// <returns></returns>
        public HttpResult DelGateway(Gateway Gateway)
        {
            HttpResult httpResult = new HttpResult();
            try
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append(@"
DELETE From Gateway WHERE Id=@Id;
DELETE From Equip WHERE GatewayId=@Id
");
                MySqlParameter[] mySqlParameters = new MySqlParameter[1];
                mySqlParameters[0] = new MySqlParameter("Id", Gateway.Id);
                Tools.DBHelper.ExecuteCommand(stringBuilder.ToString(), mySqlParameters);
                httpResult = HttpResult.GetJsonResult(true, "删除网关功能成功", "删除网关功能失败");
            }
            catch (Exception ex)
            {
                httpResult = HttpResult.GetJsonResult(false, string.Empty, ex.Message);
                Tools.Log.Add("删除网关功能异常：" + ex.Message);
            }
            return httpResult;
        }
        /// <summary>
        /// 查询网关功能列表
        /// </summary>
        /// <returns></returns>
        public HttpResult GetGatewayList()
        {
            HttpResult httpResult = new HttpResult();
            try
            {
                List<Gateway> Gateways = new List<Gateway>();
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append(@"SELECT * FROM Gateway");
                DataTable _ds = Tools.DBHelper.GetDataTable(stringBuilder.ToString());
                for (int i = 0; i < _ds.Rows.Count; i++)
                {
                    Gateway Gateway = new Gateway();
                    Gateway.Id = (int)_ds.Rows[i]["Id"];
                    Gateway.Name = _ds.Rows[i]["Name"].ToString();
                    Gateway.IP = _ds.Rows[i]["IP"].ToString();
                    Gateway.PORT = _ds.Rows[i]["PORT"].ToString();
                    Gateway.DataStatus = DataStatus.NONE;
                    Gateways.Add(Gateway);
                }
                httpResult = HttpResult.GetJsonResult(true, "查询网关列表成功", "查询网关列表失败", Gateways);
            }
            catch (Exception ex)
            {
                httpResult = HttpResult.GetJsonResult(false, string.Empty, ex.Message);
                Tools.Log.Add("查询网关列表异常：" + ex.Message);
            }
            return httpResult;
        }
        /// <summary>
        /// 根据EquipGroupId查询网关功能列表
        /// </summary>
        /// <returns></returns>
        public HttpResult GetGatewayListByEquipGroupId(int equipGroupId)
        {
            HttpResult httpResult = new HttpResult();
            try
            {
                List<Gateway> Gateways = new List<Gateway>();
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append(@"SELECT * FROM Gateway WHERE EquipGroupId=@EquipGroupId");
                MySqlParameter[] mySqlParameters = new MySqlParameter[1];
                mySqlParameters[0] = new MySqlParameter("EquipGroupId", equipGroupId);
                DataTable _ds = Tools.DBHelper.GetDataTable(stringBuilder.ToString(), mySqlParameters);
                for (int i = 0; i < _ds.Rows.Count; i++)
                {
                    Gateway Gateway = new Gateway();
                    Gateway.Id = (int)_ds.Rows[i]["Id"];
                    Gateway.Name = _ds.Rows[i]["Name"].ToString();
                    Gateway.IP = _ds.Rows[i]["IP"].ToString();
                    Gateway.PORT = _ds.Rows[i]["PORT"].ToString();
                    Gateway.DataStatus = DataStatus.NONE;
                    Gateways.Add(Gateway);
                }
                httpResult = HttpResult.GetJsonResult(true, "查询网关列表成功", "查询网关列表失败", Gateways);
            }
            catch (Exception ex)
            {
                httpResult = HttpResult.GetJsonResult(false, string.Empty, ex.Message);
                Tools.Log.Add("查询网关列表异常：" + ex.Message);
            }
            return httpResult;
        }
    }
}
