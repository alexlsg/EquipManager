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
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.FileProviders;
using System.IO;

namespace AntistaticApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = Tools.ConfigHelper.GetConfig();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            try
            {
                services.AddCors(options => options.AddPolicy("cors", p => p.AllowAnyMethod().SetIsOriginAllowed(_ => true).AllowAnyHeader().AllowCredentials()));
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
                        ValidateIssuer = false,//�Ƿ���֤Issuer
                        ValidateAudience = false,//�Ƿ���֤Audience
                        ValidateIssuerSigningKey = true,//�Ƿ���֤SigningKey
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(token.Secret)),//�õ�SigningKey
                        ValidIssuer = token.Issuer,
                        ValidAudience = token.Audience,
                        ClockSkew = TimeSpan.FromMinutes(0)//���û���ʱ�䣬token��������Чʱ��������ʱ�����jwt�Ĺ���ʱ�䣬���������Ĭ����5����
                    };
                });
                services.AddScoped<IAuthenticateService, AuthenticateService>();
                services.AddScoped<User>();
            }
            catch (Exception ex)
            {
                Tools.Log.Add(ex);
            }

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            try
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
                var provider = new FileExtensionContentTypeProvider();
                string _path = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;
                _path = Path.GetDirectoryName(_path);
                string _3wpath = Path.Combine(_path, @"wwwroot");
                if (!Directory.Exists(_3wpath))
                {
                    Directory.CreateDirectory(_3wpath);
                }
                app.UseStaticFiles(new StaticFileOptions()
                {
                    FileProvider = new PhysicalFileProvider(_3wpath),
                    ContentTypeProvider = provider
                });
                app.UseFileServer(new FileServerOptions()
                {
                    FileProvider = new PhysicalFileProvider(_3wpath),
                    EnableDirectoryBrowsing = true,
                    DefaultFilesOptions = {
                    DefaultFileNames = new List<string>(){ "/index.html"}
                    },
                    EnableDefaultFiles = true
                });
                app.UseRouting();
                app.UseCors("cors");
                app.UseAuthentication();//��֤
                app.UseAuthorization();//��Ȩ
                app.UseEndpoints(endpoints =>
                {
                    endpoints.MapControllers();
                });

            }
            catch (Exception ex)
            {
                Tools.Log.Add(ex);
            }
        }
    }
}
