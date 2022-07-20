using AntistaticApi.IdentityService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Tools;
using UserManagement;

namespace AntistaticApi.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IAuthenticateService _authenservice;
        public LoginController(IAuthenticateService service)
        {
            _authenservice = service;
        }
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            try
            {
                UserService userService = new UserService();
                User user = new User() { UserName = username, PassWord = password };
                HttpResult httpResult = userService.Login(user);
                if (httpResult.Status)
                {
                    //签发token
                    string token;
                    if (_authenservice.IsAuthenticated(user, out token))
                    {
                        httpResult.Token = token;
                    }
                }
                return new JsonResult(httpResult);
            }
            catch (Exception ex)
            {
                Log.Add(ex);
                return new JsonResult(ex.Message);
            }
        }

        [HttpPost]
        public IActionResult LogOut(string username)
        {
            try
            {
                User user = new User() { UserName = username };
                string token;
                _authenservice.IsAuthenticated(user, out token);
                HttpResult httpResult = HttpResult.GetJsonResult(true, "登出成功", String.Empty);
                return new JsonResult(httpResult);
            }
            catch (Exception ex)
            {
                Log.Add(ex);
                return new JsonResult(ex.Message);
            }
        }
    }
}
