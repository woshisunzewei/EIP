using System.Collections.Generic;
using System.Threading.Tasks;
using EIP.Common.Dapper;
using EIP.Common.DataAccess;
using EIP.System.Models.Dtos.Config;
using EIP.System.Models.Entities;

namespace EIP.System.DataAccess.Config
{
    /// <summary>
    ///     系统配置文件数据访问接口实现
    /// </summary>
    public class SystemConfigRepository : DapperAsyncRepository<SystemConfig>, ISystemConfigRepository
    {
       /// <summary>
        /// 获取配置信息
        /// </summary>
        /// <returns></returns>
        public Task<IEnumerable<SystemConfigDoubleWay>> GetConfig()
        {
            const string sql = "SELECT ConfigId C,Value V FROM System_Config";
            return SqlMapperUtil.SqlWithParams<SystemConfigDoubleWay>(sql);
        }
    }
}