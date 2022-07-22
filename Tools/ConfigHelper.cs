using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Tools
{
    public static class ConfigHelper
    {
        public static string GetConfigString(string url)
        {
            string _path = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;
            _path = Path.GetDirectoryName(_path);
            var _config = new ConfigurationBuilder().SetBasePath(_path).AddJsonFile("appsettings.json");
            IConfiguration _cf = _config.Build();
            //判断连接串是否加密
            return _cf[url];
        }
    }
}
