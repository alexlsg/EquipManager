using AntistaticApi.Model;
using EquipDataManager.Bll;
using EquipModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Tools;

namespace AntistaticApi.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    [Authorize]
    [TokenActionFilter1]
    public class CMDController : Controller
    {
        [HttpPost]
        public IActionResult ExcuteCMD(List<Command> commands)
        {
            try
            {
                return new JsonResult(DataPicker.Instance.ExecCommand(commands, true));
            }
            catch (System.Exception ex)
            {
                Log.Add(ex);
                return new JsonResult(ex.Message);
            }
        }
    }
}
