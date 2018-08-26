using System.Collections.Generic;
using System.Threading.Tasks;
using EIP.Common.Business;
using EIP.Common.Entities;
using EIP.System.Models.Dtos.Config;
using EIP.System.Models.Entities;

namespace EIP.System.Business.Config
{
    /// <summary>
    ///     系统配置文件接口
    /// </summary>
    public interface ISystemConfigLogic : IAsyncLogic<SystemConfig>
    {
        /// <summary>
        /// 获取配置信息
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<SystemConfigDoubleWay>> GetConfig();

            /// <summary>
        ///     保存系统配置文件
        /// </summary>
        /// <param name="doubleWays">系统配置文件信息</param>
        /// <returns></returns>
        Task<OperateStatus> SaveConfig(IEnumerable<SystemConfigDoubleWay> doubleWays);
    }
}