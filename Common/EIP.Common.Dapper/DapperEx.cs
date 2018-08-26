using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using EIP.Common.Core.Extensions;
using EIP.Common.Core.Log;
using EIP.Common.Dapper.AdoNet;
using EIP.Common.Dapper.SQL;
using EIP.Common.Entities;
using EIP.Common.Entities.CustomAttributes;
using Newtonsoft.Json;

namespace EIP.Common.Dapper
{
    public static class DapperEx
    {
        #region 写入日志
        /// <summary>
        /// 写入SqlLog日志
        /// </summary>
        /// <param name="log"></param>
        public static async Task WriteSqlLog(SqlLog log)
        {
            SqlLogHandler handler = new SqlLogHandler(log.OperateSql, log.EndDateTime, log.ElapsedTime, log.Parameter);
            handler.WriteLog();
        }

        private static DataLogHandler _dataLoginHandler;
        /// <summary>
        /// 写入日志
        /// </summary>
        /// <param name="operateType"></param>
        /// <param name="tableName">表名</param>
        /// <param name="current"></param>
        /// <returns></returns>
        private static async Task WriteDataLog(OperateType operateType, string tableName, object current = null)
        {
            string operateAfterData = string.Empty;
            if (operateType == OperateType.编辑)
            {
                //operateAfterData = JsonConvert.SerializeObject(Task.Run(async () => await GetByIdFromDataBase(current)).Result);
            }
            _dataLoginHandler = new DataLogHandler((byte)operateType, tableName, JsonConvert.SerializeObject(current), operateAfterData);
            _dataLoginHandler.WriteLog();
        }

        #endregion

        #region 映射

        /// <summary>
        ///     插入数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dbs"></param>
        /// <param name="t"></param>
        /// <param name="transaction"></param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        public static int Insert<T>(this DbBase dbs, T t, IDbTransaction transaction = null, int? commandTimeout = null)
            where T : class
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            var db = dbs.DbConnecttion;
            var sql = SqlQuery<T>.Builder(dbs);
            SqlLog log = new SqlLog
            {
                CreateTime = DateTime.Now,
                OperateSql = sql.InsertSql
            };
            var result = db.Execute(sql.InsertSql, t, transaction, commandTimeout);
            stopwatch.Stop();
            log.EndDateTime = DateTime.Now;
            log.ElapsedTime = stopwatch.Elapsed.TotalSeconds;
            WriteSqlLog(log);
            return result;
        }

