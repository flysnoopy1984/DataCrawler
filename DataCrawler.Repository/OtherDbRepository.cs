using DataCrawler.Model.CaiPiao;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataCrawler.Repository
{
    public class OtherDbRepository: BaseRepository<cpDaLeTouData>
    {
        public OtherDbRepository(ISqlSugarClient sugarClient)
           : base(sugarClient) { }

    }
}
