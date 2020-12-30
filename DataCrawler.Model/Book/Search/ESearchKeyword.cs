using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataCrawler.Model.Book.Search
{
    //暂时不用
    [SugarTable("douban_SearchKeyword")]
    public  class ESearchKeyword
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity =true)]
        public long Id { get; set; }
        public int pageIndex { get; set; }

        [SugarColumn(Length = 200)]
        public string keyWord { get; set; }
    }
}
