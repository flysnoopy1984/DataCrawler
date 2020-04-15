using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataCrawler.Model
{
    [SugarTable("DataSection")]
    public class EDataSection
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }

        [SugarColumn(Length = 50, ColumnDataType = "nvarchar")]
        public string SectionCode { get; set; }
        [SugarColumn(Length = 50, ColumnDataType = "nvarchar")]
        public string ItemCode { get; set; }

        [SugarColumn(Length = 40)]
        public string BatchNo { get; set; }


    }
}
