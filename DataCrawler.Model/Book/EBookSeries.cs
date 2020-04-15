using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataCrawler.Model
{
    [SugarTable("BookSeries")]
    public class EBookSeries
    {
        [SugarColumn(IsPrimaryKey =true,IsIdentity = true)]
        public int Id { get; set; }
        [SugarColumn(Length = 50, ColumnDataType = "nvarchar")]
        public string BookCode { get; set; }

        public int SeriesId { get; set; }
    }
}
