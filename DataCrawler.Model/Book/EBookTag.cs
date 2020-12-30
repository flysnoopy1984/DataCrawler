using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataCrawler.Model
{
    [SugarTable("BookTag")]
    public class EBookTag
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }
        [SugarColumn(Length = 50, ColumnDataType = "nvarchar")]
        public string BookCode { get; set; }

        [SugarColumn(Length = 50, ColumnDataType = "nvarchar")]
        public string TagCode { get; set; }

        public DateTime CreateDateTime { get; set; }


    }
}
