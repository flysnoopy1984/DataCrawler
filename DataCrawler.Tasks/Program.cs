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
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;

namespace DataCrawler.Tasks
{
   
    class Program
    {
        private static readonly IConfigurationBuilder Configuration = new ConfigurationBuilder();
        private static IConfiguration _configuration;
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



           //  RunSingle();
          RunLatestTask();


        //     test();
        //   RunPlan();
            //  Console.WriteLine("Done");
         //   TestProxy();
            Console.ReadLine();
        }

        private static void RunPlan()
        {
            int tryNum = 1;
            while(tryNum<3)
            {
                try
                {
                    _DouBanBookRepository = _ServiceProvider.GetService<DouBanBookService>();

                
                    var list = _ServiceProvider.GetService<IEnumerable<ICrawlerBatchBook>>();
                
                    _TagList = list.FirstOrDefault(a => a.GetType().Name == "TagListCrawler");
                    _DetailBook = _ServiceProvider.GetService<ICrawlerBook>();
                    _CrawlerTag = _ServiceProvider.GetService<ICrawlerTag>();

                    PlanFromTagsTask planFromTagsTask = new PlanFromTagsTask(
                    _CrawlerTag,
                    _TagList,
                    _DetailBook,
                    _DouBanBookRepository);

                    planFromTagsTask.run();
                }
                catch (ExceptionProxyConnect epc)
                {
                    NLogUtil.ErrorTxt($"代理连接错误:{epc.Message}");
                    NLogUtil.InfoTxt($"开始尝试第{tryNum++}次运行计划");

                }
            }
            NLogUtil.InfoTxt($"第{tryNum}次运行计划后结束");

        }

        private static void RunSingle()
        {

            //string url = "https://book.douban.com/subject/34845963/";
           
            string url = "https://book.douban.com/subject/2669319/";
            try
            {
                SinglgTask singlgTask = new SinglgTask(_DetailBook, _DouBanBookRepository);
                singlgTask.runAsync(url);
            }
            catch (ExceptionProxyConnect epc)
            {
                NLogUtil.ErrorTxt($"代理连接错误:{epc.Message}");
            }

        }

        private static void RunLatestTask()
        {
            try
            {
                LatestTask latestTask = new LatestTask(_LatestBatchBook, _DetailBook, _DouBanBookRepository);
                latestTask.Run();
            }
            catch(ExceptionProxyConnect epc)
            {
                NLogUtil.ErrorTxt($"代理连接错误:{epc.Message}");
            }
            catch(Exception ex)
            {
                NLogUtil.ErrorTxt($"RunLatestTask Error:{ex.Message}", true);
            }
        
        } 

        private static void TestProxy()
        {
            string url = "http://webapi.http.zhimacangku.com/getip?num=1&type=1&pro=&city=0&yys=0&port=1&pack=92851&ts=0&ys=0&cs=0&lb=1&sb=0&pb=4&mr=1&regions=";
            //   HttpWebRequest req = (HttpWebRequest)WebRequest.Create("http://webapi.http.zhimacangku.com/getip?num=1&type=1&pro=&city=0&yys=0&port=1&pack=92851&ts=0&ys=0&cs=0&lb=1&sb=0&pb=4&mr=1&regions=");
            HttpClient httpClient = new HttpClient();
            var rep = httpClient.GetAsync(url);
            var json = rep.Result.Content.ReadAsStringAsync();
            WebProxy proxyObject = new WebProxy(json.Result);//str为IP地址 port为端口号
            HttpWebRequest ReqProxy = (HttpWebRequest)WebRequest.Create("http://www.baidu.com/");
            ReqProxy.Proxy = proxyObject; //设置代理 
            HttpWebResponse Resp = (HttpWebResponse)ReqProxy.GetResponse();
            string str = "";
            string OkStr = "";
            Encoding code = Encoding.GetEncoding("UTF-8");
            using (StreamReader sr = new StreamReader(Resp.GetResponseStream(), code))
            {
                if (sr != null)
                {
                    try
                    {
                        str = sr.ReadToEnd();
                      //  StringSub.substr(str, "<h2>", "</h2>", 0);
                        //str = str.Substring(str.IndexOf(start) + start.Length);
                        //OkStr = str.Substring(0, str.IndexOf(last));
                       Console.WriteLine("验证成功！显示IP为" + OkStr);
                    }
                    catch
                    {
                        Console.WriteLine("文件读取失败！");
                    }
                    finally
                    {
                        sr.Close();
                    }
                }
            }
        }
        private  static void test()
        {
            ProxyManager.AddFeiZhuHost();

            //try
            //{
            //    throw new ExceptionProxyConnect("aaa");
            //}
            //catch(ExceptionProxyConnect epc)
            //{
            //    throw epc;
            //}
            //catch(Exception ex)
            //{
            //    throw ex;
            //}

            //ProxyManager.TestCache();
            //Thread.Sleep(1000 * 10);
            //ProxyManager.TestCache();
            //   ProxyManager.AddBeiKeHost();
            //    var list = ProxyManager.ProxyAddList_89ip();
            // ESection se = new ESection();
            // se.Code = "A•B";
            // se.Title = se.Code;
            //var db = _ServiceProvider.GetService<SectionRepository>();

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
                serviceCollection.AddMemoryCache();
              //  serviceCollection.AddScoped<IConfiguration>(_configuration);
                   //   

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
                var needInitSection = Convert.ToBoolean(_configuration["InitTask:NeedInitSection"]);
                DbSeed.InitData(_DouBanBookRepository, needInitSection);
            }
            catch (Exception ex)
            {
                NLogUtil.ErrorTxt($"Init Error:{ex.Message}", true);
            }
        }
       
    }
}
