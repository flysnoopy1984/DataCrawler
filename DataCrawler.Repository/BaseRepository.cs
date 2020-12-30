
using ContentCenter.Model;
using DataCrawler.Model;
using DataCrawler.Util;
using IQB.Util.Models;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataCrawler.Repository
{
    public class BaseRepository<T> where T : class,new()
    {

        public const int CacheEffect = 60;

        private readonly ISqlSugarClient _db;
        public ISqlSugarClient Db { get { return _db; } }
        public BaseRepository(ISqlSugarClient sqlSugarClient)
        {
            _db = sqlSugarClient;

          //  EnabelSqlLog();
        }

        public void EnabelSqlLog()
        {
            _db.Ado.IsEnableLogEvent = true;

            _db.Aop.OnLogExecuted = (sql, pars) => //SQL执行完事件
            {

            };
            _db.Aop.OnLogExecuting = (sql, pars) => //SQL执行前事件
            {
                var a = sql;
                Console.WriteLine(sql);
            };
            _db.Aop.OnError = (exp) =>//执行SQL 错误事件
            {

                Console.WriteLine(exp.Message);
                //Console.WriteLine(exp.Sql);
                //SugarParameter[] sps = (SugarParameter[])exp.Parametres;

                //foreach (var p in sps)
                //{
                //    Console.WriteLine($"{p.ParameterName}:{p.Value}--{p.Value.ToString().Length}");
                //}

                Console.ReadLine();
                     
            };
            //_db.Aop.OnExecutingChangeSql = (sql, pars) => //SQL执行前 可以修改SQL
            //{
            //    return new KeyValuePair<string, SugarParameter[]>(sql, pars);
            //};
        }

        #region Async 异步
        public async Task<int> AddAsync(T newEntity)
        {
            var insertable = _db.Insertable<T>(newEntity);
            return await insertable.ExecuteReturnIdentityAsync();
        }



        public async Task<int> AddRangeAsync(List<T> list)
        {
            var insertable = _db.Insertable(list);
            return await insertable.ExecuteCommandAsync();
        }

      

        public async Task<bool> DeleteByKeyAsync(int key)
        {
            var op = _db.Deleteable<T>(key);
            return await op.ExecuteCommandHasChangeAsync();
            
        }

        public async Task<T> GetByKeyAsync(object key)
        {
            var op = _db.Queryable<T>().InSingleAsync(key);
            return await op;
        }

        public async Task<List<T>> QueryListAsync(Expression<Func<T, bool>> whereExp, Expression<Func<T, object>> orderByExp, bool desc = true)
        {
            var op = _db.Queryable<T>().
                WhereIF(whereExp != null, whereExp).
                OrderByIF(orderByExp != null, orderByExp,desc?OrderByType.Desc:OrderByType.Asc).
                ToListAsync();

            return await op;
        }

        public async Task<ModelPager<T>> QueryPagerAsync(Expression<Func<T, bool>> whereExp, Expression<Func<T, object>> orderByExp, int pageIndex, int pageSize, bool desc = true)
        {
            RefAsync<int> totalCount = 0;
            var op = await _db.Queryable<T>().
                WhereIF(whereExp != null, whereExp).
                OrderByIF(orderByExp != null, orderByExp, desc ? OrderByType.Desc : OrderByType.Asc).
                ToPageListAsync(pageIndex, pageSize,totalCount);

            int totalapage = Math.Ceiling(totalCount > 0 ? totalCount.ObjToDecimal() / pageSize.ObjToDecimal() : 0).ObjToInt();

            return new ModelPager<T>()
            {
                datas = op,
                pageIndex = pageIndex,
                pageSize = pageSize,
                totalCount = totalCount,
                totalPage = totalapage
            };
        }

        public async Task<bool> UpdateAsync(T updateEntity)
        {
            var op = _db.Updateable<T>(updateEntity);
            return await op.ExecuteCommandHasChangeAsync();
        }


        public async Task<T> AddOrUpdate_AllAsync(T saveObj)
        {
            return await _db.Saveable<T>(saveObj).ExecuteReturnEntityAsync();
        }

        public async Task<M> AddOrUpdate_MasterDataAsync<M>(M saveObj) where M : BaseMasterData,new()
        {
            return await _db.Saveable<M>(saveObj)
                .UpdateIgnoreColumns(o=>new { o.CreateDateTime, o.DataSource })
                .ExecuteReturnEntityAsync();
        }
        public async Task<M> AddOrUpdate_MasterDataAsync<M>(List<M> saveList) where M : BaseMasterData, new()
        {
            if (saveList == null || saveList.Count == 0)
                return new M();

            return await _db.Saveable<M>(saveList)
                .UpdateIgnoreColumns(o => new { o.CreateDateTime, o.DataSource })
                .ExecuteReturnEntityAsync();
        }
        #endregion

        #region 同步
        public int AddRange(List<T> list)
        {
            if (list == null || list.Count == 0) return -1;

            var insertable = _db.Insertable(list);
            return insertable.ExecuteCommand();
        }

        public int DelAll(Expression<Func<T, bool>> whereExp = null)
        {
            var exp = _db.Deleteable<T>();
            if (whereExp != null)
                exp = exp.Where(whereExp);
            return exp.ExecuteCommand();
        }

        public  List<T> AddOrUpdate_All(List<T> list)
        {
            if(list!= null && list.Count>0)
                return  _db.Saveable<T>(list).ExecuteReturnList();
            return list;
        }

        public  M AddOrUpdate_MasterData<M>(M saveObj) where M : BaseMasterTable, new()
        {
            return _db.Saveable<M>(saveObj)
             .UpdateIgnoreColumns(o => new { o.CreateDateTime })
             .ExecuteReturnEntity();

        }
        public List<M> AddOrUpdate_MasterData<M>(List<M> saveList) where M : BaseMasterTable, new()
        {
            if (saveList == null || saveList.Count == 0)
                return saveList;

            return _db.Saveable<M>(saveList)
                .UpdateIgnoreColumns(o => new { o.CreateDateTime,})
                .ExecuteReturnList();
        }

        public List<T> QueryList(Expression<Func<T, bool>> whereExp = null, Expression<Func<T, object>> orderByExp =null, bool desc = true,
            bool isCache = false,
            int cacheSec = CacheEffect*60)
        {
            var op = _db.Queryable<T>().
                WhereIF(whereExp != null, whereExp).
                OrderByIF(orderByExp != null, orderByExp, desc ? OrderByType.Desc : OrderByType.Asc).WithCacheIF(isCache, cacheSec).
                ToList();

            return op;
        }

        public T getObjectByPK(object pkCode)
        {
            return _db.Queryable<T>().InSingle(pkCode);
        }


        public int IsExist(Expression<Func<T,CountResult>> SqlFuncResult, Expression<Func<T, bool>> whereExp = null)
        {
            var r = _db.Queryable<T>()
                         .WhereIF(whereExp != null,whereExp)
                         .Select(SqlFuncResult).First();
            return r == null ? 0 : r.Count;

          //  SqlFunc.AggregateCount()
        }

        public  int Add(T newEntity)
        {
            var insertable = _db.Insertable<T>(newEntity);
            return insertable.ExecuteReturnIdentity();
        }
        #endregion
    }
}
