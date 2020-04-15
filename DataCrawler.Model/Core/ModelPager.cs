using System;
using System.Collections.Generic;
using System.Text;

namespace DataCrawler.Model
{
    public class ModelPager<T>
    {
        public int pageIndex { get; set; } = 0;

        public int pageSize { get; set; } = 15;

        public int totalCount { get; set; }

        public int totalPage { get; set; }

        public List<T> datas { get; set; }
    }
}
