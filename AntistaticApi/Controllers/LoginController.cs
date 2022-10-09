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
                        tokenModel.LoginUser.Add(new UserToken() { UserName = user.UserName, Token = token });
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
        public IActionResult LoginOut(Params user)
        {
            try
            {
                UserToken _ut = tokenModel.LoginUser.Find(n => n.UserName == user.user && n.Token == user.token);
                if (_ut != null)
                {
                    tokenModel.LoginUser.Remove(_ut);
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
                UserToken _ut = tokenModel.LoginUser.Find(n => n.UserName == user.UserName && n.Token == user.Token);
                if (_ut != null)
                {
                    UserService userService = new UserService();
                    HttpResult httpResult = userService.Login(user);
                    if (httpResult.Status)
                    {
                        string token;
                        if (_authenservice.IsAuthenticated(user, out token))
                        {
                            _ut.Token = token;
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
