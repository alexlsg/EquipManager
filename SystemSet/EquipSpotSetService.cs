using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Tools;

namespace SystemSet
{
    /// <summary>
    /// 设备测点配置业务类
    /// </summary>
    public class EquipSpotSetService
    {
        /// <summary>
        /// 添加设备测点功能
        /// </summary>
        /// <param name="EquipSpotSet">设备测点功能对象</param>
        /// <returns></returns>
        public HttpResult AddEquipSpotSet(EquipSpotSet EquipSpotSet)
        {
            HttpResult httpResult = new HttpResult();
            try
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append(@"
INSERT INTO EquipSpotSet(EquipId,DataType,SpotNO,SaveType,SpotNm,EquipType)
VALUES(@EquipId,@DataType,@SpotNO,@SaveType,@SpotNm,@EquipType)
");
                MySqlParameter[] mySqlParameters = new MySqlParameter[6];
                mySqlParameters[0] = new MySqlParameter("EquipId", EquipSpotSet.EquipId);
                mySqlParameters[1] = new MySqlParameter("DataType", EquipSpotSet.DataType);
                mySqlParameters[2] = new MySqlParameter("SpotNO", EquipSpotSet.SpotNO);
                mySqlParameters[3] = new MySqlParameter("SaveType", EquipSpotSet.SaveType);
                mySqlParameters[4] = new MySqlParameter("SpotNm", EquipSpotSet.SpotNm);
                mySqlParameters[5] = new MySqlParameter("EquipType", EquipSpotSet.EquipType);
                int _res = DBHelper.ExecuteCommand(stringBuilder.ToString(), mySqlParameters);
                httpResult = HttpResult.GetJsonResult(_res == 1, "添加设备测点功能成功", "添加设备测点功能失败");
            }
            catch (Exception ex)
            {
                httpResult = HttpResult.GetJsonResult(false, string.Empty, ex.Message);
                Tools.Log.Add("添加设备测点功能异常：" + ex.Message);
            }
            return httpResult;
        }

        /// <summary>
        /// 修改设备测点功能
        /// </summary>

        /// <param name="EquipSpotSet">设备测点功能对象</param>
        /// <returns></returns>
        public HttpResult ModEquipSpotSet(EquipSpotSet EquipSpotSet)
        {
            HttpResult httpResult = new HttpResult();
            try
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append(@"
UPDATE EquipSpotSet SET EquipId=@EquipId, DataType=@DataType, SpotNO=@SpotNO, SaveType=@SaveType,SpotNm=@SpotNm,EquipType=@EquipType
WHERE Id=@Id
");
                MySqlParameter[] mySqlParameters = new MySqlParameter[7];
                mySqlParameters[0] = new MySqlParameter("EquipId", EquipSpotSet.EquipId);
                mySqlParameters[1] = new MySqlParameter("DataType", EquipSpotSet.DataType);
                mySqlParameters[2] = new MySqlParameter("SpotNO", EquipSpotSet.SpotNO);
                mySqlParameters[3] = new MySqlParameter("SaveType", EquipSpotSet.SaveType);
                mySqlParameters[4] = new MySqlParameter("Id", EquipSpotSet.Id);
                mySqlParameters[5] = new MySqlParameter("SpotNm", EquipSpotSet.SpotNm);
                mySqlParameters[6] = new MySqlParameter("EquipType", EquipSpotSet.EquipType);
                int _res = DBHelper.ExecuteCommand(stringBuilder.ToString(), mySqlParameters);
                httpResult = HttpResult.GetJsonResult(_res == 1, "修改设备测点功能成功", "修改设备测点功能失败");
            }
            catch (Exception ex)
            {
                httpResult = HttpResult.GetJsonResult(false, string.Empty, ex.Message);
                Tools.Log.Add("修改设备测点功能异常：" + ex.Message);
            }
            return httpResult;
        }

        /// <summary>
        /// 删除设备测点功能
        /// </summary>

