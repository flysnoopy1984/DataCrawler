using DataCrawler.Repository;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace DataCrawler.Framework.SetupServices
{
    public static  class RepositorySetup
    {
        public static void AddRepository(this IServiceCollection services, IConfiguration configuration)
        {
            var assembly = Assembly.Load("DataCrawler.Repository");

            var allRepos = assembly.GetTypes().Where(a => a.BaseType.Name.Contains("BaseRepository")).ToList();//.Where(a=>a.BaseType == typeof(BaseRepository)).ToList();
            

            foreach (var repo in allRepos)
            {
                services.AddScoped(repo);
            }
            services.AddScoped<DouBanBookService>();


        }
    }
}
