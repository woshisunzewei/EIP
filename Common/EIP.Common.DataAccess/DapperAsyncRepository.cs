using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EIP.Common.Dapper;
using EIP.Common.Entities.Paging;

namespace EIP.Common.DataAccess
{
    /// <summary>
    ///     DapperRepository仓储,T代表实体信息,规范约束为T必须继承IEntityBase接口
    /// </summary>
    /// <typeparam name="T">实体</typeparam>
    public class DapperAsyncRepository<T> : BaseRepository, IAsyncRepository<T> where T : class, new()
    {
        #region 修改

        /// <summary>
        ///     更新
        /// </summary>
        /// <param name="current">实体信息</param>
        /// <returns>影响条数</returns>
        public virtual Task<int> UpdateAsync(T current)
        {
            return SqlMapperUtil.Update<T>(current);
        }

        #endregion

        #region 增加

        /// <summary>
        ///     插入
        /// </summary>
        /// <param name="entity">实体信息</param>
        /// <returns></returns>
        public virtual Task<int> InsertAsync(T entity)
        {
            return SqlMapperUtil.Insert(entity);
        }

        /// <summary>
        ///     插入
        /// </summary>
        /// <param name="entity">实体信息</param>
        /// <returns></returns>
        public virtual Task<int> InsertScalarAsync(T entity)
        {
            return SqlMapperUtil.InsertScalar(entity);
        }

        /// <summary>
        ///     批量插入
        /// </summary>
        /// <typeparam name="T">实体信息</typeparam>
        /// <param name="list">实体集合</param>
        /// <returns>影响条数</returns>
        public virtual Task<int> InsertMultipleDapperAsync(IEnumerable<T> list)
        {
            return SqlMapperUtil.InsertBatch(list);
        }

        /// <summary>
        ///     批量插入
        /// </summary>
        /// <typeparam name="T">实体信息</typeparam>
        /// <param name="list">实体集合</param>
        /// <returns>影响条数</returns>
        public virtual Task<int> InsertMultipleAsync(IEnumerable<T> list)
        {
            return Task.Run(() => SqlMapperUtil.InsertWithBulkCopy(list.ToList()));
        }

        #endregion

        #region 删除

        /// <summary>
        ///     根据Id删除
        /// </summary>
        /// <param name="id">主键Id</param>
        /// <returns>影响条数</returns>
        public virtual Task<int> DeleteAsync(object id)
        {
            return SqlMapperUtil.DeleteById<T>(null, id);
        }

        /// <summary>
        ///     根据Id删除
        /// </summary>
        /// <param name="ids">主键Id</param>
        /// <returns>影响条数</returns>
        public virtual Task<int> DeleteByIdsAsync(string ids)
        {
            return SqlMapperUtil.DeleteByIds<T>(ids);
        }

        /// <summary>
        ///     删除所有
        /// </summary>
        /// <returns>影响条数</returns>
        public virtual Task<int> DeleteAllAsync()
        {
            return SqlMapperUtil.Delete<T>();
        }

        #endregion

        #region 查询

        /// <summary>
        ///     获取所有信息
        /// </summary>
        /// <returns></returns>
        public virtual Task<IEnumerable<T>> GetAllEnumerableAsync()
        {
            return SqlMapperUtil.Query<T>();
        }

        /// <summary>
        ///     根据主句Id获取实体信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual Task<T> GetByIdAsync(object id)
        {
            return SqlMapperUtil.SingleOrDefault<T>(null, id);
        }

        /// <summary>
        ///    获取总数
        /// </summary>
        /// <returns></returns>
        public virtual Task<int> Count()
        {
            return SqlMapperUtil.Count<T>();
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
        public virtual Task<PagedResults<T>> PagingQueryAsync<T>(string querySql, QueryParam queryParam)
        {
            return SqlMapperUtil.PagingQueryAsync<T>(querySql, queryParam);
        }

        /// <summary>
        /// 单表存储过程分页
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="queryParam">分页参数</param>
        /// <returns>返回值</returns>
        public Task<PagedResults<T>> PagingQueryProcAsync(QueryParam queryParam)
        {
            return SqlMapperUtil.PagingQueryProcAsync<T>(queryParam);
        }

        /// <summary>
        /// 自定义存储过程分页
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="queryParam">分页参数</param>
        /// <returns>返回值</returns>
        public Task<PagedResults<T>> PagingQueryProcCustomAsync(QueryParam queryParam)
        {
            return SqlMapperUtil.PagingQueryProcCustomAsync<T>(queryParam);
        }
        #endregion
    }
}