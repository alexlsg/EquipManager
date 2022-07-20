using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Tools;

namespace SystemSet
{
    /// <summary>
    /// 产线组组业务类
    /// </summary>
    public class ProductionLineGroupService
    {
        /// <summary>
        /// 添加产线组
        /// </summary>
        /// <param name="ProductionLineGroup">产线组对象</param>
        /// <returns></returns>
        public HttpResult AddProductionLineGroup(ProductionLineGroup ProductionLineGroup)
        {
            HttpResult httpResult = new HttpResult();
            try
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append(@"
INSERT INTO ProductionLineGroup(ProductionLineId,EquipNO)
VALUES(@ProductionLineId,@EquipNO)
");
                MySqlParameter[] mySqlParameters = new MySqlParameter[2];
                mySqlParameters[0] = new MySqlParameter("ProductionLineId", ProductionLineGroup.ProductionLineId);
                mySqlParameters[1] = new MySqlParameter("EquipNO", ProductionLineGroup.EquipNO);
                int _res = Tools.DBHelper.ExecuteCommand(stringBuilder.ToString(), mySqlParameters);
                httpResult = HttpResult.GetJsonResult(_res == 1, "添加产线组成功", "添加产线组失败");
            }
            catch (Exception ex)
            {
                httpResult = HttpResult.GetJsonResult(false, string.Empty, ex.Message);
                Tools.Log.Add("添加产线组异常：" + ex.Message);
            }
            return httpResult;
        }

        /// <summary>
        /// 修改产线组
        /// </summary>
        /// <param name="ProductionLineGroup">产线组对象</param>
        /// <returns></returns>
        public HttpResult ModProductionLineGroup(ProductionLineGroup ProductionLineGroup)
        {
            HttpResult httpResult = new HttpResult();
            try
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append(@"
UPDATE ProductionLineGroup SET ProductionLineId=@ProductionLineId, EquipNO=@EquipNO
WHERE ProductionLineGroupId=@ProductionLineGroupId
");
                MySqlParameter[] mySqlParameters = new MySqlParameter[3];
                mySqlParameters[0] = new MySqlParameter("ProductionLineId", ProductionLineGroup.ProductionLineId);
                mySqlParameters[1] = new MySqlParameter("EquipNO", ProductionLineGroup.EquipNO);
                mySqlParameters[2] = new MySqlParameter("ProductionLineGroupId", ProductionLineGroup.ProductionLineGroupId);
                int _res = Tools.DBHelper.ExecuteCommand(stringBuilder.ToString(), mySqlParameters);
                httpResult = HttpResult.GetJsonResult(_res == 1, "修改产线组成功", "修改产线组失败");
            }
            catch (Exception ex)
            {
                httpResult = HttpResult.GetJsonResult(false, string.Empty, ex.Message);
                Tools.Log.Add("修改产线组异常：" + ex.Message);
            }
            return httpResult;
        }

        /// <summary>
        /// 删除产线组
        /// </summary>
        /// <param name="ProductionLineGroup">产线组对象</param>
        /// <returns></returns>
        public HttpResult DelProductionLineGroup(int productionLineId)
        {
            HttpResult httpResult = new HttpResult();
            try
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append(@"
DELETE From ProductionLineGroup WHERE ProductionLineId=@ProductionLineId
");
                MySqlParameter[] mySqlParameters = new MySqlParameter[1];
                mySqlParameters[0] = new MySqlParameter("ProductionLineId", productionLineId);
                int _res = Tools.DBHelper.ExecuteCommand(stringBuilder.ToString(), mySqlParameters);
                httpResult = HttpResult.GetJsonResult(_res == 1, "删除产线组成功", "删除产线组失败");
            }
            catch (Exception ex)
            {
                httpResult = HttpResult.GetJsonResult(false, string.Empty, ex.Message);
                Tools.Log.Add("删除产线组异常：" + ex.Message);
            }
            return httpResult;
        }
        /// <summary>
        /// 查询产线组列表
        /// </summary>
        /// <returns></returns>
        public HttpResult GetProductionLineGroupList()
        {
            HttpResult httpResult = new HttpResult();
            try
            {
                List<ProductionLineGroup> ProductionLineGroups = new List<ProductionLineGroup>();
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append(@"SELECT * FROM ProductionLineGroup");
                DataTable _ds = Tools.DBHelper.GetDataTable(stringBuilder.ToString());
                for (int i = 0; i < _ds.Rows.Count; i++)
                {
                    ProductionLineGroup ProductionLineGroup = new ProductionLineGroup();
                    ProductionLineGroup.EquipNO = _ds.Rows[i]["EquipNO"].ToString();
                    ProductionLineGroup.ProductionLineId = (int)_ds.Rows[i]["ProductionLineId"];
                    ProductionLineGroup.ProductionLineGroupId = (int)_ds.Rows[i]["ProductionLineGroupId"];
                    ProductionLineGroup.DataStatus = DataStatus.NONE;
                    ProductionLineGroups.Add(ProductionLineGroup);
                }
                httpResult = HttpResult.GetJsonResult(true, "查询产线组列表成功", string.Empty, ProductionLineGroups);
            }
            catch (Exception ex)
            {
                Tools.Log.Add("查询产线组列表异常：" + ex.Message);
                httpResult = HttpResult.GetJsonResult(false, string.Empty, ex.Message);
            }
            return httpResult;
        }
        /// <summary>
        /// 根据产线Id查询产线组设别类型列表
        /// </summary>
        /// <returns></returns>
        public HttpResult GetProductionLineGroupEquipTypeListByProductionLineId(string productionLineId)
        {
            HttpResult httpResult = new HttpResult();
            try
            {
                List<ProductionLineGroup> ProductionLineGroups = new List<ProductionLineGroup>();
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append(@"
SELECT DISTINCT A.*,C.* FROM ProductionLineGroup A 
JOIN Equip B ON A.EquipNO=B.EquipNO
JOIN EquipType C ON B.TypeBinding=C.EquipTypeId
WHERE ProductionLineId=@ProductionLineId");
                MySqlParameter[] mySqlParameters = new MySqlParameter[1];
                mySqlParameters[0] = new MySqlParameter("ProductionLineId", productionLineId);
                DataTable _ds = Tools.DBHelper.GetDataTable(stringBuilder.ToString(), mySqlParameters);
                for (int i = 0; i < _ds.Rows.Count; i++)
                {
                    ProductionLineGroup ProductionLineGroup = new ProductionLineGroup();
                    ProductionLineGroup.EquipNO = _ds.Rows[i]["EquipNO"].ToString();
                    ProductionLineGroup.ProductionLineId = (int)_ds.Rows[i]["ProductionLineId"];
                    ProductionLineGroup.ProductionLineGroupId = (int)_ds.Rows[i]["ProductionLineGroupId"];
                    ProductionLineGroup.EquipType = new EquipType();
                    ProductionLineGroup.EquipType.EquipTypeId = (int)_ds.Rows[i]["EquipTypeId"];
                    ProductionLineGroup.EquipType.EquipTypeName = _ds.Rows[i]["EquipTypeName"].ToString();
                    ProductionLineGroup.EquipType.DataStatus = DataStatus.NONE;
                    ProductionLineGroup.DataStatus = DataStatus.NONE;
                    ProductionLineGroups.Add(ProductionLineGroup);
                }
                httpResult = HttpResult.GetJsonResult(true, "查询产线组列表成功", string.Empty, ProductionLineGroups);
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
