using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataCrawler.Core;
using DataCrawler.Core.DouBan;
using DataCrawler.Model;
using DataCrawler.Model.MiddleObject;
using DataCrawler.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace DataCrawler.API.Controllers
{
    [Route("[controller]/book/[Action]")]
    [ApiController]
    public class DouBanController : BaseController
    {
        ICrawlerBatchBook _CrawlerLatest;
        ICrawlerBook _CrawlerDetail;
        DouBanBookService _DouBanBookRepository;
        public DouBanController(IEnumerable<ICrawlerBatchBook> crawlerServices,
            ICrawlerBook crawlerData,
            DouBanBookService douBanBookRepository)
        {
            _DouBanBookRepository = douBanBookRepository;
            _CrawlerLatest = crawlerServices.FirstOrDefault(a => a.GetType().Name == "BookLatestCrawler");
            _CrawlerDetail = crawlerData;
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
        public async Task<ResultEntity<BookDetail_middle>> Detail()
        {
            ResultEntity<BookDetail_middle> json = new ResultEntity<BookDetail_middle>();
            try
            {
                string url = "https://book.douban.com/subject/25917736";
                json.Entity = _CrawlerDetail.Crawler(url);
             //   await _DouBanBookRepository.HandleBookMiddleAsync(json.Entity);
               // json.Message = toJson(_CrawlerDetail.Crawler(url));
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