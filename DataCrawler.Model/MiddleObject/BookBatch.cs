using DataCrawler.Model.BaseEnums;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataCrawler.Model.MiddleObject
{
    public class BookBatch
    {
        public string DetailUrl { get; set; }

        public FictionType FictionType { get; set; }

        public string SectionCode { get; set; }
    }
}
