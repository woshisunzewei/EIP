using System.Collections.Generic;
using System.Threading.Tasks;
using EIP.Common.Dapper;
using EIP.Common.DataAccess;
using EIP.System.Models.Entities;

namespace EIP.System.DataAccess.Log
{
    public class SystemLoginLogRepository : DapperAsyncRepository<SystemLoginLog>, ISystemLoginLogRepository
    {
        /// <summary>
        /// 获取日志分析报表
        /// </summary>
        /// <returns></returns>
        public Task<IEnumerable<SystemLoginLog>> GetBrowserAnalysis()
        {
            const string sql = "SELECT UserAgent FROM System_LoginLog";
            return SqlMapperUtil.SqlWithParams<SystemLoginLog>(sql);
        }
    }
}