using System;
using System.Collections.Generic;
using System.Text;

namespace DataCrawler.Util
{
    public static class  CrawlerSetting
    {
        public static int Tag_LimitLenght = 10;
        /// <summary>
        /// 当Tag特别长是否忽略
        /// </summary>
        public static bool Tag_IgnoreLongString = true;

        public const int DB_MaxIndex_TagList = 1000;
        /// <summary>
        /// TagList 每翻页Step的长度。Douban 第一页0 第二页20
        /// </summary>
        public const int DB_TagList_Step = 20;
    }
}
