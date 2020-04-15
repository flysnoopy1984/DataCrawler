using DataCrawler.Model.MiddleObject;
using DataCrawler.Util;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataCrawler.Core.DouBan
{
    public class BookTagsCrawler : DouBanBookBaseCrawlerData, ICrawlerTag
    {
        public List<Secction_Tag> getUrls(string entryUrl)
        {
            if (string.IsNullOrEmpty(entryUrl)) entryUrl = DouBanTagsUrl;
            List<Secction_Tag> result = new List<Secction_Tag>();
            NLogUtil.InfoTxt("开始抓爬豆瓣  All Tag");
            try
            {
                HtmlDocument doc = getDocbyEntryUrl(entryUrl);
                var root = doc.DocumentNode.SelectNodes("//div[@class='article']/div")[1];
                var secNodes = root.SelectNodes(".//div");
                foreach(var sec in secNodes)
                {
                    var secInfo = sec.SelectSingleNode(".//a");
                    Secction_Tag secTag = new Secction_Tag();
                    var sname = secInfo.Attributes["name"].Value;
                  //  sname = sname.Replace(".", "");
                    secTag.sectionName = sname;
                    result.Add(secTag);
                    var allHref = sec.SelectNodes(".//table[@class='tagCol']//a");
                    foreach(var tagnode in allHref)
                    {
                        secTag.TagList.Add(newTag(tagnode.InnerText.Trim()));
                    }
                  
                }

            }
            catch(Exception ex)
            {
                NLogUtil.InfoTxt($"抓爬豆瓣错误:{ex.Message}");
            }
            NLogUtil.InfoTxt("抓爬豆瓣Tag结束");
            return result;
        }
    }
}
