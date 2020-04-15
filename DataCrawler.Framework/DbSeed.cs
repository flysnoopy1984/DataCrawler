using DataCrawler.Model;
using DataCrawler.Repository;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataCrawler.Framework
{
    public class DbSeed
    {

        public static void InitData(DouBanBookService db)
        {
             db.InitData();
        }
        public static void InitTables(ISqlSugarClient db)
        {
            db.DbMaintenance.CreateDatabase(databaseName: "MasterCrawlerData");
            List<Type> tbList = new List<Type>()
                    {
                        typeof(EBookInfo),
                        typeof(EBookSeries),
                        typeof(EBookTag),
                        typeof(ESeriesInfo),
                       
                        typeof(EDataSection),
                        typeof(EPerson),
                        typeof(ESection),
                        typeof(ETag),

                        typeof(EPlan_FromDouBanTagUrls),
                       
                    };
            db.CodeFirst.InitTables(tbList.ToArray());
        
        }
    }
}