        /// <summary>
        ///     插入数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dbs"></param>
        /// <param name="t"></param>
        /// <param name="transaction"></param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        public static int InsertScalar<T>(this DbBase dbs, T t, IDbTransaction transaction = null,
            int? commandTimeout = null)
            where T : class
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            var db = dbs.DbConnecttion;
            var sql = SqlQuery<T>.Builder(dbs);
            SqlLog log = new SqlLog
            {
                CreateTime = DateTime.Now,
                OperateSql = sql.InsertSql + ";SELECT @@Identity"
            };
            var result = db.ExecuteScalar<int>(sql.InsertSql + ";SELECT @@Identity", t, transaction, commandTimeout);
            stopwatch.Stop();
            log.EndDateTime = DateTime.Now;
            log.ElapsedTime = stopwatch.Elapsed.TotalSeconds;
            WriteSqlLog(log);
            return result;
        }

        /// <summary>
        ///     批量插入数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dbs"></param>
        /// <param name="lt"></param>
        /// <param name="transaction"></param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        public static int InsertBatch<T>(this DbBase dbs, IEnumerable<T> lt, IDbTransaction transaction = null,
            int? commandTimeout = null) where T : class
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            var db = dbs.DbConnecttion;
            var sql = SqlQuery<T>.Builder(dbs);
            SqlLog log = new SqlLog
            {
                CreateTime = DateTime.Now,
                OperateSql = sql.InsertSql
            };
            var result = db.Execute(sql.InsertSql, lt, transaction, commandTimeout);
            stopwatch.Stop();
            log.EndDateTime = DateTime.Now;
            log.ElapsedTime = stopwatch.Elapsed.TotalSeconds;
            WriteSqlLog(log);
            return result;
        }

        /// <summary>
        ///     按条件删除
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dbs"></param>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static int Delete<T>(this DbBase dbs, SqlQuery sql = null) where T : class
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            var db = dbs.DbConnecttion;
            if (sql == null)
                sql = SqlQuery<T>.Builder(dbs);
            SqlLog log = new SqlLog
            {
                CreateTime = DateTime.Now,
                OperateSql = sql.DeleteSql,
                Parameter = sql.Param == null ? "" : sql.Param.ToString()
            };
            var result = db.Execute(sql.DeleteSql, sql.Param);
            stopwatch.Stop();
            log.EndDateTime = DateTime.Now;
            log.ElapsedTime = stopwatch.Elapsed.TotalSeconds;
            WriteSqlLog(log);
            return result;
        }

        /// <summary>
        ///     按Id删除
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dbs"></param>
        /// <param name="sql"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static int DeleteById<T>(this DbBase dbs, SqlQuery sql = null, object id = null) where T : class
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            var db = dbs.DbConnecttion;
            if (sql == null)
                sql = SqlQuery<T>.Builder(dbs);
            SqlLog log = new SqlLog
            {
                CreateTime = DateTime.Now,
                OperateSql = sql.DeleteSqlById,
                Parameter = id.ToString()
            };
            var result = db.Execute(sql.DeleteSqlById, new { id });
            stopwatch.Stop();
            log.EndDateTime = DateTime.Now;
            log.ElapsedTime = stopwatch.Elapsed.TotalSeconds;
            WriteSqlLog(log);
            return result;
        }

        /// <summary>
        ///     按Id删除
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dbs"></param>
        /// <param name="sql"></param>
        /// <param name="ids"></param>
        /// <returns></returns>
        public static int DeleteByIds<T>(this DbBase dbs, string ids, SqlQuery sql = null) where T : class
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            var db = dbs.DbConnecttion;
            if (sql == null)
                sql = SqlQuery<T>.Builder(dbs);

            SqlLog log = new SqlLog
            {
                CreateTime = DateTime.Now,
                OperateSql = sql.DeleteSqlByIds,
                Parameter = ids
            };
            var result = db.Execute(sql.DeleteSqlByIds.Replace("@id", ids.TrimEnd(',').SqlRemoveStr()));
            stopwatch.Stop();
            log.EndDateTime = DateTime.Now;
            log.ElapsedTime = stopwatch.Elapsed.TotalSeconds;
            WriteSqlLog(log);
            return result;
        }

        /// <summary>
        ///     修改
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dbs"></param>
        /// <param name="t">如果sql为null，则根据t的主键进行修改</param>
        /// <param name="sql">按条件修改</param>
        /// <returns></returns>
        public static int Update<T>(this DbBase dbs, T t, SqlQuery sql = null) where T : class
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            var db = dbs.DbConnecttion;
            if (sql == null)
                sql = SqlQuery<T>.Builder(dbs);
            sql = sql.AppendParam(t);
            SqlLog log = new SqlLog
            {
                CreateTime = DateTime.Now,
                OperateSql = sql.UpdateSql,
                Parameter = sql.Param == null ? "" : sql.Param.ToString()
            };
            var result = db.Execute(sql.UpdateSql, sql.Param);
            stopwatch.Stop();
            log.EndDateTime = DateTime.Now;
            log.ElapsedTime = stopwatch.Elapsed.TotalSeconds;
            WriteSqlLog(log);
            return result;
        }

        /// <summary>
        ///     修改
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dbs"></param>
        /// <param name="t">如果sql为null，则根据t的主键进行修改</param>
        /// <param name="updateProperties">要修改的属性集合</param>
        /// <param name="sql">按条件修改</param>
        /// <returns></returns>
        public static int Update<T>(this DbBase dbs, T t, IList<string> updateProperties, SqlQuery sql = null)
            where T : class
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            var db = dbs.DbConnecttion;
            if (sql == null)
                sql = SqlQuery<T>.Builder(dbs);
            sql = sql.AppendParam(t)
                .SetExcProperties<T>(updateProperties);

            SqlLog log = new SqlLog
            {
                CreateTime = DateTime.Now,
                OperateSql = sql.UpdateSql,
                Parameter = sql.Param == null ? "" : sql.Param.ToString()
            };
            var result = db.Execute(sql.UpdateSql, sql.Param);
            stopwatch.Stop();
            log.EndDateTime = DateTime.Now;
            log.ElapsedTime = stopwatch.Elapsed.TotalSeconds;
            WriteSqlLog(log);
            return result;
        }

        /// <summary>
        ///     获取默认一条数据，没有则为NULL
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dbs"></param>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static T SingleOrDefault<T>(this DbBase dbs, SqlQuery sql) where T : class
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            var db = dbs.DbConnecttion;
            if (sql == null)
                sql = SqlQuery<T>.Builder(dbs);
            sql = sql.Top(1);
            SqlLog log = new SqlLog
            {
                CreateTime = DateTime.Now,
                OperateSql = sql.QuerySql,
                Parameter = sql.Param == null ? "" : sql.Param.ToString()
            };
            var result = db.Query<T>(sql.QuerySql, sql.Param).FirstOrDefault();
            stopwatch.Stop();
            log.EndDateTime = DateTime.Now;
            log.ElapsedTime = stopwatch.Elapsed.TotalSeconds;
            WriteSqlLog(log);
            return result;
        }

        /// <summary>
        ///     获取默认一条数据，没有则为NULL
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dbs"></param>
        /// <param name="sql"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static T SingleOrDefault<T>(this DbBase dbs, SqlQuery sql = null, object id = null) where T : class
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            var db = dbs.DbConnecttion;
            if (sql == null)
                sql = SqlQuery<T>.Builder(dbs);
            sql = sql.Top(1);
            SqlLog log = new SqlLog
            {
                CreateTime = DateTime.Now,
                OperateSql = sql.QuerySqlById,
                Parameter = sql.Param == null ? "" : sql.Param.ToString()
            };
            var result = db.Query<T>(sql.QuerySqlById, new { id }).FirstOrDefault();
            stopwatch.Stop();
            log.EndDateTime = DateTime.Now;
            log.ElapsedTime = stopwatch.Elapsed.TotalSeconds;
            WriteSqlLog(log);
            return result;
        }

        /// <summary>
        ///     分页查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dbs"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="dataCount"></param>
        /// <param name="sqlQuery"></param>
        /// <returns></returns>
        public static IEnumerable<T> Page<T>(this DbBase dbs, int pageIndex, int pageSize, out long dataCount,
            SqlQuery sqlQuery = null) where T : class
        {
            var db = dbs.DbConnecttion;
            if (sqlQuery == null)
                sqlQuery = SqlQuery<T>.Builder(dbs);
            sqlQuery = sqlQuery.Page(pageIndex, pageSize);
            var para = sqlQuery.Param;
            var cr = db.Query(sqlQuery.CountSql, para).SingleOrDefault();
            dataCount = (long)cr.DataCount;
            var result = db.Query<T>(sqlQuery.PageSql, para).ToList();
            return result;
        }

        /// <summary>
        ///     查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dbs"></param>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static IEnumerable<T> Query<T>(this DbBase dbs, SqlQuery sql = null) where T : class
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            var db = dbs.DbConnecttion;
            if (sql == null)
                sql = SqlQuery<T>.Builder(dbs);
            SqlLog log = new SqlLog
            {
                CreateTime = DateTime.Now,
                OperateSql = sql.QuerySql,
                Parameter = sql.Param == null ? "" : sql.Param.ToString()
            };
            var result = db.Query<T>(sql.QuerySql, sql.Param);
            stopwatch.Stop();
            log.EndDateTime = DateTime.Now;
            log.ElapsedTime = stopwatch.Elapsed.TotalSeconds;
            WriteSqlLog(log);
            return result;
        }

        /// <summary>
        ///     数据数量
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dbs"></param>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static int Count<T>(this DbBase dbs, SqlQuery sql = null) where T : class
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            var db = dbs.DbConnecttion;
            if (sql == null)
                sql = SqlQuery<T>.Builder(dbs);
            SqlLog log = new SqlLog
            {
                CreateTime = DateTime.Now,
                OperateSql = sql.CountSql,
                Parameter = sql.Param == null ? "" : sql.Param.ToString()
            };
            var result = db.Query(sql.CountSql, sql.Param).SingleOrDefault();
            stopwatch.Stop();
            log.EndDateTime = DateTime.Now;
            log.ElapsedTime = stopwatch.Elapsed.TotalSeconds;
            WriteSqlLog(log);
            return result;
        }

        #endregion

        #region 增删改

        /// <summary>
        ///     执行增加删除修改语句
        /// </summary>
        /// <param name="dbs"></param>
        /// <param name="sql">Sql语句</param>
        /// <param name="parms">参数信息</param>
        /// <param name="isSetConnectionStr">是否需要重置连接字符串</param>
        /// <returns>影响数</returns>
        public static int InsertUpdateOrDeleteSql(this DbBase dbs, string sql, dynamic parms = null,
            bool isSetConnectionStr = true)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            SqlLog log = new SqlLog
            {
                CreateTime = DateTime.Now,
                OperateSql = sql,
                Parameter = parms == null ? "" : parms.ToString(),
            };
            var result = dbs.DbConnecttion.Execute(sql, (object)parms);
            stopwatch.Stop();
            log.EndDateTime = DateTime.Now;
            log.ElapsedTime = stopwatch.Elapsed.TotalSeconds;
            WriteSqlLog(log);
            return result;
        }

        /// <summary>
        ///     使用SqlBulkCopy批量进行插入数据
        /// </summary>
        /// <typeparam name="T">实体对象</typeparam>
        /// <param name="dbs"></param>
        /// <param name="entitys">实体对象集合</param>
        public static int InsertWithBulkCopy<T>(this DbBase dbs, List<T> entitys) where T : new()
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            int result = 1;
            using (var destinationConnection = (SqlConnection)dbs.DbConnecttion)
            {
                using (var bulkCopy = new SqlBulkCopy(destinationConnection))
                {
                    Type type = entitys[0].GetType();
                    object classAttr = type.GetCustomAttributes(false)[0];
                    if (classAttr is TableAttribute)
                    {
                        TableAttribute tableAttr = classAttr as TableAttribute;
                        bulkCopy.DestinationTableName = tableAttr.Name; //要插入的表的表明 
                    }
                    ModelHandler<T> mh = new ModelHandler<T>();
                    DataTable dt = mh.FillDataTable(entitys);
                    if (dt != null && dt.Rows.Count != 0)
                    {
                        bulkCopy.WriteToServer(dt);
                    }
                }
            }
            SqlLog log = new SqlLog
            {
                CreateTime = DateTime.Now,
                OperateSql = "BulkCopy批量插入"
            };
            stopwatch.Stop();
            log.EndDateTime = DateTime.Now;
            log.ElapsedTime = stopwatch.Elapsed.TotalSeconds;
            WriteSqlLog(log);
            return result;
        }

        #endregion

        #region 查询

        /// <summary>
        ///     执行语句返回bool
        /// </summary>
        /// <param name="dbs"></param>
        /// <param name="sql"></param>
        /// <param name="parms"></param>
        /// <returns></returns>
        public static bool SqlWithParamsBool(this DbBase dbs, string sql, dynamic parms = null)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            SqlLog log = new SqlLog
            {
                CreateTime = DateTime.Now,
                OperateSql = sql,
                Parameter = parms == null ? "" : parms.ToString()
            };
            var result = dbs.DbConnecttion.Query(sql, (object)parms).Any();
            stopwatch.Stop();
            log.EndDateTime = DateTime.Now;
            log.ElapsedTime = stopwatch.Elapsed.TotalSeconds;
            WriteSqlLog(log);
            return result;
        }

        /// <summary>
        ///     执行Sql语句带参数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dbs"></param>
        /// <param name="sql"></param>
        /// <param name="parms"></param>
        /// <returns></returns>
        public static IEnumerable<T> SqlWithParams<T>(this DbBase dbs, string sql, dynamic parms = null)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            SqlLog log = new SqlLog
            {
                CreateTime = DateTime.Now,
                OperateSql = sql,
                Parameter = parms == null ? "" : parms.ToString()
            };
            var result = dbs.DbConnecttion.Query<T>(sql, (object)parms);
            stopwatch.Stop();
            log.EndDateTime = DateTime.Now;
            log.ElapsedTime = stopwatch.Elapsed.TotalSeconds;
            WriteSqlLog(log);
            return result;
        }

        /// <summary>
        ///     返回符合要求的第一个
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dbs"></param>
        /// <param name="sql"></param>
        /// <param name="parms"></param>
        /// <returns></returns>
        public static T SqlWithParamsSingle<T>(this DbBase dbs, string sql, dynamic parms = null)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            SqlLog log = new SqlLog
            {
                CreateTime = DateTime.Now,
                OperateSql = sql,
                Parameter = parms == null ? "" : parms.ToString()
            };
            var result = dbs.DbConnecttion.Query<T>(sql, (object)parms).FirstOrDefault();
            stopwatch.Stop();
            log.EndDateTime = DateTime.Now;
            log.ElapsedTime = stopwatch.Elapsed.TotalSeconds;
            WriteSqlLog(log);
            return result;
        }

        #endregion

        #region 存储过程

        /// <summary>
        ///     存储过程增加删除修改
        /// </summary>
        /// <param name="dbs"></param>
        /// <param name="procName">存储过程名称</param>
        /// <param name="parms">参数</param>
        /// <returns>影响条数</returns>
        public static int InsertUpdateOrDeleteStoredProc(this DbBase dbs, string procName, dynamic parms = null)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            SqlLog log = new SqlLog
            {
                CreateTime = DateTime.Now,
                OperateSql = procName,
                Parameter = parms == null ? "" : parms.ToString()
            };
            var result = dbs.DbConnecttion.Execute(procName, (object)parms, commandType: CommandType.StoredProcedure);
            stopwatch.Stop();
            log.EndDateTime = DateTime.Now;
            log.ElapsedTime = stopwatch.Elapsed.TotalSeconds;
            WriteSqlLog(log);
            return result;
        }

        /// <summary>
        ///     存储过程查询所有值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dbs"></param>
        /// <param name="procName">The procName.</param>
        /// <param name="parms">The parms.</param>
        /// <returns></returns>
        public static IEnumerable<T> StoredProcWithParams<T>(this DbBase dbs, string procName, dynamic parms)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            SqlLog log = new SqlLog
            {
                CreateTime = DateTime.Now,
                OperateSql = procName,
                Parameter = parms == null ? "" : parms.ToString()
            };
            var result = dbs.DbConnecttion.Query<T>(procName, (object)parms, commandType: CommandType.StoredProcedure);
            stopwatch.Stop();
            log.EndDateTime = DateTime.Now;
            log.ElapsedTime = stopwatch.Elapsed.TotalSeconds;
            WriteSqlLog(log);
            return result;
        }

        /// <summary>
        ///     存储过程返回满足条件的第一条数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dbs"></param>
        /// <param name="procName"></param>
        /// <param name="parms"></param>
        /// <returns></returns>
        public static T StoredProcWithParamsSingle<T>(this DbBase dbs, string procName, dynamic parms = null)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            SqlLog log = new SqlLog
            {
                CreateTime = DateTime.Now,
                OperateSql = procName,
                Parameter = parms == null ? "" : parms.ToString()
            };
            var result = dbs.DbConnecttion.Query<T>(procName, (object)parms, commandType: CommandType.StoredProcedure)
                .SingleOrDefault();
            stopwatch.Stop();
            log.EndDateTime = DateTime.Now;
            log.ElapsedTime = stopwatch.Elapsed.TotalSeconds;
            WriteSqlLog(log);
            return result;
        }

        #endregion
    }
}