        /// <param name="EquipSpotSet">设备测点功能对象</param>
        /// <returns></returns>
        public HttpResult DelEquipSpotSet(EquipSpotSet EquipSpotSet)
        {
            HttpResult httpResult = new HttpResult();
            try
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append(@"
DELETE From EquipSpotSet WHERE Id=@Id
");
                MySqlParameter[] mySqlParameters = new MySqlParameter[1];
                mySqlParameters[0] = new MySqlParameter("Id", EquipSpotSet.Id);
                int _res = DBHelper.ExecuteCommand(stringBuilder.ToString(), mySqlParameters);
                httpResult = HttpResult.GetJsonResult(_res == 1, "删除设备测点功能成功", "删除设备测点功能失败");
            }
            catch (Exception ex)
            {
                httpResult = HttpResult.GetJsonResult(false, string.Empty, ex.Message);
                Tools.Log.Add("删除设备测点功能异常：" + ex.Message);
            }
            return httpResult;
        }
        /// <summary>
        /// 查询设备测点功能列表
        /// </summary>
        
        /// <returns></returns>
        public HttpResult GetEquipSpotSetList()
        {

            HttpResult httpResult = new HttpResult();
            try
            {
                List<EquipSpotSet> EquipSpotSets = new List<EquipSpotSet>();
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append(@"SELECT * FROM EquipSpotSet");
                DataTable _ds = DBHelper.GetDataTable(stringBuilder.ToString());
                for (int i = 0; i < _ds.Rows.Count; i++)
                {
                    EquipSpotSet EquipSpotSet = new EquipSpotSet();
                    EquipSpotSet.Id = (int)_ds.Rows[i]["Id"];
                    EquipSpotSet.EquipId = (int)_ds.Rows[i]["EquipId"];
                    EquipSpotSet.DataType = _ds.Rows[i]["DataType"].ToString();
                    EquipSpotSet.EquipType = _ds.Rows[i].Field<int?>("EquipType");
                    EquipSpotSet.SpotNO = _ds.Rows[i]["SpotNO"].ToString();
                    EquipSpotSet.SpotNm = _ds.Rows[i]["SpotNm"].ToString();
                    EquipSpotSet.SaveType = (int)_ds.Rows[i]["SaveType"];
                    EquipSpotSet.DataStatus = DataStatus.NONE;
                    EquipSpotSets.Add(EquipSpotSet);
                }
                httpResult = HttpResult.GetJsonResult(true, "查询设备测点功能列表成功", "查询设备测点功能列表失败", EquipSpotSets);
            }
            catch (Exception ex)
            {
                httpResult = HttpResult.GetJsonResult(false, string.Empty, ex.Message);
                Tools.Log.Add("查询设备测点功能列表异常：" + ex.Message);
            }
            return httpResult;
        }

        /// <summary>
        /// 根据EquipId查询设备测点功能列表
        /// </summary>
        /// <returns></returns>
        public HttpResult GetEquipSpotSetListByEquipId(int equipId)
        {

            HttpResult httpResult = new HttpResult();
            try
            {
                List<EquipSpotSet> EquipSpotSets = new List<EquipSpotSet>();
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append(@"SELECT * FROM EquipSpotSet WHERE EquipId=@EquipId");
                MySqlParameter[] mySqlParameters = new MySqlParameter[1];
                mySqlParameters[0] = new MySqlParameter("EquipId", equipId);
                DataTable _ds = DBHelper.GetDataTable(stringBuilder.ToString(), mySqlParameters);
                for (int i = 0; i < _ds.Rows.Count; i++)
                {
                    EquipSpotSet EquipSpotSet = new EquipSpotSet();
                    EquipSpotSet.Id = (int)_ds.Rows[i]["Id"];
                    EquipSpotSet.EquipId = (int)_ds.Rows[i]["EquipId"];
                    EquipSpotSet.DataType = _ds.Rows[i]["DataType"].ToString();
                    EquipSpotSet.SpotNO = _ds.Rows[i]["SpotNO"].ToString();
                    EquipSpotSet.SpotNm = _ds.Rows[i]["SpotNm"].ToString();
                    EquipSpotSet.SaveType = (int)_ds.Rows[i]["SaveType"];
                    EquipSpotSet.DataStatus = DataStatus.NONE;
                    EquipSpotSets.Add(EquipSpotSet);
                }
                httpResult = HttpResult.GetJsonResult(true, "查询设备测点功能列表成功", "查询设备测点功能列表失败", EquipSpotSets);
            }
            catch (Exception ex)
            {
                httpResult = HttpResult.GetJsonResult(false, string.Empty, ex.Message);
                Tools.Log.Add("查询设备测点功能列表异常：" + ex.Message);
            }
            return httpResult;
        }
    }
}
