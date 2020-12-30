using DataCrawler.Core.Other;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DataCrawler.Tasks.Other
{
    public class DaReTouTask
    {
        private DaReTouCrawler _crawler;
        public DaReTouTask(DaReTouCrawler crawler)
        {
            _crawler = crawler;

        }

        public void runHtml(string filePath)
        {
        
            _crawler.AnalyFile_Html(filePath);
        }
    }
}
