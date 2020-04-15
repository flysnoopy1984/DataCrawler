using DataCrawler.Core;
using DataCrawler.Core.DouBan;
using DataCrawler.Model.MiddleObject;
using DataCrawler.Repository;
using DataCrawler.Util;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataCrawler.Tasks.DouBan
{
    public class PlanFromTagsTask: BaseTask
    {

        private readonly ICrawlerTag _TagsCrawler;
        private ICrawlerBook _DetailCrawler;
        private ICrawlerBatchBook _TagListCrawler;
        private DouBanBookService _DouBanBookRepository;
        public PlanFromTagsTask(ICrawlerTag crawlerTag,
                          ICrawlerBatchBook crawlerTagList,
                          ICrawlerBook crawlerBook,
                          DouBanBookService douBanBookRepository)
        {
            _DouBanBookRepository = douBanBookRepository;
            _TagsCrawler = crawlerTag;
            _DetailCrawler = crawlerBook;
            _TagListCrawler = crawlerTagList;

        }
        public void run()
        {
            NLogUtil.InfoTxt("开始豆瓣爬书计划");

            var taglist = _TagsCrawler.getUrls("");
            if(taglist != null && taglist.Count>0)
            {
               var allList =  _DouBanBookRepository.InitPlanFromTagUrl(taglist);
                foreach(var plan in allList)
                {
                    try
                    {
                        while(plan.ProcessPageIndex< CrawlerSetting.DB_MaxIndex_TagList)
                        {
                            NLogUtil.InfoTxt($"豆瓣爬书计划-TagCode:{plan.Code},Index:{plan.ProcessPageIndex}");
                            var url = $"{DouBanBookBaseCrawlerData.DouBanBookPrefix}/tag/{plan.TagCode}?start={plan.ProcessPageIndex}&type=T";
                            List<BookDetail_middle> bnList = _TagListCrawler.Crawler(url);
                            HandleBookMiddleList(bnList);

                            plan.ProcessPageIndex += CrawlerSetting.DB_TagList_Step;
                            _DouBanBookRepository.UpdatePlan(plan);
                        }
                     


                    }
                    catch(Exception ex)
                    {
                        NLogUtil.ErrorTxt($"【错误】豆瓣爬书计划-TagList:{ex.Message}");
                    }
                 }
            } 
        }

        public async void HandleBookMiddleList(List<BookDetail_middle> bookMiddleList)
        {
            foreach(var book in bookMiddleList)
            {
                await _DouBanBookRepository.HandleBookMiddleAsync(book);
            }
        }
    }
}
