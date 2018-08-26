using System.Collections.Generic;
using System.Threading.Tasks;
using EIP.Common.DataAccess;
using EIP.Common.Entities.Dtos;
using EIP.Common.Entities.Tree;
using EIP.System.Models.Dtos.Identity;
using EIP.System.Models.Entities;

namespace EIP.System.DataAccess.Identity
{
    public interface ISystemOrganizationRepository : IAsyncRepository<SystemOrganization>
    {
        /// <summary>
        ///     根据父级查询下级
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<IEnumerable<TreeEntity>> GetSystemOrganizationByPid(IdInput input);

        /// <summary>
        ///     获取所有组织机构信息
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<TreeEntity>> GetSystemOrganization();

        /// <summary>
        ///     根据父级查询下级
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<IEnumerable<SystemOrganizationOutput>> GetOrganizationResultByTreeId(IdInput input);

        /// <summary>
        ///     检查代码:唯一性检查
        /// </summary>
        /// <param name="input">代码</param>
        /// <returns></returns>
        Task<bool> CheckOrganizationCode(CheckSameValueInput input);

        /// <summary>
        /// 数据权限组织机构树
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<IEnumerable<TreeEntity>> GetOrganizationResultByDataPermission(IdInput<string> input);
    }
}