using System.Collections.Generic;
using System.Threading.Tasks;
using EIP.Common.Business;
using EIP.Common.Entities;
using EIP.Common.Entities.Dtos;
using EIP.System.Models.Dtos.Identity;
using EIP.System.Models.Entities;

namespace EIP.System.Business.Identity
{
    public interface ISystemPostLogic : IAsyncLogic<SystemPost>
    {
        /// <summary>
        ///     根据组织机构获取岗位信息
        /// </summary>
        /// <param name="input">组织机构Id</param>
        /// <returns></returns>
        Task<IEnumerable<SystemPostOutput>> GetPostByOrganizationId(NullableIdInput input);

        /// <summary>
        ///     检测配置项代码是否已经具有重复项
        /// </summary>
        /// <param name="input">需要验证的参数</param>
        /// <returns></returns>
        Task<OperateStatus> CheckPostCode(CheckSameValueInput input);

        /// <summary>
        ///     删除岗位信息
        /// </summary>
        /// <param name="input">岗位信息Id</param>
        /// <returns></returns>
        Task<OperateStatus> DeletePost(IdInput input);

        /// <summary>
        ///     保存岗位信息
        /// </summary>
        /// <param name="post">岗位信息</param>
        /// <returns></returns>
        Task<OperateStatus> SavePost(SystemPost post);
    }
}