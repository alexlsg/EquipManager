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
        public IActionResult LogOut(User user)
        {
            try
            {

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
