using DataCrawler.Model.MiddleObject;
using DataCrawler.Util;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataCrawler.Core.DouBan
{
    public class TagListCrawler : DouBanBookBaseCrawlerData, ICrawlerBatchBook
    {
        private ICrawlerBook _CrawlerBook;
        public TagListCrawler(ICrawlerBook crawlerBook)
        {
            _CrawlerBook = crawlerBook;
        }
        public List<BookDetail_middle> Crawler(string entryUrl = "")
        {
            NLogUtil.InfoTxt($"[开始]抓爬TagList{entryUrl}");
            var urlList = CrawlerUrls(entryUrl); 

            List<BookDetail_middle> result = new List<BookDetail_middle>();

            foreach (var bookUrl in urlList)
            {
               var bd = ToDetail(bookUrl.DetailUrl, _CrawlerBook);
               
                result.Add(bd);
            }
            NLogUtil.InfoTxt($"[结束]抓爬TagList{entryUrl}");

            return result;
        }

        public List<BookBatch> CrawlerUrls(string entryUrl)
        {
            if (string.IsNullOrEmpty(entryUrl)) NLogUtil.ErrorTxt("TagListCrawler 没有入口Url"); 
            List<BookBatch> result = new List<BookBatch>();
           
            var doc = getDocbyEntryUrl(entryUrl);
            var nodes = doc.DocumentNode.SelectNodes("//div[@class='article']//ul[@class='subject-list']/li");
            if(nodes !=null)
            {
                foreach (var n in nodes)
                {
                    var url = n.SelectSingleNode("./div/a").Attributes["href"].Value;
                    result.Add(new BookBatch
                    {
                        DetailUrl = url
                    });
                }
            }
           

            return result;
        }
    }
}
