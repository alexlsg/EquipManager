
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Tools;

namespace UserManagement
{
    /// <summary>
    /// 用户业务类
    /// </summary>
    public class UserService
    {
        /// <summary>
        /// 添加用户
        /// </summary>

        /// <param name="user">用户对象</param>
        /// <returns></returns>
       public HttpResult AddUser(User user)
        {
            HttpResult httpResult = new HttpResult();
            try
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append(@"
INSERT INTO USER(UserName,RoleGroup,PassWord,ThemeColor,LastLoginTime,Remarks,ZoneBinding)
VALUES(@UserName,@RoleGroup,@PassWord,@ThemeColor,@LastLoginTime,@Remarks,@ZoneBinding)
");
                MySqlParameter[] mySqlParameters = new MySqlParameter[7];
                mySqlParameters[0] = new MySqlParameter("UserName", user.UserName);
                mySqlParameters[1] = new MySqlParameter("RoleGroup", user.RoleGroup);
                mySqlParameters[2] = new MySqlParameter("PassWord", DES.MD5Encrypt(user.PassWord));
                mySqlParameters[3] = new MySqlParameter("ThemeColor", user.ThemeColor);
                mySqlParameters[4] = new MySqlParameter("LastLoginTime", user.LastLoginTime);
                mySqlParameters[5] = new MySqlParameter("Remarks", user.Remarks);
                mySqlParameters[6] = new MySqlParameter("ZoneBinding", user.ZoneBinding);
                int _res = Tools.DBHelper.ExecuteCommand(stringBuilder.ToString(), mySqlParameters);
                httpResult = HttpResult.GetJsonResult(_res == 1, "添加用户成功", "添加用户失败");
            }
            catch (Exception ex)
            {
                httpResult = HttpResult.GetJsonResult(false, string.Empty, ex.Message);
                Tools.Log.Add("添加用户异常：" + ex.Message);
            }
            return httpResult;
        }

        /// <summary>
        /// 修改用户
        /// </summary>
        /// <param name="user">用户对象</param>
        /// <returns></returns>
       public HttpResult ModUser(User user)
        {
            HttpResult httpResult = new HttpResult();
            try
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append(@"
UPDATE USER SET UserName=@UserName,RoleGroup=@RoleGroup,PassWord=@PassWord,ThemeColor=@ThemeColor,LastLoginTime=@LastLoginTime,Remarks=@Remarks,ZoneBinding=@ZoneBinding
WHERE UserName=@UserName
");
                MySqlParameter[] mySqlParameters = new MySqlParameter[7];
                mySqlParameters[0] = new MySqlParameter("UserName", user.UserName);
                mySqlParameters[1] = new MySqlParameter("RoleGroup", user.RoleGroup);
                mySqlParameters[2] = new MySqlParameter("PassWord", DES.MD5Encrypt(user.PassWord));
                mySqlParameters[3] = new MySqlParameter("ThemeColor", user.ThemeColor);
                mySqlParameters[4] = new MySqlParameter("LastLoginTime", user.LastLoginTime);
                mySqlParameters[5] = new MySqlParameter("Remarks", user.Remarks);
                mySqlParameters[6] = new MySqlParameter("ZoneBinding", user.ZoneBinding);
                int _res = Tools.DBHelper.ExecuteCommand(stringBuilder.ToString(), mySqlParameters);
                httpResult = HttpResult.GetJsonResult(_res == 1, "修改用户成功", "修改用户失败");
            }
            catch (Exception ex)
            {
                httpResult = HttpResult.GetJsonResult(false, string.Empty, ex.Message);
                Tools.Log.Add("修改用户异常：" + ex.Message);
            }
            return httpResult;
        }

        /// <summary>
        /// 删除用户
        /// </summary>

