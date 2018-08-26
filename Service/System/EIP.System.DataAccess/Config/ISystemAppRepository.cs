using System.Threading.Tasks;
using EIP.Common.DataAccess;
using EIP.Common.Entities.Dtos;
using EIP.System.Models.Entities;

namespace EIP.System.DataAccess.Config
{
    /// <summary>
    /// 系统信息
    /// </summary>
    public interface ISystemAppRepository : IAsyncRepository<SystemApp>
    {
        /// <summary>
        /// 检查信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<bool> CheckAppCode(CheckSameValueInput input);
    }
}