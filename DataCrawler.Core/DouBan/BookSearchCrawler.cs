
using DataCrawler.Model.Book.Search;
using HtmlAgilityPack;
using IQB.Util.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataCrawler.Core.DouBan
{
    public class BookSearchCrawler : DouBanBookBaseCrawlerData,ICrawlerSearchBook
    {

        public static string getSearchUrlByKeyword(string keyword)
        {
            return $"https://www.douban.com/search?cat=1001&q={keyword}";
        }

        public ModelPager<ESearchOneBookResult> CrawlerSearch(string keyword,int maxUserLine = 5)
        {
            ModelPager<ESearchOneBookResult> result = new ModelPager<ESearchOneBookResult>();
            var searchUrl = getSearchUrlByKeyword(keyword);

            HtmlDocument htmlDoc = getDocbyEntryUrl(searchUrl);

            var nodeResultList = htmlDoc.DocumentNode.SelectNodes("//div[@class='result-list']/div[@class='result']");
            if (nodeResultList == null) return result;

            result.datas = new List<ESearchOneBookResult>();
            foreach (var nodeResult in nodeResultList)
            {
                ESearchOneBookResult resultItem = new ESearchOneBookResult();
               var imgNode =  nodeResult.SelectSingleNode("div[@class='pic']/a/img");
                resultItem.CoverUrl = imgNode.Attributes["src"].Value;

                var hrefNode = nodeResult.SelectSingleNode("div[@class='content']/div/h3/a");
                resultItem.Name = hrefNode.InnerText;

                var onClickString = hrefNode.Attributes["onclick"].Value;
                resultItem.Code = getCodeFromSearchOnClickString(onClickString);
                resultItem.keyWord = keyword;
                
                result.datas.Add(resultItem);
                if (result.datas.Count == maxUserLine) break;
            }
            return result;
        }

        private string getCodeFromSearchOnClickString(string onClickString)
        {
            int sPos = onClickString.IndexOf("sid");
            int ePos = onClickString.IndexOf(", ", sPos+1);
            int sPosLenth = sPos + 5;
            string eStr = onClickString.Substring(ePos);
            var eStrLen = eStr.Length;
            //      int ePosLenth = 

            string r = onClickString.Substring(sPosLenth, onClickString.Length - eStrLen - sPosLenth);
            return r;
        }
    }
}
