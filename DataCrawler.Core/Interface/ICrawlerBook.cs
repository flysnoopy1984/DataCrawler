using DataCrawler.Model.MiddleObject;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DataCrawler.Core
{
    public interface ICrawlerBook
    {

        Task<BookDetail_middle> CrawlerAsync(string entryUrl);
        BookDetail_middle Crawler(string entryUrl);
    }
}
