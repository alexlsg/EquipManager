
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Tools;

namespace UserManagement
{
    /// <summary>
    /// 角色业务类
    /// </summary>
    public class RoleService
    {
        /// <summary>
        /// 添加角色
        /// </summary>

        /// <param name="Role">角色对象</param>
        /// <returns></returns>
        public HttpResult AddRole(Role Role)
        {
            HttpResult httpResult = new HttpResult();
            try
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append(@"
INSERT INTO Role(RoleId,RoleName)
VALUES(@RoleId,@RoleName)
");
                MySqlParameter[] mySqlParameters = new MySqlParameter[2];
                mySqlParameters[0] = new MySqlParameter("RoleName", Role.RoleName);
                mySqlParameters[1] = new MySqlParameter("RoleId", Role.RoleId);
                int _res = Tools.DBHelper.ExecuteCommand(stringBuilder.ToString(), mySqlParameters);
                httpResult = HttpResult.GetJsonResult(_res == 1, "添加角色成功", "添加角色失败");
            }
            catch (Exception ex)
            {
                httpResult = HttpResult.GetJsonResult(false, string.Empty, ex.Message);
                Tools.Log.Add("添加角色异常：" + ex.Message);
            }
            return httpResult;
        }

        /// <summary>
        /// 修改角色
        /// </summary>

        /// <param name="Role">角色对象</param>
        /// <returns></returns>
        public HttpResult ModRole(Role Role)
        {
            HttpResult httpResult = new HttpResult();
            try
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append(@"
UPDATE Role SET RoleName=@RoleName
WHERE RoleId=@RoleId
");
                MySqlParameter[] mySqlParameters = new MySqlParameter[2];
                mySqlParameters[0] = new MySqlParameter("RoleName", Role.RoleName);
                mySqlParameters[1] = new MySqlParameter("RoleId", Role.RoleId);
                int _res = Tools.DBHelper.ExecuteCommand(stringBuilder.ToString(), mySqlParameters);
                httpResult = HttpResult.GetJsonResult(_res == 1, "修改角色成功", "修改角色失败");
            }
            catch (Exception ex)
            {
                httpResult = HttpResult.GetJsonResult(false, string.Empty, ex.Message);
                Tools.Log.Add("修改角色异常：" + ex.Message);
            }
            return httpResult;
        }

        /// <summary>
        /// 删除角色
        /// </summary>

        /// <param name="Role">角色对象</param>
        /// <returns></returns>
        public HttpResult DelRole(Role Role)
        {
            HttpResult httpResult = new HttpResult();
            try
            {
                if (Role.RoleId == (int)Enum_Role.超级管理员)
                {
                    httpResult = HttpResult.GetJsonResult(false, string.Empty, "超级管理员不能删除");
                }
                else
                {
                    StringBuilder stringBuilder = new StringBuilder();
                    stringBuilder.Append(@"
DELETE Role WHERE RoleId=@RoleId
DELETE RoleFunction WHERE RoleId=@RoleId
");
                    MySqlParameter[] mySqlParameters = new MySqlParameter[1];
                    mySqlParameters[0] = new MySqlParameter("RoleId", Role.RoleId);
                    int _res = Tools.DBHelper.ExecuteCommand(stringBuilder.ToString(), mySqlParameters);
                    httpResult = HttpResult.GetJsonResult(_res == 1, "删除角色成功", "删除角色失败");
                }
            }
            catch (Exception ex)
            {
                httpResult = HttpResult.GetJsonResult(false, string.Empty, ex.Message);
                Tools.Log.Add("删除角色异常：" + ex.Message);
            }
            return httpResult;
        }
        /// <summary>
        /// 查询角色列表
        /// </summary>

        /// <returns></returns>
        public HttpResult GetRoleList(string text)
        {
            HttpResult httpResult = new HttpResult();
            try
            {
                List<Role> Roles = new List<Role>();
                StringBuilder stringBuilder = new StringBuilder();
                if (string.IsNullOrWhiteSpace(text))
                {
                    stringBuilder.Append(@"SELECT * FROM Role");
                }
                else
                {
                    stringBuilder.Append(@"SELECT * FROM Role WHERE RoleName LIKE '%" + text + "%'");
                }
                DataTable _ds = Tools.DBHelper.GetDataTable(stringBuilder.ToString());
                RoleFunctionService roleFunctionService = new RoleFunctionService();
                List<RoleFunction> roleFunctions = roleFunctionService.GetList();
                for (int i = 0; i < _ds.Rows.Count; i++)
                {
                    Role Role = new Role();
                    Role.RoleName = _ds.Rows[i]["RoleName"].ToString();
                    Role.RoleId = (int)_ds.Rows[i]["RoleId"];
                    Role.RoleFunctions = roleFunctions.FindAll(n => n.RoleId == Role.RoleId);
                    Roles.Add(Role);
                }
                httpResult = HttpResult.GetJsonResult(true, "查询角色列表成功", string.Empty, Roles);
            }
            catch (Exception ex)
            {
                Tools.Log.Add("查询角色列表异常：" + ex.Message);
                httpResult = HttpResult.GetJsonResult(false, string.Empty, ex.Message);
            }
            return httpResult;
        }
    }
}
