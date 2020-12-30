using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataCrawler.Framework.SetupServices;
using DataCrawler.Model;
using DataCrawler.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;


namespace DataCrawler.API
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
            services.AddControllers();
            services.AddInitDbSeed(Configuration);
            services.AddSqlSugarSetup(Configuration);
            services.AddCrawlers(Configuration);
            services.AddRepository(Configuration);
            //  services.AddScoped<DouBanBookRepository>();
            #region  CROS
            services.AddCors(c =>
            {
                c.AddPolicy("AllowedRequest", p =>
                {
                    var sec = new string[] { "apiConfig", "Cors", "IPs" };

                    var origns = Configuration[string.Join(":", sec)].Split(',');
                    p.WithOrigins(origns)
                     .AllowAnyHeader()//Ensures that the policy allows any header.
                     .AllowAnyMethod();
                });
            });
            #endregion
            //Microsoft.AspNetCore.Mvc.NewtonsoftJson
            //  Microsoft.Extensions.DependencyInjection.NewtonsoftJsonMvcBuilderExtensions.AddNewtonsoftJson
            services.AddControllers()
                .AddJsonOptions(o =>
                {
                    o.JsonSerializerOptions.PropertyNamingPolicy = null;
                });
               //.AddNewtonsoftJson(options =>
               //{
               //    options.SerializerSettings.ContractResolver = new DefaultContractResolver();
               //    options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Local;  // 设置时区为 UTC)
               //       options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
               //});

        }



        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
        
            app.UseHttpsRedirection();

            app.UseCors("AllowedRequest");
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
