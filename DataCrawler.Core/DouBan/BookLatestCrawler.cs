using DataCrawler.Model;
using DataCrawler.Model.MiddleObject;
using HtmlAgilityPack;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataCrawler.Core.DouBan
{

   
    public class BookLatestCrawler: DouBanBookBaseCrawlerData, ICrawlerBatchBook
    {

        private ICrawlerBook _CrawlerBook;
        public BookLatestCrawler(ICrawlerBook crawlerBook)   
        {
            _CrawlerBook = crawlerBook;
        }

     
        public List<BookDetail_middle> Crawler(string entryUrl = "")
        {
            if (string.IsNullOrEmpty(entryUrl)) entryUrl = DouBanLatestUrl;

            var urlList = CrawlerUrls(entryUrl); //获取最新书的Urls

            List<BookDetail_middle> result = new List<BookDetail_middle>();

            foreach(var bookUrl in urlList)
            {
                var bd = ToDetail(bookUrl.DetailUrl, _CrawlerBook);
                if(bd != null)
                {
                    bd.DouBanBookInfo.FictionType = bookUrl.FictionType;
                    result.Add(bd);
                }
              
            }

          
            return result;
         
        }

        public List<BookBatch> CrawlerUrls(string entryUrl)
        {
            if (string.IsNullOrEmpty(entryUrl)) entryUrl = DouBanLatestUrl;

            List<BookBatch> result = new List<BookBatch>();

            HtmlAgilityPack.HtmlDocument htmlDoc = getDocbyEntryUrl(entryUrl);
            var fiction = htmlDoc.DocumentNode.SelectNodes("//div[@class='article']/ul/li");

            foreach (var item in fiction)
            {
                var url = item.SelectSingleNode(".//a").Attributes["href"].Value;
                result.Add(new BookBatch()
                {
                    DetailUrl = url,
                    FictionType = Model.BaseEnums.FictionType.Fiction,
                    SectionCode = "NewExpress",
                    
                }) ;
                //    jArray.Add(new JObject { { "Url", url } });
            }

            var aside = htmlDoc.DocumentNode.SelectNodes("//div[@class='aside']/ul/li");
            foreach (var item in aside)
            {
                var url = item.SelectSingleNode(".//a").Attributes["href"].Value;
                result.Add(new BookBatch()
                {
                    DetailUrl = url,
                    FictionType = Model.BaseEnums.FictionType.Aside,
                    SectionCode = "NewExpress",

                });
                //  jArray.Add(new JObject { { "Url", url } });
            }
            return result;
        }

        /// <summary>
        /// 从首页进入
        /// </summary>
        private string EntryFromIndexPage(string entryUrl)
        {
            JArray jArray = new JArray();

            HtmlWeb htmlWeb = new HtmlWeb();
            htmlWeb.OverrideEncoding = Encoding.UTF8;
            HtmlAgilityPack.HtmlDocument htmlDoc = htmlWeb.Load(entryUrl);

          
             var list = htmlDoc.DocumentNode.SelectSingleNode("//div[@class='section books-express ']/div[@class='bd']/div[@class='carousel']/div[@class='slide-list']");
            var targetNode = list.ChildNodes[1].SelectNodes(".//li");
            foreach (var item in targetNode)
            {
                var detailUrlNode = item.SelectSingleNode(".//div[@class='cover']/a");
                var url = detailUrlNode.Attributes["href"].Value;
                var name = detailUrlNode.Attributes["title"].Value;


                jArray.Add(new JObject { { "Name", name }, { "Url", url } });
            }

            //    DouBanBookInfo entity = new DouBanBookInfo();


            return jArray.ToString();
        }

       

    }
}
