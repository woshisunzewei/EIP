using System.Threading.Tasks;
using EIP.Common.Core.Extensions;
using EIP.Common.Dapper;
using EIP.Common.DataAccess;
using EIP.Common.Entities.Dtos;
using EIP.System.Models.Entities;

namespace EIP.System.DataAccess.Config
{
    /// <summary>
    ///     系统配置文件数据访问接口实现
    /// </summary>
    public class SystemAppRepository : DapperAsyncRepository<SystemApp>, ISystemAppRepository
    {
        /// <summary>
        ///     检查配置项代码:唯一性检查
        /// </summary>
        /// <param name="input">查询参数</param>
        /// <returns></returns>
        public Task<bool> CheckAppCode(CheckSameValueInput input)
        {
            var sql = "SELECT AppId FROM System_App WHERE Code=@param";
            if (!input.Id.IsNullOrEmptyGuid())
            {
                sql += " AND AppId!=@appId";
            }
            return  SqlMapperUtil.SqlWithParamsBool<SystemApp>(sql, new
            {
                param = input.Param,
                appId = input.Id
            });
        }
    }
}