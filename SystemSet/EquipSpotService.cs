using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Tools;

namespace SystemSet
{
    /// <summary>
    /// 设备测点业务类
    /// </summary>
    public class EquipSpotService
    {
        /// <summary>
        /// 根据设备编号查询设备测点功能列表
        /// </summary>
        /// <returns></returns>
        public HttpResult GetEquipListByEquipNO(string equipNO)
        {
            HttpResult httpResult = new HttpResult();
            try
            {
                List<EquipSpot> EquipSpots = new List<EquipSpot>();
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append(@"SELECT * FROM EquipSpot WHERE EquipNO=@EquipNO");
                MySqlParameter[] mySqlParameters = new MySqlParameter[1];
                mySqlParameters[0] = new MySqlParameter("EquipNO", equipNO);
                DataTable _ds = Tools.DBHelper.GetDataTable(stringBuilder.ToString(), mySqlParameters);
                for (int i = 0; i < _ds.Rows.Count; i++)
                {
                    EquipSpot EquipSpot = new EquipSpot();
                    EquipSpot.Id = (int)_ds.Rows[i]["Id"];
                    EquipSpot.State = (int)_ds.Rows[i]["State"];
                    EquipSpot.NoState = (int)_ds.Rows[i]["NoState"];
                    EquipSpot.EquipNo = _ds.Rows[i]["EquipNO"].ToString();
                    EquipSpot.Type = _ds.Rows[i]["Type"].ToString();
                    EquipSpot.SpotNo = _ds.Rows[i]["SpotNo"].ToString();
                    EquipSpot.Unit = _ds.Rows[i]["Unit"].ToString();
                    EquipSpot.Value = _ds.Rows[i]["Value"].ToString();
                    EquipSpot.SpotNm = _ds.Rows[i]["SpotNm"].ToString();
                    EquipSpot.DataStatus = DataStatus.NONE;
                    EquipSpots.Add(EquipSpot);
                }
                httpResult = HttpResult.GetJsonResult(true, "查询设备测点功能列表成功", string.Empty, EquipSpots);
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
