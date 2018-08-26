using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using EIP.Common.Core.Extensions;
using EIP.Common.Dapper;
using EIP.Common.DataAccess;
using EIP.Common.Entities.Dtos;
using EIP.Common.Entities.Tree;
using EIP.System.Models.Dtos.Identity;
using EIP.System.Models.Entities;

namespace EIP.System.DataAccess.Identity
{
    public class SystemOrganizationRepository : DapperAsyncRepository<SystemOrganization>, ISystemOrganizationRepository
    {
        /// <summary>
        ///     根据父级查询下级
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public Task<IEnumerable<TreeEntity>> GetSystemOrganizationByPid(IdInput input)
        {
            var sql = new StringBuilder();
            sql.Append(
                "SELECT OrganizationId id,ParentId pId,Name,code FROM System_Organization WHERE ParentId=@pId ORDER BY OrderNo");
            return  SqlMapperUtil.SqlWithParams<TreeEntity>(sql.ToString(), new { pId = input.Id });
        }

        /// <summary>
        ///     获取所有组织机构信息
        /// </summary>
        /// <returns></returns>
        public Task<IEnumerable<TreeEntity>> GetSystemOrganization()
        {
            var sql = new StringBuilder();
            sql.Append(
                "SELECT OrganizationId id,ParentId pId,Name,code FROM System_Organization ORDER BY OrderNo");
            return  SqlMapperUtil.SqlWithParams<TreeEntity>(sql.ToString());
        }

        /// <summary>
        ///     根据父级查询下级
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public Task<IEnumerable<SystemOrganizationOutput>> GetOrganizationResultByTreeId(IdInput input)
        {
            var sql = new StringBuilder();
            sql.Append("SELECT * FROM System_Organization WHERE ParentId=@pId ORDER BY OrderNo");
            return  SqlMapperUtil.SqlWithParams<SystemOrganizationOutput>(sql.ToString(), new { pId = input.Id });
        }

        /// <summary>
        ///     检查代码:唯一性检查
        /// </summary>
        /// <param name="input">代码</param>
        /// <returns></returns>
        public Task<bool> CheckOrganizationCode(CheckSameValueInput input)
        {
            var sql = "SELECT OrganizationId FROM System_Organization WHERE Code=@param";
            if (!input.Id.IsNullOrEmptyGuid())
            {
                sql += " AND OrganizationId!=@organizationId";
            }
            return SqlMapperUtil.SqlWithParamsBool<SystemOrganization>(sql, new
            {
                param = input.Param,
                organizationId = input.Id
            });
        }

        /// <summary>
        /// 数据权限组织机构树
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public Task<IEnumerable<TreeEntity>> GetOrganizationResultByDataPermission(IdInput<string> input)
        {
            if (input.Id.IsNullOrEmpty())
            {
                return null;
            }
            string sql = "SELECT OrganizationId id,ParentId pId,Name,code,code FROM System_Organization WHERE 1=1 AND " + input.Id + " ORDER BY OrderNo";
            return  SqlMapperUtil.SqlWithParams<TreeEntity>(sql);
        }
    }
}