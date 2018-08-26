using System.Collections.Generic;
using System.Threading.Tasks;
using EIP.Common.DataAccess;
using EIP.System.Models.Dtos.Config;
using EIP.System.Models.Entities;

namespace EIP.System.DataAccess.Config
{
    /// <summary>
    ///     系统配置文件数据访问接口
    /// </summary>
    public interface ISystemConfigRepository : IAsyncRepository<SystemConfig>
    {
        /// <summary>
        /// 获取配置信息
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<SystemConfigDoubleWay>> GetConfig();
    }
}