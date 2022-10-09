using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Tools;
using UserManagement;

namespace AntistaticApi.Model
{
    public class TokenActionFilter1 : ActionFilterAttribute
    {
        /// <summary>
        /// 菜单权限需求，1超管、2管理员、3操作员
        /// </summary>
        static Dictionary<string, string> qx = new Dictionary<string, string>()
        {
            {"/cmd/excutecmd","1"},
            {"/usermanagement/adduser","1,2"},
            {"/usermanagement/moduser","1,2"},
            {"/usermanagement/deluser","1,2"},
            {"/usermanagement/getuserlist","1,2"},
            {"/usermanagement/getuser","1,2"},
            {"/usermanagement/addrole","1,2"},
            {"/usermanagement/modrole","1,2"},
            {"/usermanagement/delrole","1,2"},
            {"/usermanagement/getrolelist","1,2"},
            {"/statistics/getlsqx","1,2"},
            {"/statistics/getlsjlbyequipid","1,2"},
            {"/statistics/getalllsjl","1,2"},
            {"/statistics/getlsjlbygroupid","1,2"},
            {"/statistics/getevent","1,2"},
            {"/statistics/getcurrentevent","1,2"},
            {"/systemset/getproductionlinegrouplist","1,2"},
        };
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var test = context.HttpContext.Request.Path;
            string bearer = context.HttpContext.Request.Headers["Authorization"].FirstOrDefault();
            if (string.IsNullOrEmpty(bearer) || !bearer.Contains("Bearer")) return;
            string[] jwt = bearer.Split(' ');
            var tokenObj = new JwtSecurityToken(jwt[1]);
            
            var claimsIdentity = new ClaimsIdentity(tokenObj.Claims);
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
            context.HttpContext.User = claimsPrincipal;
            if (context.ActionArguments.Count > 0)
            {
                if (context.ActionArguments.ToList()[0].Value is Params)
                {
                    Params _p = context.ActionArguments.ToList()[0].Value as Params;
                    _p.user = claimsPrincipal.Identity.Name;
                    _p.token = tokenObj.RawData;
                }
                else if(context.ActionArguments.ToList()[0].Value is User)
                {
                    User _p = context.ActionArguments.ToList()[0].Value as User;
                    _p.Token = tokenObj.RawData;
                }
            }
            if (!tokenModel.LoginUser.Exists(n => n.UserName == claimsPrincipal.Identity.Name && n.Token == tokenObj.RawData))
            {
                context.Result = new RedirectResult("/home/error");
            }
            else
            {
                if (qx.ContainsKey(test.Value.ToLower()))
                {
                    UserRole _ur = UserService.UserRoles.Find(n => n.User?.ToLower() == claimsPrincipal.Identity.Name?.ToLower());
                    int _role = 3;
                    if (_ur != null)
                    {
                        _role = (int)_ur.Role;
                    }
                    if (!qx[test.Value.ToLower()].Split(',').Contains(_role.ToString()))
                    {
                        context.Result = new RedirectResult("/home/qxerror");
                    }
                }
            }
        }
    }
}
