using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataCrawler.Model
{
    [SugarTable("SectionTag")]
    public class ESectionTag
    {
        public string SectionCode { get; set; }

        public string SectionName { get; set; }

        public string TagCode { get; set; }
        public string TagName { get; set; }
    }
}
