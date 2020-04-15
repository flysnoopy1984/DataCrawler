using DataCrawler.Model;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataCrawler.Repository
{
    public class PersonRepository : BaseRepository<EPerson>
    {
        public PersonRepository(ISqlSugarClient sugarClient) : base(sugarClient)
        {

        }

    

    }
}
