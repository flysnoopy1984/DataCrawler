using DataCrawler.Model;
using DataCrawler.Repository.Core;
using DataCrawler.Util;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DataCrawler.Repository
{
    public class DataSectionRepository:BaseRepository<EDataSection>
    {
        public DataSectionRepository(ISqlSugarClient sugarClient)
           : base(sugarClient) { }

        /// <summary>
        /// 根据Section Code ，覆盖所有相关 DataSecion.
        /// 
        /// </summary>
        public async Task CoverNewSectionCodeAsync(List<EDataSection> newList)
        {
            var rAll = await base.Db.Ado.UseTranAsync(() =>
            {
                foreach(var es in newList)
                {
                    int n = base.IsExist(a => new CountResult { Count = SqlFunc.AggregateCount(a.Id) }, a => a.SectionCode == es.SectionCode && a.ItemCode == es.ItemCode);
                    if(n == 0) base.Add(es);
                    else
                    {
                        int r = base.DelAll(a => a.SectionCode == es.SectionCode && a.ItemCode == es.ItemCode && a.CreateDateTime.AddDays(30) < DateTime.Today);
                        if (r > 0)
                            base.Add(es);
                    }
                  
                }
            //    base.AddRange(newList);

                
            });
            if (!rAll.IsSuccess)
                NLogUtil.ErrorTxt($"[CoverNewSectionCode]建立Book和 Section 关系:{rAll.ErrorMessage}");
           

        }

        public static EDataSection newModelInstance(string secCode,string itemCode)
        {
            return new EDataSection
            {
                BatchNo = GenCodeHelper.DataSection_BatchNo(secCode),
                ItemCode = itemCode,
                SectionCode = secCode,
                CreateDateTime = DateTime.Now,
                
            };
        }
    }
}
