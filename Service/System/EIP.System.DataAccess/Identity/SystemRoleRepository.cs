using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using EIP.Common.Core.Extensions;
using EIP.Common.Dapper;
using EIP.Common.DataAccess;
using EIP.Common.Entities.Dtos;
using EIP.System.Models.Dtos.Identity;
using EIP.System.Models.Entities;
using EIP.System.Models.Enums;

namespace EIP.System.DataAccess.Identity
{
    public class SystemRoleRepository : DapperAsyncRepository<SystemRole>, ISystemRoleRepository
    {
        /// <summary>
        ///     根据组织机构获取角色信息
        /// </summary>
        /// <param name="input"></param>
        /// <param name="isFreeze">是否冻结</param>
        /// <returns></returns>
        public Task<IEnumerable<SystemRoleOutput>> GetRolesByOrganizationId(NullableIdInput input = null,
            bool? isFreeze = null)
        {
            var sql =
                new StringBuilder(
                    @"SELECT RoleId,role.Name,role.Remark,role.OrderNo,role.CanbeDelete,role.State,role.IsFreeze,role.CreateTime,org.CreateUserId,org.CreateUserName,org.UpdateTime,org.UpdateUserId,org.UpdateUserName,org.Name OrganizationName,role.OrganizationId FROM System_Role role
                                                    LEFT JOIN System_Organization org ON org.OrganizationId=role.OrganizationId");
            sql.Append(" WHERE 1=1 ");
            if (input != null && !input.Id.IsNullOrEmptyGuid())
            {
                sql.Append(" AND role.OrganizationId=@orgId");
            }
            if (isFreeze != null)
            {
                sql.Append(" AND role.IsFreeze=@isFreeze");
            }
            sql.Append(" ORDER BY role.OrganizationId,role.OrderNo");
            if (input != null && !input.Id.IsNullOrEmptyGuid())
            {
                return SqlMapperUtil.SqlWithParams<SystemRoleOutput>(sql.ToString(), new {orgId = input.Id, isFreeze});
            }
            return SqlMapperUtil.SqlWithParams<SystemRoleOutput>(sql.ToString(), new {isFreeze});
        }

        /// <summary>
        ///     检查代码:唯一性检查
        /// </summary>
        /// <param name="input">代码</param>
        /// <returns></returns>
        public Task<bool> CheckPostCode(CheckSameValueInput input)
        {
            var sql = "SELECT RoleId FROM System_Role WHERE Code=@param";
            if (!input.Id.IsNullOrEmptyGuid())
            {
                sql += " AND RoleId!=@roleId";
            }
            return SqlMapperUtil.SqlWithParamsBool<SystemRole>(sql, new { param = input.Param, roleId = input.Id });
        }

        /// <summary>
        ///     获取该用户已经具有的角色信息
        /// </summary>
        /// <param name="privilegeMaster"></param>
        /// <param name="userId">需要查询的用户id</param>
        /// <returns></returns>
        public Task<IEnumerable<SystemRoleOutput>> GetHaveUserRole(EnumPrivilegeMaster privilegeMaster,
            Guid userId)
        {
            var sql = new StringBuilder(@"SELECT * FROM System_Role ro
                                                    LEFT JOIN System_PermissionUser per ON per.PrivilegeMasterValue=ro.RoleId
                                                    WHERE per.PrivilegeMasterUserId=@userId AND PrivilegeMaster=@privilegeMaster");
            return SqlMapperUtil.SqlWithParams<SystemRoleOutput>(sql.ToString(),
                new {privilegeMaster = (byte) privilegeMaster, userId});
        }
    }
}