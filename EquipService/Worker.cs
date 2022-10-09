using AntistaticApi;
using EquipDataManager.Bll;
using EquipDataManager.Relealise;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using Tools;

namespace EquipService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }
        static int port;
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            DataPicker.Instance.Start(new DataPickerRealise());
            //DataPicker.Instance.LoadFile();
            Log.Add("���ݷ�����������");
            SocketListen.Instance.Start();
            Log.Add("TCP�����ѿ���!");
            try
            {
                port = int.Parse(ConfigHelper.GetConfigString("port"));
                Log.Add($"��վ������,�˿�{port}��");
                CreateWebHostBuilder(Program.args1).Build().Run();
            }
            catch (Exception ex)
            {
                Log.Add(ex.Message);
            }
        }

        public static IHostBuilder CreateWebHostBuilder(string[] args) => Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.ConfigureKestrel(serverOptions =>
                  {
                      serverOptions.ListenAnyIP(port);
                  }).UseStartup<Startup>();
            });
    }
}
