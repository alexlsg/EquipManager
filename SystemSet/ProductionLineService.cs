using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Tools;

namespace SystemSet
{
    /// <summary>
    /// 产线业务类
    /// </summary>
    public class ProductionLineService
    {
        /// <summary>
        /// 添加产线
        /// </summary>
        /// <param name="ProductionLine">产线对象</param>
        /// <returns></returns>
        public HttpResult AddProductionLine(ProductionLine ProductionLine)
        {
            HttpResult httpResult = new HttpResult();
            try
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append(@"
INSERT INTO ProductionLine(ProductionLineName)
VALUES(@ProductionLineName)
");
                MySqlParameter[] mySqlParameters = new MySqlParameter[1];
                mySqlParameters[0] = new MySqlParameter("ProductionLineName", ProductionLine.ProductionLineName);
                int _res = Tools.DBHelper.ExecuteCommand(stringBuilder.ToString(), mySqlParameters);
                httpResult = HttpResult.GetJsonResult(_res == 1, "添加产线成功", "添加产线失败");
            }
            catch (Exception ex)
            {
                Tools.Log.Add("添加产线异常：" + ex.Message);
            }
            return httpResult;
        }

        /// <summary>
        /// 修改产线
        /// </summary>
        /// <param name="ProductionLine">产线对象</param>
        /// <returns></returns>
        public HttpResult ModProductionLine(ProductionLine ProductionLine)
        {
            HttpResult httpResult = new HttpResult();
            try
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append(@"
UPDATE ProductionLine SET ProductionLineName=@ProductionLineName
WHERE ProductionLineId=@ProductionLineId
");
                MySqlParameter[] mySqlParameters = new MySqlParameter[2];
                mySqlParameters[0] = new MySqlParameter("ProductionLineName", ProductionLine.ProductionLineName);
                mySqlParameters[1] = new MySqlParameter("ProductionLineId", ProductionLine.ProductionLineId);
                int _res = Tools.DBHelper.ExecuteCommand(stringBuilder.ToString(), mySqlParameters);
                httpResult = HttpResult.GetJsonResult(_res == 1, "修改产线成功", "修改产线失败");
            }
            catch (Exception ex)
            {
                Tools.Log.Add("修改产线异常：" + ex.Message);
            }
            return httpResult;
        }

        /// <summary>
        /// 删除产线
        /// </summary>
        /// <param name="ProductionLine">产线对象</param>
        /// <returns></returns>
        public HttpResult DelProductionLine(ProductionLine ProductionLine)
        {
            HttpResult httpResult = new HttpResult();
            try
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append(@"
DELETE From ProductionLine WHERE ProductionLineId=@ProductionLineId
");
                StringBuilder stringBuilder1 = new StringBuilder();
                stringBuilder1.Append(@"
DELETE From ProductionLineGroup WHERE ProductionLineId=@ProductionLineId
");
                MySqlParameter[] mySqlParameters = new MySqlParameter[1];
                mySqlParameters[0] = new MySqlParameter("ProductionLineId", ProductionLine.ProductionLineId);
                Tools.DBHelper.ExecuteCommand(stringBuilder.ToString(), mySqlParameters);
                Tools.DBHelper.ExecuteCommand(stringBuilder1.ToString(), mySqlParameters);
                httpResult = HttpResult.GetJsonResult(true, "删除产线成功", "删除产线失败");
            }
            catch (Exception ex)
            {
                Tools.Log.Add("删除产线异常：" + ex.Message);
            }
            return httpResult;
        }
        /// <summary>
        /// 查询产线列表
        /// </summary>
        /// <returns></returns>
        public HttpResult GetProductionLineList()
        {
            HttpResult httpResult = new HttpResult();
            try
            {
                List<ProductionLine> ProductionLines = new List<ProductionLine>();
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append(@"SELECT * FROM ProductionLine");
                DataTable _ds = Tools.DBHelper.GetDataTable(stringBuilder.ToString());
                for (int i = 0; i < _ds.Rows.Count; i++)
                {
                    ProductionLine ProductionLine = new ProductionLine();
                    ProductionLine.ProductionLineName = _ds.Rows[i]["ProductionLineName"].ToString();
                    ProductionLine.ProductionLineId = (int)_ds.Rows[i]["ProductionLineId"];
                    ProductionLine.DataStatus = DataStatus.NONE;
                    ProductionLines.Add(ProductionLine);
                }
                httpResult = HttpResult.GetJsonResult(true, "查询产线列表成功", string.Empty, ProductionLines);
            }
            catch (Exception ex)
            {
                Tools.Log.Add("查询产线列表异常：" + ex.Message);
                httpResult = HttpResult.GetJsonResult(false, string.Empty, ex.Message);
            }
            return httpResult;
        }
    }
}
