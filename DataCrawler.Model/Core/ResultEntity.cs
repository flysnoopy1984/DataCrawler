using System;
using System.Collections.Generic;
using System.Text;

namespace DataCrawler.Model
{
    public class ResultEntity<T>:ResultNormal where T : class
    {
        public T Entity { get; set; }
    }
}
