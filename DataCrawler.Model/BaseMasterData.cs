using DataCrawler.Model.BaseEnums;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataCrawler.Model
{
    public abstract class BaseMasterData
    {
        public BaseMasterData()
        {
            CreateDateTime = DateTime.Now;
            UpdateDateTime = DateTime.Now;
            DataSource = DataSource.DouBan;
        }
        /// <summary>
        /// 首次爬虫入库时间
        /// </summary>
        public DateTime CreateDateTime { get; set; }

        public DateTime UpdateDateTime { get; set; }

        [SugarColumn(Length = 50,IsNullable =true)]
        public string UpdatedBy { get; set; }

        public DataSource DataSource { get; set; }
    }
}
