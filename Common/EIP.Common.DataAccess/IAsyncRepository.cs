using System.Collections.Generic;
using System.Threading.Tasks;
using EIP.Common.Entities.Paging;

namespace EIP.Common.DataAccess
{
    public interface IAsyncRepository<T> where T : class
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity">新增实体</param>
        /// <returns></returns>
        Task<int> InsertAsync(T entity);

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity">新增实体</param>
        /// <returns></returns>
        Task<int> InsertScalarAsync(T entity);

        /// <summary>
        /// Dapper批量新增
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        Task<int> InsertMultipleDapperAsync(IEnumerable<T> list);

        /// <summary>
        /// SqlBulkCopy批量新增
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        Task<int> InsertMultipleAsync(IEnumerable<T> list);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="current">更新实体</param>
        /// <returns></returns>
        Task<int> UpdateAsync(T current);

        /// <summary>
        /// 根据主键删除
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns></returns>
        Task<int> DeleteAsync(object id);

        /// <summary>
        /// 根据主键删除
        /// </summary>
        /// <returns></returns>
        Task<int> DeleteByIdsAsync(string ids);

        /// <summary>
        /// 根据主键删除
        /// </summary>
        /// <returns></returns>
        Task<int> DeleteAllAsync();

        /// <summary>
        /// 获取总数
        /// </summary>
        /// <returns></returns>
        Task<int> Count();

        /// <summary>
        /// 获取集合数据
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<T>> GetAllEnumerableAsync();

        /// <summary>
        /// 根据主键获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<T> GetByIdAsync(object id);

        /// <summary>
        /// 复杂查询分页
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="querySql">查询语句</param>
        /// <param name="queryParam">查询参数</param>
        /// <returns>分页结果</returns>
        /// <remarks>
        /// 注意事项：
        /// 1.sql语句中需要加上@where、@orderBy、@rowNumber、@recordCount标记
        ///     如: "select *, @rowNumber, @recordCount from ADM_Rule @where"
        /// 2.实体中需增加扩展属性，作记录总数输出：RecordCount
        /// 3.标记解释:
        ///     @where：      查询条件
        ///     @orderBy：    排序
        ///     @x：          分页记录起点
        ///     @y：          分页记录终点
        ///     @recordCount：记录总数
        ///     @rowNumber：  行号
        /// 4.示例参考:
        /// </remarks>
        Task<PagedResults<T>> PagingQueryAsync<T>(string querySql, QueryParam queryParam);

        /// <summary>
        /// 存储过程分页
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="queryParam"></param>
        /// <returns></returns>
        Task<PagedResults<T>> PagingQueryProcAsync(QueryParam queryParam);
    }
}