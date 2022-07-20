using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Tools;

namespace SystemSet
{
    /// <summary>
    /// 系统设置业务类
    /// </summary>
    public class SystemSettingService
    {
        /// <summary>
        /// 添加系统设置
        /// </summary>
        /// <param name="SystemSetting">系统设置对象</param>
        /// <returns></returns>
        public HttpResult AddSystemSetting(SystemSetting SystemSetting)
        {
            HttpResult httpResult = new HttpResult();
            try
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append(@"
INSERT INTO SystemSetting(Logo,Title)
VALUES(@Logo,@Title)
");
                MySqlParameter[] mySqlParameters = new MySqlParameter[2];
                mySqlParameters[0] = new MySqlParameter("Logo", SystemSetting.Logo);
                mySqlParameters[1] = new MySqlParameter("Title", SystemSetting.Title);
                int _res = Tools.DBHelper.ExecuteCommand(stringBuilder.ToString(), mySqlParameters);
                httpResult = HttpResult.GetJsonResult(_res == 1, "添加系统设置成功", "添加系统设置失败");
            }
            catch (Exception ex)
            {
                httpResult = HttpResult.GetJsonResult(false, string.Empty, ex.Message);
                Tools.Log.Add("添加系统设置异常：" + ex.Message);
            }
            return httpResult;
        }

        /// <summary>
        /// 修改系统设置
        /// </summary>

        /// <param name="SystemSetting">系统设置对象</param>
        /// <returns></returns>
        public HttpResult ModSystemSetting(SystemSetting SystemSetting)
        {
            HttpResult httpResult = new HttpResult();
            try
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append(@"
UPDATE SystemSetting SET Logo=@Logo, Title=@Title
WHERE Id=@Id
");
                MySqlParameter[] mySqlParameters = new MySqlParameter[3];
                mySqlParameters[0] = new MySqlParameter("Logo", SystemSetting.Logo);
                mySqlParameters[1] = new MySqlParameter("Title", SystemSetting.Title);
                mySqlParameters[2] = new MySqlParameter("Id", SystemSetting.Id);
                int _res = Tools.DBHelper.ExecuteCommand(stringBuilder.ToString(), mySqlParameters);
                httpResult = HttpResult.GetJsonResult(_res == 1, "修改系统设置成功", "修改系统设置失败");
            }
            catch (Exception ex)
            {
                httpResult = HttpResult.GetJsonResult(false, string.Empty, ex.Message);
                Tools.Log.Add("修改系统设置异常：" + ex.Message);
            }
            return httpResult;
        }

        /// <summary>
        /// 删除系统设置
        /// </summary>

        /// <param name="SystemSetting">系统设置对象</param>
        /// <returns></returns>
        public HttpResult DelSystemSetting(SystemSetting SystemSetting)
        {
            HttpResult httpResult = new HttpResult();
            try
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append(@"
DELETE From SystemSetting WHERE Id=@Id
");
                MySqlParameter[] mySqlParameters = new MySqlParameter[1];
                mySqlParameters[0] = new MySqlParameter("Id", SystemSetting.Id);
                int _res = Tools.DBHelper.ExecuteCommand(stringBuilder.ToString(), mySqlParameters);
                httpResult = HttpResult.GetJsonResult(_res == 1, "删除系统设置成功", "删除系统设置失败");
            }
            catch (Exception ex)
            {
                httpResult = HttpResult.GetJsonResult(false, string.Empty, ex.Message);
                Tools.Log.Add("删除系统设置异常：" + ex.Message);
            }
            return httpResult;
        }
        /// <summary>
        /// 查询系统设置列表
        /// </summary>

        /// <returns></returns>
        public HttpResult GetSystemSettingList()
        {
            HttpResult httpResult = new HttpResult();
            try
            {
                List<SystemSetting> SystemSettings = new List<SystemSetting>();
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append(@"SELECT * FROM SystemSetting");
                DataTable _ds = Tools.DBHelper.GetDataTable(stringBuilder.ToString());
                for (int i = 0; i < _ds.Rows.Count; i++)
                {
                    SystemSetting SystemSetting = new SystemSetting();
                    SystemSetting.Id = (int)_ds.Rows[i]["Id"];
                    SystemSetting.Logo = System.Text.Encoding.Default.GetBytes(_ds.Rows[i]["Logo"].ToString());
                    SystemSetting.Title = _ds.Rows[i]["Title"].ToString();
                    SystemSettings.Add(SystemSetting);
                }
                httpResult = HttpResult.GetJsonResult(true, "查询系统设置成功", "查询系统设置失败", SystemSettings);
            }
            catch (Exception ex)
            {
                httpResult = HttpResult.GetJsonResult(false, string.Empty, ex.Message);
                Tools.Log.Add("查询系统设置列表异常：" + ex.Message);
            }
            return httpResult;
        }
    }
}
