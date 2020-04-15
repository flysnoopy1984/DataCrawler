using DataCrawler.Model;
using DataCrawler.Model.MiddleObject;
using DataCrawler.Util;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
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

        public HtmlDocument getDocbyEntryUrl(string entryUrl)
        {
            HtmlWeb htmlWeb = new HtmlWeb();
            htmlWeb.OverrideEncoding = Encoding.UTF8;
            HtmlDocument htmlDoc = htmlWeb.Load(entryUrl);
            return htmlDoc;
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
