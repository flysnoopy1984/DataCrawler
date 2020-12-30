using DataCrawler.Model;
using DataCrawler.Model.Book.Search;
using DataCrawler.Model.CaiPiao;
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

        public static void InitData(DouBanBookService db,bool needsection=false)
        {
             db.InitData(needsection);
        }
        public static void InitTables(ISqlSugarClient db)
        {


            //List<Type> tbList = new List<Type>()
            //        {
         
            //           typeof(cpDaLeTouData),
            //           typeof(ESearchOneBookResult)
            //        };
            //db.CodeFirst.InitTables(tbList.ToArray());
        
        }
    }
}
