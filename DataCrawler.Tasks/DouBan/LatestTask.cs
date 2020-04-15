using DataCrawler.Core;
using DataCrawler.Core.DouBan;
using DataCrawler.Model;
using DataCrawler.Repository;
using DataCrawler.Repository.Core;
using DataCrawler.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataCrawler.Tasks.DouBan
{
    public class LatestTask: BaseTask
    {
        
        private readonly ICrawlerBatchBook _LatestCrawler;
        private ICrawlerBook _DetailCrawler;
       
        private DouBanBookService _DouBanBookRepository;
        public LatestTask(ICrawlerBatchBook crawlerBatchData,
            ICrawlerBook crawlerBook,
                          DouBanBookService douBanBookRepository)
        {
            _DetailCrawler = crawlerBook;
            _DouBanBookRepository = douBanBookRepository;
            _LatestCrawler = crawlerBatchData;
         
        }
        public async void Run()
        {
            NLogUtil.InfoTxt("开始批量【豆瓣新书】",true);
            var list = _LatestCrawler.CrawlerUrls("");
            List<EDataSection> secList = new List<EDataSection>(); 
            foreach(var url in list)
            {
              //  _DetailCrawler = new BookDetailCrawler();

                var midData = _DetailCrawler.Crawler(url.DetailUrl);

                // 添加虚拟非虚拟
                midData.DouBanBookInfo.FictionType = url.FictionType;                
                //　添加到最新专栏
                var dataSection = _DouBanBookRepository.GetSection_NewExpress();
                secList.Add(DataSectionRepository.newModelInstance(dataSection.Code, midData.DouBanBookInfo.Code));
          
              
                await _DouBanBookRepository.HandleBookMiddleAsync(midData);
                
            }

            await _DouBanBookRepository.CoverSection(secList);

          NLogUtil.InfoTxt("结束批量【豆瓣新书】", true);
        }

        private void HandleDataSection()
        {

        }
        

       
    }
}
