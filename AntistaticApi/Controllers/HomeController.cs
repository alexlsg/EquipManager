using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tools;

namespace AntistaticApi.Controllers
{
    [Route("[controller]/[action]")]
    public class HomeController : Controller
    {

        public IActionResult Index()
        {
            return Ok("防静电信息化管理数据分析API启动...");
        }
        [Authorize]
        public IActionResult Error()
        {
            return new JsonResult(HttpResult.GetJsonResult(false, "", "用户已经失效，请重新登录"));
        }
    }
}
