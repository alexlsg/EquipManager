using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Tools;

namespace SystemSet
{
    /// <summary>
    /// 设备类型类业务类
    /// </summary>
    public class EquipTypeService
    {
        /// <summary>
        /// 添加设备类型
        /// </summary>
        /// <param name="EquipType">设备类型对象</param>
        /// <returns></returns>
        public HttpResult AddEquipType(EquipType EquipType)
        {
            HttpResult httpResult = new HttpResult();
            try
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append(@"
INSERT INTO EquipType(EquipTypeName)
VALUES(@EquipTypeName)
");
                MySqlParameter[] mySqlParameters = new MySqlParameter[1];
                mySqlParameters[0] = new MySqlParameter("EquipTypeName", EquipType.EquipTypeName);
                int _res = Tools.DBHelper.ExecuteCommand(stringBuilder.ToString(), mySqlParameters);
                httpResult = HttpResult.GetJsonResult(_res == 1, "添加设备类型成功", "添加设备类型失败");
            }
            catch (Exception ex)
            {
                httpResult = HttpResult.GetJsonResult(false, string.Empty, ex.Message);
                Tools.Log.Add("添加设备类型异常：" + ex.Message);
            }
            return httpResult;
        }

        /// <summary>
        /// 修改设备类型
        /// </summary>
        /// <param name="EquipType">设备类型对象</param>
        /// <returns></returns>
        public HttpResult ModEquipType(EquipType EquipType)
        {
            HttpResult httpResult = new HttpResult();
            try
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append(@"
UPDATE EquipType SET EquipTypeName=@EquipTypeName
WHERE EquipTypeId=@EquipTypeId
");
                MySqlParameter[] mySqlParameters = new MySqlParameter[2];
                mySqlParameters[0] = new MySqlParameter("EquipTypeName", EquipType.EquipTypeName);
                mySqlParameters[1] = new MySqlParameter("EquipTypeId", EquipType.EquipTypeId);
                int _res = Tools.DBHelper.ExecuteCommand(stringBuilder.ToString(), mySqlParameters);
                httpResult = HttpResult.GetJsonResult(_res == 1, "修改设备类型成功", "修改设备类型失败");
            }
            catch (Exception ex)
            {
                httpResult = HttpResult.GetJsonResult(false, string.Empty, ex.Message);
                Tools.Log.Add("修改设备类型异常：" + ex.Message);
            }
            return httpResult;
        }

        /// <summary>
        /// 删除设备类型
        /// </summary>
        /// <param name="EquipType">设备类型对象</param>
        /// <returns></returns>
        public HttpResult DelEquipType(EquipType EquipType)
        {
            HttpResult httpResult = new HttpResult();
            try
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append(@"
DELETE From EquipType WHERE EquipTypeId=@EquipTypeId
");
                MySqlParameter[] mySqlParameters = new MySqlParameter[1];
                mySqlParameters[0] = new MySqlParameter("EquipTypeId", EquipType.EquipTypeId);
                int _res = Tools.DBHelper.ExecuteCommand(stringBuilder.ToString(), mySqlParameters);
                httpResult = HttpResult.GetJsonResult(_res == 1, "删除设备类型成功", "删除设备类型失败");
            }
            catch (Exception ex)
            {
                httpResult = HttpResult.GetJsonResult(false, string.Empty, ex.Message);
                Tools.Log.Add("删除设备类型异常：" + ex.Message);
            }
            return httpResult;
        }
        /// <summary>
        /// 查询设备类型列表
        /// </summary>
        /// <returns></returns>
        public HttpResult GetEquipTypeList()
        {
            HttpResult httpResult = new HttpResult();
            try
            {
                List<EquipType> EquipTypes = new List<EquipType>();
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append(@"SELECT * FROM EquipType");
                DataTable _ds = Tools.DBHelper.GetDataTable(stringBuilder.ToString());
                for (int i = 0; i < _ds.Rows.Count; i++)
                {
                    EquipType EquipType = new EquipType();
                    EquipType.EquipTypeName = _ds.Rows[i]["EquipTypeName"].ToString();
                    EquipType.EquipTypeId = (int)_ds.Rows[i]["EquipTypeId"];
                    EquipType.DataStatus = DataStatus.NONE;
                    EquipTypes.Add(EquipType);
                }
                httpResult = HttpResult.GetJsonResult(true, "查询设备类型列表成功", string.Empty, EquipTypes);
            }
            catch (Exception ex)
            {
                Tools.Log.Add("查询设备类型列表异常：" + ex.Message);
                httpResult = HttpResult.GetJsonResult(false, string.Empty, ex.Message);
            }
            return httpResult;
        }
    }
}
