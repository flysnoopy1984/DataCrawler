using DataCrawler.Core;
using DataCrawler.Framework;
using DataCrawler.Framework.SetupServices;
using DataCrawler.Model;
using DataCrawler.Repository;
using DataCrawler.Tasks.DouBan;
using DataCrawler.Util;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DataCrawler.Tasks
{
    class Program
    {
        private static readonly IConfigurationBuilder Configuration = new ConfigurationBuilder();
        private static IConfigurationRoot _configuration;
        private static DouBanBookService _DouBanBookRepository;

        private static ServiceProvider _ServiceProvider;
        private static ICrawlerBatchBook _LatestBatchBook;
        private static ICrawlerBatchBook _TagList;
        private static ICrawlerBook _DetailBook;
        private static ICrawlerTag _CrawlerTag;
        private static SqlSugarClient _Db;
        static void Main(string[] args)
        {
            Init();



             RunSingle();
             //  RunLatestTask();


          //    test();
           // RunPlan();
            //  Console.WriteLine("Done");
            Console.ReadLine();
        }

        private static void RunPlan()
        {
            PlanFromTagsTask planFromTagsTask = new PlanFromTagsTask(
                _CrawlerTag, 
                _TagList,
                _DetailBook,
                _DouBanBookRepository);
            planFromTagsTask.run();
        }

        private static void RunSingle()
        {

            //string url = "https://book.douban.com/subject/34845963/";
            string url = "https://book.douban.com/subject/2669319/";
            SinglgTask singlgTask = new SinglgTask(_DetailBook, _DouBanBookRepository);
            singlgTask.runAsync(url);
        }

        private static void RunLatestTask()
        {
            try
            {
                LatestTask latestTask = new LatestTask(_LatestBatchBook, _DetailBook, _DouBanBookRepository);
                latestTask.Run();
            }
            catch(Exception ex)
            {
                NLogUtil.ErrorTxt($"RunLatestTask Error:{ex.Message}", true);
            }
        
        } 

        private  static void test()
        {
            ESection se = new ESection();
            se.Code = "A•B";
            se.Title = se.Code;
           var db = _ServiceProvider.GetService<SectionRepository>();

          //  db.Add(se);
            // var list =  _TagList.CrawlerUrls("");
            //   var list = _TagList.Crawler("https://book.douban.com/tag/小说?start=0&type=T");
            //   var a =  sectionRepository.QueryList(null, null, false,true);
            //ICrawlerTag crawlerTag = _ServiceProvider.GetService<ICrawlerTag>();
            //var st = crawlerTag.getUrls("");

            //string s = "1932~1942年美国驻日大使约瑟夫·C.格鲁的日记及公私文件摘录";
            //Console.WriteLine(s.Length);
            /*  List<EDataSection> list = new List<EDataSection>();
              for(int i=1;i<20;i++)
              {
                  list.Add(new EDataSection()
                  {
                      ItemCode = $"testItemCode{i}",
                      SectionCode = $"testCode{i}",
                  });
              }
              var db = _ServiceProvider.GetService<ISqlSugarClient>();
              try
              {
                  db.Ado.BeginTran();
                  _DouBanBookRepository.HandleDataSection(list);
                  db.Ado.CommitTran();
              }
              catch(Exception ex)
              {
                  db.Ado.RollbackTran();
              }
            */
            //   DateTime.Now.Ticks
            //await _DouBanBookRepository.
        }

        private static void InitSystem()
        {
            try
            {
                dynamic type = (new Program()).GetType();
                string currentDirectory = Path.GetDirectoryName(type.Assembly.Location);


                _configuration = Configuration.SetBasePath(currentDirectory)
                      .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                      .Build();


                var serviceCollection = new ServiceCollection();
                serviceCollection.AddSqlSugarSetup(_configuration);
                serviceCollection.AddCrawlers(_configuration);
                serviceCollection.AddRepository(_configuration);
                serviceCollection.AddInitDbSeed(_configuration);

                _ServiceProvider = serviceCollection.BuildServiceProvider();
                _DouBanBookRepository = _ServiceProvider.GetService<DouBanBookService>();
                _Db = _ServiceProvider.GetService<SqlSugarClient>();

                var list = _ServiceProvider.GetService<IEnumerable<ICrawlerBatchBook>>();
                _LatestBatchBook = list.FirstOrDefault(a => a.GetType().Name == "BookLatestCrawler");
                _TagList = list.FirstOrDefault(a => a.GetType().Name == "TagListCrawler");
                _DetailBook = _ServiceProvider.GetService<ICrawlerBook>();
                _CrawlerTag = _ServiceProvider.GetService<ICrawlerTag>();
            }
            catch(Exception ex)
            {
                NLogUtil.ErrorTxt($"Init Error:{ex.Message}",true);
            }
         
            


        }

        private static void Init()
        {
            try
            {
                Console.WriteLine("Init");
                InitSystem();
                DbSeed.InitTables(_Db);
                DbSeed.InitData(_DouBanBookRepository);
            }
            catch (Exception ex)
            {
                NLogUtil.ErrorTxt($"Init Error:{ex.Message}", true);
            }
        }
       
    }
}
