using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataCrawler.Model
{
   
    [SugarTable("SeriesInfo")]
    public class ESeriesInfo
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }

        [SugarColumn(Length = 50, ColumnDataType = "nvarchar")]
        public string Name { get; set; }

        public int SeriesCount { get; set; }

        [SugarColumn(Length = 50,IsNullable =true, ColumnDataType = "nvarchar")]
        public string Publisher { get; set; }
    }
}
