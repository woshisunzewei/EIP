using EIP.Common.Dapper.SQL;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using EIP.Common.Entities.Paging;

namespace EIP.Common.Dapper
{
    /// <summary>
    /// Dapper操作工具
    /// </summary>
    public static class SqlMapperUtil
    {
        public static string ConnectionName = "EIP";

        public static DbBase CreateDbBase()
        {
            var dbBase= new DbBase(ConnectionName);
            ConnectionName = "EIP";
            return dbBase;
        }

        #region 映射
        /// <summary>
        /// 增加实体
        /// </summary>
        /// <param name="t">实体</param>
        /// <param name="transaction">事物</param>
        /// <param name="commandTimeout">超时</param>
        /// <returns></returns>
        public static Task<int> Insert<T>(T t, IDbTransaction transaction = null,
            int? commandTimeout = null) where T : class
        {
            using (var db = CreateDbBase())
            {
                var result = db.Insert(t);
                return Task.Factory.StartNew(() => result);
            }
        }

        /// <summary>
        /// 增加实体
        /// </summary>
        /// <param name="t"></param>
        /// <param name="transaction"></param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        public static Task<int> InsertScalar<T>(T t, IDbTransaction transaction = null,
            int? commandTimeout = null) where T : class
        {
            using (var db = CreateDbBase())
            {
                var result = db.InsertScalar(t);
                return Task.Factory.StartNew(() => result);
            }
        }

        /// <summary>
        /// 批量增加实体
        /// </summary>
        /// <param name="lt"></param>
        /// <param name="transaction"></param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        public static Task<int> InsertBatch<T>(IEnumerable<T> lt, IDbTransaction transaction = null, int? commandTimeout = null) where T : class
        {
            using (var db = CreateDbBase())
            {
                var result = db.InsertBatch(lt, transaction, commandTimeout);
                return Task.Factory.StartNew(() => result);
            }
        }

