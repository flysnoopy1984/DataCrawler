using DataCrawler.Model.BaseEnums;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataCrawler.Model
{
    [SugarTable("TagInfo")]
    public class ETag: BaseMasterData
    {
        [SugarColumn(IsIdentity = true)]
        public int Id { get; set; }

        [SugarColumn(IsPrimaryKey = true,Length = 150, ColumnDataType = "nvarchar")]
        public string Code { get; set; }

        [SugarColumn(Length = 30, ColumnDataType = "nvarchar")]
        public string Name { get; set; }

        public TagType Type { get; set; }

        [SugarColumn(Length = 100,IsNullable =true)]
        public string Url { get; set; }


    }
}
