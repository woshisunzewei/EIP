using System.Collections.Generic;
using System.Threading.Tasks;
using EIP.Common.Business;
using EIP.Common.Entities;
using EIP.Common.Entities.Dtos;
using EIP.System.Models.Enums;
using EIP.System.Models.Dtos.Identity;
using EIP.System.Models.Entities;

namespace EIP.System.Business.Identity
{
    public interface ISystemGroupLogic : IAsyncLogic<SystemGroup>
    {
        /// <summary>
        ///     根据组织机构获取组信息
        /// </summary>
        /// <param name="input">组织机构</param>
        /// <returns></returns>
        Task<IEnumerable<SystemGroupOutput>> GetGroupByOrganizationId(NullableIdInput input);

        /// <summary>
        ///     检测配置项代码是否已经具有重复项
        /// </summary>
        /// <param name="input">需要验证的参数</param>
        /// <returns></returns>
        Task<OperateStatus> CheckGroupCode(CheckSameValueInput input);

        /// <summary>
        ///     删除组信息
        /// </summary>
        /// <param name="input">组Id</param>
        /// <returns></returns>
       Task< OperateStatus> DeleteGroup(IdInput input);

        /// <summary>
        ///     保存组信息
        /// </summary>
        /// <param name="group">岗位信息</param>
        /// <param name="belongTo">归属</param>
        /// <returns></returns>
        Task<OperateStatus> SaveGroup(SystemGroup group,
            EnumGroupBelongTo belongTo);
    }
}