using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataCrawler.Model
{
    [SugarTable("Plan_FromDouBanTagUrls")]
    public class EPlan_FromDouBanTagUrls
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }

        [SugarColumn(Length = 50)]
        public string Code { get; set; }

        [SugarColumn(Length = 255)]
        public string Url { get; set; }

        [SugarColumn(Length = 50)]
        public string TagCode { get; set; }


        /// <summary>
        /// DouBand Tag List 只能获取1000内的 ，https://book.douban.com/tag/%E5%B0%8F%E8%AF%B4?type=
        /// 每一页+20，首页为0
        /// </summary>
        public int ProcessPageIndex { get; set; }
    }
}
