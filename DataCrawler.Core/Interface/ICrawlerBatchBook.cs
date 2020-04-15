using DataCrawler.Model.MiddleObject;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataCrawler.Core
{
    public interface ICrawlerBatchBook
    {
        public List<BookDetail_middle> Crawler(string entryUrl = "");

        public List<BookBatch> CrawlerUrls(string entryUrl);
    }
}
