using DataCrawler.Model;
using DataCrawler.Model.MiddleObject;
using DataCrawler.Repository.Core;
using DataCrawler.Util;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataCrawler.Repository
{
    public class PlanFTURepository: BaseRepository<EPlan_FromDouBanTagUrls>
    {
        public PlanFTURepository(ISqlSugarClient sugarClient) : base(sugarClient)
        {

        }

        public bool IsExistPlan(string planCode = GenCodeHelper.Plan_FromDouBanTagUrls)
        {
           // var ct = SqlFunc.AggregateCount<EPlan_FromDouBanTagUrls>(a=>a.Code);
            var c = IsExist(s => new CountResult { Count = SqlFunc.AggregateCount(s.Code) },
                      a => a.Code == GenCodeHelper.Plan_FromDouBanTagUrls);
            return (c > 0);
        }

        public static EPlan_FromDouBanTagUrls NewModelInstance(string Code,string url)
        {
            return new EPlan_FromDouBanTagUrls
            {
                Code = GenCodeHelper.Plan_FromDouBanTagUrls,
                ProcessPageIndex = 0,
                TagCode = Code,
                Url = url
            };
        }

        public void CoverPlans(string code, List<EPlan_FromDouBanTagUrls> list)
        {
            var rAll = Db.Ado.UseTran(() =>
            {
                base.DelAll(a => a.Code == code);
                base.AddRange(list);
            });

        }

        public List<EPlan_FromDouBanTagUrls> QueryPlan(string Code)
        {
            if(Code == GenCodeHelper.Plan_FromDouBanTagUrls)
                return QueryList(a => a.ProcessPageIndex < CrawlerSetting.DB_MaxIndex_TagList && a.Code == Code);
            else
                return QueryList(a => a.Code == Code);
        }
        public async void UpdatePlan(EPlan_FromDouBanTagUrls obj)
        {
            await UpdateAsync(obj);
          //  var obj = getObjectByPK(Id.ToString());
        }
    }
}
