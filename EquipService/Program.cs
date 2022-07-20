using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace EquipService
{
    public class Program
    {
        public static string[] args1;
        public static void Main(string[] args)
        {
            args1 = args;
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseWindowsService()
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHostedService<Worker>();
                });
 
    }
}
