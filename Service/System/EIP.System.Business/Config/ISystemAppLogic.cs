using System.Threading.Tasks;
using EIP.Common.Business;
using EIP.Common.Entities;
using EIP.Common.Entities.Dtos;
using EIP.System.Models.Entities;

namespace EIP.System.Business.Config
{
    public interface ISystemAppLogic : IAsyncLogic<SystemApp>
    {
        /// <summary>
        ///     保存数据库配置
        /// </summary>
        /// <param name="app">数据库配置</param>
        /// <returns></returns>
        Task<OperateStatus> SaveApp(SystemApp app);

        /// <summary>
        ///     检查代码是否具有重复
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<OperateStatus> CheckAppCode(CheckSameValueInput input);
    }
}