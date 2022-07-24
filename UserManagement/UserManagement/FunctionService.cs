using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Tools;

namespace UserManagement
{
    /// <summary>
    /// 功能业务类
    /// </summary>
    public class FunctionService
    {
        /// <summary>
        /// 添加功能
        /// </summary>
        /// <param name="Function">功能对象</param>
        /// <returns></returns>
        public HttpResult AddFunction(Function Function)
        {
            HttpResult httpResult = new HttpResult();
            try
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append(@"
INSERT INTO functiona(FunctionName)
VALUES(@FunctionName)
");
                MySqlParameter[] mySqlParameters = new MySqlParameter[1];
                mySqlParameters[0] = new MySqlParameter("FunctionName", Function.FunctionName);
                int _res = Tools.DBHelper.ExecuteCommand(stringBuilder.ToString(), mySqlParameters);
                httpResult = HttpResult.GetJsonResult(_res == 1, "添加功能成功", "添加功能失败");
            }
            catch (Exception ex)
            {
                httpResult = HttpResult.GetJsonResult(false, string.Empty, ex.Message);
                Tools.Log.Add("添加功能异常：" + ex.Message);
            }
            return httpResult;
        }

        /// <summary>
        /// 修改功能
        /// </summary>
        /// <param name="Function">功能对象</param>
        /// <returns></returns>
        public HttpResult ModFunction(Function Function)
        {
            HttpResult httpResult = new HttpResult();
            try
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append(@"
UPDATE functiona SET FunctionName=@FunctionName
WHERE FunctionId=@FunctionId
");
                MySqlParameter[] mySqlParameters = new MySqlParameter[2];
                mySqlParameters[0] = new MySqlParameter("FunctionName", Function.FunctionName);
                mySqlParameters[1] = new MySqlParameter("FunctionId", Function.FunctionId);
                int _res = Tools.DBHelper.ExecuteCommand(stringBuilder.ToString(), mySqlParameters);
                httpResult = HttpResult.GetJsonResult(_res == 1, "修改功能成功", "修改功能失败");
            }
            catch (Exception ex)
            {
                httpResult = HttpResult.GetJsonResult(false, string.Empty, ex.Message);
                Tools.Log.Add("修改功能异常：" + ex.Message);
            }
            return httpResult;
        }

        /// <summary>
        /// 删除功能
        /// </summary>
        /// <param name="Function">功能对象</param>
        /// <returns></returns>
        public HttpResult DelFunction(Function Function)
        {
            HttpResult httpResult = new HttpResult();
            try
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append(@"
DELETE functiona WHERE FunctionId=@FunctionId
");
                MySqlParameter[] mySqlParameters = new MySqlParameter[1];
                mySqlParameters[0] = new MySqlParameter("FunctionId", Function.FunctionId);
                int _res = Tools.DBHelper.ExecuteCommand(stringBuilder.ToString(), mySqlParameters);
                httpResult = HttpResult.GetJsonResult(_res == 1, "删除功能成功", "删除功能失败");
            }
            catch (Exception ex)
            {
                httpResult = HttpResult.GetJsonResult(false, string.Empty, ex.Message);
                Tools.Log.Add("删除功能异常：" + ex.Message);
            }
            return httpResult;
        }
        /// <summary>
        /// 查询功能列表
        /// </summary>
        /// <returns></returns>
        public HttpResult GetFunctionList(string text)
        {
            HttpResult httpResult = new HttpResult();
            try
            {
                List<Function> Functions = new List<Function>();
                StringBuilder stringBuilder = new StringBuilder();
                if (string.IsNullOrWhiteSpace(text))
                {
                    stringBuilder.Append(@"SELECT * FROM functiona");
                }
                else
                {
                    stringBuilder.Append(@"SELECT * FROM functiona WHERE FunctionName like '%" + text + "%'");
                }
                DataTable _ds = Tools.DBHelper.GetDataTable(stringBuilder.ToString());
                for (int i = 0; i < _ds.Rows.Count; i++)
                {
                    Function Function = new Function();
                    Function.FunctionName = _ds.Rows[i]["FunctionName"].ToString();
                    Function.FunctionId = (int)_ds.Rows[i]["FunctionId"];
                    Functions.Add(Function);
                }
                httpResult = HttpResult.GetJsonResult(true, "查询功能列表成功", string.Empty, Functions);
            }
            catch (Exception ex)
            {
                Tools.Log.Add("查询功能列表异常：" + ex.Message);
                httpResult = HttpResult.GetJsonResult(false, string.Empty, ex.Message);
            }
            return httpResult;
        }

    }
}
