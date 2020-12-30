using ContentCenter.Model;
using ContentCenter.Model.BaseEnum;
using DataCrawler.Model;
using DataCrawler.Model.BaseEnums;
using IQB.Util.Models;
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
                 
                      var dataList = new List<ESection>()
                    {
                        new ESection{Title="小说文学",Code="XiaoShuo", SectionType = SectionType.Column},
                        new ESection{Title="设计创业",Code="Sheji", SectionType = SectionType.Column},
                        new ESection{Title="科普兴趣",Code="Kepu", SectionType = SectionType.Column},
                        new ESection{Title="管理经营",Code="Guanli", SectionType = SectionType.Column},
                        new ESection{Title="商业金融",Code="Shangye", SectionType = SectionType.Column},
                        new ESection{Title="历史纵横",Code="LiShi", SectionType = SectionType.Column},
                        new ESection{Title="生活百态",Code="ShengHuo", SectionType = SectionType.Column},
                    };
                    var fs = typeof(WebSection).GetFields();
                    foreach(var f in fs)
                    {
                        var v = f.GetRawConstantValue();
                        dataList.Add(new ESection
                        {
                            Title = f.Name,
                            Code = v.ToString()
                        });
                       
                    }
                    //foreach(var s in webSections)
                    //{

                    //}
                    base.DelAll();
                    base.AddRange(dataList);

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
