using AntistaticApi.Model;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UserManagement;

namespace AntistaticApi.IdentityService
{
    public class AuthenticateService : IAuthenticateService
    {
        public readonly tokenModel _jwtModel;
        public AuthenticateService(IOptions<tokenModel> jwtModel)
        {
            _jwtModel = jwtModel.Value;
        }

        public bool IsAuthenticated(User user, out string token)
        {

            token = string.Empty;

            var claims = new[] {
                new Claim(ClaimTypes.Name,user.UserName),
                new Claim(ClaimTypes.Role, "admin, Manage")
            };
            //密钥
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtModel.Secret));
            //凭证
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            //生成Token
            var jwtToken = new JwtSecurityToken(
                _jwtModel.Issuer,
                _jwtModel.Audience,
                claims,
                expires: DateTime.Now.AddMinutes(_jwtModel.AccessExpiration),
                signingCredentials:
                credentials);

            token = new JwtSecurityTokenHandler().WriteToken(jwtToken);
            return true;

        }
    }
}
