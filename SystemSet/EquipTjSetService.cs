using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Tools;

namespace SystemSet
{
    /// <summary>
    /// 统计配置业务类
    /// </summary>
    public class EquipTjSetService
    {
        /// <summary>
        /// 添加统计配置
        /// </summary>
        /// <param name="EquipTjSet">统计配置对象</param>
        /// <returns></returns>
        public HttpResult AddEquipTjSet(EquipTjSet EquipTjSet)
        {
            HttpResult httpResult = new HttpResult();
            try
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append(@"
INSERT INTO EquipTjSet(EquipType,DataType,SpotNO,Tjlx,Tjzt,Data,Tjtj,Cyhj,Cyzshj)
VALUES(@EquipType,@DataType,@SpotNO,@Tjlx,@Tjzt,@Data,@Tjtj,@Cyhj,@Cyzshj)
");
                MySqlParameter[] mySqlParameters = new MySqlParameter[9];
                mySqlParameters[0] = new MySqlParameter("EquipType", EquipTjSet.EquipType);
                mySqlParameters[1] = new MySqlParameter("DataType", EquipTjSet.DataType);
                mySqlParameters[2] = new MySqlParameter("SpotNO", EquipTjSet.SpotNO);
                mySqlParameters[3] = new MySqlParameter("Tjlx", EquipTjSet.Tjlx);
                mySqlParameters[4] = new MySqlParameter("Tjzt", EquipTjSet.Tjzt);
                mySqlParameters[5] = new MySqlParameter("Data", EquipTjSet.Data);
                mySqlParameters[6] = new MySqlParameter("Tjtj", EquipTjSet.Tjtj);
                mySqlParameters[7] = new MySqlParameter("Cyhj", EquipTjSet.Cyhj);
                mySqlParameters[8] = new MySqlParameter("Cyzshj", EquipTjSet.Cyzshj);
                int _res = Tools.DBHelper.ExecuteCommand(stringBuilder.ToString(), mySqlParameters);
                httpResult = HttpResult.GetJsonResult(_res == 1, "添加统计配置成功", "添加统计配置失败");
            }
            catch (Exception ex)
            {
                Tools.Log.Add("添加统计配置异常：" + ex.Message);
            }
            return httpResult;
        }

        /// <summary>
        /// 修改统计配置
        /// </summary>
        /// <param name="EquipTjSet">统计配置对象</param>
        /// <returns></returns>
        public HttpResult ModEquipTjSet(EquipTjSet EquipTjSet)
        {
            HttpResult httpResult = new HttpResult();
            try
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append(@"
UPDATE EquipTjSet SET EquipType=@EquipType,DataType=@DataType,SpotNO=@SpotNO,Tjlx=@Tjlx,Tjzt=@Tjzt,Data=@Data,Tjtj=@Tjtj,Cyhj=@Cyhj,Cyzshj=@Cyzshj
WHERE Id=@Id
");
                MySqlParameter[] mySqlParameters = new MySqlParameter[10];
                mySqlParameters[0] = new MySqlParameter("EquipType", EquipTjSet.EquipType);
                mySqlParameters[1] = new MySqlParameter("DataType", EquipTjSet.DataType);
                mySqlParameters[2] = new MySqlParameter("SpotNO", EquipTjSet.SpotNO);
                mySqlParameters[3] = new MySqlParameter("Tjlx", EquipTjSet.Tjlx);
                mySqlParameters[4] = new MySqlParameter("Tjzt", EquipTjSet.Tjzt);
                mySqlParameters[5] = new MySqlParameter("Data", EquipTjSet.Data);
                mySqlParameters[6] = new MySqlParameter("Tjtj", EquipTjSet.Tjtj);
                mySqlParameters[7] = new MySqlParameter("Cyhj", EquipTjSet.Cyhj);
                mySqlParameters[8] = new MySqlParameter("Cyzshj", EquipTjSet.Cyzshj);
                mySqlParameters[9] = new MySqlParameter("Id", EquipTjSet.Id);
                int _res = Tools.DBHelper.ExecuteCommand(stringBuilder.ToString(), mySqlParameters);
                httpResult = HttpResult.GetJsonResult(_res == 1, "修改统计配置成功", "修改统计配置失败");
            }
            catch (Exception ex)
            {
                Tools.Log.Add("修改统计配置异常：" + ex.Message);
            }
            return httpResult;
        }

        /// <summary>
        /// 删除统计配置
        /// </summary>
        /// <param name="EquipTjSet">统计配置对象</param>
        /// <returns></returns>
        public HttpResult DelEquipTjSet(EquipTjSet EquipTjSet)
        {
            HttpResult httpResult = new HttpResult();
            try
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append(@"
DELETE From EquipTjSet WHERE Id=@Id
");
                MySqlParameter[] mySqlParameters = new MySqlParameter[1];
                mySqlParameters[0] = new MySqlParameter("Id", EquipTjSet.Id);
                int _res = Tools.DBHelper.ExecuteCommand(stringBuilder.ToString(), mySqlParameters);
                httpResult = HttpResult.GetJsonResult(_res == 1, "删除统计配置成功", "删除统计配置失败");
            }
            catch (Exception ex)
            {
                Tools.Log.Add("删除统计配置异常：" + ex.Message);
            }
            return httpResult;
        }
        /// <summary>
        /// 查询统计配置列表
        /// </summary>
        /// <returns></returns>
        public HttpResult GetEquipTjSetList()
        {
            HttpResult httpResult = new HttpResult();
            try
            {
                List<EquipTjSet> EquipTjSets = new List<EquipTjSet>();
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append(@"SELECT * FROM EquipTjSet");
                DataTable _ds = Tools.DBHelper.GetDataTable(stringBuilder.ToString());
                for (int i = 0; i < _ds.Rows.Count; i++)
                {
                    EquipTjSet EquipTjSet = new EquipTjSet();
                    EquipTjSet.EquipType = (int)_ds.Rows[i]["EquipType"];
                    EquipTjSet.Id = (int)_ds.Rows[i]["Id"];
                    EquipTjSet.DataType = _ds.Rows[i]["DataType"].ToString();
                    EquipTjSet.SpotNO = _ds.Rows[i]["SpotNO"].ToString();
                    EquipTjSet.Tjlx = _ds.Rows[i]["Tjlx"].ToString();
                    EquipTjSet.Tjzt = _ds.Rows[i]["Tjzt"].ToString();
                    EquipTjSet.Data = _ds.Rows[i]["Data"].ToString();
                    EquipTjSet.Tjtj = _ds.Rows[i]["Tjtj"].ToString();
                    EquipTjSet.Cyhj = Convert.ToBoolean(_ds.Rows[i]["Cyhj"]);
                    EquipTjSet.Cyzshj = Convert.ToBoolean(_ds.Rows[i]["Cyzshj"]);
                    EquipTjSet.DataStatus = DataStatus.NONE;
                    EquipTjSets.Add(EquipTjSet);
                }
                httpResult = HttpResult.GetJsonResult(true, "查询统计配置列表成功", string.Empty, EquipTjSets);
            }
            catch (Exception ex)
            {
                Tools.Log.Add("查询统计配置列表异常：" + ex.Message);
                httpResult = HttpResult.GetJsonResult(false, string.Empty, ex.Message);
            }
            return httpResult;
        }
    }
}
