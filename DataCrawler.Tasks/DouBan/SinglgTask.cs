using DataCrawler.Core;
using DataCrawler.Model.MiddleObject;
using DataCrawler.Repository;
using DataCrawler.Util;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataCrawler.Tasks.DouBan
{
    public class SinglgTask: BaseTask
    {
        private ICrawlerBook _DetailCrawler;
        private DouBanBookService _DouBanBookRepository;
        public SinglgTask(ICrawlerBook crawlerDetail,
                        DouBanBookService douBanBookRepository)
        {
            _DouBanBookRepository = douBanBookRepository;
            _DetailCrawler = crawlerDetail;

        }

        public async  void runAsync(string url)
        {
            NLogUtil.InfoTxt($"开始抓爬单本书:{url}");
            BookDetail_middle task_midData = null;
            try
            {
                task_midData = await _DetailCrawler.CrawlerAsync(url);
            }
            catch(ExceptionProxyConnect epc)
            {
                throw epc;
            }
           

            NLogUtil.InfoTxt($"抓爬结束");
        //    _DouBanBookRepository.Test(task_midData);
         //   var rBook = _BookDb.AddOrUpdate_MasterData<EBookInfo>(middle.DouBanBookInfo);
           await _DouBanBookRepository.HandleBookMiddleAsync(task_midData);


        }
    }
}
