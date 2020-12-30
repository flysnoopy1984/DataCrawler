using DataCrawler.Model.Book.Search;
using IQB.Util.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataCrawler.Core
{
    public interface ICrawlerSearchBook
    {
        ModelPager<ESearchOneBookResult> CrawlerSearch(string searchUrl, int maxUserLine = 5);
    }
}
