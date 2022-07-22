using Microsoft.AspNetCore.Mvc;

namespace AntistaticApi.Controllers
{
    [Route("[controller]/[action]")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return Ok("防静电信息化管理数据分析API启动...");
        }
    }
}
