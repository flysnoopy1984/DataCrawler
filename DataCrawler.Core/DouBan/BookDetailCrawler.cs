using ContentCenter.Model;
using DataCrawler.Model;
using DataCrawler.Model.MiddleObject;
using DataCrawler.Repository.Core;
using DataCrawler.Util;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;


namespace DataCrawler.Core.DouBan
{
    public class BookDetailCrawler : DouBanBookBaseCrawlerData, ICrawlerBook
    {
        private BookDetail_middle _bookDetailData;
        private string _DouBanBookId;
        private string _entryUrl;

        public BookDetailCrawler()   
        {
            _bookDetailData = NewDetailMiddle();
        }
        public BookDetailCrawler(string douBanId)
        {
            _DouBanBookId = douBanId;
            _entryUrl = $"{DouBanBookPrefix}/subject/{douBanId}";
            _bookDetailData = NewDetailMiddle();
        }

       

        private void InitData(string entryUrl)
        {
  

            _entryUrl = entryUrl;
            int sp = _entryUrl.IndexOf("subject") + "subject/".Length;
            int ep = _entryUrl.IndexOf("/", sp + 1);
            if (ep == -1)
                ep = _entryUrl.Length;

            _DouBanBookId = _entryUrl.Substring(sp, ep - sp);

            _bookDetailData = NewDetailMiddle();
            _bookDetailData.DouBanBookInfo.SourceBookId = _DouBanBookId;
            _bookDetailData.DouBanBookInfo.Code = GenCodeHelper.Book_Code(DouBanAbbr, _DouBanBookId); //$"{DouBanAbbr}_{_DouBanBookId}";
        }

      


        public BookDetail_middle Crawler(string entryUrl = "")
        {
           
            InitData(entryUrl);
            NLogUtil.InfoTxt($"Crawler Book:${_entryUrl}");
            
           
            //List<BookDetail_middle> datas = new List<BookDetail_middle>();   
            //datas.Add(_bookDetailData);

            var bi = _bookDetailData.DouBanBookInfo;

        
            HtmlDocument htmlDoc = getDocbyEntryUrl(entryUrl);
            VerifyHeader(htmlDoc);

            var bn = htmlDoc.DocumentNode.SelectSingleNode("//div[@id='wrapper']/h1/span");
            if (bn == null)
                return null;
               // throw new Exception("No Book Title");
            bi.Title = bn.InnerText;
            var node = htmlDoc.DocumentNode.SelectSingleNode("//div[@id='mainpic']/a");
            bi.CoverUrl_Big = node.Attributes["href"].Value; //大图片

            node = node.SelectSingleNode("//img");
            bi.CoverUrl = node.Attributes["src"].Value; //小图片

            var info = htmlDoc.DocumentNode.SelectSingleNode("//div[@id='info']");
            var infoAttrs = info.SelectNodes(".//span");
            if (infoAttrs == null) return null;
            foreach(var span in infoAttrs)
            {
                try
                {
                    AnalyInfo(span);
                }
                catch(Exception ex)
                {
                    NLogUtil.ErrorTxt($"BookDetailCrawler Book Property-{_DouBanBookId}:[{span.InnerHtml}]{ex.Message}");
                }
               
            }
            
            AnalyContent(htmlDoc);  //简介

            AnalyAuthor(htmlDoc);  //作者

            AnalyCatalog(htmlDoc); //目录

            AnalyScore(htmlDoc); //分数

            AnalyTags(htmlDoc);  //Tags

            return _bookDetailData; 
        
        }



        private void AnalyInfo(HtmlNode node)
        {
            EBookInfo bi = _bookDetailData.DouBanBookInfo;
            string name = node.InnerText.Trim();
            string infoValue = node.NextSibling.InnerText.Trim();

            HtmlNode cNode = node.SelectSingleNode(".//span");
            if (cNode != null) 
                AnalyInfo_SpanChild(cNode);
            else
            {
                switch (name)
                {
                    case "出版社:":
                        bi.Publisher = infoValue;
                        break;
                    case "出品方:":
                        AnalyInfo_SpanChild(node);
                        //   bi.Producer = infoValue;
                        break;
                    case "副标题:":
                        bi.SubTitle = infoValue;
                        break;
                    case "原作名:":
                        bi.OrigTitle = infoValue;
                        break;
                    case "出版年:":
                        bi.PublishDate = infoValue;
                        break;
                    case "页数:":
                        bi.PageCount = infoValue;
                        break;
                    case "定价:":

                        bi.Pricing = infoValue;
                        break;
                    case "装帧:":
                        bi.Makeup = infoValue;
                        break;
                    case "丛书:":
                        AnalyInfo_SpanChild(node);
                        break;
                    case "ISBN:":
                        bi.ISBN = infoValue;
                        break;
                    case "作者:":
                        AnalyInfo_SpanChild(node);
                        break;
                }
            }
           
          
        }

