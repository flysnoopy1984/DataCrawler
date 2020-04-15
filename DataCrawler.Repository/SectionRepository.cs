using DataCrawler.Model;
using DataCrawler.Model.BaseEnums;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DataCrawler.Repository
{
    public class SectionRepository: BaseRepository<ESection>
    {
        public SectionRepository(ISqlSugarClient sugarClient) : base(sugarClient)
        {

        }

        public ESection getNewExpressCode()
        {
            return base.getObjectByPK("NewExpress");
        }

        public Dictionary<string,ESection> AllSection()
        {
            Dictionary<string, ESection> result = new Dictionary<string, ESection>();
            var secList = QueryList(null,null,false,false,3600*24);
            foreach(var sec in secList)
            {
                result.Add(sec.Title, sec);
            }
            return result;
        }
        
        public async Task<ResultNormal> Init()
        {
            ResultNormal result = new ResultNormal();
            try
            {
                var r = await Db.Ado.UseTranAsync(() =>
                {
                  
                    base.DelAll();
                    base.AddRange(new List<ESection>()
                    {
                        new ESection{Title="新书快递",Code="NewExpress"},
                        new ESection{Title="最受欢迎",Code="Popular"},
                        new ESection{Title="热门书刊",Code="Hot"},

                        new ESection{Title="文学",Code="WenXue", SectionType = SectionType.Tag},
                        new ESection{Title="流行",Code="LiuXin", SectionType = SectionType.Tag},
                        new ESection{Title="文化",Code="WenHua", SectionType = SectionType.Tag},
                        new ESection{Title="生活",Code="ShengHuo", SectionType = SectionType.Tag},
                        new ESection{Title="经管",Code="JingGuan", SectionType = SectionType.Tag},
                        new ESection{Title="科技",Code="KeJi", SectionType = SectionType.Tag},
                    });

                });
                if (r.IsSuccess)
                    result.Message = "Init Done";
                else
                    result.ErrorMsg = r.ErrorMessage;
              
            }
            catch(Exception ex)
            {
                result.ErrorMsg = ex.Message;
            }
            
            return result;
          
        }

        
    }
}
