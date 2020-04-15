using DataCrawler.Model.MiddleObject;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataCrawler.Core
{
    public interface ICrawlerTag
    {

        public List<Secction_Tag> getUrls(string entryUrl);
    }
}
