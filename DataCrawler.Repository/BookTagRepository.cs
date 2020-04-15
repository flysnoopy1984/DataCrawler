using DataCrawler.Model;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataCrawler.Repository
{
    public class BookTagRepository : BaseRepository<EBookTag>
    {
        public BookTagRepository(ISqlSugarClient sugarClient)
           : base(sugarClient) { }

    }
}
