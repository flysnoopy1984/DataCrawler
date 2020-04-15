using System;
using System.Collections.Generic;
using System.Text;

namespace DataCrawler.Model.MiddleObject
{
    public class Secction_Tag
    {
        public string sectionName { get; set; }

        private List<ETag> _tagList;
        public List<ETag> TagList
        {
            get
            {
                if (_tagList == null)
                    _tagList = new List<ETag>();
                return _tagList;
            }
        }
    }
}
