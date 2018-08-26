using System.Collections.Generic;
using System.Threading.Tasks;
using EIP.Common.Business;
using EIP.Common.Entities;
using EIP.Common.Entities.Dtos;
using EIP.Common.Entities.Tree;
using EIP.System.Models.Dtos.Identity;
using EIP.System.Models.Entities;

namespace EIP.System.Business.Identity
{
    public interface ISystemOrganizationLogic : IAsyncLogic<SystemOrganization>
    {
        /// <summary>
        ///     异步读取树数据
        /// </summary>
        /// <param name="input">父级id</param>
        /// <returns></returns>
        Task<IEnumerable<TreeEntity>> GetOrganizationTreeAsync(IdInput input);

        /// <summary>
        ///     同步读取所有树数据
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<TreeEntity>> GetOrganizationTree();

        /// <summary>
        ///     根据父级查询下级
        /// </summary>
        /// <param name="input">父级id</param>
        /// <returns></returns>
        Task<IEnumerable<SystemOrganizationOutput>> GetOrganizationResultByTreeId(IdInput input);

        /// <summary>
        ///     检测代码是否已经具有重复项
        /// </summary>
        /// <param name="input">需要验证的参数</param>
        /// <returns></returns>
        Task<OperateStatus> CheckOrganizationCode(CheckSameValueInput input);

        /// <summary>
        ///     保存组织机构
        /// </summary>
        /// <param name="organization">组织机构</param>
        /// <returns></returns>
        Task<OperateStatus> SaveOrganization(SystemOrganization organization);

        /// <summary>
        ///     删除组织机构下级数据
        /// </summary>
        /// <param name="input">父级id</param>
        /// <returns></returns>
        Task<OperateStatus> DeleteOrganization(IdInput input);

        /// <summary>
        ///     批量生成代码
        /// </summary>
        /// <returns></returns>
        Task<OperateStatus> GeneratingCode();

        /// <summary>
        /// 数据权限组织机构树
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<IEnumerable<TreeEntity>> GetOrganizationResultByDataPermission(IdInput<string> input);

    }
}