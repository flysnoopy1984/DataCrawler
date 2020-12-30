using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataCrawler.Model.CaiPiao
{
    [SugarTable("cp_DaLeTouData")]
    public class cpDaLeTouData
    {
        [SugarColumn(IsPrimaryKey = true,IsIdentity =true)]
        public int Id { get; set; }
        public int red1 { get; set; }
        public int red2 { get; set; }

        public int red3 { get; set; }

        public int red4 { get; set; }

        public int red5 { get; set; }

        public int blue1 { get; set; }

        public int blue2 { get; set; }

        [SugarColumn(Length = 200)]
        public string remark { get; set; }
    }
}
