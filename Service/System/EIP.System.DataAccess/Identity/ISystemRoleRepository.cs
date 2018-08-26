using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EIP.Common.DataAccess;
using EIP.Common.Entities.Dtos;
using EIP.System.Models.Dtos.Identity;
using EIP.System.Models.Entities;
using EIP.System.Models.Enums;

namespace EIP.System.DataAccess.Identity
{
    public interface ISystemRoleRepository : IAsyncRepository<SystemRole>
    {
        /// <summary>
        ///     根据组织机构获取角色信息
        /// </summary>
        /// <param name="input"></param>
        /// <param name="isFreeze">是否冻结</param>
        /// <returns></returns>
        Task<IEnumerable<SystemRoleOutput>> GetRolesByOrganizationId(NullableIdInput input = null,
            bool? isFreeze = null);

        /// <summary>
        ///     检查代码:唯一性检查
        /// </summary>
        /// <param name="input">代码</param>
        /// <returns></returns>
        Task<bool> CheckPostCode(CheckSameValueInput input);

        /// <summary>
        ///     获取该用户已经具有的角色信息
        /// </summary>
        /// <param name="privilegeMaster"></param>
        /// <param name="userId">需要查询的用户id</param>
        /// <returns></returns>
        Task<IEnumerable<SystemRoleOutput>> GetHaveUserRole(EnumPrivilegeMaster privilegeMaster,
            Guid userId);
    }
}