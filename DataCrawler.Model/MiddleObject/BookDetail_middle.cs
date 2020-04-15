using System;
using System.Collections.Generic;
using System.Text;

namespace DataCrawler.Model.MiddleObject
{
    public class BookDetail_middle
    {
        private EBookInfo _DouBanBookInfo;
        public EBookInfo DouBanBookInfo {
            get {
                if (_DouBanBookInfo == null)
                    _DouBanBookInfo = new EBookInfo();
                return _DouBanBookInfo; } 
            set { _DouBanBookInfo = value; }
        }

        private EPerson _Author;
        public EPerson Author
        {
            get
            {
                if (_Author == null) _Author = new EPerson();
                return _Author;
            }
            set
            {
                _Author = value;
            }
        }

        private List<ETag> _tagList;
        public List<ETag> tagList
        {
            get
            {
                if (_tagList == null) 
                    _tagList = new List<ETag>();
                return _tagList;
            }
        }

        private List<ESection> _sectionList;
        public List<ESection> SectionList
        {
            get
            {
                if (_sectionList == null)
                    _sectionList = new List<ESection>();
                return _sectionList;
            }
        }

        public List<EDataSection> GetDataSections()
        {
            List<EDataSection> list = new List<EDataSection>();
            if (SectionList.Count > 0)
            {
               
                SectionList.ForEach(sec =>
                {
                    list.Add(new EDataSection
                    {
                        ItemCode = DouBanBookInfo.Code,
                        SectionCode = sec.Code
                    });
                });
            }
            return list;
        }

        public List<EBookTag> GetBookTags()
        {
            List<EBookTag> list = new List<EBookTag>();
            if (tagList.Count > 0)
            {
                tagList.ForEach(tag =>
                {
                    list.Add(new EBookTag
                    {
                        BookCode = DouBanBookInfo.Code,
                        TagCode = tag.Code,
                    });
                });
            }
            return list;
        }
    }
}
