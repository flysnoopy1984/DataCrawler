using DataCrawler.Core;
using DataCrawler.Core.DouBan;
using DataCrawler.Core.Other;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataCrawler.Framework.SetupServices
{
    public static class CrawlerSetup
    {
        public static void AddCrawlers(this IServiceCollection services, IConfiguration configuration)
        {
            //    var type = System.Reflection.Assembly
            services.AddScoped<ICrawlerBatchBook,BookLatestCrawler>();
            services.AddScoped<ICrawlerBatchBook,BookPopularCrawler>();
            services.AddScoped<ICrawlerBook, BookDetailCrawler>();
            services.AddScoped<ICrawlerTag, BookTagsCrawler>();
            services.AddScoped<ICrawlerBatchBook, TagListCrawler>();
            services.AddScoped<ICrawlerSearchBook, BookSearchCrawler>();
            services.AddScoped<DaReTouCrawler>();
         
        }
    }
}
