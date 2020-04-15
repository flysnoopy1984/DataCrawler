using DataCrawler.Model.BaseEnums;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataCrawler.Model
{
    [SugarTable("PersonInfo")]
    public class EPerson: BaseMasterData
    {
        [SugarColumn(IsIdentity = true)]
        public int Id { get; set; }

        [SugarColumn(IsPrimaryKey = true,Length = 50, ColumnDataType = "nvarchar")]
        public string Code { get; set; }
        /// <summary>
        /// 作者姓名
        /// </summary>
        [SugarColumn(Length = 50, ColumnDataType = "nvarchar")]
        public string Name { get; set; }

        /// <summary>
        /// 作者简介
        /// </summary>
        [SugarColumn(ColumnDataType = "text",IsNullable = true)]
        public string Summery { get; set; }

        /// <summary>
        /// 国家
        /// </summary>
        [SugarColumn(Length = 20,IsNullable = true)]
        public string Country { get; set; }


        /// <summary>
        /// 作者详情Url,可能是Search页面
        /// </summary>
        [SugarColumn(Length = 255,IsNullable = true)]
        public string SourceUrl { get; set; }

       

    }
}
