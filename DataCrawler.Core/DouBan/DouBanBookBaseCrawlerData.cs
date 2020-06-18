using DataCrawler.Model;
using DataCrawler.Model.MiddleObject;
using DataCrawler.Util;
using HtmlAgilityPack;
using Microsoft.Extensions.Configuration;


using System;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;

namespace DataCrawler.Core.DouBan
{
    public class DouBanBookBaseCrawlerData:BaseCrawler
    {
        //DouBan简写
        public const string DouBanAbbr = "DB";
        public const string DouBanBookPrefix = "https://book.douban.com";
        public const string DouBanLatestUrl = "https://book.douban.com/latest?icn=index-latestbook-all";
        public const string DouBanTagsUrl = "https://book.douban.com/tag/?view=type";//https://book.douban.com/tag/?view=cloud
        public BookDetail_middle NewDetailMiddle()
        {
            var result =  new BookDetail_middle();
            result.DouBanBookInfo.DataSource = Model.BaseEnums.DataSource.DouBan;
            return result;
        }

        public ETag newTag(string Code)
        {
            return new ETag
            {
                Code = Code.ToPinYin().ToLowerInvariant(),
                Name = Code,
                Type = Model.BaseEnums.TagType.Book,
                Url = $"/tag/{Code}",
            };
        }

        protected void VerifyHeader(HtmlDocument htmlDoc)
        {
            var titleNode = htmlDoc.DocumentNode.SelectSingleNode("//title");
            if (titleNode != null)
            {
                var title = titleNode.InnerText;
                if (title != null && title.Contains("407 Proxy Authentication Required"))
                {
                    throw new ExceptionProxyConnect("飞猪代理错误！407 Proxy Authentication Required");
                }
            };
           
        }

        public HtmlDocument getDocbyEntryUrl(string entryUrl)
        {
            HtmlDocument htmlDoc = null;
            try
            {
                htmlDoc = newLoadWeb(entryUrl);
            }
            catch(ExceptionProxyConnect epc)
            {
                throw epc;
            }
            catch
            {
                NLogUtil.InfoTxt("Connect Error,Auto Try Next Proxy Connect");
                try
                {

                    ProxyManager.RemoveProxyHostCache();
                    htmlDoc = newLoadWeb(entryUrl);
                }
                catch(Exception ex)
                {
                    throw new ExceptionProxyConnect("Connect Error while LoadDoc");
                }
              
                
            }
           
            return htmlDoc;
        }

        private HtmlDocument newLoadWeb(string url)
        {
            HtmlWeb htmlWeb = new HtmlWeb();
            htmlWeb.OverrideEncoding = Encoding.UTF8;
            htmlWeb.UserAgent = "Mozilla/5.0 (Windows; U; Windows NT 5.1; en-US; rv:1.9.2) Gecko/20100115 Firefox/3.6";

            var c = GetConfiguration();
            HtmlDocument htmlDoc = null;
            if (Convert.ToBoolean(c["ProxyMsg:NeedProxy"]) )
            {
                WebProxy webProxy = newWebProxy();
               

                htmlDoc = htmlWeb.Load(url, "Get", webProxy, null);
            }
            else
                htmlDoc = htmlWeb.Load(url, "Get");


            return htmlDoc;
        }

        private IConfiguration GetConfiguration()
        {
            
            string currentDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var builder = new ConfigurationBuilder();
             return  builder.SetBasePath(currentDirectory)
                             .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                             .Build();
        }
        private WebProxy newWebProxy(int c =0)
        {
            var PorxyHost = ProxyManager.getProxyHost();
            if (string.IsNullOrEmpty(PorxyHost)) 
                throw new ExceptionProxyConnect("No Proxy Host Got");

            WebProxy webProxy = new WebProxy(PorxyHost);
            return webProxy;
        }

        public BookDetail_middle ToDetail(string url,ICrawlerBook crawlerBook)
        {
        
           var r = crawlerBook.Crawler(url);
            return r;


        }

        #region 特殊处理
        //public void SpecalAuthorName(string Name)
        //{
        //    34845963
        //}
        #endregion






    }
}
