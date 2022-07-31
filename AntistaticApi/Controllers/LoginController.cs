using AntistaticApi.IdentityService;
using AntistaticApi.Model;
using Microsoft.AspNetCore.Authorization;
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
        [HttpGet]
        public IActionResult Test()
        {
            return new JsonResult("Test");
        }
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Login(User user)
        {
            try
            {
                UserService userService = new UserService();
                HttpResult httpResult = userService.Login(user);
                if (httpResult.Status)
                {
                    //签发token
                    string token;
                    if (_authenservice.IsAuthenticated(user, out token))
                    {
                        httpResult.Token = token;
                        tokenModel.LoginUser.Add(user.UserName);
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
        [Authorize]
        [HttpPost]
        public IActionResult LoginOut(User user)
        {
            try
            {
                if (tokenModel.LoginUser.Exists(n => n == user.UserName))
                {
                    tokenModel.LoginUser.Remove(user.UserName);
                    HttpResult httpResult = HttpResult.GetJsonResult(true, "用户登出成功", String.Empty);
                    return new JsonResult(httpResult);
                }
                else
                {
                    HttpResult httpResult = HttpResult.GetJsonResult(false, "", "用户未正常登录,登出失败");
                    return new JsonResult(httpResult);
                }
            }
            catch (Exception ex)
            {
                Log.Add(ex);
                return new JsonResult(ex.Message);
            }
        }
        [Authorize]
        [HttpPost]
        public IActionResult RefreshToken(User user)
        {
            try
            {
                HttpResult httpResult1 = new HttpResult();
                if (tokenModel.LoginUser.Exists(n => n == user.UserName))
                {
                    UserService userService = new UserService();
                    HttpResult httpResult = userService.Login(user);
                    if (httpResult.Status)
                    {
                        string token;
                        if (_authenservice.IsAuthenticated(user, out token))
                        {
                            httpResult1.Token = token;
                            httpResult1.Status = true;
                            httpResult1.Message = "刷新Token成功";
                        }
                    }
                }
                else
                {
                    httpResult1 = HttpResult.GetJsonResult(false, "", "用户未正常登录,刷新Token失败");
                }
                return new JsonResult(httpResult1);

            }
            catch (Exception ex)
            {
                Log.Add(ex);
                return new JsonResult(ex.Message);
            }
        }

    }
}