        /// <param name="user">用户对象</param>
        /// <returns></returns>
       public HttpResult DelUser(User user)
        {
            HttpResult httpResult = new HttpResult();
            try
            {
                if (user.UserName == Const.Const_Admin)
                {
                    httpResult = HttpResult.GetJsonResult(false, string.Empty, "Admin账号不能删除");
                }
                else
                {
                    StringBuilder stringBuilder = new StringBuilder();
                    stringBuilder.Append(@"
DELETE User WHERE UserName=@UserName
");
                    MySqlParameter[] mySqlParameters = new MySqlParameter[1];
                    mySqlParameters[0] = new MySqlParameter("UserName", user.UserName);
                    int _res = Tools.DBHelper.ExecuteCommand(stringBuilder.ToString(), mySqlParameters);
                    httpResult = HttpResult.GetJsonResult(_res == 1, "删除用户成功", "删除用户失败");
                }
            }
            catch (Exception ex)
            {
                httpResult = HttpResult.GetJsonResult(false, string.Empty, ex.Message);
                Tools.Log.Add("删除用户异常：" + ex.Message);
            }
            return httpResult;
        }
        /// <summary>
        /// 查询用户列表
        /// </summary>

        /// <returns></returns>
       public HttpResult GetUserList()
        {
            HttpResult httpResult = new HttpResult();
            try
            {
                List<User> users = new List<User>();
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append(@"SELECT * FROM USER");
                DataTable _ds = Tools.DBHelper.GetDataTable(stringBuilder.ToString());
                for (int i = 0; i < _ds.Rows.Count; i++)
                {
                    User user = new User();
                    user.UserName = _ds.Rows[i]["UserName"].ToString();
                    user.RoleGroup = _ds.Rows[i]["RoleGroup"].ToString();
                    user.PassWord = DES.MD5Dencrypt(_ds.Rows[i]["PassWord"].ToString());
                    user.ThemeColor = _ds.Rows[i]["ThemeColor"].ToString();
                    user.LastLoginTime = (DateTime?)_ds.Rows[i]["LastLoginTime"];
                    user.Remarks = _ds.Rows[i]["Remarks"].ToString();
                    user.ZoneBinding = _ds.Rows[i]["ZoneBinding"].ToString();
                    users.Add(user);
                }
                httpResult = HttpResult.GetJsonResult(true, "查询用户列表成功", string.Empty, users);
            }
            catch (Exception ex)
            {
                Tools.Log.Add("查询用户列表异常：" + ex.Message);
                httpResult = HttpResult.GetJsonResult(false, string.Empty, ex.Message);
            }
            return httpResult;
        }
        /// <summary>
        /// 查询用单个用户
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
       public HttpResult GetUser(string username)
        {
            HttpResult httpResult = new HttpResult();
            try
            {
                User user = GetModel(username);
                httpResult = HttpResult.GetJsonResult(true, "查询用户成功", string.Empty, user);
            }
            catch (Exception ex)
            {
                Tools.Log.Add("查询用户异常：" + ex.Message);
                httpResult = HttpResult.GetJsonResult(false, string.Empty, ex.Message);
            }
            return httpResult;
        }
        /// <summary>
        /// 根据username查询用户
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        private User GetModel(string username)
        {
            User user = null;
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(@"SELECT * FROM USER WHERE UserName=@UserName");
            MySqlParameter[] mySqlParameters = new MySqlParameter[1];
            mySqlParameters[0] = new MySqlParameter("UserName", username);
            DataTable _ds = Tools.DBHelper.GetDataTable(stringBuilder.ToString(), mySqlParameters);
            if (_ds.Rows.Count > 0)
            {
                user = new User();
                user.UserName = _ds.Rows[0]["UserName"].ToString();
                user.RoleGroup = _ds.Rows[0]["RoleGroup"].ToString();
                user.PassWord = _ds.Rows[0]["PassWord"].ToString();
                user.ThemeColor = _ds.Rows[0]["ThemeColor"].ToString();
                user.LastLoginTime = _ds.Rows[0].Field<DateTime?>("LastLoginTime");
                user.Remarks = _ds.Rows[0]["Remarks"].ToString();
                user.ZoneBinding = _ds.Rows[0]["ZoneBinding"].ToString();
            }
            return user;
        }
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public HttpResult Login(User user)
        {
            HttpResult httpResult = new HttpResult();
            try
            {
                User user1 = GetModel(user.UserName);
                if (user1 == null)
                    httpResult = HttpResult.GetJsonResult(false, string.Empty, $"登录用户{user.UserName}不存在");
                else
                {
                    if (user1.PassWord == user.PassWord)
                    {
                        httpResult = HttpResult.GetJsonResult(true, "登录成功", string.Empty, user1);
                    }
                    else
                    {
                        httpResult = HttpResult.GetJsonResult(false, string.Empty, "密码不正确");
                    }
                }
            }
            catch (Exception ex)
            {
                Tools.Log.Add("登录异常：" + ex.Message);
                httpResult = HttpResult.GetJsonResult(false, string.Empty, ex.Message);
            }
            return httpResult;
        }
    }
}