        /// <summary>
        /// 处理 Span嵌套Span
        /// </summary>
        /// <param name="node"></param>
        /// <param name="bi"></param>
        private void AnalyInfo_SpanChild(HtmlNode node)
        {
            EBookInfo bi = _bookDetailData.DouBanBookInfo;
            EPerson author = _bookDetailData.Author;
            string name = node.InnerText.Trim();
            HtmlNode ci = node.NextSibling.NextSibling; ;
            string url = ci.Attributes["href"].Value;
            if (!url.StartsWith(DouBanBookPrefix))
                url = DouBanBookPrefix + url;

            switch (name)
            {
                case "作者:":
                case "作者":
                    var authorName = ci.InnerText.Trim();
                    if (authorName.StartsWith("["))
                    {
                        var ep = authorName.IndexOf("]");
                        author.Country= authorName.Substring(1, ep-1);
                        author.Name = authorName.Substring(ep+1);
                    }
                    if (authorName.StartsWith("<"))
                    {
                        var ep = authorName.IndexOf(">");
                        author.Country = authorName.Substring(1, ep - 1);
                        author.Name = authorName.Substring(ep + 1);
                    }
                    else if (authorName.StartsWith("("))
                    {
                        var ep = authorName.IndexOf(")");
                        author.Country = authorName.Substring(1, ep - 1);
                        author.Name = authorName.Substring(ep + 1);
                    }
                    else if (authorName.StartsWith("【"))
                    {
                        var ep = authorName.IndexOf("】");
                        author.Country = authorName.Substring(1, ep - 1);
                        author.Name = authorName.Substring(ep + 1);
                    }
                    else
                    {
                        author.Name = authorName;
                        author.Country = "中国";
                    }
                    author.Code = GenCodeHelper.Person_Code(author.Name.Trim());
                    bi.AuthorCode = author.Code;
                    author.SourceUrl = url;
                   
                    break;
                case "译者:":
                case "译者":
                    bi.Translater = ci.InnerText;
                    bi.TranslaterUrl = url;
                    break;
                case "丛书":
                case "丛书:":
                    bi.Series = ci.InnerText;
                    bi.SeriesUrl = url;
                    break;
                case "出品方":
                case "出品方:":
                    bi.Producer = ci.InnerText;
                    bi.ProducerUrl = url;
                    break;
            }
        }
       
        /// <summary>
        /// 书简介
        /// </summary>
        /// <param name="doc"></param>
        public void AnalyContent(HtmlDocument doc)
        {
            EBookInfo bi = _bookDetailData.DouBanBookInfo;
            string summery = "";
            try
            {
                var hiddenNode = doc.DocumentNode.SelectSingleNode("//div[@class='related_info']//span[@class='all hidden']//div[@class='intro']");
                if(hiddenNode != null)
                {
                    summery = hiddenNode.InnerHtml.Trim();
                }
                else
                {
                    var shortNode = doc.DocumentNode.SelectSingleNode("//div[@class='related_info']//div[@class='intro']");
                    if(shortNode!=null)
                        summery = shortNode.InnerHtml.Trim();
                }
                bi.Summery = summery;
                
            }
            catch(Exception ex)
            {
                NLogUtil.ErrorTxt("BookDetailCrawler AnalyContent:" + ex.Message);
            }
        }

        /// <summary>
        /// 目录
        /// </summary>
        /// <param name="doc"></param>
        public void AnalyCatalog(HtmlDocument doc)
        {
            if (!string.IsNullOrEmpty(_DouBanBookId))
            {
                var nodeId = $"dir_{_DouBanBookId}_full";
                var node = doc.DocumentNode.SelectSingleNode($"//div[@class='related_info']/div[@id='{nodeId}']");
                if(node!=null)
                {
                    var sp = node.InnerHtml.LastIndexOf("<br>")+"<br>".Length;
                    _bookDetailData.DouBanBookInfo.Catalog = node.InnerHtml.Remove(sp).Trim().Replace("\n","");
                }
            }
            
        }

        /// <summary>
        /// 作者
        /// </summary>
        /// <param name="doc"></param>
        public void AnalyAuthor(HtmlDocument doc)
        {
         //   var id = dir_34970027_full
        
            var n1 = doc.DocumentNode.SelectSingleNode("//div[@class='related_info']//h2/span[text()='作者简介']");
            if (n1 != null)
            {
                var nextNode= n1.ParentNode.SelectSingleNode(".//following-sibling::div[1]");
                var authNode = nextNode.SelectSingleNode(".//div[@class='intro']");
                if (authNode != null)
                    _bookDetailData.Author.Summery = authNode.InnerHtml.Trim();
            }
        }

        /// <summary>
        /// 分数
        /// </summary>
        /// <param name="doc"></param>
        public void AnalyScore(HtmlDocument doc)
        {
            var rate_num = doc.DocumentNode.SelectSingleNode("//strong[@class='ll rating_num ']");
            if(rate_num!=null)
            {
                var score = rate_num.InnerText.Trim();

                if (string.IsNullOrEmpty(score))
                    score = "0";
                _bookDetailData.DouBanBookInfo.Score = Convert.ToDouble(score);
            }
           

        }

        /// <summary>
        /// Tags
        /// </summary>
        /// <param name="doc"></param>
        public void AnalyTags(HtmlDocument doc)
        {
            int num = 3;
            var tagData = _bookDetailData.tagList;
            var tagNodes = doc.DocumentNode.SelectNodes("//div[@id='db-tags-section']/div[@class='indent']/span");
            if(tagNodes !=null && tagNodes.Count >0)
            {
                foreach(var tn in tagNodes)
                {
                    var tag = tn.SelectSingleNode(".//a").InnerText.Trim();
                    if (tag.Length > CrawlerSetting.Tag_LimitLenght && CrawlerSetting.Tag_IgnoreLongString) continue;
                    tagData.Add(newTag(tag));
                    num--;
                    if (num < 0) break;
                }
            }
        
        }

       

        public Task<BookDetail_middle> CrawlerAsync(string entryUrl)
        {
            //  BookDetail_middle result = Crawler(entryUrl);
            //  return result;
            try
            {
                var task = new Task<BookDetail_middle>(() =>Crawler(entryUrl));
                task.Start();
                return task;
            }
            catch(Exception ex)
            {
                throw ex;
            }
           
        }
    }
}