        /// <summary>
        /// 批量增加实体
        /// </summary>
        /// <param name="lt"></param>
        /// <returns></returns>
        public static Task<int> InsertWithBulkCopy<T>(List<T> lt) where T : new()
        {
            using (var db = CreateDbBase())
            {
                var result = db.InsertWithBulkCopy(lt);
                return Task.Factory.StartNew(() => result);
            }
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static Task<int> Update<T>(T t, SqlQuery sql = null) where T : class
        {
            using (var db = CreateDbBase())
            {
                var result = db.Update(t, sql);
                return Task.Factory.StartNew(() => result);
            }
        }

        /// <summary>
        /// 删除所有
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static Task<int> Delete<T>(SqlQuery sql = null, object id = null) where T : class
        {
            using (var db = CreateDbBase())
            {
                var result = db.Delete<T>(sql);
                return Task.Factory.StartNew(() => result);
            }
        }

        /// <summary>
        /// 根据Id删除
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static Task<int> DeleteById<T>(SqlQuery sql = null, object id = null) where T : class
        {
            using (var db = CreateDbBase())
            {
                var result = db.DeleteById<T>(sql, id);
                return Task.Factory.StartNew(() => result);
            }
        }

        /// <summary>
        /// 根据Id删除
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="ids"></param>
        /// <returns></returns>
        public static Task<int> DeleteByIds<T>(string ids, SqlQuery sql = null) where T : class
        {
            using (var db = CreateDbBase())
            {
                var result = db.DeleteByIds<T>(ids, sql);
                return Task.Factory.StartNew(() => result);
            }
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static Task<IEnumerable<T>> Query<T>(SqlQuery sql = null) where T : class
        {
            using (var db = CreateDbBase())
            {
                var result = db.Query<T>(sql);
                return Task.Factory.StartNew(() => result);
            }
        }

        /// <summary>
        /// 获取满足条件的第一个数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static Task<T> SingleOrDefault<T>(SqlQuery sql) where T : class
        {
            using (var db = CreateDbBase())
            {
                var result = db.SingleOrDefault<T>(sql);
                return Task.Factory.StartNew(() => result);
            }
        }

        /// <summary>
        /// 获取满足条件的第一个数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static Task<T> SingleOrDefault<T>(SqlQuery sql = null, object id = null) where T : class
        {
            using (var db = CreateDbBase())
            {
                var result = db.SingleOrDefault<T>(sql, id);
                return Task.Factory.StartNew(() => result);
            }
        }

        /// <summary>
        /// 获取总数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static Task<int> Count<T>(SqlQuery sql = null) where T : class
        {
            using (var db = CreateDbBase())
            {
                var result = db.Count<T>(sql);
                return Task.Factory.StartNew(() => result);
            }
        }

        /// <summary>
        ///     复杂查询分页
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="querySql">查询语句</param>
        /// <param name="queryParam">查询参数</param>
        /// <returns>分页结果</returns>
        /// <remarks>
        ///     注意事项：
        ///     1.sql语句中需要加上@where、@orderBy、@rowNumber、@recordCount标记
        ///     如: "select *, @rowNumber, @recordCount from ADM_Rule @where"
        ///     2.实体中需增加扩展属性，作记录总数输出：RecordCount
        ///     3.标记解释:
        ///     @where：      查询条件
        ///     @orderBy：    排序
        ///     @x：          分页记录起点
        ///     @y：          分页记录终点
        ///     @recordCount：记录总数
        ///     @rowNumber：  行号
        ///     4.示例参考:
        /// </remarks>
        public static Task<PagedResults<T>> PagingQueryAsync<T>(string querySql, QueryParam queryParam)
        {
            var sql = queryParam.IsReport ?
                string.Format(@"select * from ({0}) seq ", querySql) :
                string.Format(@"select * from ({0}) seq where seq.rownum between @x and @y", querySql);
            var currentPage = queryParam.Page; //当前页号
            var pageSize = queryParam.Rows; //每页记录数
            var lower = ((currentPage - 1) * pageSize) + 1; //记录起点
            var upper = currentPage * pageSize; //记录终点
            var where = @" where 1=1 ";
            if (!string.IsNullOrEmpty(queryParam._filters))
            {
                where += queryParam.Filters;
            }
            var parms = new DynamicParameters();
            parms.Add("x", lower);
            parms.Add("y", upper);
            //排序字段
            var orderString = string.Format("{0} {1}", queryParam.Sidx, queryParam.Sord);
            sql = sql.Replace("@recordCount", " count(*) over() as RecordCount ")
                .Replace("@rowNumber", " row_number() over (order by @orderBy) as rownum ")
                .Replace("@orderBy", orderString)
                .Replace("@where", where);
            var data = SqlWithParams<T>(sql, parms).Result.ToList();
            var pagerInfo = new PagerInfo();
            var first = data.FirstOrDefault();
            //记录总数
            if (first != null)
                pagerInfo.RecordCount = (int)first.GetType().GetProperty("RecordCount").GetValue(first, null);
            pagerInfo.Page = queryParam.Page;
            pagerInfo.PageCount = (pagerInfo.RecordCount + queryParam.Rows - 1) / queryParam.Rows; //页总数 
            return Task.Factory.StartNew(() => new PagedResults<T> { Data = data, PagerInfo = pagerInfo });
        }

        /// <summary>
        /// 单表存储过程分页
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="queryParam">分页参数</param>
        /// <returns>返回值</returns>
        public static Task<PagedResults<T>> PagingQueryProcAsync<T>(QueryParam queryParam, SqlQuery sql = null) where T : class
        {
            using (var dbs = CreateDbBase())
            {
                if (sql == null)
                    sql = SqlQuery<T>.Builder(dbs);
                //类型
                var parms = new DynamicParameters();
                parms.Add("TableName", sql._ModelDes.TableName);
                parms.Add("PrimaryKey", DapperCacheCommon.GetPrimary(sql._ModelDes).Field);
                parms.Add("Fields", "*");
                parms.Add("Filters", queryParam.Filters);
                parms.Add("PageIndex", queryParam.Page);
                parms.Add("PageSize", queryParam.Rows);
                parms.Add("Sort", queryParam.Sidx + " " + queryParam.Sord);
                parms.Add("RecordCount", dbType: DbType.Int32, direction: ParameterDirection.Output);
                var pagerInfo = new PagerInfo();
                var data = StoredProcWithParams<T>("System_Proc_Paging", parms).Result.ToList();
                pagerInfo.RecordCount = parms.Get<int>("RecordCount");
                pagerInfo.Page = queryParam.Page;
                pagerInfo.PageCount = (pagerInfo.RecordCount + queryParam.Rows - 1) / queryParam.Rows; //页总数 
                return Task.Factory.StartNew(() => new PagedResults<T> { Data = data, PagerInfo = pagerInfo });
            }
        }

        /// <summary>
        /// 自定义存储过程分页
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="queryParam">分页参数</param>
        /// <returns>返回值</returns>
        public static Task<PagedResults<T>> PagingQueryProcCustomAsync<T>(QueryParam queryParam)
        {
            var parms = new DynamicParameters();
            parms.Add("TableName", queryParam.TableName);
            parms.Add("PrimaryKey", queryParam.PrimaryKey);
            parms.Add("Fields", queryParam.Fields);
            parms.Add("Filters", queryParam.Filters);
            parms.Add("PageIndex", queryParam.Page);
            parms.Add("PageSize", queryParam.Rows);
            parms.Add("Sort", queryParam.Sidx + " " + queryParam.Sord);
            parms.Add("RecordCount", dbType: DbType.Int32, direction: ParameterDirection.Output);
            var pagerInfo = new PagerInfo();
            var data = StoredProcWithParams<T>("System_Proc_Paging", parms).Result.ToList();
            pagerInfo.RecordCount = parms.Get<int>("RecordCount");
            pagerInfo.Page = queryParam.Page;
            pagerInfo.PageCount = (pagerInfo.RecordCount + queryParam.Rows - 1) / queryParam.Rows; //页总数 
            return Task.Factory.StartNew(() => new PagedResults<T> { Data = data, PagerInfo = pagerInfo }); ;
        }
        #endregion

        #region 增删改
        /// <summary>
        ///     执行增加删除修改语句
        /// </summary>
        /// <param name="sql">Sql语句</param>
        /// <param name="parms">参数信息</param>
        /// <param name="isSetConnectionStr">是否需要重置连接字符串</param>
        /// <returns>影响数</returns>
        public static Task<int> InsertUpdateOrDeleteSql<T>(string sql, dynamic parms = null, bool isSetConnectionStr = true)
        {
            using (var db = CreateDbBase())
            {
                var result = db.InsertUpdateOrDeleteSql(sql, (object)parms);
                return Task.Factory.StartNew(() => result);
            }
        }

        /// <summary>
        ///     执行增加删除修改语句
        /// </summary>
        /// <param name="sql">Sql语句</param>
        /// <param name="parms">参数信息</param>
        /// <param name="isSetConnectionStr">是否需要重置连接字符串</param>
        /// <returns>影响数</returns>
        public static Task<bool> InsertUpdateOrDeleteSqlBool<T>(string sql, dynamic parms = null, bool isSetConnectionStr = true)
        {
            using (var db = CreateDbBase())
            {
                var result = db.InsertUpdateOrDeleteSql(sql, (object)parms) > 0;
                return Task.Factory.StartNew(() => result);
            }
        }
        #endregion

        #region 查询
        /// <summary>
        ///     执行Sql语句带参数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="parms"></param>
        /// <param name="isSetConnectionStr"></param>
        /// <returns></returns>
        public static Task<IEnumerable<T>> SqlWithParams<T>(string sql, dynamic parms = null, bool isSetConnectionStr = true)
        {
            using (var db = CreateDbBase())
            {
                var result = db.SqlWithParams<T>(sql, (object)parms);
                return Task.Factory.StartNew(() => result);
            }
        }

        /// <summary>
        ///     执行语句返回bool
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parms"></param>
        /// <param name="isSetConnectionStr"></param>
        /// <returns></returns>
        public static Task<bool> SqlWithParamsBool<T>(string sql, dynamic parms, bool isSetConnectionStr = true)
        {
            using (var db = CreateDbBase())
            {
                var result = db.SqlWithParamsBool(sql, (object)parms);
                return Task.Factory.StartNew(() => result);
            }
        }

        /// <summary>
        /// 执行语句返回第一个
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="parms"></param>
        /// <param name="isSetConnectionStr"></param>
        /// <returns></returns>
        public static Task<T> SqlWithParamsSingle<T>(string sql, dynamic parms = null, bool isSetConnectionStr = true)
        {
            using (var db = CreateDbBase())
            {
                var result = db.SqlWithParamsSingle<T>(sql, (object)parms);
                return Task.Factory.StartNew(() => result);
            }
        }

        #endregion

        #region 存储过程

        /// <summary>
        ///     存储过程查询所有值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="procName">The procname.</param>
        /// <param name="parms">The parms.</param>
        /// <param name="isSetConnectionStr"></param>
        /// <returns></returns>
        public static Task<IEnumerable<T>> StoredProcWithParams<T>(string procName, dynamic parms,
            bool isSetConnectionStr = true)
        {
            using (var db = CreateDbBase())
            {
                var result = db.StoredProcWithParams<T>(procName, (object)parms);
                return Task.Factory.StartNew(() => result);
            }
        }

        /// <summary>
        /// 存储过程查询所有值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="procName"></param>
        /// <param name="parms"></param>
        /// <param name="isSetConnectionStr"></param>
        /// <returns></returns>
        public static IEnumerable<T> StoredProcWithParamsSync<T>(string procName, dynamic parms,
            bool isSetConnectionStr = true)
        {
            using (var db = CreateDbBase())
            {
                return db.StoredProcWithParams<T>(procName, (object)parms);
            }
        }

        /// <summary>
        /// 增删改查存储过程
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="procName"></param>
        /// <param name="parms"></param>
        /// <param name="isSetConnectionStr"></param>
        /// <returns></returns>
        public static Task<int> InsertUpdateOrDeleteStoredProc<T>(string procName, dynamic parms = null,
            bool isSetConnectionStr = true)
        {
            using (var db = CreateDbBase())
            {
                var result = db.InsertUpdateOrDeleteStoredProc(procName, (object)parms);
                return Task.Factory.StartNew(() => result);
            }
        }

        /// <summary>
        /// 返回存储过程第一个
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="procName"></param>
        /// <param name="parms"></param>
        /// <param name="isSetConnectionStr"></param>
        /// <returns></returns>
        public static Task<T> StoredProcWithParamsSingle<T>(string procName, dynamic parms = null,
            bool isSetConnectionStr = true)
        {
            using (var db = CreateDbBase())
            {
                var result = db.StoredProcWithParamsSingle<T>(procName, (object)parms);
                return Task.Factory.StartNew(() => result);
            }
        }
        #endregion
    }
}