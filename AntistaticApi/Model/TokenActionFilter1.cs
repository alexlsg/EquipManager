using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;

namespace AntistaticApi.Model
{
    public class TokenActionFilter1 : ActionFilterAttribute
    {
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
            if (!tokenModel.LoginUser.Exists(n => n == claimsPrincipal.Identity.Name))
            {
                context.Result = new RedirectResult("/home/error");
            }
        }
    }
}
