using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EIP.Common.Business;
using EIP.Common.Entities;
using EIP.Common.Entities.Dtos;
using EIP.System.Models.Dtos.Identity;
using EIP.System.Models.Entities;
using EIP.System.Models.Enums;

namespace EIP.System.Business.Identity
{
    public interface ISystemRoleLogic : IAsyncLogic<SystemRole>
    {
        /// <summary>
        ///     根据组织机构Id查询角色信息
        /// </summary>
        /// <param name="input"></param>
        /// <param name="isFreeze">是否冻结</param>
        /// <returns></returns>
        Task<IEnumerable<SystemRoleOutput>> GetRolesByOrganizationId(NullableIdInput input = null,
            bool? isFreeze = null);

        /// <summary>
        ///     检测配置项代码是否已经具有重复项
        /// </summary>
        /// <param name="input">需要验证的参数</param>
        /// <returns></returns>
        Task<OperateStatus> CheckRoleCode(CheckSameValueInput input);

        /// <summary>
        ///     保存岗位信息
        /// </summary>
        /// <param name="role">岗位信息</param>
        /// <returns></returns>
        Task<OperateStatus> SaveRole(SystemRole role);

        /// <summary>
        ///     删除角色信息
        /// </summary>
        /// <param name="input">角色Id</param>
        /// <returns></returns>
        Task<OperateStatus> DeleteRole(IdInput input);

        /// <summary>
        ///     获取该用户已经具有的角色信息
        /// </summary>
        /// <param name="privilegeMaster"></param>
        /// <param name="userId">需要查询的用户id</param>
        /// <returns></returns>
        Task<IEnumerable<SystemRole>> GetHaveUserRole(EnumPrivilegeMaster privilegeMaster,
            Guid userId);
    }
}