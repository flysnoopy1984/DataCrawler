using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataCrawler.Core;
using DataCrawler.Core.DouBan;
using DataCrawler.Framework;
using DataCrawler.Model;
using DataCrawler.Model.Book.Search;
using DataCrawler.Model.MiddleObject;
using DataCrawler.Repository;
using IQB.Util.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace DataCrawler.API.Controllers
{
    [Route("book/[Action]")]
    [ApiController]
    public class DouBanController : BaseController
    {
        ICrawlerBatchBook _CrawlerLatest;
        ICrawlerBook _CrawlerDetail;
        ICrawlerSearchBook _CrawlerSearchBook;
        
        DouBanBookService _DouBanBookRepository;
        public DouBanController(IEnumerable<ICrawlerBatchBook> crawlerServices,
            ICrawlerBook crawlerData,
            ICrawlerSearchBook crawlerSearchBook,
        DouBanBookService douBanBookRepository)
        {
            _DouBanBookRepository = douBanBookRepository;
            _CrawlerLatest = crawlerServices.FirstOrDefault(a => a.GetType().Name == "BookLatestCrawler");
            _CrawlerDetail = crawlerData;
            _CrawlerSearchBook = crawlerSearchBook;
        }

        [HttpGet]
        public ResultEntity<List<BookDetail_middle>> NewExpressList()
        {
            ResultEntity<List<BookDetail_middle>> json = new ResultEntity<List<BookDetail_middle>>();
            try
            {
                json.Entity = _CrawlerLatest.Crawler();
                return json;
            }
            catch(Exception ex)
            {
                json.ErrorMsg = ex.Message;
            }
            return json;
        }

        [HttpGet]
        public ResultPager<ESearchOneBookResult> Search(string keyword)
        {
            ResultPager<ESearchOneBookResult> result = new ResultPager<ESearchOneBookResult>();
            try
            {
                result.PageData = _DouBanBookRepository.SearchBook(new QueryBookSearch
                {
                    keyword = keyword,
                    pageIndex = 1,
                    pageSize = SysConfig.SearchUseMaxLine
                });
                if (result.PageData.datas.Count == 0)
                {
                    result.PageData = _CrawlerSearchBook.CrawlerSearch(keyword, SysConfig.SearchUseMaxLine);
                    if (result.PageData.datas.Count > 0)
                    {
                        _DouBanBookRepository.SaveSearchResult(result.PageData.datas);
                    }
                }
            }
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
            }
            return result;
        }

        [HttpGet]
        public ResultEntity<BookDetail_middle> Detail(string dbCode)
        {
            ResultEntity<BookDetail_middle> json = new ResultEntity<BookDetail_middle>();
            try
            {
                string url = $"https://book.douban.com/subject/{dbCode}";
                json.Entity = _CrawlerDetail.Crawler(url);

                return json;
            }
            catch (Exception ex)
            {
                json.ErrorMsg = ex.Message;
            }
            return json;
        }
    }
}