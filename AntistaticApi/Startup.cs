using AntistaticApi.Model;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using AntistaticApi.IdentityService;
using UserManagement;
using Ubiety.Dns.Core;
using Microsoft.AspNetCore.Http;

namespace AntistaticApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options => options.AddPolicy("cors", p => p.AllowAnyMethod().SetIsOriginAllowed(_=>true).AllowAnyHeader().AllowCredentials()));
            services.AddControllers();
            services.Configure<tokenModel>(Configuration.GetSection("JWTTokenconfig"));
            var token = Configuration.GetSection("JWTTokenconfig").Get<tokenModel>();
            services.AddAuthentication(option =>
            {
                option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(option =>
            {
                option.RequireHttpsMetadata = false;
                option.SaveToken = true;
                option.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,//是否验证Issuer
                    ValidateAudience = false,//是否验证Audience
                    ValidateIssuerSigningKey = true,//是否验证SigningKey
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(token.Secret)),//拿到SigningKey
                    ValidIssuer = token.Issuer,
                    ValidAudience = token.Audience,
                    ClockSkew = TimeSpan.FromMinutes(0)//设置缓冲时间，token的总有有效时间等于这个时间加上jwt的过期时间，如果不配置默认是5分钟
                };
            });
            services.AddScoped<IAuthenticateService, AuthenticateService>();
            services.AddScoped<User>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            //app.UseHttpsRedirection();
            app.Use((context, next) =>
            {
                context.Request.EnableBuffering();
                return next();
            });
            app.UseRouting();
            app.UseAuthentication();//认证
            app.UseAuthorization();//授权
            app.UseCors("cors");
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
