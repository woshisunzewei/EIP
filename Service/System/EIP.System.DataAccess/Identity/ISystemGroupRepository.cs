using System.Collections.Generic;
using System.Threading.Tasks;
using EIP.Common.DataAccess;
using EIP.Common.Entities.Dtos;
using EIP.System.Models.Dtos.Identity;
using EIP.System.Models.Entities;

namespace EIP.System.DataAccess.Identity
{
    public interface ISystemGroupRepository : IAsyncRepository<SystemGroup>
    {
        /// <summary>
        ///     查询归属某组织下的组信息
        /// </summary>
        /// <param name="input">组织机构PostId</param>
        /// <returns>组信息</returns>
        Task<IEnumerable<SystemGroupOutput>> GetGroupByOrganizationId(NullableIdInput input);

        /// <summary>
        ///     检查代码:唯一性检查
        /// </summary>
        /// <param name="input">代码</param>
        /// <returns></returns>
        Task<bool> CheckGroupCode(CheckSameValueInput input);
    }
}