using DataCrawler.Model;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataCrawler.Repository
{
    public class BookRepository:BaseRepository<EBookInfo>
    {
         public BookRepository(ISqlSugarClient sugarClient) 
            : base(sugarClient) { }


     
  
    }
}
