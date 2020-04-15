using DataCrawler.Model.MiddleObject;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataCrawler.Core.DouBan
{
    public class BookPopularCrawler: BaseCrawler, ICrawlerBatchBook
    {
        public BookPopularCrawler()
        
        {

        }

        public List<BookDetail_middle> Crawler(string entryUrl = "https://book.douban.com/")
        {
            throw new NotImplementedException();
        }

        public List<BookBatch> CrawlerUrls(string entryUrl)
        {
            //https://www.douban.com/feed/review/book

            throw new NotImplementedException();
        }
    }
}
