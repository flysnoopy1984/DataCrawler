using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataCrawler.Framework;
using DataCrawler.Model;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Web;
using SqlSugar;

namespace DataCrawler.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args)
                .ConfigureLogging(log=> {
                    log.ClearProviders();
                    log.SetMinimumLevel(LogLevel.Trace);
                }).UseNLog().Build();
            try
            {
                RunExternalCode(host);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in RunExternalCode:{ex.Message}");
            }
            
            host.Run();
        }

        public static void RunExternalCode(IHost host)
        {
            /* 启动时初始化数据库（如果需要） */
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var config = services.GetRequiredService<IConfiguration>();
                bool needDbInit = Convert.ToBoolean(config.GetSection("InitTask")["NeedDbInit"]);
                if (needDbInit)
                {
                    ISqlSugarClient db = services.GetRequiredService<SqlSugarClient>();
                    DbSeed.InitTables(db);

                }
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
