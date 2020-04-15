using DataCrawler.Model;
using DataCrawler.Model.MiddleObject;
using DataCrawler.Repository.Core;
using DataCrawler.Util;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataCrawler.Repository
{
    public class DouBanBookService
    {

        private BookRepository _BookDb;
        private SectionRepository _SectionDb;
        private TagRepository _TagDb;
        private PersonRepository _PersonDb;
        private BookTagRepository _BookTagDb;
        private DataSectionRepository _DataSectionDb;
        private PlanFTURepository _PlanFTURepository;
        private ISqlSugarClient _Db;
        public DouBanBookService(BookRepository bookRepository,
            SectionRepository sectionRepository,
            TagRepository tagRepository,
            PersonRepository personRepository,
            BookTagRepository bookTagRepository,
            DataSectionRepository dataSectionRepository,
            PlanFTURepository planFTURepository,
            ISqlSugarClient sqlSugarClient)
        {
            _PersonDb = personRepository;
            _BookDb = bookRepository;
            _SectionDb = sectionRepository;
            _TagDb = tagRepository;
            _BookTagDb = bookTagRepository;
            _DataSectionDb = dataSectionRepository;
            _Db = sqlSugarClient;
            _PlanFTURepository = planFTURepository;
        }

        public void VerifyBookData(BookDetail_middle middle)
        {
            string msg = "[DouBanBookRepository]:";
            if (string.IsNullOrEmpty(middle.DouBanBookInfo.Code))
                throw new Exception(msg + "没有BookCode");
            if (string.IsNullOrEmpty(middle.DouBanBookInfo.AuthorCode))
                throw new Exception(msg + "没有作者Code");


        }


        /// <summary>
        /// 处理抓爬的书本Middle数据 
        /// </summary>
        /// <param name="middle"></param>
        public async Task<bool> HandleBookMiddleAsync(BookDetail_middle middle)
        {
           
            try
            {
                NLogUtil.InfoTxt($"开始处理书本到数据库:{middle.DouBanBookInfo.Code}-{middle.DouBanBookInfo.Title}");
                VerifyBookData(middle);
               
                    var rAll = await _Db.Ado.UseTranAsync(() => {
                    try
                    {
                        var ePerson = _PersonDb.AddOrUpdate_MasterData<EPerson>(middle.Author);
                    }
                    catch (Exception ex)
                    {
                        NLogUtil.ErrorTxt($"【错误】写入人物:{ex.Message}");
                    }



                        var rSection = _SectionDb.AddOrUpdate_MasterData<ESection>(middle.SectionList);
                    var rBook = _BookDb.AddOrUpdate_MasterData<EBookInfo>(middle.DouBanBookInfo);

                    try
                    {
                        var rTag = _TagDb.AddOrUpdate_MasterData<ETag>(middle.tagList);
                    }
                    catch(Exception ex)
                    {
                            NLogUtil.ErrorTxt($"【错误】写入更新Tags:{ex.Message}");
                    }
                  

                    HandleDataSection(middle.GetDataSections());
                    HandleBookTag(middle.GetBookTags());

                });


                if (!rAll.IsSuccess)
                    NLogUtil.ErrorTxt($"[数据库]录入书本失败:{middle.DouBanBookInfo.Code}-{middle.DouBanBookInfo.Title} -- {rAll.ErrorMessage}");
                else
                    NLogUtil.InfoTxt($"【成功】处理书本到数据库:{middle.DouBanBookInfo.Code}-{middle.DouBanBookInfo.Title}");
                return rAll.IsSuccess;
            }
            catch(Exception ex)
            {
              //  _Db.Ado.RollbackTran();
                NLogUtil.ErrorTxt($"[数据库]录入书本失败:{middle.DouBanBookInfo.Code}-{middle.DouBanBookInfo.Title}--{ex.Message}");
                return false;
            }

            return true;
       
           
        }

       
        public void HandleDataSection(List<EDataSection> list)
        {
            List<EDataSection> newList = new List<EDataSection>();
            foreach (var es in list)
            {
                var c = _DataSectionDb.Db.Queryable<EDataSection>()
                     .Where(a => a.ItemCode == es.ItemCode && a.SectionCode == es.SectionCode)
                     .Select(s => new { ct = SqlFunc.AggregateCount(s.Id) }).First();
                if (c.ct == 0)
                {
                    newList.Add(es);
                  
                    // _DataSectionDb.Db.Insertable<EDataSection>(es).ExecuteCommand();
                }
               // _DataSectionDb.sa

            }
            _DataSectionDb.AddRange(newList);
        }

        public void HandleBookTag(List<EBookTag> list)
        {
            List<EBookTag> newList = new List<EBookTag>();
            try
            {
                foreach (var tag in list)
                {
                    var c = _BookTagDb.IsExist(s => new CountResult { Count = SqlFunc.AggregateCount(s.Id) },
                    a => a.BookCode == tag.BookCode && a.TagCode == tag.TagCode);

                    //var c = _BookTagDb.IsExist(s => new { ct = SqlFunc.AggregateCount(s.Id) },
                    //    a => a.BookCode == tag.BookCode && a.TagCode == tag.TagCode);
                    if (c == 0) newList.Add(tag);

                }
                _BookTagDb.AddRange(newList);
            }
            catch(Exception ex)
            {
                throw ex;
            }
           
        }

        public ESection GetSection_NewExpress()
        {
            return _SectionDb.getNewExpressCode();
        }

        public async Task CoverSection(List<EDataSection> newlist)
        {
            await _DataSectionDb.CoverNewSectionCodeAsync(newlist);
        }

        #region Plan 

        public bool IsExistPlan(string code = GenCodeHelper.Plan_FromDouBanTagUrls)
        {
            return _PlanFTURepository.IsExistPlan();
        }


       
        public List<EPlan_FromDouBanTagUrls> InitPlanFromTagUrl(List<Secction_Tag> tagLists)
        {
            List<EPlan_FromDouBanTagUrls> result = new List<EPlan_FromDouBanTagUrls>();
            if (!_PlanFTURepository.IsExistPlan())
            {
                NLogUtil.InfoTxt("豆瓣计划写入到数据库");
                if (tagLists != null && tagLists.Count > 0)
                {
                    List<EDataSection> dsList = new List<EDataSection>();
                   // List<EPlan_FromDouBanTagUrls> planList = new List<EPlan_FromDouBanTagUrls>();
                    
                    var allSection = _SectionDb.AllSection();
                    foreach (Secction_Tag st in tagLists)
                    {
                        ESection section = null;
                        try
                        {
                            section = allSection[st.sectionName];
                        }
                        catch
                        {
                            section = null;
                        }
                        foreach (var tag in st.TagList)
                        {
                            result.Add(PlanFTURepository.NewModelInstance(tag.Name, tag.Url));
                            if (section != null)
                                dsList.Add(DataSectionRepository.newModelInstance(section.Code, tag.Code));
                        }
                    }
                    var rAll = _Db.Ado.UseTran(async () =>
                   {
                       await _DataSectionDb.CoverNewSectionCodeAsync(dsList);
                       _PlanFTURepository.CoverPlans(GenCodeHelper.Plan_FromDouBanTagUrls, result);
                   });
                    if (rAll.IsSuccess)
                        NLogUtil.InfoTxt("【成功】豆瓣计划已在数据库初始化");
                    else
                        NLogUtil.ErrorTxt($"【失败】豆瓣计划:{rAll.ErrorMessage}");
                }
            }
            else
            {
                NLogUtil.InfoTxt("DouBan Plan 已经存在数据库中！");
                //DouBand Tag List 只能获取1000内的
                result = _PlanFTURepository.QueryPlan(GenCodeHelper.Plan_FromDouBanTagUrls);
            }
            return result;
               


        }


        public void UpdatePlan(EPlan_FromDouBanTagUrls obj)
        {
            _PlanFTURepository.UpdatePlan(obj);
        }
        #endregion

        public  void Test(BookDetail_middle middle)
        {
            var rBook = _BookDb.AddOrUpdate_MasterData<EBookInfo>(middle.DouBanBookInfo);
        }


        public async void InitData()
        {
            await _SectionDb.Init();
        }

    }
}
