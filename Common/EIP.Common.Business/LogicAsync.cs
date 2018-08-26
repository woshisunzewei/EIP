using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EIP.Common.DataAccess;
using EIP.Common.Entities;
using EIP.Common.Core.Resource;
using EIP.Common.Entities.Paging;

namespace EIP.Common.Business
{
    /// <summary>
    /// 异步Logic
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class AsyncLogic<T> : IAsyncLogic<T> where T : class, new()
    {
        public AsyncLogic() { }

        public IAsyncRepository<T> Repository;

        public AsyncLogic(IAsyncRepository<T> repository)
        {
            if (repository == null)
            {
                throw new ArgumentNullException("repository", "repository为空");
            }
            Repository = repository;
        }

        /// <summary>
        ///     新增
        /// </summary>
        /// <param name="entity">新增实体</param>
        /// <returns></returns>
        public async Task<OperateStatus> InsertAsync(T entity)
        {
            var operateStatus = new OperateStatus();
            try
            {
                var resultNum = await Repository.InsertAsync(entity);
                operateStatus.ResultSign = resultNum > 0 ? ResultSign.Successful : ResultSign.Error;
                operateStatus.Message = resultNum > 0 ? Chs.Successful : Chs.Error;
            }
            catch (Exception exception)
            {
                operateStatus.Message = string.Format(Chs.Error, exception.Message);
            }
            return operateStatus;
        }

        /// <summary>
        ///     新增并返回新增值(int)
        /// </summary>
        /// <param name="entity">新增实体</param>
        /// <returns></returns>
        public async Task<OperateStatus<int>> InsertScalarAsync(T entity)
        {
            var operateStatus = new OperateStatus<int>();
            try
            {
                var resultNum = await Repository.InsertScalarAsync(entity);
                operateStatus.ResultSign = resultNum > 0 ? ResultSign.Successful : ResultSign.Error;
                operateStatus.Message = resultNum > 0 ? Chs.Successful : Chs.Error;
                operateStatus.Data = resultNum;
            }
            catch (Exception exception)
            {
                operateStatus.Message = string.Format(Chs.Error, exception.Message);
            }
            return operateStatus;
        }

        /// <summary>
        ///     新增
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public async Task<OperateStatus> InsertMultipleDapperAsync(IEnumerable<T> list)
        {
            var operateStatus = new OperateStatus();
            try
            {
                var resultNum = await Repository.InsertMultipleDapperAsync(list);
                operateStatus.ResultSign = resultNum > 0 ? ResultSign.Successful : ResultSign.Error;
                operateStatus.Message = resultNum > 0 ? Chs.Successful : Chs.Error;
            }
            catch (Exception exception)
            {
                operateStatus.Message = string.Format(Chs.Error, exception.Message);
            }
            return operateStatus;
        }

        /// <summary>
        ///     新增
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public async Task<OperateStatus> InsertMultipleAsync(IEnumerable<T> list)
        {
            var operateStatus = new OperateStatus();
            try
            {
                var resultNum = await Repository.InsertMultipleAsync(list);
                operateStatus.ResultSign = resultNum > 0 ? ResultSign.Successful : ResultSign.Error;
                operateStatus.Message = resultNum > 0 ? Chs.Successful : Chs.Error;
            }
            catch (Exception exception)
            {
                operateStatus.Message = string.Format(Chs.Error, exception.Message);
            }
            return operateStatus;
        }

        /// <summary>
        ///     更新
        /// </summary>
        /// <param name="current">更新实体</param>
        /// <returns></returns>
        public async Task<OperateStatus> UpdateAsync(T current)
        {
            var operateStatus = new OperateStatus();
            try
            {
                var resultNum = await Repository.UpdateAsync(current);
                operateStatus.ResultSign = resultNum > 0 ? ResultSign.Successful : ResultSign.Error;
                operateStatus.Message = resultNum > 0 ? Chs.Successful : Chs.Error;
            }
            catch (Exception exception)
            {
                operateStatus.Message = string.Format(Chs.Error, exception.Message);
            }
            return operateStatus;
        }

        /// <summary>
        ///     删除
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns></returns>
        public async Task<OperateStatus> DeleteAsync(T entity)
        {
            var operateStatus = new OperateStatus();
            try
            {
                var resultNum = await Repository.DeleteAsync(entity);
                operateStatus.ResultSign = resultNum > 0 ? ResultSign.Successful : ResultSign.Error;
                operateStatus.Message = resultNum > 0 ? Chs.Successful : Chs.Error;
            }
            catch (Exception exception)
            {
                operateStatus.Message = string.Format(Chs.Error, exception.Message);
            }
            return operateStatus;
        }

        /// <summary>
        ///     根据主键删除
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns></returns>
        public async Task<OperateStatus> DeleteAsync(object id)
        {
            var operateStatus = new OperateStatus();
            try
            {
                var resultNum = await Repository.DeleteAsync(id);
                operateStatus.ResultSign = resultNum > 0 ? ResultSign.Successful : ResultSign.Error;
                operateStatus.Message = resultNum > 0 ? Chs.Successful : Chs.Error;
            }
            catch (Exception exception)
            {
                operateStatus.Message = string.Format(Chs.Error, exception.Message);
            }
            return operateStatus;
        }

        /// <summary>
        ///     删除匹配项
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<OperateStatus> DeleteByIdsAsync(string ids)
        {
            var operateStatus = new OperateStatus();
            try
            {
                var resultNum = await Repository.DeleteByIdsAsync(ids);
                operateStatus.ResultSign = resultNum > 0 ? ResultSign.Successful : ResultSign.Error;
                operateStatus.Message = resultNum > 0 ? Chs.Successful : Chs.Error;
            }
            catch (Exception exception)
            {
                operateStatus.Message = string.Format(Chs.Error, exception.Message);
            }
            return operateStatus;
        }

        /// <summary>
        /// 删除所有
        /// </summary>
        /// <returns></returns>
        public async Task<OperateStatus> DeleteAllAsync()
        {
            var operateStatus = new OperateStatus();
            try
            {
                var resultNum = await Repository.DeleteAllAsync();
                operateStatus.ResultSign = resultNum > 0 ? ResultSign.Successful : ResultSign.Error;
                operateStatus.Message = resultNum > 0 ? Chs.Successful : Chs.Error;
            }
            catch (Exception exception)
            {
                operateStatus.Message = string.Format(Chs.Error, exception.Message);
            }
            return operateStatus;
        }

        /// <summary>
        ///     获取集合数据
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<T>> GetAllEnumerableAsync()
        {
            return await Repository.GetAllEnumerableAsync();
        }

        /// <summary>
        ///     根据主键获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<T> GetByIdAsync(object id)
        {
            return await Repository.GetByIdAsync(id);
        }

        /// <summary>
        /// 单表分页查询
        /// </summary>
        /// <param name="queryParam"></param>
        /// <returns></returns>
        public async Task<PagedResults<T>> PagingQueryProcAsync(QueryParam queryParam)
        {
            return await Repository.PagingQueryProcAsync(queryParam);
        }

        /// <summary>
        /// 获取总数
        /// </summary>
        /// <returns></returns>
        public async Task<int> Count()
        {
            return await Repository.Count();
        }
    }
}