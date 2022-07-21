using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using Tools;
using UserManagement;

namespace AntistaticApi.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    //[Authorize]
    public class UserManagementController : ControllerBase
    {
        public static List<User> users = new List<User>();
        /// <summary>
        /// 新增用户
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult AddUser(User user)
        {
            UserService userService = new UserService();
            HttpResult httpResult = userService.AddUser(user);
            return new JsonResult(httpResult);
        }
        /// <summary>
        /// 修改用户
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult ModUser(User user)
        {
            UserService userService = new UserService();
            HttpResult httpResult = userService.AddUser(user);
            return new JsonResult(httpResult);
        }
        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult DelUser(User user)
        {
            UserService userService = new UserService();
            HttpResult httpResult = userService.DelUser(user);
            return new JsonResult(httpResult);
        }
        /// <summary>
        /// 查询用户列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult GetUserList(string text)
        {
            UserService userService = new UserService();
            HttpResult httpResult = userService.GetUserList(text);
            return new JsonResult(httpResult);
        }
        /// <summary>
        /// 根据用户名称查询用户信息
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult GetUser(string username)
        {
            UserService userService = new UserService();
            HttpResult httpResult = userService.GetUser(username);
            return new JsonResult(httpResult);
        }
        /// <summary>
        /// 新增角色
        /// </summary>
        /// <param name="Role"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult AddRole(Role Role)
        {
            RoleService userService = new RoleService();
            HttpResult httpResult = userService.AddRole(Role);
            return new JsonResult(httpResult);
        }
        /// <summary>
        /// 修改角色
        /// </summary>
        /// <param name="Role"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult ModRole(Role Role)
        {
            RoleService userService = new RoleService();
            HttpResult httpResult = userService.ModRole(Role);
            return new JsonResult(httpResult);
        }
        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="Role"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult DelRole(Role Role)
        {
            RoleService userService = new RoleService();
            HttpResult httpResult = userService.DelRole(Role);
            return new JsonResult(httpResult);
        }
        /// <summary>
        /// 查询角色列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult GetRoleList(string text)
        {
            RoleService userService = new RoleService();
            HttpResult httpResult = userService.GetRoleList(text);
            return new JsonResult(httpResult);
        }
        /// <summary>
        /// 新增功能
        /// </summary>
        /// <param name="Function"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult AddFunction(Function Function)
        {
            FunctionService userService = new FunctionService();
            HttpResult httpResult = userService.AddFunction(Function);
            return new JsonResult(httpResult);
        }
        /// <summary>
        /// 修改功能
        /// </summary>
        /// <param name="Function"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult ModFunction(Function Function)
        {
            FunctionService userService = new FunctionService();
            HttpResult httpResult = userService.ModFunction(Function);
            return new JsonResult(httpResult);
        }
        /// <summary>
        /// 删除功能
        /// </summary>
        /// <param name="Function"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult DelFunction(Function Function)
        {
            FunctionService userService = new FunctionService();
            HttpResult httpResult = userService.DelFunction(Function);
            return new JsonResult(httpResult);
        }
        /// <summary>
        /// 查询功能列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult GetFunctionList(string text)
        {
            FunctionService userService = new FunctionService();
            HttpResult httpResult = userService.GetFunctionList(text);
            return new JsonResult(httpResult);
        }
        /// <summary>
        /// 新增角色功能
        /// </summary>
        /// <param name="RoleFunction"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult AddRoleFunction(RoleFunction RoleFunction)
        {
            RoleFunctionService userService = new RoleFunctionService();
            HttpResult httpResult = userService.AddRoleFunction(RoleFunction);
            return new JsonResult(httpResult);
        }
        /// <summary>
        /// 修改角色功能
        /// </summary>
        /// <param name="RoleFunction"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult ModRoleFunction(RoleFunction RoleFunction)
        {
            RoleFunctionService userService = new RoleFunctionService();
            HttpResult httpResult = userService.ModRoleFunction(RoleFunction);
            return new JsonResult(httpResult);
        }
        /// <summary>
        /// 删除角色功能
        /// </summary>
        /// <param name="RoleFunction"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult DelRoleFunction(RoleFunction RoleFunction)
        {
            RoleFunctionService userService = new RoleFunctionService();
            HttpResult httpResult = userService.DelRoleFunction(RoleFunction);
            return new JsonResult(httpResult);
        }
        /// <summary>
        /// 查询角色功能列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult GetRoleFunctionList()
        {
            RoleFunctionService userService = new RoleFunctionService();
            HttpResult httpResult = userService.GetRoleFunctionList();
            return new JsonResult(httpResult);
        }
        /// <summary>
        /// 根据角色Id查询角色功能列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult GetRoleFunctionListByRoleID(string roleid)
        {
            RoleFunctionService userService = new RoleFunctionService();
            HttpResult httpResult = userService.GetRoleFunctionListByRoleID(roleid);
            return new JsonResult(httpResult);
        }
    }
}
