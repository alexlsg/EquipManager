
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Tools;

namespace UserManagement
{
    /// <summary>
    /// 角色功能类
    /// </summary>
    public class RoleFunctionService
    {
        /// <summary>
        /// 添加角色功能
        /// </summary>    
        /// <param name="RoleFunction">角色功能对象</param>
        /// <returns></returns>
        public HttpResult AddRoleFunction(RoleFunction RoleFunction)
        {
            HttpResult httpResult = new HttpResult();
            try
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append(@"
INSERT INTO RoleFunction(RoleId,FunctionId)
VALUES(@RoleId,@FunctionId)
");
                MySqlParameter[] mySqlParameters = new MySqlParameter[2];
                mySqlParameters[0] = new MySqlParameter("RoleId", RoleFunction.RoleId);
                mySqlParameters[1] = new MySqlParameter("FunctionId", RoleFunction.FunctionId);
                int _res = Tools.DBHelper.ExecuteCommand(stringBuilder.ToString(), mySqlParameters);
                httpResult = HttpResult.GetJsonResult(_res == 1, "添加角色功能成功", "添加角色功能失败");
            }
            catch (Exception ex)
            {
                httpResult = HttpResult.GetJsonResult(false, string.Empty, ex.Message);
                Tools.Log.Add("添加角色功能异常：" + ex.Message);
            }
            return httpResult;
        }

        /// <summary>
        /// 修改角色功能
        /// </summary>
        /// <param name="RoleFunction">角色功能对象</param>
        /// <returns></returns>
        public HttpResult ModRoleFunction(RoleFunction RoleFunction)
        {
            HttpResult httpResult = new HttpResult();
            try
            {
                if (RoleFunction.RoleId == (int)Enum_Role.超级管理员)
                {
                    httpResult = HttpResult.GetJsonResult(false, string.Empty, "超级管理员的权限不能修改");
                }
                else
                {
                    StringBuilder stringBuilder = new StringBuilder();
                    stringBuilder.Append(@"
UPDATE RoleFunction SET RoleId=@RoleId, FunctionId=@FunctionId
WHERE Id=@Id
");
                    MySqlParameter[] mySqlParameters = new MySqlParameter[3];
                    mySqlParameters[0] = new MySqlParameter("RoleId", RoleFunction.RoleId);
                    mySqlParameters[1] = new MySqlParameter("FunctionId", RoleFunction.FunctionId);
                    mySqlParameters[2] = new MySqlParameter("Id", RoleFunction.Id);
                    int _res = Tools.DBHelper.ExecuteCommand(stringBuilder.ToString(), mySqlParameters);
                    httpResult = HttpResult.GetJsonResult(_res == 1, "修改角色功能成功", "修改角色功能失败");
                }
            }
            catch (Exception ex)
            {
                httpResult = HttpResult.GetJsonResult(false, string.Empty, ex.Message);
                Tools.Log.Add("修改角色功能异常：" + ex.Message);
            }
            return httpResult;
        }

        /// <summary>
        /// 删除角色功能
        /// </summary>

        /// <param name="RoleFunction">角色功能对象</param>
        /// <returns></returns>
        public HttpResult DelRoleFunction(RoleFunction RoleFunction)
        {
            HttpResult httpResult = new HttpResult();
            try
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append(@"
DELETE RoleFunction WHERE Id=@Id
");
                MySqlParameter[] mySqlParameters = new MySqlParameter[1];
                mySqlParameters[0] = new MySqlParameter("Id", RoleFunction.Id);
                int _res = Tools.DBHelper.ExecuteCommand(stringBuilder.ToString(), mySqlParameters);
                httpResult = HttpResult.GetJsonResult(_res == 1, "删除角色功能成功", "删除角色功能失败");
            }
            catch (Exception ex)
            {
                httpResult = HttpResult.GetJsonResult(false, string.Empty, ex.Message);
                Tools.Log.Add("删除角色功能异常：" + ex.Message);
            }
            return httpResult;
        }
        /// <summary>
        /// 查询角色功能列表
        /// </summary>

        /// <returns></returns>
        public HttpResult GetRoleFunctionList()
        {
            HttpResult httpResult = new HttpResult();
            try
            {
                List<RoleFunction> RoleFunctions = GetList();
                httpResult = HttpResult.GetJsonResult(true, "查询角色功能列表成功", "查询角色功能列表失败");
            }
            catch (Exception ex)
            {
                httpResult = HttpResult.GetJsonResult(false, string.Empty, ex.Message);
                Tools.Log.Add("查询角色功能列表异常：" + ex.Message);
            }
            return httpResult;
        }
        internal List<RoleFunction> GetList()
        {
            List<RoleFunction> RoleFunctions = new List<RoleFunction>();
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(@"SELECT * FROM RoleFunction");
            DataTable _ds = Tools.DBHelper.GetDataTable(stringBuilder.ToString());
            for (int i = 0; i < _ds.Rows.Count; i++)
            {
                RoleFunction RoleFunction = new RoleFunction();
                RoleFunction.FunctionId = (int)_ds.Rows[i]["FunctionId"];
                RoleFunction.RoleId = (int)_ds.Rows[i]["RoleId"];
                RoleFunctions.Add(RoleFunction);
            }
            return RoleFunctions;
        }
        /// <summary>
        /// 根据角色ID查询角色功能列表
        /// </summary>

        /// <param name="roleid"></param>
        /// <returns></returns>
        public HttpResult GetRoleFunctionListByRoleID(string roleid)
        {
            HttpResult httpResult = new HttpResult();
            try
            {
                List<RoleFunction> RoleFunctions = new List<RoleFunction>();
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append(@"SELECT * FROM RoleFunction WHERE ROLEID=@ROLEID");
                MySqlParameter[] mySqlParameters = new MySqlParameter[1];
                mySqlParameters[0] = new MySqlParameter("ROLEID", roleid);
                DataTable _ds = Tools.DBHelper.GetDataTable(stringBuilder.ToString(), mySqlParameters);
                for (int i = 0; i < _ds.Rows.Count; i++)
                {
                    RoleFunction RoleFunction = new RoleFunction();
                    RoleFunction.FunctionId = (int)_ds.Rows[i]["FunctionId"];
                    RoleFunction.RoleId = (int)_ds.Rows[i]["RoleId"];
                    RoleFunctions.Add(RoleFunction);
                }
                httpResult = HttpResult.GetJsonResult(true, "根据角色ID查询角色功能列表成功", string.Empty, RoleFunctions);
            }
            catch (Exception ex)
            {
                Tools.Log.Add("根据角色ID查询角色功能列表异常：" + ex.Message);
                httpResult = HttpResult.GetJsonResult(false, string.Empty, ex.Message);
            }
            return httpResult;
        }
    }
}
