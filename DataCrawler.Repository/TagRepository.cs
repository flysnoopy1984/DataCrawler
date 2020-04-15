using DataCrawler.Model;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataCrawler.Repository
{
    public class TagRepository:BaseRepository<ETag>
    {
        public TagRepository(ISqlSugarClient sugarClient) : base(sugarClient)
        {

        }
    }
}
