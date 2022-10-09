
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
        public HttpResult AddRole(Role role)
        {
            HttpResult httpResult = new HttpResult();
            try
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append($@"
INSERT INTO User_role(RoleType,RoleName,GroupQx,EquipQx,PageQx)
Values({role.RoleType},'{role.Name}','{role.Groups}','{role.Equips}','{role.Pages}')
");
                int _res = Tools.DBHelper.ExecuteCommand(stringBuilder.ToString());
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
        public HttpResult ModRole(Role role)
        {
            HttpResult httpResult = new HttpResult();
            try
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append($@"
UPDATE User_role SET RoleType = {role.RoleType},RoleName = '{role.Name}',GroupQx = '{role.Groups}',EquipQx = '{role.Equips}',PageQx = '{role.Pages}'
WHERE ID={role.ID}
");
                int _res = Tools.DBHelper.ExecuteCommand(stringBuilder.ToString());
                httpResult = HttpResult.GetJsonResult(_res == 1, "修改角色成功", "修改角色失败");
                UserService.RefreshUserRoles();
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
        public HttpResult DelRole(Role role)
        {
            HttpResult httpResult = new HttpResult();
            try
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append($@"
DELETE from User_role WHERE ID={role.ID}
");
                int _res = Tools.DBHelper.ExecuteCommand(stringBuilder.ToString());
                httpResult = HttpResult.GetJsonResult(_res == 1, "删除角色成功", "删除角色失败");
                UserService.RefreshUserRoles();
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
        public HttpResult GetRoleList()
        {
            HttpResult httpResult = new HttpResult();
            try
            {
                List<Role> Roles = new List<Role>();
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append(@"SELECT * FROM User_role");
                DataTable _ds = Tools.DBHelper.GetDataTable(stringBuilder.ToString());
                for (int i = 0; i < _ds.Rows.Count; i++)
                {
                    Role Role = new Role()
                    {
                        ID = _ds.Rows[i].Field<int>("ID"),
                        RoleType = _ds.Rows[i].Field<int>("RoleType"),
                        Pages = _ds.Rows[i].Field<string>("PageQx"),
                        Equips = _ds.Rows[i].Field<string>("EquipQx"),
                        Groups = _ds.Rows[i].Field<string>("GroupQx"),
                        Name = _ds.Rows[i].Field<string>("RoleName")
                    };
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

        public HttpResult GetRole(int id)
        {
            HttpResult httpResult = new HttpResult();
            try
            {
                DataTable _dt = DBHelper.GetDataTable($"select * from User_role where id = {id}");
                if (_dt.Rows.Count == 1)
                {
                    Role _r = new Role()
                    {
                        ID = _dt.Rows[0].Field<int>("ID"),
                        RoleType = _dt.Rows[0].Field<int>("RoleType"),
                        Pages = _dt.Rows[0].Field<string>("PageQx"),
                        Equips = _dt.Rows[0].Field<string>("EquipQx"),
                        Groups = _dt.Rows[0].Field<string>("GroupQx"),
                        Name = _dt.Rows[0].Field<string>("RoleName")
                    };
                    httpResult = HttpResult.GetJsonResult(true, "查询角色列表成功", string.Empty, _r);
                }
                else
                    httpResult = HttpResult.GetJsonResult(false, string.Empty,"查询角色失败,id="+id);
            }
            catch (Exception ex)
            {
                Tools.Log.Add("查询角色异常：" + ex.Message);
                httpResult = HttpResult.GetJsonResult(false, string.Empty, ex.Message);
            }
            return httpResult;
        }
    }
}
